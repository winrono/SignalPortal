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
                if (response.status) {
                    location.href = '/Books/DownloadBook?id=' + $scope.book.bookId + '&fileExtension=' + $scope.book.fileExtension;
                }
                else {
                    alert('Файл книги не найден');
                }
            });
        }
    }
})();
