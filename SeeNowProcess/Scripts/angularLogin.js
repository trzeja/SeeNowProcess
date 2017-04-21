var loginApp = angular.module("loginForm", []);
loginApp.controller("loginCtrl", [ '$scope', '$http', function ($scope, $http) {

    $scope.logins =
    [
        {
            Login: "logins",
            Password: "passwords"
        }
    ];
    $scope.myWelcome = "Still Waiting";

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
            if (response.data.indexOf("Error") === -1) {                //tutaj to wszystko trzeba poprawic bo na razie nie robi przekierowania na inna strone
                $http({
                    method: "GET",
                    url: "/People/Index",
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
                })
                $scope.myWelcome = response.data;
            }
            else if (response.data.indexOf("Login") === -1) {
                $scope.myWelcome = "Wrong Password";
            }
            else {
                $scope.myWelcome = "Wrong Login - User doesn't exist";
            }
        }, function myError(response) {
                $scope.myWelcome = "Error";
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