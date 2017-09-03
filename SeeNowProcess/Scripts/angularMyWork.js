var myWorkAngular = angular.module("dragAndDrop", ['dndLists']);
myWorkAngular.controller("dragging", ['$scope', '$http', function ($scope, $http) {
//angular.module("dragAndDrop", ['dndLists']).controller("dragging", function ($scope, $http) {

    //$http({
    //    method: "GET",
    //    url: "/MyWork/GetCurrentUser",
    //    headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    //}).then(function success(result) {
    //    $scope.user = result.data;
    //}).catch(function fail(result) { });
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
            //za pierwszym razem wysyła zawsze, gdy nie mamy ustalonego jeszcze currentProject - dlatego są błędy. 
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
            //$scope.models.lists.A = result.data;
            $scope.wholeList = result.data;

            for (var x = 0; x < $scope.wholeList.length; x++) {
                //switch ($scope.allProblemsList[x].Box.Order) { //moja propozycja (Mikolaj)
                //nie dziala, moze nie da sie odwolac do tak zagnizdzonego pola... :c konsola na 
                //przegladarce wywala ze nie znapola "Order"

                for (var j = 0; j < $scope.lists.length; j++) {
                    if ($scope.wholeList[x].BoxOrder == $scope.lists[j].Order) {
                        $scope.lists[j].Tasks.push($scope.wholeList[x]);
                    }
                }
                //switch ($scope.wholeList[x].BoxOrder) {
                //    case 0:
                //        $scope.models.lists.A.push($scope.wholeList[x]);
                //        break;
                //    case 1:
                //        $scope.models.lists.B.push($scope.wholeList[x]);
                //        break;
                //    case 2:
                //        $scope.models.lists.C.push($scope.wholeList[x]);
                //        break;
                //    case 3:
                //        $scope.models.lists.D.push($scope.wholeList[x]);
                //        break;
                //    case 4:
                //        $scope.models.lists.E.push($scope.wholeList[x]);
                //        break;
                //    case 5:
                //    default:
                //        $scope.models.lists.F.push($scope.wholeList[x]);
                //        break;
                //}
            }


        }).catch(function fail(result) {
            $scope.list = ":(";
        })
    }).catch(function fail(result) {
        $scope.boxOrder = ":(";
    })




    $scope.wholeList = [];

    $scope.addTask = function (title, description) {
        $scope.models.lists.A.push({ title, description});
    };


}]);