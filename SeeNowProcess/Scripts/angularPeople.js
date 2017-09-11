var peopleApp = angular.module("peoplePart", []);
peopleApp.directive('checkOld', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, mCtrl, ngModel) {
            function checkPassword(value) {
                if (value == "") {
                    mCtrl.$setValidity('oldPass', true)
                } else {
                    $http({
                        method: "POST",
                        url: "/People/CheckOldPassword",
                        data: $.param({ "userId": $scope.currentUser.id, 'password': value }),
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
                    }).then(function mySuccess(response) {
                        if (response.data == "match") {
                            mCtrl.$setValidity('oldPass', true);
                        }
                        else {
                            mCtrl.$setValidity('oldPass',false);
                        }
                    }, function myError(response) {
                        mCtrl.$setValidity('oldPass', false);
                    })
                }
                return value;
            }
            mCtrl.$parsers.push(checkPassword);
        }
    }
});
peopleApp.controller("peopleCtrl", ['$scope', '$http', function ($scope, $http) {
    $scope.message = '';
    $scope.lists = [];  //people from database
    $scope.userTasks = [];
    $scope.userTeams = [];
    $scope.userUserStories = [];
    $scope.userProjects = [];
    $scope.selected = null;
    $scope.userName = 'Select user to show content.';
    $scope.currentUser = null;
    $scope.info;
    $scope.passwords = { "oldPassword": "", "newPassword": "", "confirmPassword": "" };
    $scope.roles = ["admin", "headMaster", "seniorDev", "juniorDev", "intern", "tester", "client"];
    $scope.fullRoles = ["Admin", "Head Master", "Senior Developer", "Junior Developer", "Intern", "Tester", "Client"];
    $scope.importance = ["None", "Trivial", "Regular", "Important", "Critical"];
    $scope.allTeams = [];
    $scope.oldPasswordInvalid = true;
    $scope.showInvalid = false;
    $scope.oldChanged = false;
    $scope.lock = false;
    $scope.logged = null;
    $scope.isAdmin = false;
    $http({
        method: "GET",
        url: "/People/GetCurrentUser",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySuccess(response) {
        $scope.logged = response.data[0];
        if (response.data[1] == 'Admin') {
            $scope.isAdmin = true;
        }
    }, function myError(response) {
        $scope.message = "Error";
    })
    $scope.checkIfMe = function () {
        if (($scope.currentUser != null) && ($scope.logged!=null)) {
            if ($scope.currentUser.id == $scope.logged) {
                return true;
            }
        }
        return false;
    }

    $scope.checkPassword = function () {
        //tutaj po stracie focusu, sprawdzenie starego hasla
        $scope.oldChanged = false;
        if (($scope.passwords.oldPassword == "") || ($scope.passwords.oldPassword.length < 6) || ($scope.passwords.oldPassword.length > 100)) {
            $scope.oldPasswordInvalid = true;
            $scope.showInvalid = false;
        }
        else {
            $http({
                method: "POST",
                url: "/People/CheckOldPassword",
                data: $.param({ "userId": $scope.currentUser.id, 'password': $scope.passwords.oldPassword }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
            }).then(function mySuccess(response) {
                if (response.data == "match") {
                    $scope.oldPasswordInvalid = false;
                }
                else {
                    $scope.oldPasswordInvalid = true;
                    $scope.showInvalid = true;
                }
            }, function myError(response) {
                $scope.message = response.data;
            })
        }
        
    }
    
    $http({
        method: "GET",
        url: "/People/AllPeople",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.lists = response.data;
    }, function myError(response) {
        $scope.message = "Error";
    })

    $scope.showUser = function (ID, name) {
        $scope.userName = name;
        $http({
            method: "POST",
            url: "/People/GetUserInfo",
            data: $.param({"id": ID}),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

        }).then(function mySucces(response) {
            $scope.currentUser = response.data[0];
            $scope.role = $scope.roles[$scope.currentUser.role];
            $scope.currentRole = $scope.fullRoles[$scope.currentUser.role];
            $scope.passwords.oldPassword = "";
            $scope.passwords.newPassword = "";
            $scope.passwords.confirmPassword = "";
            $scope.oldPasswordInvalid = true;
            $scope.showInvalid = false;
            $scope.oldChanged = false;
            $scope.message = $scope.currentUser.id;
            $scope.info = {e: "Email: ", n: "Phone Number: ", r: "Role: "};
            $http({
                method: "POST",
                url: "/People/GetTasks",
                data: $.param({ "userId": $scope.currentUser.id }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                $scope.userTasks = response.data.Tasks;
                for (var i = 0; i < $scope.userTasks.length; ++i) {
                    $scope.userTasks[i].Importance = $scope.importance[$scope.userTasks[i].Importance];
                }
                

            }, function myError(response) {
                $scope.message = "Error in displaying tasks";
            })

           /* $http({
                method: "POST",
                url: "/People/GetAssignments",
                data: $.param({ "userId": $scope.currentUser.id }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                $scope.userTeams = response.data.Teams;

            }, function myError(response) {
                $scope.message = "Error in displaying User Stories.";
            })*/
            $http({
                method: "POST",
                url: "/People/GetTeams",
                data: $.param({ "userId": $scope.currentUser.id }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                $scope.userTeams = response.data.Teams;

            }, function myError(response) {
                $scope.message = "Error in displaying User Stories.";
            })
            $http({
                method: "POST",
                url: "/People/GetProjects",
                data: $.param({ "userId": $scope.currentUser.id }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                $scope.userProjects = response.data.Projects;

            }, function myError(response) {
                $scope.message = "Error in displaying User Stories.";
            })
            $http({
                method: "POST",
                url: "/People/GetUserStories",
                data: $.param({ "userId": $scope.currentUser.id }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                $scope.userUserStories= response.data.UserStories;

            }, function myError(response) {
                $scope.message = "Error in displaying User Stories.";
            })
            $http({
                method: "GET",
                url: "/People/AllPeople",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
            }).then(function mySucces(response) {
                $scope.lists = response.data;
            }, function myError(response) {
                $scope.message = "Error";
            })
        }, function myError(response) {
            $scope.message = "Error taking User";
        })
    }

    $scope.updateUser = function (id) {
        $scope.lock = true;
        $scope.roleNumber = $scope.roles.indexOf($scope.role);
        if ($scope.passwords.newPassword == $scope.passwords.confirmPassword && $scope.passwords.newPassword != "") {
            $http({
                method: "POST",
                url: "/People/updateUserPassword",
                data: $.param({ "id": $scope.currentUser.id, "oldPassword": $scope.passwords.oldPassword, "newPassword": $scope.passwords.newPassword }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                if (response.data == "Success") {
                    //$scope.message = "Success";
                    $http({
                        method: "POST",
                        url: "/People/updateUserData",
                        data: $.param({ "id": $scope.currentUser.id, "name": $scope.currentUser.name, "login": $scope.currentUser.login, "email": $scope.currentUser.email, "phone": $scope.currentUser.phone, "role": $scope.roleNumber }),
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

                    }).then(function mySucces(response) {
                        if (response.data == "Success") {
                            //$scope.message = response.data;
                            $scope.showUser($scope.currentUser.id); //może tu do głownej zakładki jakiś redirect?
                            $scope.userName = $scope.currentUser.name;
                            //$scope.lock = false;
                        }
                        else {
                            $scope.lock = false;
                            $scope.message = response.data;
                        }

                    }, function myError(response) {
                        $scope.lock = false;
                        $scope.message = "Error in displaying User Stories."; //na pewno takie cos?
                    })
                }
                else {
                    $scope.lock = false;
                    $scope.message = response.data;
                }
            }, function myError(response) {
                $scope.lock = false;
                $scope.message = "Error in passwords"; //na pewno takie cos?
            })

        } else if ($scope.passwords.newPassword == "") {

            $http({
                method: "POST",
                url: "/People/updateUserData",
                data: $.param({ "id": $scope.currentUser.id, "name": $scope.currentUser.name, "login": $scope.currentUser.login, "email": $scope.currentUser.email, "phone": $scope.currentUser.phone, "role": $scope.roleNumber }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                $scope.message = "Success";
                $scope.showUser($scope.currentUser.id);
                $scope.userName = $scope.currentUser.name;

            }, function myError(response) {
                $scope.message = "Error in displaying User Stories.";
            })
        }
        else
            $scope.message = "Passwords don't match";
        $scope.passwords.oldPassword = "";
        $scope.passwords.newPassword = "";
        $scope.passwords.confirmPassword = "";
    }

    $scope.deleteUser = function (id) {
        $scope.lock = true;
        $http({
            method: "POST",
            url: "/People/deleteUser",
            data: $.param({ "id": $scope.currentUser.id }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

        }).then(function mySucces(response) {
            if (response.data == "Success") {
                //$scope.message = "Success";
                $http({
                    method: "GET",
                    url: "/People/AllPeople",
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
                }).then(function mySucces(response) {
                    $scope.lists = response.data;
                    $scope.currentUser = null;
                    $scope.userName = 'Select user to show content.';
                    $scope.role = null;
                }, function myError(response) {
                    $scope.lock = false;
                    $scope.message = "Error";
                })
            }
            else {
                $scope.lock = false;
                $scope.message = response.data;
            }


        }, function myError(response) {
            $scope.lock = false;
            $scope.message = "Error in displaying User Stories.";
        })
    }

    $scope.deleteTask = function (UserID, TaskID) {
        /*$scope.message = ID;*/
        $http({
            method: "POST",
            url: "/People/RevokeTask",
            data: $.param({ "userId": UserID, "taskId": TaskID }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

        }).then(function mySucces(response) {
            $scope.message = response.data;
            $http({
                method: "POST",
                url: "/People/GetTasks",
                data: $.param({ "userId": $scope.currentUser.id }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                $scope.userTasks = response.data.Tasks;
                for (var i = 0; i < $scope.userTasks.length; ++i) {
                    $scope.userTasks[i].Importance = $scope.importance[$scope.userTasks[i].Importance];
                }
            }, function myError(response) {
                $scope.message = "Error in displaying tasks";
            })

            $http({
                method: "POST",
                url: "/People/GetAssignments",
                data: $.param({ "userId": $scope.currentUser.id }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                $scope.userTeams = response.data.Teams;

            }, function myError(response) {
                $scope.message = "Error in displaying User Stories.";
            })
            $http({
                method: "GET",
                url: "/People/AllPeople",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
            }).then(function mySucces(response) {
                $scope.lists = response.data;
            }, function myError(response) {
                $scope.message = "Error";
            })
        }, function myError(response) {
            $scope.message = "Error taking User";
        })
    }

    $scope.deleteStory = function (UserID, StoryID) {
        /*$scope.message = ID;*/
        $http({
            method: "POST",
            url: "/People/RevokeStory",
            data: $.param({ "userId": UserID, "storyId": StoryID }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

        }).then(function mySucces(response) {
            $scope.message = response.data;
            $http({
                method: "POST",
                url: "/People/GetTasks",
                data: $.param({ "userId": $scope.currentUser.id }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                $scope.userTasks = response.data.Tasks;
                for (var i = 0; i < $scope.userTasks.length; ++i) {
                    $scope.userTasks[i].Importance = $scope.importance[$scope.userTasks[i].Importance];
                }
            }, function myError(response) {
                $scope.message = "Error in displaying tasks";
            })

            $http({
                method: "POST",
                url: "/People/GetAssignments",
                data: $.param({ "userId": $scope.currentUser.id }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                $scope.userTeams = response.data.Teams;

            }, function myError(response) {
                $scope.message = "Error in displaying User Stories.";
            })
            $http({
                method: "GET",
                url: "/People/AllPeople",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
            }).then(function mySucces(response) {
                $scope.lists = response.data;
            }, function myError(response) {
                $scope.message = "Error";
            })
        }, function myError(response) {
            $scope.message = "Error taking User";
        })
    }

    $scope.deleteTeam = function (UserID, TeamID) {
        /*$scope.message = ID;*/
        $http({
            method: "POST",
            url: "/People/RevokeTeam",
            data: $.param({ "userId": UserID, "teamId": TeamID }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

        }).then(function mySucces(response) {
            $scope.message = response.data;
            $http({
                method: "POST",
                url: "/People/GetAssignments",
                data: $.param({ "userId": $scope.currentUser.id }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                $scope.userTeams = response.data.Teams;

            }, function myError(response) {
                $scope.message = "Error in displaying User Stories.";
            })
          /*  $http({
                method: "GET",
                url: "/People/AllPeople",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
            }).then(function mySucces(response) {
                $scope.lists = response.data;
            }, function myError(response) {
                $scope.message = "Error";
            })*/
        }, function myError(response) {
            $scope.message = "Error taking User";
        })
    }

    $http({
        method: "GET",
        url: "/People/GetAllTeams",
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

    }).then(function mySucces(response) {
        $scope.allTeams = response.data;

    }, function myError(response) {
        $scope.message = "Error in displaying User Stories.";
    })

    $scope.addUserToTeam = function (id) {
        $http({
            method: "POST",
            url: "/People/AssignTeam",
            data: $.param({ "userId": id, "teamId": $scope.newTeam.TeamID}),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

        }).then(function mySucces(response) {
            $scope.message = response.data;
            $http({
                method: "POST",
                url: "/People/GetTeams",
                data: $.param({ "userId": $scope.currentUser.id }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                $scope.userTeams = response.data.Teams;

            }, function myError(response) {
                $scope.message = "Error in adding User to team.";
            });
        })
    }



}]);