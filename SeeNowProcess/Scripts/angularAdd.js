var taskApp = angular.module("taskForm", []);
taskApp.directive('timeRest', function () {
    return {
        require: "ngModel",
        link: function (scope, element, attr, mCtrl, ngModel) {
            function validateTime(value) {
                if (value < 1) {
                    mCtrl.$setValidity('userSel', true);
                }
                else {
                    mCtrl.$setValidity('userSel', false);
                }
                return value;
            }
            mCtrl.$parsers.push(validateTime);
        }
    };
});
taskApp.controller("taskCtrl", function ($scope,$http) {
    $scope.tasks = [];
    $scope.lock = false;
    $scope.response = [];
    $scope.parent_options = [];
    $scope.project_options = [];
    $scope.users = [];
    $scope.selected_users = [];
    $scope.all_parent_options = [];
    $scope.team_options = [];
    $scope.noProject = true;
    $scope.noUserStory = true;

    $http({
        method: "GET",
        url: "/Add/GetStories",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.response = response.data.stories;
        for (var i = 0; i < $scope.response.length; ++i) {
            $scope.all_parent_options.push({ id: $scope.response[i].id, value: $scope.response[i].UserStory, project_id: $scope.response[i].project_id })
            $scope.parent_options.push({ id: $scope.response[i].id, value: $scope.response[i].UserStory, project_id: $scope.response[i].project_id })
        }

    }, function myError(response) {
        $scope.message = "Error";
    })

    $http({
        method: "GET",
        url: "/Add/GetProjects",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.response = response.data;
        for (var i = 0; i < $scope.response.length; ++i) {
            $scope.project_options.push({ id: $scope.response[i].id, value: $scope.response[i].name })
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

    $scope.status_options =
        [
            { value: "open", description: "Open" },
            { value: "suspended", description: "Suspended" },
            { value: "closed", description: "Closed" }
        ];
    $scope.importance_options =
        [
             { value: "none", description: "None" },
             { value: "trivial", description: "Trivial" },
             { value: "regular", description: "Regular" },
             { value: "important", description: "Important" },
             { value: "critical", description: "Critical" }
        ];

    $scope.getUserStories = function (id) {
        $scope.noProject = false;
        $scope.parent_options = [];
        for (var i = 0; i < $scope.all_parent_options.length; ++i) {
            if ($scope.all_parent_options[i].project_id == id)
                $scope.parent_options.push({ id: $scope.all_parent_options[i].id, value: $scope.all_parent_options[i].value, project_id: $scope.all_parent_options[i].project_id})
        }
        
    }

    $scope.getTeamsForProject = function (id) {
        $scope.noUserStory = false;
        $http({
            method: "GET",
            url: "/Add/GetTeams?userStoryId="+id,
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function mySucces(response) {
            $scope.response = response.data.teams;
            $scope.team_options = [];
            for (var i = 0; i < $scope.response.length; ++i) {
                $scope.team_options.push({ id: $scope.response[i].id, NAME: $scope.response[i].name })
            }

        }, function myError(response) {
            $scope.message = "Error";
        })
    }

    $scope.getUsersForProject = function (id) {
        $http({
            method: "GET",
            url: "/Add/GetUsersFromTeam?teamId="+id,
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function mySucces(response) {
            $scope.response = response.data;
            $scope.users = [];
            for (var i = 0; i < $scope.response.length; ++i) {
                $scope.users.push({ id: $scope.response[i].id, NAME: $scope.response[i].NAME })
            }

        }, function myError(response) {
            $scope.message = "Error";
        })
    }

    $scope.addTask = function () {
        $scope.message = "Please wait...";
        $scope.lock = true;
        $scope.progress = 0;
        $http({
            method: "POST",
            url: "/Add/IndexAdd",
            data: $.param({
                'title': $scope.title, 'description': $scope.description,
                'importance': $scope.importance.value, 'progress': $scope.progress,
                'estimatedTime': $scope.estimated_time, 'userStory': $scope.parent.value,
                'users': $scope.selected_users
            }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(response) {
            if (response.data == "success") {
                window.location.href = "/TaskBoard/TaskBoardIndex";
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



 