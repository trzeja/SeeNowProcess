var taskApp = angular.module("taskForm", []);
taskApp.directive('selUsers', function () {
    return {
        require: "ngModel",
        link: function (scope, element, attr, mCtrl, ngModel) {
            function validatePeople(value) {
                if (value.length < 1) {
                    mCtrl.$setValidity('userSel', true);
                }
                else {
                    mCtrl.$setValidity('userSel', false);
                }
                return value;
            }
            mCtrl.$parsers.push(validatePeople);
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
    $scope.isDisabled = true;

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

    /*$http({
        method: "GET",
        url: "/Add/AllPeople",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.response = response.data;
        for (var i = 0; i < $scope.response.length; ++i) {
            $scope.users.push({ id: $scope.response[i].id, NAME: $scope.response[i].NAME })
        }

    }, function myError(response) {
        $scope.message = "Error";
    })*/
    
    $scope.checkUser = function (id) {
        //$scope.users.splice(0, $scope.users.length);
        //$scope.user.roles.push(1);
        if ($scope.selected_users.indexOf(id) === -1) {
            $scope.selected_users.push(id);
        }
        else {
            $scope.selected_users.splice($scope.selected_users.indexOf(id), 1);
        }
       // $scope.selected_users.push(id);
    };


    /* Roksana:
    1. Nie dodaję na razie "Choose an option" które dodała Ania, zostaje puste pole które po wyborze opcji
    znika z listy wyboru. Nie wiem na razie czy to dobry pomysł, zależy czy to będzie obowiązkowe pole (?)
    2. Chyba niepotrzebnie dodałam to jako obiekty, ale proszę o weryfikację */
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
        $scope.parent_options = [];
        $scope.isDisabled = false;
        for (var i = 0; i < $scope.all_parent_options.length; ++i) {
            if ($scope.all_parent_options[i].project_id == id)
                $scope.parent_options.push({ id: $scope.all_parent_options[i].id, value: $scope.all_parent_options[i].value, project_id: $scope.all_parent_options[i].project_id})
        }
        
    }

    $scope.getTeamsForProject = function (id) {
        $http({
            method: "GET",
            url: "/Add/GetTeams?userStoryId="+id,
            //data: $.param({'userStoryId': id}),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function mySucces(response) {
            $scope.response = response.data.teams;
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
            for (var i = 0; i < $scope.response.length; ++i) {
                $scope.users.push({ id: $scope.response[i].id, NAME: $scope.response[i].NAME })
            }

        }, function myError(response) {
            $scope.message = "Error";
        })
    }

    $scope.addTask = function () {
        /*$scope.tasks.push({
            title: $scope.title, description: $scope.description, status: $scope.status.value,
            importance: $scope.importance.value, estimated_time: $scope.estimated_time, parent: $scope.parent.value
        });*/
        $scope.message = "Please wait...";
        $scope.lock = true;
        $http({
            method: "POST",
            url: "/Add/IndexAdd",
            data: $.param({
                'title': $scope.title, 'description': $scope.description,
                'status': $scope.status.value, 'importance': $scope.importance.value,
                'estimatedTime': $scope.estimated_time, 'userStory': $scope.parent.value,
                'users': $scope.selected_users
            }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(response) {
            if (response.data == "success") {
                //$scope.message = "Did it!";
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



 