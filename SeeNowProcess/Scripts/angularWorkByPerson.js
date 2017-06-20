﻿angular.module("WorkByPersonPart", ['dndLists']).controller("WorkByPersonCtrl", function ($scope, $http) {
    $scope.message = "DZIALAAA";
    $scope.show = 0;
    $scope.currentProject;
    $scope.currentName;
    $scope.list = [];
    $scope.lists = [];
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
                    window.location.href = "/WorkByPerson/Index";
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
        url: "/WorkByPerson/GetProjects",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.projects = response.data;
        $http({
            method: "GET",
            url: "/WorkByPerson/GetCurrentProject",
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function mySucces(response) {
            $scope.currentProject = response.data;
            for (var i = 0; i < $scope.projects.length; i++) {
                if ($scope.projects[i].id == $scope.currentProject) {
                    $scope.currentName = $scope.projects[i].name;
                }
            }
        }, function myError(response) {
        })

    }, function myError(response) {
    })
    $http({
        method: "GET",
        url: "/WorkByPerson/GetBoxes",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function success(result) {
        //pobrac tu trzeba tez do ktorego nalezy 
        //$scope.models.lists.A = result.data;
        $scope.boxes = result.data;
        $http({
            method: "GET",
            url: "/WorkByPerson/GetUsers",
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(result) {
            //pobrac tu trzeba tez do ktorego nalezy -> nie wiem o co mi chodzilo...
            //$scope.models.lists.A = result.data;
            $scope.users = result.data;
            for (var i = 0; i < $scope.users.length; i++) {
                var user = { id: $scope.users[i].UserID, name: $scope.users[i].Name, boxes: [] };
                $scope.lists.push(user);
            }
            /*for (var i = 0; i < $scope.lists.length; i++) {
                var user = { id: $scope.users[i].UserID, name: $scope.users[i].Name, boxes: [] };
                $scope.lists.push(user);
            }*/
           $http({
                method: "GET",
                url: "/WorkByPerson/GetProblems",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
            }).then(function success(result) {
                $scope.problems = result.data;
               // $scope.list = [];
                //tutaj rozdzial na listy
                for (var i = 0; i < $scope.lists.length; i++) {
                    for (var j = 0; j < $scope.boxes.length; j++) {
                        var box = { order: $scope.boxes[j].BoxOrder, name: $scope.boxes[j].Name, tasks: [] };
                        $scope.lists[i].boxes.push(box);
                    }
                }
                for (var i = 0; i < $scope.problems.length; i++) {
                    for (var j = 0; j < $scope.lists.length; j++) {
                        for (var k = 0; k < $scope.problems[i].AssignedUsers.length; k++) {
                            if ($scope.problems[i].AssignedUsers[k]== $scope.lists[j].id) {
                                for (var k = 0; k < $scope.lists[j].boxes.length; k++) {
                                    if ($scope.problems[i].BoxOrder == $scope.lists[j].boxes[k].order) //czy w tym boxie
                                    {
                                        var task = {Id: $scope.problems[i].Id, Title: $scope.problems[i].Title, Description: $scope.problems[i].Description, UserId: $scope.lists[j].id};
                                        $scope.lists[i].boxes[k].tasks.push(task);
                                        //$scope.lists[j].boxes[k].tasks.push({$scope.problems[i]});
                                    }
                                }
                            }
                        }
                        /*if ($scope.problems[i].UserSID == $scope.list[j].id) //czy jest w tym userstory
                        {
                            for (var k = 0; k < $scope.list[j].boxes.length; k++) {
                                if ($scope.problems[i].BoxOrder == $scope.list[j].boxes[k].order) //czy w tym boxie
                                {
                                    $scope.list[j].boxes[k].tasks.push($scope.problems[i]);
                                }
                            }
                        }*/
                    }
                }
                var ok = 0;
                for (var i = 0; i < $scope.lists.length; i++) {
                    var ok = 0;
                    for (var j = 0; j <$scope.boxes.length; j++) {
                        if ($scope.lists[i].boxes[j].tasks.length > 0) {
                            ok = 1;
                        }
                    }
                    if (ok == 1)
                        $scope.list.push($scope.lists[i]);
                    ok = 0;
                        //if ($scope.lists[i].boxes[j].tasks)
                    //}
                }
            }).catch(function fail(result) {
                $scope.problems = ":(";
            })
        }).catch(function fail(result) {
            $scope.users = ":(";
        })
    }).catch(function fail(result) {
        $scope.boxes = ":(";
    })

    $scope.dropCallback = function (item, oldUser, newUser, box) { //userstory->id, box->order
        //w item mamy jaki był poprzednio - w number jaki ma byc nowy
        $http({
            method: "POST",
            url: "/WorkByPerson/UpdateDatabase",
            data: $.param({//int problemID, int oldUserID, int newUserID, int newBoxOrder
                'problemID': item.Id, 'oldUserID': oldUser, 'newUserID': newUser, 'newBoxOrder': box
            }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(response) {
            $scope.hello = ":)";
        }, function failure(response) {
            $scope.hello = ":(";
        });
        //item.BoxID = number;
        //tu problem ze zmienianiem boxid a nie boxorder
        return item;
    };

});