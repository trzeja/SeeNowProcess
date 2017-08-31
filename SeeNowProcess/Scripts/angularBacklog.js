var backlogAngular = angular.module("backlog", []);
backlogAngular.controller("backlogCtrl", ['$scope', '$http', function ($scope, $http) {
    $scope.importance = ["None", "Trivial", "Regular", "Important", "Critical"];
    $scope.userstory = "Select user story to show content";
    $scope.currentUS;
    $scope.tasksUS;
    $scope.info;

    $scope.show = 0;
    $scope.currentProject;
    //$scope.currentName;
    

    $scope.projects;

    $scope.$watch('currentProject', function () {
        if ($scope.show != 0) {
            $http({
                method: "POST",
                url: "/Backlog/ChangeCurrentProject",
                data: $.param({ id: $scope.currentProject }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
            }).then(function mySucces(response) {
                if (response.data == "Change") {
                    window.location.href = "/Backlog/BacklogIndex";
                }
            }, function myError(response) {
            })

        } else {
            $scope.show = 1;
            //za pierwszym razem wysyła zawsze, gdy nie mamy ustalonego jeszcze currentProject - dlatego są błędy. 
        }

    });

    //$http({
    //    method: "GET",
    //    url: "/Backlog/GetProjects",
    //    headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    //}).then(function mySucces(response) {
    //    $scope.projects = response.data;
    //    var all = { id: "", name: "All projects" };
    //    $scope.projects.push(all);
    //    //$scope.$apply();
        $http({
            method: "GET",
            url: "/Backlog/GetCurrentProject",
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function mySucces(response) {
            $scope.currentProject = response.data;
            //if ($scope.currentProject == 0) {
            //    $scope.currentName = "All projects";
            //} else {
            //    for (var i = 0; i < $scope.projects.length; i++) {
            //        if ($scope.projects[i].id == $scope.currentProject) {
            //            $scope.currentName = $scope.projects[i].name;
            //        }
            //    }
            //}
        }, function myError(response) {
        })

    //}, function myError(response) {
    //})

    //$http({
    //    method: "GET",
    //    url: "/Backlog/GetCurrentProject",
    //    headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    //}).then(function mySucces(response) {
    //    $scope.currentProject = response.data;
    //    for (var i in $scope.projects)
    //    {
    //        if (i.id == $scope.currentProject)
    //        {
    //            $scope.currentName = i.name;
    //        }
    //    }
    //}, function myError(response) {
    //})

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
            for (var i = 0; i < $scope.tasksUS.length; ++i) {
                $scope.tasksUS[i].Importance = $scope.importance[$scope.tasksUS[i].Importance];
            }

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