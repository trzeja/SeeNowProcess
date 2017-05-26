var peopleApp = angular.module("peoplePart", []);
peopleApp.controller("peopleCtrl", ['$scope', '$http', function ($scope, $http) {
    $scope.message = '';
    $scope.iterations = [];
    $scope.lists = [];
    $scope.selected = null;
    $scope.userName = 'Select user to show content.';
    $scope.currentUser;
    $scope.info;

    
    $http({
        method: "GET",
        url: "/People/AllPeople",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.lists = response.data;
    }, function myError(response) {
        $scope.message = "Error";
    })

    $scope.showUser = function (ID, name) {
        /*$scope.message = ID;*/
        $scope.userName = name;
        $http({
            method: "POST",
            url: "/People/GetUserInfo",
            data: $.param({"id": ID}),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

        }).then(function mySucces(response) {
            $scope.currentUser = response.data[0];
            switch ($scope.currentUser.role) {
                case 0:
                    $scope.currentUser.role = "Admin";
                    break;
                case 1:
                    $scope.currentUser.role = "Head Master";
                    break;
                case 2:
                    $scope.currentUser.role = "Senior Developer";
                    break;
                case 3:
                    $scope.currentUser.role = "Junior Developer";
                    break;
                case 4:
                    $scope.currentUser.role = "Intern";
                    break;
                case 5:
                    $scope.currentUser.role = "Tester";
                    break;
                case 6:
                    $scope.currentUser.role = "Client";
                    break;
            }
            $scope.message = $scope.currentUser.id;
            $scope.info = {e: "Email: ", n: "Phone Number: ", r: "Role: "};
            
        }, function myError(response) {
            $scope.message = "Error taking User";
        })
    }

    $scope.dropCallback = function (item, x) {
        $scope.message = "DropCallback: " + x;
        $http({
            method: "POST",
            url: "/IterationPlan/MoveTask",
            data: $.param({ "taskId": item.id, "newIterationId": x }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

        }).then(function mySucces(response) {
            $scope.message = "Result: " + response.data;
            if (response.data == "Success")
                //window.location.href = "/IterationPlan/Index";
                $scope.message = "Task moved";
        }, function myError(response) {
            $scope.message = "Error in moving task";
        })



        return item;
    }

    $scope.addIteration = function () {
        $http({
            method: "POST",
            url: "/IterationPlan/AddingIteration",
            data: $.param({ "name": $scope.iterationName, "description": $scope.iterationDescription, "startDate": $scope.startDate, "endDate": $scope.endDate }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

        }).then(function mySucces(response) {
            $scope.message = "Result: " + response.data;
            if (response.data == "Success")
                window.location.href = "/IterationPlan/Index";
        }, function myError(response) {
        })


    }
}]);