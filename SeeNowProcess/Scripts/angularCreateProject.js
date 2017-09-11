var projectApp = angular.module("projectForm", []);
projectApp.controller("projectCtrl", function ($scope, $http) {
    $scope.endDate = null;
    $scope.lock = false;
    $scope.status_options =
        [
            { value: "open", description: "Open" },
            { value: "suspended", description: "Suspended" },
            { value: "closed", description: "Closed" }
        ];
   

    $scope.addProject = function () {
        $scope.message = "Please wait...";
        $scope.lock = true;
        if ($scope.endDate != null) {
            $scope.endingDate = $scope.endDate.getFullYear().toString() + "-" + ($scope.endDate.getMonth() + 1).toString() + "-" + $scope.endDate.getDate().toString();
        }
        else {
            $scope. endingDate = null
        }
        $http({
            method: "POST",
            url: "/Create/AddProject",
            data: $.param({
                'name': $scope.title, 'description': $scope.description,
                'status': $scope.status.value, 'startDate':  $scope.startDate.getFullYear().toString() + "-" + ($scope.startDate.getMonth()+1).toString() + "-" + $scope.startDate.getDate().toString(),
                'completionDate': $scope.endingDate
            }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(response) {
            window.location.href = "/Home/Index";
           // $scope.message = "Did it!";
        },
        function failure(response) {
            $scope.message = "Fail...";
            $scope.lock = false;
        });
    }
});



