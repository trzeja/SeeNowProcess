var backlogAngular = angular.module("backlog", []);
backlogAngular.controller("backlogCtrl", ['$scope', '$http', function ($scope, $http) {
    $scope.userstory = "Select user story to show content";
    $scope.currentUS;
    $scope.tasksUS;
    $scope.info;

    $http({
        method: "GET",
        url: "/Backlog/GetUserStories",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.userstories = response.data;
    }, function myError(response) {    
    })

    $scope.showUserStory = function (id, userstory) {
        $scope.userstory = userstory;
        
        $http({
            method: "POST",
            url: "/Backlog/GetUserStory",
            data: $.param({ "id": id }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(result) {
            $scope.currentUS = result.data[0];
        }).catch(function fail(result) {
            $scope.problems = ":(";
        })
        $http({
            method: "POST",
            url: "/Backlog/GetUSTasks",
            data: $.param({ "id": id }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(result) {
            $scope.tasksUS = result.data;
        }).catch(function fail(result) {
            $scope.problems = ":(";
        })
        $scope.info = {
            description: 'Description',
            notes: 'Notes',
            criteria: 'Criteria',
            owner: 'Owner: ',
            project: 'Project: ',
            size: 'Size: '
        };
        
    }
}]);