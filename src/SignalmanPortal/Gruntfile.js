/// <binding BeforeBuild='concat' />
module.exports = function (grunt) {

    var jsFolder = 'wwwroot/js/';
    var libJsFolder = 'wwwroot/lib/';

    var appJsFiles = [
        jsFolder + 'site.js',
        jsFolder + 'app.module.js',
        jsFolder + 'app.route.js',
        jsFolder + 'bookCategoriesController.js',
        jsFolder + 'booksController.js',
        jsFolder + 'bookController.js'
    ]

    var libJsFiles = [
        libJsFolder + 'jquery/dist/jquery.js',
        libJsFolder + 'bootstrap/dist/js/bootstrap.js',
        libJsFolder + 'angular/angular.js',
        libJsFolder + 'angular-route/angular-route.js'
    ]

    grunt.initConfig({
        concat: {
            libJs: {
                src: libJsFiles,
                dest: jsFolder + 'build/libs.js'
            },
            appJs: {
                src: appJsFiles,
                dest: jsFolder + 'build/app.js'
            }
        }
    });

    grunt.loadNpmTasks("grunt-contrib-concat");

};