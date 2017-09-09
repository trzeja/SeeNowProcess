﻿var iterationApp = angular.module("iterationPart", ['dndLists']);

iterationApp.service('iterationService', function () {
    var iterationId;

    var addIterationId= function (id) {
        iterationId = id;
    };

    var getIterationId= function () {
        return iterationId;
    };

    return {
        addIterationId: addIterationId,
        getIterationId: getIterationId
    };

});

iterationApp.controller("iterationCtrl", ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope,iterationService) {
    $scope.message = '';
    $scope.iterations = [];
    $scope.lists = [];
    $scope.selected = null;
    $scope.startDate = '';
    $scope.endDate = '';
    $scope.startingDate;
    $scope.endingDate;
    //$scope.idProject;


    $scope.show = 0;
    $scope.currentProject;
    $scope.currentProjectName;

    $scope.$watch('currentProject', function () {
        if ($scope.show != 0) {
            $http({
                method: "POST",
                url: "/IterationPlan/ChangeCurrentProject",
                data: $.param({ id: $scope.currentProject }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
            }).then(function mySucces(response) {
                if (response.data == "Change") {
                    window.location.href = "/IterationPlan/IterationPlanIndex";
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
        url: "/IterationPlan/GetCurrentProject",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.currentProject = response.data;
    }, function myError(response) {
    })

    $http({
        method: "GET",
        url: "/IterationPlan/GetCurrentProjectName",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySuccess(response) {
        $scope.currentProjectName = response.data;
    }, function myError(response) {
    })

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
                var object = { "id": $scope.iterations[i].id, "name": $scope.iterations[i].name, "data": [] };
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

        $scope.getTeamsForProject = function (id) {
            $http({
                method: "GET",
                url: "/Add/GetTeams?userStoryId=" + id,
                //data: $.param({'userStoryId': id}),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
            }).then(function mySucces(response) {
                $scope.response = response.data.teams;
                for (var i = 0; i < $scope.response.length; ++i) {
                    $scope.team_options.push({ id: $scope.response[i].id, NAME: $scope.response[i].name })
                }

            }, function myError(response) {
                $scope.message = "Error";
            })
        }

        $scope.getUsersForProject = function (id) {
            $http({
                method: "GET",
                url: "/Add/GetUsersFromTeam?teamId=" + id,
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
            }).then(function mySucces(response) {
                $scope.response = response.data;
                for (var i = 0; i < $scope.response.length; ++i) {
                    $scope.users.push({ id: $scope.response[i].id, NAME: $scope.response[i].NAME })
                }

            }, function myError(response) {
                $scope.message = "Error";
            })
        }

        $scope.addIteration = function () {
            $scope.message = "Please wait...";
            $scope.lock = true;
            $scope.startingDate = $scope.startDate.getFullYear().toString() + "-" + ($scope.startDate.getMonth()+1).toString() + "-" + $scope.startDate.getDate().toString();
            $scope.endingDate = $scope.endDate.getFullYear().toString() + "-" + ($scope.endDate.getMonth() + 1).toString() + "-" + $scope.endDate.getDate().toString();
            $scope.send;
            if ($scope.currentProject == '0') {
                $scope.send = $scope.project;
            }
            else {
                $scope.send = $scope.currentProject;
            }
            $http({
                method: "POST",
                url: "/IterationPlan/AddingIteration",
                data: $.param({ "name": $scope.iterationName, "description": $scope.iterationDescription, "startDate": $scope.startingDate, "endDate": $scope.endingDate , "idProject": $scope.send}),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                //$scope.message = "Result: " + response.data;
                if (response.data == "Success") {
                    window.location.href = "/IterationPlan/IterationPlanIndex";
                }
                else {
                    $scope.lock = false;
                    $scope.message = response.data;
                }

            }, function myError(response) {
                $scope.lock = false;
                $scope.message = "Fail...";
            })


        }

        $scope.passIterationId = function (id,name) {
            //iterationService.addIterationId(id);
            $rootScope.$emit("CallMe", {id,name});
        }
}]);

iterationApp.controller("taskCtrl", ['$scope', '$http', '$rootScope', function ($scope,$http, $rootScope,iterationService) {
    $scope.tasks = [];

    $scope.status;
    $scope.title;
    $scope.description;
    $scope.response = [];
    $scope.parent_options = [];
    $scope.project_options = [];
    $scope.users = [];
    $scope.selected_users = [];
    $scope.all_parent_options = [];
    $scope.isDisabled = true;
    $scope.team_options = [];
    $scope.noUserStory = true;

    $rootScope.$on("CallMe", function(info,data) {
        $scope.itName = data.name;
        $scope.iterationId = data.id;
        $http({
            method: "POST",
            url: "/IterationPlan/GetProjectForIteration",
            data: $.param({
                'iterationId': $scope.iterationId
            }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(response) {
            $scope.project = response.data[1];
            $scope.projectID = response.data[0];
            $scope.getUserStories($scope.projectID);
        },
        function failure(response) {
           
        });
    });

    $http({
        method: "GET",
        url: "/Add/GetStories",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.response = response.data.stories;
        for (var i = 0; i < $scope.response.length; ++i) {
            $scope.all_parent_options.push({ id: $scope.response[i].id, value: $scope.response[i].UserStory, project_id: $scope.response[i].project_id })
            $scope.parent_options.push({ id: $scope.response[i].id, value: $scope.response[i].UserStory, project_id: $scope.response[i].project_id })
        }

    }, function myError(response) {
        $scope.message = "Error";
    })

    $http({
        method: "GET",
        url: "/Add/GetProjects",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.response = response.data;
        for (var i = 0; i < $scope.response.length; ++i) {
            $scope.project_options.push({ id: $scope.response[i].id, value: $scope.response[i].name })
        }
    }, function myError(response) {
        $scope.message = "Error";
    })

    $scope.checkUser = function (id) {
        //$scope.users.splice(0, $scope.users.length);
        //$scope.user.roles.push(1);
        if ($scope.selected_users.indexOf(id) === -1) {
            $scope.selected_users.push(id);
        }
        else {
            $scope.selected_users.splice($scope.selected_users.indexOf(id), 1);
        }
        // $scope.selected_users.push(id);
    };


    /* Roksana:
    1. Nie dodaję na razie "Choose an option" które dodała Ania, zostaje puste pole które po wyborze opcji
    znika z listy wyboru. Nie wiem na razie czy to dobry pomysł, zależy czy to będzie obowiązkowe pole (?)
    2. Chyba niepotrzebnie dodałam to jako obiekty, ale proszę o weryfikację */
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

    $scope.getUserStories = function (id) {
        $scope.parent_options = [];
        $scope.isDisabled = false;
        for (var i = 0; i < $scope.all_parent_options.length; ++i) {
            if ($scope.all_parent_options[i].project_id == id)
                $scope.parent_options.push({ id: $scope.all_parent_options[i].id, value: $scope.all_parent_options[i].value, project_id: $scope.all_parent_options[i].project_id })
        }
    }

    $scope.getTeamsForProject = function (id) {
        $scope.noUserStory = false;
        $http({
            method: "GET",
            url: "/Add/GetTeams?userStoryId=" + id,
            //data: $.param({'userStoryId': id}),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function mySucces(response) {
            $scope.response = response.data.teams;
            for (var i = 0; i < $scope.response.length; ++i) {
                $scope.team_options.push({ id: $scope.response[i].id, NAME: $scope.response[i].name })
            }

        }, function myError(response) {
            $scope.message = "Error";
        })
    }

    $scope.getUsersForProject = function (id) {
        $http({
            method: "GET",
            url: "/Add/GetUsersFromTeam?teamId=" + id,
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function mySucces(response) {
            $scope.response = response.data;
            $scope.users = [];
            for (var i = 0; i < $scope.response.length; ++i) {
                $scope.users.push({ id: $scope.response[i].id, NAME: $scope.response[i].NAME })
            }

        }, function myError(response) {
            $scope.message = "Error";
        })
    }

    $scope.addTask = function () {
        /*$scope.tasks.push({
            title: $scope.title, description: $scope.description, status: $scope.status.value,
            importance: $scope.importance.value, estimated_time: $scope.estimated_time, parent: $scope.parent.value
        });*/
        //var iterationId = iterationService.getIterationId()
        $scope.lock = true;
        $scope.message = "Please wait...";
        $scope.progress = 0;
        $http({
            method: "POST",
            url: "/Add/IndexAddWithIteration",
            data: $.param({
                'title': $scope.title, 'description': $scope.description,
                'importance': $scope.importance.value, 'progress': $scope.progress,
                'estimatedTime': $scope.estimated_time, 'userStory': $scope.parent.value,
                'users': $scope.selected_users, 'iterationId': $scope.iterationId
            }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(response) {
            if (response.data == "success") {
                //$scope.message = "Did it!";
                window.location.href = "/IterationPlan/IterationPlanIndex"
            }
            else {
                $scope.lock = false;
                $scope.message = response.data;
            }
            
        },
        function failure(response) {
            $scope.lock = false;
            $scope.message = "Fail...";
        });
    }
}]);