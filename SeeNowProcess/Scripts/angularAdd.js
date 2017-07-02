var taskApp = angular.module("taskForm", []);
taskApp.controller("taskCtrl", function ($scope,$http) {
    $scope.tasks = [];

    $scope.response = [];
    $scope.parent_options = [];
    $scope.users = [];
    $scope.selected_users = [];

    $http({
        method: "GET",
        url: "/Add/GetStories",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.response = response.data.stories;
        for (var i = 0; i < $scope.response.length; ++i) {
            $scope.parent_options.push({ id: $scope.response[i].id, value: $scope.response[i].UserStory })
        }

    }, function myError(response) {
        $scope.message = "Error";
    })

    $http({
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
    })
    
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

    $scope.addTask = function () {
        /*$scope.tasks.push({
            title: $scope.title, description: $scope.description, status: $scope.status.value,
            importance: $scope.importance.value, estimated_time: $scope.estimated_time, parent: $scope.parent.value
        });*/
        $http({
            method: "POST",
            url: "/Add/IndexAdd",
            data: $.param({
                'title': $scope.title, 'description': $scope.description,
                'status': $scope.status.value, 'importance': $scope.importance.value,
                'estimatedTime': $scope.estimated_time, 'userStory': $scope.parent.id,
                'users': $scope.selected_users
            }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(response) {
            $scope.message = "Did it!";
        },
        function failure(response)
        {
            $scope.message = "Fail...";
        });
    }
});



 