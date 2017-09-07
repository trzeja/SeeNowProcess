var teamApp = angular.module("teamForm", []);
teamApp.controller("teamCtrl", function ($scope,$http) {
    $scope.lock = false;
    $scope.response = [];
    $scope.users = [];
    $scope.selected_users = [];
    $scope.leader_options = [];

   

    $http({
        method: "GET",
        url: "/Add/AllPeople",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.response = response.data;
        for (var i = 0; i < $scope.response.length; ++i) {
            $scope.users.push({ id: $scope.response[i].id, NAME: $scope.response[i].NAME })
            $scope.leader_options.push({ id: $scope.response[i].id, NAME: $scope.response[i].NAME })
        }

    }, function myError(response) {
        $scope.message = "Error";
    })
    
    $scope.checkUser = function (id) {
        if ($scope.selected_users.indexOf(id) === -1) {
            $scope.selected_users.push(id);
        }
        else {
            $scope.selected_users.splice($scope.selected_users.indexOf(id), 1);
        }
    };

    $scope.addTeam = function () {
        $scope.message = "Please wait...";
        $scope.lock = true;
        $http({
            method: "POST",
            url: "/Add/IndexAddTeam",
            data: $.param({
                'name': $scope.name,'leader': $scope.leader.id,
                'users': $scope.selected_users
            }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(response) {
            if (response.data == "success") {
                window.location.href = "/People/PeopleIndex";
            }
            else {
                $scope.lock = false;
                $scope.message = response.data;
            }
        },
        function failure(response)
        {
            $scope.lock = false;
            $scope.message = "Fail...";
        });
    }
});



 