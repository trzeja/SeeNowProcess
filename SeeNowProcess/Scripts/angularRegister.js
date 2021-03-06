﻿var registerApp = angular.module("registerForm", []);
registerApp.directive('matchPassword', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, mCtrl, ngModel) {
            function passwordMatching(value) {
                if (value != password.value) {
                    mCtrl.$setValidity('matchPass', false);
                } else {
                    mCtrl.$setValidity('matchPass', true);
                }
                return value;
            }
            mCtrl.$parsers.push(passwordMatching);
        }
    }
});
registerApp.controller("registerCtrl",  [ '$scope', '$http', function ($scope, $http) {
    $scope.message = '';
    $scope.lock = false;
    $scope.roles_options =
    [
         { value: "admin", description: "Admin" },
         { value: "headMaster", description: "Head Master" },
         { value: "seniorDev", description: "Senior Developer" },
         { value: "juniorDev", description: "Junior Developer" },
         { value: "intern", description: "Intern" },
         { value: "tester", description: "Tester" },
         { value: "client", description: "Client" },


    ];

    $scope.forms = function () {
        $scope.registerForm.$error;
    }

    $scope.registerUser = function () {
        if ($scope.password !== $scope.confirmPassword) {
            $scope.message = "Passwords don't match!";
        }
        else {
            $scope.lock = true;
            $scope.message = "Please wait...";
            $http({
                method: "POST",
                url: "/Account/RegisterAction",
                data: $.param({ 'password': $scope.password, 'login': $scope.login, 'name': $scope.name, 'email': $scope.email, 'phoneNumber': $scope.phone, 'role': $scope.role.value }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                if (response.data === "Success")
                    window.location.href = "/Add/Index";
                else {
                    $scope.lock = false;
                    $scope.message = response.data;
                    $scope.password = "";
                    $scope.confirmPassword = "";
                }
            }, function myError(response) {
                $scope.lock = false;
                $scope.message = "Error";
            })


        }
        
    }
}]);