var loginApp = angular.module("loginForm", []);
loginApp.controller("loginCtrl", [ '$scope', '$http', function ($scope, $http) {
    $scope.lock = false;
    $scope.myWelcome = "";

    $scope.loginCheck = function () {
        $scope.myWelcome = "Please wait...";
        $scope.lock = true;
        $http({
            method: "POST",
            url: "/Account/LoginAction",
            data: $.param({'password': $scope.password, 'login': $scope.login}),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

        }).then(function mySucces(response) {
            if (response.data === "Success") {
                window.location.href = "/Home/Index";
            }
                
            else {
                $scope.myWelcome = "Error - incorrect data!";
                $scope.login = "";
                $scope.password = "";
                $scope.lock = false;
            }
        }, function myError(response) {
            $scope.myWelcome = "Error";
            $scope.lock = false;
        })
    }

}])