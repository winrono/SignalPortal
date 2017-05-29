$(document).ready(function () {
    $('#main-navbar [title]').attr('data-toggle', 'tooltip');
    $('#main-navbar [title]').attr('data-placement', 'bottom');
    $('[data-toggle="tooltip"]').tooltip();

    $('#adminList').on('shown.bs.dropdown', function () {
        $(this).tooltip('hide');
        $(this).tooltip('disable');
    });

    $('#adminList').on('hidden.bs.dropdown', function () {
        $(this).tooltip('enable');
    });

    $('body').on('click', '[deleteNoveltyLink], [deleteBookLink], [deleteBookCategoryLink]', function (e) {
        var that = this;

        e.preventDefault();
        $.ajax({

            url: $(this).attr("href"),
            success: function (isDeleted) {
                if (isDeleted) {
                    var parentRow = $(that.closest('.row, .tr-row'));
                    $(that.closest('.row, .tr-row')).fadeOut();

                    var input = parentRow.find('.removed-input');
                    if (input) {
                        input.val(true);
                    }
                }
                else {
                    alert('Информации нет в базе данных. Возможно, кто-то уже удалил её');
                }
            }

        });
    });
});
(function () {
    'use strict';

    angular.module('app', ['ngRoute']);
})();
;
(function () {
    "use strict";

    angular.module("app")
        .config(["$routeProvider", "$locationProvider", function ($routeProvider, $locationProvider) {

            $locationProvider.hashPrefix('');

            $routeProvider
                .when('/books', {
                    templateUrl: "/views/Books/Books.html"
                })
                .when("/books/details/:id", {
                    controller: 'BookController',
                    templateUrl: "/views/Books/Details.html"
                });
        }]);
})();
(function () {
    'use strict';

    angular
        .module('app')
        .controller('bookCategoriesController', controller);

    controller.$inject = ['$scope', '$http'];

    function controller($scope, $http) {
        $http.get('/books/GetBookCategories').then(function (response) {
            $scope.categories = response.data;
            $("#bookCategories").removeClass('hidden');
        });

        $scope.saveCategories = function () {
            $http.post('/admin/SaveCategories', $scope.categories).then(function () {
                location.reload();
            });
        };

        $scope.addCategory = function (categoryName) {
            var newCategory = {
                name: categoryName,
                isRemoved: false
            }
            $scope.categories.push(newCategory);
        }
    }
})();

(function () {
    'use strict';

    angular
        .module('app')
        .controller('booksController', booksController);

    booksController.$inject = ['$scope', '$http', '$q'];

    function booksController($scope, $http, $q) {

        function init() {
            var getBookCategories = $http.get('/books/GetBookCategories').then(function (response) {
                $scope.categories = [{ name: "Все" }];
                $scope.categories = $scope.categories.concat(response.data);
            });

            var selectFirstCategory = $scope.selectCategory(null, 0);

            $q.all([getBookCategories, selectFirstCategory]).then(function () {
                $("#booksView").removeClass('hidden');
            });

            turnOnScrollEvents();
        };

        $scope.selectCategory = function (categoryId, index) {

            var deferred = $q.defer();

            $http.get("/Books/GetBooksForCategory?categoryId=" + categoryId).then(function (response) {
                $scope.books = response.data;
                $scope.page = 0;
                $scope.categoryId = categoryId;
                $scope.selectedCategoryId = index;
                $scope.booksLoadedCompletely = false;
                deferred.resolve();
            });

            return deferred.promise;
        }

        var scrollEventHandler = function () {
            if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                $scope.loadNextItems();
            }
        }

        var mousewheelEventHandler = function (event) {
            if (event.originalEvent.wheelDelta > 0 || event.originalEvent.detail < 0) {
                //scrolling from bottom to top - no need to perform any action
            }
            else if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                $scope.loadNextItems();
            }
        }

        function turnOnScrollEvents() {
            $(window).bind('scroll', scrollEventHandler);

            $(window).bind('mousewheel DOMMouseScroll', mousewheelEventHandler);
        }

        function turnOffScrollEvents() {
            $(window).unbind("scroll", scrollEventHandler);

            $(window).unbind('mousewheel DOMMouseScroll', mousewheelEventHandler);
        }

        $scope.loadNextItems = function () {

            if (!$scope.booksLoadedCompletely) {

                $scope.page++;

                turnOffScrollEvents();

                $http.get("/Books/GetBooksForCategory?categoryId=" + $scope.categoryId + "&pageId=" + $scope.page).then(function (response) {

                    if (response.data.length === 0) {
                        $scope.booksLoadedCompletely = true;
                    }
                    else {
                        $scope.books = $scope.books.concat(response.data);
                    }

                    turnOnScrollEvents();

                });
            }
        }

        init();
    }
})();

;
(function () {
    'use strict';

    angular
        .module('app')
        .controller('BookController', BookController);

    BookController.$inject = ['$scope', '$http', '$routeParams'];

    function BookController($scope, $http, $routeParams) {

        var bookId = $routeParams.id;

        $http.get('/books/GetBookById/' + bookId).then(function (response) {
            $scope.book = response.data;
        });

        $scope.downloadBook = function () {

            $http.get('/Books/CheckFileExistance?id=' + $scope.book.bookId + '&fileExtension=' + $scope.book.fileExtension).then(function (response) {
                if (response.status === 200) {
                    location.href = '/Books/DownloadBook?id=' + $scope.book.bookId + '&fileExtension=' + $scope.book.fileExtension;
                }
            }).catch(function (error) {
                alert('Файл книги не найден');
            });
        }
    }
})();

