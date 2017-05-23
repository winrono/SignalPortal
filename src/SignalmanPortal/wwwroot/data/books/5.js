const path = require('path');
const Logger = require('../../logger.js');
const UglifyJS = require('uglify-js');
const minify = require('html-minifier').minify;
const htmlparser = require('htmlparser2');
const parse5 = require('parse5');
const dom5 = require('dom5');
const fs = require('fs');
const crisper = require('crisper');
const Bundler = require('polymer-bundler').Bundler;
const Analyzer = require('polymer-analyzer').Analyzer;
const fs_url_loader_1 = require("polymer-analyzer/lib/url-loader/fs-url-loader")
const analyzer = new Analyzer({
  urlLoader: new fs_url_loader_1.FSUrlLoader("D:\\")
});
const bundler = new Bundler({
  analyzer: analyzer,
  excludes: [],
  inlineScripts: true,
  inlineCss: true,
  stripComments: true
});
const getFilesizeInKb = function (filename) {
  const stats = fs.statSync(filename);
  const fileSizeInBytes = stats.size;
  return (fileSizeInBytes / 1000);
};
let grunt;
const TaskRunner = {
  _grunt: null,
  _fileName: 'build',
  _exportNameSpace: 'components',
  _exportSuffix: 'build',
  _suffix: 'app-dev-tmp',
  _keyName: 'include-files',
  bowerDirLocation: {
    rel: undefined,
    abs: undefined
  },
  _bowerDepth: false,
  init: function (_grunt) {
    grunt = _grunt;
    this._grunt = grunt;
    this.loadNPM();
    this.register();
    return this.getGruntConfig();
  },
  loadNPM: function () {
    grunt.loadNpmTasks('grunt-shell-spawn');
    grunt.loadNpmTasks('grunt-contrib-copy');
  },
  /**
   * @description placeholder for future feature to let custom export namespace to be passed
   * @private
   * @returns {string} export name space
   */
  _getExportNameSpace: function () {
    return this._exportNameSpace;
  },
  getGruntConfig: function () {
    if (grunt.option('import')) {
      let importName = this.getImportName();
      this._fileName = importName + '.' + this._suffix;
    }
    return {};
  },
  stripPathForSourceMap: function (arr) {
    let self = this;
    let out = [];
    arr.forEach(function (item) {
      out.push(self.getFileFromPath(item));
    });
    return out;
  },
  getFileFromPath: function (path) {
    return (path.substring(path.lastIndexOf('/') + 1));
  },
  beautifyJsFromFile: function (file) {
    return this.beautifyJs(grunt.file.read(file));
  },
  beautifyJs: function (src) {
    let ast = UglifyJS.parse(src);

    let stream = UglifyJS.OutputStream({
      beautify: true,
      indent_level: 2
    });
    ast.print(stream);
    return stream.toString();
  },
  getOutputDir: function () {
    return (grunt.option('output') ? grunt.option('output') : '');
  },
  getImportDir: function () {
    return grunt.option('import').substring(0, grunt.option('import').lastIndexOf('/') + 1);
  },
  getImportFileWithPath: function () {
    return this.getInputPath() + this._fileName + '.html';
  },
  getImportJsFileWithPath: function () {
    return this.getInputPath() + this._fileName + '.js';
  },
  getExportFileWithPath: function () {
    return this.getInputPath() + this._fileName + '.html';
  },
  getOutputPath: function () {
    return path.join(grunt.option('project'), this.getOutputDir());
  },
  getExportFolder: function () {
    return path.join(grunt.option('project'), grunt.option('output')) || '';
  },
  getInputPath: function () {
    return path.join(grunt.option('project'), this.getImportDir(), path.sep);
  },
  getImportName: function () {
    let importFile = this.getFileFromPath(grunt.option('import'));
    return importFile.substring(0, importFile.lastIndexOf('.'));
  },
  getDirectory: function () {
    return grunt.option('project') || ('.' + path.sep);
  },
  getOriginalImportFile: function () {
    return path.join(grunt.option('project'), grunt.option('import'));
  },
  /*
   * gets the bower components folder
   * will find it and make sure the import exists and that there is only one bower folder
   * @returns {*}
   */
  getBowerComponentsFolder: function () {
    if (TaskRunner.bowerDirLocation.abs === undefined) {

      let file = this.getOriginalImportFile();
      let text = grunt.file.read(file);
      let inputPath = this.getInputPath();
      let bowerLocation = '';
      let match = 'bower_components';
      let parser = new htmlparser.Parser({
        onopentag: function (name, attribs) {
          if (name === 'link' && attribs.rel === 'import' && attribs.href) {
            let curRef = attribs.href;
            let filePath = path.join(inputPath, path.normalize(attribs.href));
            let fileExists = grunt.file.isFile(filePath);
            if (!fileExists) {
              TaskRunner.failAndCleanup('Cannot locate referenced import file for "' + filePath + '", aborting.');
            }
            else {
              let pathSplit = curRef.split(match);
              if (pathSplit.length > 1) {
                let pathBowerLocation = pathSplit[0] + match;
                if (bowerLocation === '') {
                  bowerLocation = pathBowerLocation;
                }
                else if (pathBowerLocation !== bowerLocation) {
                  // check this path to ensure we don't have multiple bower folders, which could cause issue
                  TaskRunner.failAndCleanup('Cannot handle multiple bower folders, "' + inputPath + bowerLocation + '", and "' + inputPath + pathBowerLocation + ' do not match", aborting.');
                }
              }
            }
          }
        }
      }, { decodeEntities: true });
      parser.write(text);
      parser.end();

      TaskRunner.bowerDirLocation.abs = bowerLocation ? path.join(inputPath, bowerLocation) : '';
      TaskRunner.bowerDirLocation.rel = bowerLocation ? bowerLocation : '';

    }
    return TaskRunner.bowerDirLocation;
  },
  /**
   * get the bower dir depth relative to the import dir
   * @returns {Number} depth of the bower dir relative to the import dir
   */
  getBowerComponentsDepth: function () {
    if (TaskRunner._bowerDepth === false) {
      TaskRunner._bowerDepth = (TaskRunner.getBowerComponentsFolder().rel.match(/\.\./g) || []).length;
    }
    return TaskRunner._bowerDepth;
  },
  cleanUp: function (suppressErrorReporting) {
    suppressErrorReporting = suppressErrorReporting || false;
    let toDelete = [
      {
        file: this.getImportFileWithPath()
      },
      {
        file: this.getImportJsFileWithPath()
      }
    ];
    toDelete.forEach(function (item) {
      if (grunt.file.isFile(item.file)) {
        let res = grunt.file.delete(item.file, {
          force: true
        });
        let fileName = TaskRunner.getFileFromPath(item.file);
        if (res) {
          if (!suppressErrorReporting) {
            Logger.ok('Deleted "' + fileName + '"');
          }
        }
        else {
          Logger.error('Failed to delete "' + fileName + '"');
        }
      }
    });
  },
  removeOutputDir: function () {
    let outputDir = this.getExportFolder();
    if (grunt.file.isDir(outputDir)) {
      let removeOutput = this._grunt.file.delete(outputDir, { force: true });
      if (!removeOutput) {
        Logger.error('Failed to delete "' + outputDir + 'this"');
      }
    }
  },
  failAndCleanup: function (error) {
    this.removeOutputDir();
    this.cleanUp(true);
    Logger.error('Build failed. Cleaning up tmp files.');
    grunt.fail.fatal(error || 'Build failed. Unspecified reasons.', 1);
  },
  getWildCardFiles: function (name, subdir, filePath, directory, bower) {
    const fileCollection = grunt.file.expand({
      cwd: filePath
    }, name);
    for (let f = 0; f < fileCollection.length; f++) {
      if (!this.isIgnoreFile(fileCollection[f], bower, filePath)) {
        let dist = path.join(grunt.option('output'), subdir, fileCollection[f]);
        if (grunt.file.isFile(path.join(filePath, fileCollection[f]))) {
          Logger.ok('Copying - ' + subdir + '/' + fileCollection[f]);
          grunt.file.copy(path.join(filePath, fileCollection[f]), path.join(directory, dist));
        }
      }
    }
  },
  isIgnoreFile: function (path, bower, filePath) {
    if (bower['x-meta'].build['ignore-files']) {
      let ignoreFiles = bower['x-meta'].build['ignore-files'];
      for (let a = 0; a < ignoreFiles.length; a++) {
        let ignorePath = ignoreFiles[a];
        if (ignorePath.indexOf('*') >= 0) {
          let fileCollection = grunt.file.expand({
            cwd: filePath
          }, ignorePath);

          for (let b = 0; b < fileCollection.length; b++) {
            if (fileCollection[b] === path) {
              return true;
            }
          }
        }
        else if (ignoreFiles[a] === path) {
          return true;
        }
      }
    }
    return false;
  },
  registerCustomTasks: {
    preFlight: function () {
      console.log('CWD: %s', TaskRunner.getInputPath());
      console.log('Validating bower reference');
      let bowerFolder = TaskRunner.getBowerComponentsFolder();
      let importFileExists = false;
      if (fs.existsSync(TaskRunner.getOriginalImportFile())) {
        importFileExists = true;
      }
      if (bowerFolder.abs === '' || !importFileExists) {
        TaskRunner.failAndCleanup('bower failed validation');
      }
      else {
        Logger.ok('bower seems ok');
      }

    },

    replaceBowerPaths: function () {

      const grunt = TaskRunner._grunt;

      let content = grunt.file.read(TaskRunner.getImportFileWithPath());
      const exportFile = TaskRunner.getExportFileWithPath();
      // should be looking here to identify link tags and work out where the components live
      let bowerToMatch = new RegExp(TaskRunner.getBowerComponentsFolder().rel + '/', 'g');
      content = content.replace(bowerToMatch, '../');
      content = content.replace('src="' + TaskRunner._fileName + '.js"', 'src="' + TaskRunner._getExportNameSpace() + '.min.js"');
      grunt.file.write(exportFile, content);
      Logger.ok('Wrote to "' + exportFile + '" with modified bower paths');
    },
    copyFilesToDist: function () {
      const grunt = TaskRunner._grunt;
      const output = TaskRunner.getOutputPath();
      const htmlPath = TaskRunner.getExportFileWithPath();
      const jsPath = TaskRunner.getImportJsFileWithPath();
      const htmlOutputFile = path.join(output, 'import', TaskRunner._getExportNameSpace() + '.html');
      const jsOutputFile = path.join(output, 'import', TaskRunner._getExportNameSpace() + '.js');
      const jsOutputFileMin = path.join(output, 'import', TaskRunner._getExportNameSpace() + '.min.js');
      const jsOutMapFileName = TaskRunner._getExportNameSpace() + '.js.map';
      const jsOutputFileMap = path.join(output, 'import', jsOutMapFileName);

      Logger.ok('Copying js from "' + TaskRunner.getFileFromPath(jsPath) + '" to "' + TaskRunner.getFileFromPath(jsOutputFile) + '" and making more readable');
      grunt.file.write(jsOutputFile, TaskRunner.beautifyJsFromFile(jsPath));

      Logger.ok('Copying html and minifying from "' + TaskRunner.getFileFromPath(htmlPath) + '" to "' + TaskRunner.getFileFromPath(htmlOutputFile) + '"');
      const htmlMinified = minify(grunt.file.read(htmlPath), {
        collapseWhitespace: true,
        collapseInlineTagWhitespace: true,
        removeComments: true,
        minifyCSS: true
      });
      grunt.file.write(htmlOutputFile, htmlMinified);
      console.log('min html size: %skB', getFilesizeInKb(htmlOutputFile));
      Logger.ok('Copying and minifying js from "' + TaskRunner.getFileFromPath(jsPath) + '" to "' + TaskRunner.getFileFromPath(jsOutputFileMin) + '"');
      const result = UglifyJS.minify([jsOutputFile], {
        outSourceMap: jsOutMapFileName,
        compress: {
          conditionals: true,
          join_vars: true,
          if_return: true,
          keep_fnames: true,
          booleans: true,
          loops: true
        }
      });
      grunt.file.write(jsOutputFileMin, result.code);
      console.log('min js size: %skB', getFilesizeInKb(jsOutputFileMin));

      Logger.ok('Created basic source map to "' + jsOutMapFileName + '"');
      const map = JSON.parse(result.map);
      // rewrite sources due to bug in npm package
      // https://github.com/NodeBB/NodeBB/issues/1609
      map.sources = TaskRunner.stripPathForSourceMap(map.sources);
      grunt.file.write(jsOutputFileMap, JSON.stringify(map));
    },
    cleanPolybuild: function () {
      TaskRunner.cleanUp();
    },
    /**
     * processes the source file, creating a composite file from all the imports
     * will write a composite html file and a min js file of all scripts, src is injected into the html file
     */
    runBundle: function () {
      console.log('Bundling project');
      let origFile = this.getOriginalImportFile();
      let htmlPath = TaskRunner.getExportFileWithPath();
      let jsFile = TaskRunner.getImportJsFileWithPath();
      let done = grunt.task.current.async();
      /**
       * fix polymer bug in windows
       * /node_modules/polymer-bundler/lib/url-utils.js L:74 does not use posix when calculating relative paths
       * Corrects:
       * 1) os specific paths being generated for the assetpath attr
       * 2) original html import file being left in vulcanised content
       * @param {Object} doc parse5 document of the vulcanised content
       */
      let handlePolymerWindowsBug = function (doc) {
        // establish predicates
        if (process.platform !== 'win32') {
          return;
        }
        let matchers = {
          domModule: dom5.predicates.AND(dom5.predicates.hasTagName('dom-module'), dom5.predicates.hasAttr('assetpath')),
          htmlImport: dom5.predicates.AND(dom5.predicates.hasTagName('link'),
          dom5.predicates.hasAttrValue('rel', 'import'),
          dom5.predicates.hasAttr('href'),
          dom5.predicates.OR(dom5.predicates.hasAttrValue('type', 'text/html'), dom5.predicates.hasAttrValue('type', 'html'),
          dom5.predicates.NOT(dom5.predicates.hasAttr('type'))))
        };
        let domModules = dom5.queryAll(doc, matchers.domModule);
        // correct the assetpath from windows format to unix
        domModules.forEach(function (domModule) {
          dom5.setAttribute(domModule, 'assetpath', dom5.getAttribute(domModule, 'assetpath').replace(/\\/g, '/'));
        });

        let htmlImports = dom5.queryAll(doc, matchers.htmlImport);
        let originalHtmlName = TaskRunner.getImportName() + '.html';
        // remove the spurious html import which echos the original import
        htmlImports.forEach(function (htmlImport) {
          if (dom5.getAttribute(htmlImport, 'href') === originalHtmlName) {
            dom5.remove(htmlImport);
          }
        });
      };
      bundler.bundle([origFile]).then(function (bundlesMap) {
        Logger.ok('Bundled');
        console.log('Processing bundle...');
        // get the single file we currently use
        return bundlesMap.get(origFile).ast;
      }).then(function (doc) {
        handlePolymerWindowsBug(doc);
        let docString = parse5.serialize(doc);
        // pass this to crisper
        let out = crisper({
          source: docString,
          jsFileName: TaskRunner._fileName + '.js',
          scriptInHead: true, // default true
          onlySplit: false, // default false
          alwaysWriteScript: false // default false
        });
        // write the split js and html to files - to let the rest of this task process it
        grunt.file.write(htmlPath, out.html);
        grunt.file.write(jsFile, out.js);
        Logger.ok('Bundle processed');
        // let grunt now we are ready to proceed
        done();
      });
    },
    moveBowerFilesToDist: function () {
      const grunt = TaskRunner._grunt;
      console.log('Locating bundle-include files');
      const directory = grunt.option('project') || './';
      const _this = this;
      const bowerFolder = TaskRunner.getBowerComponentsFolder().abs;
      if (bowerFolder !== '' && grunt.file.isDir(bowerFolder)) {
        grunt.file.recurse(bowerFolder, function (abspath, rootdir, subdir, filename) {
          if (subdir && subdir.indexOf(path.sep) < 0) {
            if (filename === 'bower.json') {
              let bower = grunt.file.readJSON(abspath);
              let filePath = abspath.substring(0, abspath.lastIndexOf('/'));

              if (bower['x-meta'] && bower['x-meta'].build && bower['x-meta'].build[TaskRunner._keyName]) {
                for (let a = 0; a < bower['x-meta'].build[TaskRunner._keyName].length; a++) {
                  let name = bower['x-meta'].build[TaskRunner._keyName][a];

                  let dist;
                  if (name.indexOf('*') >= 0) {
                    _this.getWildCardFiles(name, subdir, filePath, directory, bower);
                  }
                  else {
                    dist = path.join(grunt.option('output'), subdir, name);
                    Logger.ok('Copying - ' + path.join(subdir, name));
                    grunt.file.copy(path.join(filePath, name), path.join(directory, dist));
                  }
                }
              }
            }
          }
        });
      }
      else {
        TaskRunner.failAndCleanup('Failed to locate bower folder at "' + bowerFolder + '", aborting.');
      }
    }
  },
  register: function () {
    grunt.registerTask('preFlight', this.registerCustomTasks.preFlight);
    grunt.registerTask('replaceBowerPaths', this.registerCustomTasks.replaceBowerPaths);
    grunt.registerTask('copyFilesToDist', this.registerCustomTasks.copyFilesToDist);
    grunt.registerTask('cleanPolybuild', this.registerCustomTasks.cleanPolybuild);
    grunt.registerTask('runBundle', this.registerCustomTasks.runBundle.bind(this));
    grunt.registerTask('moveBowerFilesToDist', this.registerCustomTasks.moveBowerFilesToDist.bind(this));
    // task to run externally
    grunt.registerTask('bundle', ['preFlight', 'runBundle', 'replaceBowerPaths', 'copyFilesToDist', 'moveBowerFilesToDist', 'cleanPolybuild']);
  }
};
module.exports = TaskRunner.init.bind(TaskRunner);
