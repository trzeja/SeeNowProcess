var loginApp = angular.module("loginForm", []);
loginApp.controller("loginCtrl", [ '$scope', '$http', function ($scope, $http) {
    $scope.lock = false;
    $scope.myWelcome = "";

    $scope.loginCheck = function () {
        $scope.myWelcome = "Please wait...";
        $scope.lock = true;
        //var objUser = $.param({
        //    password: $scope.password,
        //    login: $scope.login
        //})
        $http({
            method: "POST",
            url: "/Account/LoginAction",
            data: $.param({'password': $scope.password, 'login': $scope.login}),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

        }).then(function mySucces(response) {
            if (response.data === "Success") {
                window.location.href = "/People/PeopleIndex";
            }
                
            else {
                // ewentualnie mozna rozdzielic na password i username zle, jesli mi kontroler zwroci info
                $scope.myWelcome = "Error - incorrect data!";
                $scope.login = "";
                $scope.password = "";
                $scope.lock = false;
            }
        }, function myError(response) {
            $scope.myWelcome = "Error";
            //$scope.login = "";
            //$scope.password = "";
            $scope.lock = false;
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