var WorkApp = angular.module("WorkByPersonPart", ['dndLists'])
WorkApp.controller("WorkByPersonCtrl", function ($scope, $http) {
    $scope.message = "DZIALAAA";
    $scope.show = 0;
    $scope.currentProject;
    $scope.currentName;
    $scope.list = [];
    $scope.lists = [];
    $scope.projects;
    $scope.showModal = false;

    $scope.$watch('currentProject', function () {
        if ($scope.show != 0) {
            $http({
                method: "POST",
                url: "/TaskBoard/ChangeCurrentProject",
                data: $.param({ id: $scope.currentProject }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
            }).then(function mySucces(response) {
                if (response.data == "Change") {
                    window.location.href = "/WorkByPerson/WorkByPersonIndex";
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
                                        var task = {Id: $scope.problems[i].Id, Title: $scope.problems[i].Title, Description: $scope.problems[i].Description, projectID: $scope.problems[i].ProjectID, UserId: $scope.lists[j].id};
                                        $scope.lists[j].boxes[k].tasks.push(task);
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
                $scope.problems = "Fail";
            })
        }).catch(function fail(result) {
            $scope.users = "Fail";
        })
    }).catch(function fail(result) {
        $scope.boxes = "Fail";
    })

    $scope.dropCallback = function (item, newUser, box) { //userstory->id, box->order
        //w item mamy jaki był poprzednio - w number jaki ma byc nowy
        $http({
            method: "POST",
            url: "/WorkByPerson/UpdateDatabase",
            data: $.param({//int problemID, int oldUserID, int newUserID, int newBoxOrder
                'problemID': item.Id, 'oldUserID': item.UserId, 'newUserID': newUser, 'newBoxOrder': box, 'projectID': item.projectID
            }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(response) {
            if (response.data.error == true) {
                $scope.showModal = true;
                $scope.message = response.data.result;
            }
            //item.BoxID = box;
            item.UserId = newUser;
            $scope.hello = ":)";
            $http({
                method: "GET",
                url: "/WorkByPerson/GetBoxes",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
            }).then(function success(result) {
                //pobrac tu trzeba tez do ktorego nalezy 
                $scope.boxes = [];
                $scope.boxes = result.data;
                $http({
                    method: "GET",
                    url: "/WorkByPerson/GetUsers",
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
                }).then(function success(result) {
                    $scope.lists = [];
                    $scope.users = result.data;
                    for (var i = 0; i < $scope.users.length; i++) {
                        var user = { id: $scope.users[i].UserID, name: $scope.users[i].Name, boxes: [] };
                        $scope.lists.push(user);
                    }
                    $http({
                        method: "GET",
                        url: "/WorkByPerson/GetProblems",
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
                    }).then(function success(result) {
                        $scope.problems = result.data;
                        $scope.list = [];
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
                                    if ($scope.problems[i].AssignedUsers[k] == $scope.lists[j].id) {
                                        for (var k = 0; k < $scope.lists[j].boxes.length; k++) {
                                            if ($scope.problems[i].BoxOrder == $scope.lists[j].boxes[k].order) //czy w tym boxie
                                            {
                                                var task = { Id: $scope.problems[i].Id, Title: $scope.problems[i].Title, Description: $scope.problems[i].Description, projectID: $scope.problems[i].ProjectID, UserId: $scope.lists[j].id };
                                                $scope.lists[j].boxes[k].tasks.push(task);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        var ok = 0;
                        $scope.list = [];
                        for (var i = 0; i < $scope.lists.length; i++) {
                            var ok = 0;
                            for (var j = 0; j < $scope.boxes.length; j++) {
                                if ($scope.lists[i].boxes[j].tasks.length > 0) {
                                    ok = 1;
                                }
                            }
                            if (ok == 1)
                                $scope.list.push($scope.lists[i]);
                            ok = 0;
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
        }, function failure(response) {
            $scope.hello = ":(";
        });
        return item;
    };

});
WorkApp.directive('modal', function () {
    return {
        template: '<div class="modal fade">' +
            '<div class="modal-dialog">' +
              '<div class="modal-content">' +
                '<div class="modal-header">' +
                  '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
                  '<h4 class="modal-title">ERROR</h4>' +
                '</div>' +
                '<div class="modal-body" ng-transclude></div>'+
              '</div>' +
            '</div>' +
          '</div>',
                    restrict: 'E',
                    transclude: true,
                    replace: true,
                    scope: true,
                    link: function postLink(scope, element, attrs) {
                        scope.$watch(attrs.visible, function (value) {
                            if (value == true)
                                $(element).modal('show');
                            else
                                $(element).modal('hide');
                        });

                        $(element).on('shown.bs.modal', function () {
                            scope.$apply(function () {
                                scope.$parent[attrs.visible] = true;
                            });
                        });

                        $(element).on('hidden.bs.modal', function () {
                            scope.$apply(function () {
                                scope.$parent[attrs.visible] = false;
                            });
                        });
                    }
                };
});