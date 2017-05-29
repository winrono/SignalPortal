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