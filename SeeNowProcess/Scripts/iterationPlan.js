var iterationApp = angular.module("iteration", []);
iterationApp.controller("iterationCtrl", ['dndLists','$scope', '$http', function ($scope, $http) {
    $scope.message = '';
    $scope.iterations = [];
    $scope.lists = [];
    $scope.selected = null;
    $scope.getIterations = function () {
            $http({
                method: "GET",
                url: "/IterationPlan/GetNumber",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                iterations.push = response.data;
            }, function myError(response) {
                $scope.message = "Error";
            })
    }

    $scope.getIterationsData = function () {
        for ( var i=0; i<iterations.length();++i){
            $http({
                method: "POST",
                url: "/IterationPlan/GetIteration",
                data: $.param({"name": iterations.name}),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                lists.push = response.data;
            }, function myError(response) {
                $scope.message = "Error";
            })
        }
        
        
   }
        

}]);