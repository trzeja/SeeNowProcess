var projectApp = angular.module("projectForm", []);
projectApp.controller("projectCtrl", function ($scope, $http) {

    $scope.status =
        [
            { value: "open", description: "Open" },
            { value: "suspended", description: "Suspended" },
            { value: "closed", description: "Closed" }
        ];
   

    $scope.addProject = function () {
        $http({
            method: "POST",
            url: "/Create/AddProject",
            data: $.param({
                'title': $scope.title, 'description': $scope.description,
                'status': $scope.status.value, 'startDate':  $scope.startDate.getFullYear().toString() + "-" + ($scope.startDate.getMonth()+1).toString() + "-" + $scope.startDate.getDay().toString(),
                'endDate': $scope.endDate.getFullYear().toString() + "-" + ($scope.endDate.getMonth()+1).toString() + "-" + $scope.endDate.getDay().toString()
            }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(response) {
            $scope.message = "Did it!";
        },
        function failure(response) {
            $scope.message = "Fail...";
        });
    }
});



