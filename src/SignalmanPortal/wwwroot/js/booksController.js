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
