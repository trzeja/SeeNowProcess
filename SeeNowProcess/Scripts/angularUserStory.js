var userStoryApp = angular.module("userStoryForm", []);
userStoryApp.controller("userStoryCtrl", function ($scope, $http) {
    $scope.userStoriesTeams = [];
    $scope.selected_teams = [];
    $scope.userStoriesProjects_options = [];


    $http({
        method: "GET",
        url: "/Add/GetProjects",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.response = response.data;
        for (var i = 0; i < $scope.response.length; ++i) {
            $scope.userStoriesProjects_options.push({ id: $scope.response[i].id, name: $scope.response[i].name })
        }
    }, function myError(response) {
        $scope.message = "Error";
    })

    $scope.checkTeam = function (id) {
        if ($scope.selected_teams.indexOf(id) === -1) {
            $scope.selected_teams.push(id);
        }
        else {
            $scope.selected_teams.splice($scope.selected_teams.indexOf(id), 1);
        }
    };

    $http({
        method: "GET",
        url: "/Backlog/GetTeams",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.response = response.data;
        for (var i = 0; i < $scope.response.length; ++i) {
            $scope.userStoriesTeams.push({ id: $scope.response[i].ID, name: $scope.response[i].name })
        }
    }, function myError(response) {
        $scope.message = "Error";
    })


    $scope.addUserStory = function () {
        $http({
            method: "POST",
            url: "/Backlog/AddUserStory",
            data: $.param({
                'title': $scope.userStoryTitle, 'description': $scope.userStoryDescription,
                'size': $scope.userStorySize, 'unit': $scope.userStoryUnit,
                'notes': $scope.userStoryNotes, 'criteria': $scope.userStoryCriteria,
                'project': $scope.userStoryProject.id, 'teams': $scope.selected_teams
            }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(response) {
            window.location.href = "/Backlog/BacklogIndex";
        },
        function failure(response) {
            $scope.message = "Fail...";
        });
    }
});



 