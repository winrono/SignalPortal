(function () {
    'use strict';

    angular
        .module('app')
        .controller('bookCategoriesController', controller);

    controller.$inject = ['$scope', '$http'];

    function controller($scope, $http) {
        $http.get('/admin/GetBookCategories').then(function (response) {
            $scope.categories = response.data;
        });

        $scope.saveCategories = function () {
            $http.post('/admin/SaveCategories', $scope.categories).then(function () {
                console.log('called!');
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
