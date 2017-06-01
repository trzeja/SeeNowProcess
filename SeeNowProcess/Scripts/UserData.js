var userApp = angular.module("userApp", []);
userApp.controller("userCtrl", function ($scope, $http) {
    $scope.userData = 'User';

    $http({
        method: "GET",
        url: "/Home/GetUser",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.userData = response.data;
    }, function myError(response) {
        $scope.userData = "ErrorAngular";
    })
});