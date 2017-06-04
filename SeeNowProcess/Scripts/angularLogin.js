var loginApp = angular.module("loginForm", []);
loginApp.controller("loginCtrl", [ '$scope', '$http', function ($scope, $http) {

    $scope.myWelcome = "";

    $scope.loginCheck = function () {
        var objUser = $.param({
            password: $scope.password,
            login: $scope.login
        })
        $http({
            method: "POST",
            url: "/Account/LoginAction",
            data: $.param({'password': $scope.password, 'login': $scope.login}),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

        }).then(function mySucces(response) {
            if (response.data === "Success")
                window.location.href = "/People/PeopleIndex";
            else
                $scope.myWelcome = "Error - incorrect data!";
                $scope.login = "";
                $scope.password = "";
        }, function myError(response) {
            $scope.myWelcome = "Error";
            $scope.login = "";
            $scope.password = "";
        })

        /*
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({
            Login: $scope.login,
            Password: $scope.password
        });

        $http.post('/Account/LoginAction',data, config).
            success(function (data) {
                $scope.myWelcome = "Success";
            }).
            error(function () {
                $scope.myWelcome = "Error";
            })*/

        /*.post("/Account/Login", objUser).then(function (response) {
            alert("You did it!");
        }, function (response) {

        });*/
    }

}])