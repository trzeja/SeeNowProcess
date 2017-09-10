var taskBoardAngular = angular.module("taskBoardPart", ['dndLists']);
taskBoardAngular.filter('trustAsHtml', ['$sce', function ($sce) {
    return function (text) {
        return $sce.trustAsHtml(text);
    };
}]);
taskBoardAngular.controller("taskBoardCtrl", ['$scope', '$http', function ($scope, $http) {
    $scope.message = "";
    $scope.show = 0;
    $scope.currentProject;
    $scope.projects;
    $scope.$watch('currentProject', function () {
        if ($scope.show != 0) {
            $http({
                method: "POST",
                url: "/TaskBoard/ChangeCurrentProject",
                data: $.param({ id: $scope.currentProject }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
            }).then(function mySucces(response) {
                if (response.data == "Change") {
                    window.location.href = "/TaskBoard/TaskBoardIndex";
                }
            }, function myError(response) {
            })

        } else {
            $scope.show = 1;
        }

    });

    $http({
        method: "GET",
        url: "/TaskBoard/GetCurrentProject",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.currentProject = response.data;
        $http({
            method: "GET",
            url: "/TaskBoard/GetBoxes",
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(result) {
            //pobrac tu trzeba tez do ktorego nalezy 
            $scope.boxes = result.data;
            $http({
                method: "GET",
                url: "/TaskBoard/GetUserStories",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
            }).then(function success(result) {
                $scope.userstories = result.data;

                $http({
                    method: "GET",
                    url: "/TaskBoard/GetProblems",
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
                }).then(function success(result) {
                    $scope.problems = result.data;
                    $scope.list = [];
                    //tutaj rozdzial na listy
                    for (var i = 0; i < $scope.userstories.length; i++) {
                        var userstory;
                        if ($scope.currentProject == 0) {
                            userstory = { id: $scope.userstories[i].UserSID, title: $scope.userstories[i].Title + "<br/><br/>" + $scope.userstories[i].ProjectName, boxes: [] };
                            $scope.list.push(userstory);
                        }
                        else {
                            userstory = { id: $scope.userstories[i].UserSID, title: $scope.userstories[i].Title, boxes: [] };
                            $scope.list.push(userstory);
                        }
                        for (var j = 0; j < $scope.boxes.length; j++) {
                            var box = { order: $scope.boxes[j].BoxOrder, name: $scope.boxes[j].Name, tasks: [] };
                            $scope.list[i].boxes.push(box);
                        }
                    }
                    for (var i = 0; i < $scope.problems.length; i++) {
                        for (var j = 0; j < $scope.list.length; j++) {
                            if ($scope.problems[i].UserSID == $scope.list[j].id) //czy jest w tym userstory
                            {
                                for (var k = 0; k < $scope.list[j].boxes.length; k++) {
                                    if ($scope.problems[i].BoxOrder == $scope.list[j].boxes[k].order) //czy w tym boxie
                                    {
                                        $scope.list[j].boxes[k].tasks.push($scope.problems[i]);
                                    }
                                }
                            }
                        }
                    }
                }).catch(function fail(result) {
                    $scope.problems = ":(";
                })
            }).catch(function fail(result) {
                $scope.userstories = ":(";
            })
        }).catch(function fail(result) {
            $scope.boxes = ":(";
        })
    }, function myError(response) {
    })

    

    $scope.dropCallback = function (item, userstory, box) { //userstory->id, box->order
        //w item mamy jaki był poprzednio - w number jaki ma byc nowy
        $http({
            method: "POST",
            url: "/TaskBoard/UpdateDatabase",
            data: $.param({
                'problemID': item.Id, 'newUserStoryID': userstory, 'newBoxOrder': box, 'newProjectId': item.Project
            }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(response) {
            $scope.hello = ":)";
            item.BoxID = response.data[0];
        }, function failure(response) {
            $scope.hello = ":(";
        });

        item.UserSID = userstory;
        return item;
    };
}]);