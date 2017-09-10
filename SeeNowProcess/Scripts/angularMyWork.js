var myWorkAngular = angular.module("dragAndDrop", ['dndLists']);
myWorkAngular.controller("dragging", ['$scope', '$http', function ($scope, $http) {
    $scope.show = 0;
    $scope.currentProject;

    $scope.$watch('currentProject', function () {
        if ($scope.show != 0) {
            $http({
                method: "POST",
                url: "/MyWork/ChangeCurrentProject",
                data: $.param({ id: $scope.currentProject }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
            }).then(function mySucces(response) {
                if (response.data == "Change") {
                    window.location.href = "/MyWork/MyWorkIndex";
                }
            }, function myError(response) {
            })

        } else {
            $scope.show = 1;
        }
    });

    $http({
        method: "GET",
        url: "/MyWork/GetCurrentProject",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.currentProject = response.data;
    }, function myError(response) {
    })

    $scope.dropCallback = function (item, number) {
        //w item mamy jaki był poprzednio - w number jaki ma byc nowy
        $http({
            method: "POST",
            url: "/MyWork/UpdateDatabase",
            data: $.param({
                'ProblemID': item.ProblemID, 'NewState': number
            }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(response) { }, function failure(response) { });
        item.BoxID = number;
        return item;
    };


    $http({
        method: "GET",
        url: "/MyWork/GetBoxOrder",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    })
    .then(function success(result) {
        $scope.boxOrder = result.data;

        //utworzenie listy na przechowywanie taskow
        $scope.lists = [];
        for (var i = 0; i < $scope.boxOrder.length; i++) {
            $scope.lists.push({ Order: $scope.boxOrder[i].BoxOrder, Tasks: [] });
        }
        $http({
            method: "GET",
            url: "/MyWork/IndexJS",
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        })
        .then(function success(result) {
            //pobrac tu trzeba tez do ktorego nalezy 
            $scope.wholeList = result.data;

            for (var x = 0; x < $scope.wholeList.length; x++) {
                for (var j = 0; j < $scope.lists.length; j++) {
                    if ($scope.wholeList[x].BoxOrder == $scope.lists[j].Order) {
                        $scope.lists[j].Tasks.push($scope.wholeList[x]);
                    }
                }
            }


        }).catch(function fail(result) {
            $scope.list = "Fail";
        })
    }).catch(function fail(result) {
        $scope.boxOrder = "Fail";
    })




    $scope.wholeList = [];

    $scope.addTask = function (title, description) {
        $scope.models.lists.A.push({ title, description});
    };


}]);