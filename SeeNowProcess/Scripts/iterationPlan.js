var iterationApp = angular.module("iterationPart", ['dndLists']);
iterationApp.controller("iterationCtrl", ['$scope', '$http', function ($scope, $http) {
    $scope.message = '';
    $scope.iterations = [];
    $scope.lists = [];
    $scope.selected = null;
    $scope.startDate = '';
    $scope.endDate = '';
    $scope.startingDate;
    $scope.endingDate;
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
                    //window.location.href = "/IterationPlan/Index";
                    $scope.message = "Task moved";
            }, function myError(response) {
                     $scope.message = "Error in moving task";
            })



            return item;
        }

        $scope.addIteration = function () {
            $scope.startingDate = $scope.startDate.getFullYear().toString() + "-" + ($scope.startDate.getMonth()+1).toString() + "-" + $scope.startDate.getDay().toString();
            $scope.endingDate = $scope.endDate.getFullYear().toString() + "-" + ($scope.endDate.getMonth()+1).toString() + "-" + $scope.endDate.getDay().toString();
            $http({
                method: "POST",
                url: "/IterationPlan/AddingIteration",
                data: $.param({ "name": $scope.iterationName, "description": $scope.iterationDescription, "startDate": $scope.startingDate, "endDate": $scope.endingDate}),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                $scope.message = "Result: " + response.data;
                if (response.data == "Success")
                    window.location.href = "/IterationPlan/IterationPlanIndex";
            }, function myError(response) {
            })


        }
}]);

iterationApp.controller("taskCtrl", function ($scope,$http) {

    /* Roksana:
    1. Nie dodaję na razie "Choose an option" które dodała Ania, zostaje puste pole które po wyborze opcji
    znika z listy wyboru. Nie wiem na razie czy to dobry pomysł, zależy czy to będzie obowiązkowe pole (?)
    2. Chyba niepotrzebnie dodałam to jako obiekty, ale proszę o weryfikację */

    $scope.response=[];
    $scope.parent_options =[];

    $http({
        method: "GET",
        url: "/Add/GetStories",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.response = response.data.stories;
        for (var i=0; i<$scope.response.length; ++i) {
            $scope.parent_options.push({ value: $scope.response[i].UserStory, description: $scope.response[i].UserStory })
        }

    }, function myError(response) {
        $scope.message = "Error";
    })



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
        $http({
            method: "POST",
            url: "/Add/IndexAdd",
            data: $.param({
                'title': $scope.title, 'description': $scope.description,
                'status': $scope.status.value, 'importance': $scope.importance.value,
                'estimatedTime': $scope.estimated_time, 'userStory': $scope.parent.value
            }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(response) {
            $scope.message = "Did it!";
            window.location.href = "/IterationPlan/Index";
        },
        function failure(response)
        {
            $scope.message = "Fail...";
        });
    }
});