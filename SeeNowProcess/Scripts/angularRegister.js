var registerApp = angular.module("registerForm", []);
registerApp.controller("registerCtrl", function ($scope) {
    $scope.users =
        [
            {
                login: "", password: "",
                name: "", email: "",
                phone: ""
            }
        ];

    $scope.registerUser = function () {
        $scope.users.push({
            login: $scope.login, password: $scope.password, name: $scope.name, email: $scope.email, phone: $scope.phone
        });
    }
});