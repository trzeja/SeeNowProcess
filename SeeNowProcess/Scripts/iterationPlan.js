var iterationApp = angular.module("iterationPart", ['dndLists']);
iterationApp.controller("iterationCtrl", ['$scope', '$http', function ($scope, $http) {
    $scope.message = 'Cos';
    $scope.iterations = [];
    $scope.lists = [];
    $scope.selected = null;
  /*  $scope.addIteration = function () {
        $http({
            method: "POST",
            url: "/IterationPlan/AddingIteration",
            data: $.param({ "name": iterations.Name }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

        }).then(function mySucces(response) {
            lists.push = response.data;
        }, function myError(response) {
            $scope.message = "Error";
        })
    }*/
       /* $scope.iterations.push({ "Name": "Backlog", "StartDate": "dhefuieff", "EndDate": "jeoefof;rg" });
        $scope.iterations.push({ "Name": "Backlog 2", "StartDate": "dhefuiesgfhff", "EndDate": "jeoeffhgfhfof;rg" });
        $scope.iterations.push({ "Name": "Backlog 2", "StartDate": "dhefuiesgfhff", "EndDate": "jeoeffhgfhfof;rg" });
        $scope.iterations.push({ "Name": "Backlog 2", "StartDate": "dhefuiesgfhff", "EndDate": "jeoeffhgfhfof;rg" });
        for (var i = 0; i < $scope.iterations.length; ++i) {
            var lista_pomocnicza = [];
            lista_pomocnicza.push({ Title: "Sierotka Marysia", Description: "Whatever" });
            lista_pomocnicza.push({ Title: "Sierotka Marysia2", Description: "Whatever Happens" });
            $scope.lists.push(lista_pomocnicza);
           // $scope.lists.i.push({ Title: "Sierotka Marysia", Description: "Whatever" });
        }*/
        /*$http({
            method: "GET",
            url: "/IterationPlan/GetNumber",
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function mySucces(response) {
            $scope.message = response.data;
            $scope.iterations = response.data;
            for (var i = 0; i < $scope.iterations.length; ++i) {
                var id = $scope.iterations[i].id;
                $http({
                    method: "POST",
                    url: "/IterationPlan/GetIteration",
                    data: $.param({ "id": $scope.iterations[i].id }),
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

                }).then(function mySucces(response) {
                    var object = { "id":/* $scope.iterations[i].id i, "data": response.data };
                    $scope.lists.push(object);
                  //  $scope.message = response.data;
                }, function myError(response) {
                    $scope.message = "Error";
                })
            }
        }, function myError(response) {
            $scope.message = "Error";
        })*/
        
    
    $http({
        method: "GET",
        url: "/IterationPlan/GetNumber",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.iterations = response.data;

        $http({
            method: "GET",
            url: "/IterationPlan/GetAllIterations",
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function mySuccess(response) {
            var list = response.data;
            for (var i = 0; i < $scope.iterations.length; ++i) {
                var object = { "id": $scope.iterations[i].id, "data": [] };
                $scope.lists.push(object);
            }
            for (var j = 0; j < list.length; ++j) {
                for (var i = 0; i < $scope.lists.length; ++i) {
                    if (list[j].id_iteracji == $scope.lists[i].id) {
                        $scope.lists[i].data.push(list[j]);
                    }
                }
            }
        }, function myError(response) {
            $scope.message = "Error All";
        });
        }, function myError(response) {
            $scope.message = "Error";
        })

        $scope.dropCallback = function (item, x) {
            $scope.message = "DropCallback: " + x;
            $http({
                method: "POST",
                url: "/IterationPlan/MoveTask",
                data: $.param({ "taskId": item.id, "newIterationId": x}),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                $scope.message = "Result: " + response.data;
                if (response.data == "Success")
                    window.location.href = "/IterationPlan/Index";
            }, function myError(response) {
                     $scope.message = "Error in moving task";
            })



            return item;
        }

        $scope.addIteration = function () {
            $http({
                method: "POST",
                url: "/IterationPlan/AddingIteration",
                data: $.param({ "name": $scope.iterationName, "description": $scope.iterationDescription, "startDate": $scope.startDate, "endDate": $scope.endDate}),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                $scope.message = "Result: " + response.data;
                if (response.data == "Success")
                    window.location.href = "/IterationPlan/Index";
            }, function myError(response) {
           //     $scope.message = "Error in adding iteration";
            })


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