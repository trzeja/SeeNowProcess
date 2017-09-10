var backlogAngular = angular.module("backlog", []);
backlogAngular.controller("backlogCtrl", ['$scope', '$http', function ($scope, $http) {
    $scope.importance = ["None", "Trivial", "Regular", "Important", "Critical"];
    $scope.userstory = "Select user story to show content";
    $scope.userStoryId;
    $scope.currentUS;
    $scope.tasksUS;
    $scope.info;
    $scope.selected_teams = [];
    $scope.userStoriesProjects_options = [];
    $scope.userStoriesTeams = [];
    $scope.userStoriesOwnerOptions = [];

    $scope.show = 0;
    $scope.currentProject;
    $scope.currentProjectName;
    $scope.lock = false;

    $http({
        method: "GET",
        url: "/Add/AllPeople",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySuccess(response) {
        $scope.responseOwner = response.data;
        for (var i = 0; i < $scope.responseOwner.length; i++) {
            $scope.userStoriesOwnerOptions.push({ id: $scope.responseOwner[i].id, name: $scope.responseOwner[i].NAME });
        }
    }, function myError(response) {
        $scope.message = "Error";
    })

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
        }

    });

        $http({
            method: "GET",
            url: "/Backlog/GetCurrentProject",
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function mySucces(response) {
            $scope.currentProject = response.data;
        }, function myError(response) {
        })

        $http({
            method: "GET",
            url: "/Backlog/GetCurrentProjectName",
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function mySuccess(response) {
            $scope.currentProjectName = response.data;
        }, function myError(response) {
        })

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
        $scope.userStoryId = id;
        
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

    $scope.deleteTask = function (TaskID) {
        $http({
            method: "POST",
            url: "/Backlog/DeleteTask",
            data: $.param({"taskId": TaskID}),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

        }).then(function mySucces(response) {
            $scope.message = response.data;
            $scope.showUserStory($scope.userStoryId, $scope.userstory)
        }, function myError(response) {
            $scope.message = "Error deleting Task";
        })
    }


    //ADD USERSTORY MODAL
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
                $scope.userStoriesTeams.push({ id: $scope.response[i].ID, NAME: $scope.response[i].name })
            }
        }, function myError(response) {
            $scope.message = "Error";
        })


        $scope.addUserStory = function () {
            $scope.message = "Please wait...";
            $scope.lock = true;
            if ($scope.currentProject == 0) {
                $scope.send = $scope.userStoryProject.id
            }
            else {
                $scope.send = $scope.currentProject;
            }
        $http({
            method: "POST",
            url: "/Backlog/AddUserStory",
            data: $.param({
                'title': $scope.userStoryTitle, 'description': $scope.userStoryDescription,
                'size': $scope.userStorySize, 'unit': $scope.userStoryUnit,
                'notes': $scope.userStoryNotes, 'criteria': $scope.userStoryCriteria,
                'project': $scope.send, 'teams': $scope.selected_teams,
                'owner': $scope.userStoryOwner.id
            }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(response) {
            if (response.data == "success") {
                window.location.href = "/Backlog/BacklogIndex";
            }
            else {
                $scope.lock = false;
                $scope.message = response.data;
            }
        },
        function failure(response) {
            $scope.message = "Fail...";
        });
    }
}]);