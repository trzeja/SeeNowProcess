var registerApp = angular.module("registerForm", []);
registerApp.directive('matchPassword', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, mCtrl, ngModel) {
            function passwordMatching(value) {
                if (value != password.value) {
                    mCtrl.$setValidity('matchPass', true);
                } else {
                    mCtrl.$setValidity('matchPass', false);
                }
                return value;
            }
            mCtrl.$parsers.push(passwordMatching);
        }
    }
});
registerApp.controller("registerCtrl",  [ '$scope', '$http', function ($scope, $http) {
    $scope.message = '';
    $scope.registerUser = function () {
        if ($scope.password !== $scope.confirmPassword) {
            $scope.message = "Passwords don't match!";
        }
        else {
            $http({
                method: "POST",
                url: "/Account/RegisterAction",
                data: $.param({ 'password': $scope.password, 'login': $scope.login, 'name': $scope.name, 'email': $scope.email, 'phone': $scope.phone }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                if (response.data === "Success")
                    window.location.href = "/Add/Index";
                else {
                    $scope.message = response.data;
                }
            }, function myError(response) {
                $scope.message = "Error";
            })


        }
        
    }
}]);