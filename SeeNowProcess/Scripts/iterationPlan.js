var iterationApp = angular.module("iterationPart", ['dndLists']);
iterationApp.controller("iterationCtrl", ['$scope', '$http', function ($scope, $http) {
    $scope.message = "Cos nie dziala nie wiem co...";
    $scope.iterations = [];
    $scope.lists = [];
    $scope.selected = null;

    $scope.addIteration = function () {

    }
        $scope.iterations.push({ "Name": "Backlog", "StartDate": "dhefuieff", "EndDate": "jeoefof;rg" });
        for (var i = 0; i < $scope.iterations.length; ++i) {
            var lista_pomocnicza = [];
            lista_pomocnicza.push({ Title: "Sierotka Marysia", Description: "Whatever" });
            $scope.lists.push({"i": lista_pomocnicza});
           // $scope.lists.i.push({ Title: "Sierotka Marysia", Description: "Whatever" });
        }
        
        //var item = { "Title": "Sierotka Marysia", "Description": "Whatever" };
        //$scope.lists.push({Title: "Sierotka Marysia", Description: "Whatever"});
    /*
    function loadData() {
        iterations.push = { "Name": "Backlog", "StartDate": "dhefuieff", "EndDate": "jeoefof;rg" };
        var item = { "Title": "Sierotka Marysia", "Description": "Whatever" };
        lists.push = item;

        $http({
            method: "GET",
            url: "/IterationPlan/GetNumber",
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function mySucces(response) {
            iterations.push = response.data;
            getIterationsData();
        }, function myError(response) {
            $scope.message = "Error";
        })

        $scope.getIterationsData = function () {
            for (var i = 0; i < iterations.length() ; ++i) {
                $http({
                    method: "POST",
                    url: "/IterationPlan/GetIteration",
                    data: $.param({ "name": iterations.Name }),
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

                }).then(function mySucces(response) {
                    lists.push = response.data;
                }, function myError(response) {
                    $scope.message = "Error";
                })
            }


        }
    }

    loadData();*/
        

}]);