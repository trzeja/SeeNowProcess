﻿var peopleApp = angular.module("peoplePart", []);
peopleApp.controller("peopleCtrl", ['$scope', '$http', function ($scope, $http) {
    $scope.message = '';
    $scope.lists = [];  //people from database
    $scope.userTasks = [];
    $scope.userTeams = [];
    $scope.selected = null;
    $scope.userName = 'Select user to show content.';
    $scope.currentUser;
    $scope.info;
    $scope.passwords = { "oldPassword": "", "newPassword": "", "confirmPassword": "" };


    
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
        /*$scope.message = ID;*/
        $scope.userName = name;
        $http({
            method: "POST",
            url: "/People/GetUserInfo",
            data: $.param({"id": ID}),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

        }).then(function mySucces(response) {
            $scope.currentUser = response.data[0];
            switch ($scope.currentUser.role) {
                case 0:
                    $scope.currentRole = "Admin";
                    $scope.role = "admin";
                    break;
                case 1:
                    $scope.currentRole = "Head Master";
                    $scope.role = "headMaster";
                    break;
                case 2:
                    $scope.currentRole = "Senior Developer";
                    $scope.role = "seniorDev";
                    break;
                case 3:
                    $scope.currentRole = "Junior Developer";
                    $scope.role = "juniorDev";
                    break;
                case 4:
                    $scope.currentRole = "Intern";
                    $scope.role = "intern";
                    break;
                case 5:
                    $scope.currentRole = "Tester";
                    $scope.role = "tester";
                    break;
                case 6:
                    $scope.currentRole = "Client";
                    $scope.role = "client";
                    break;
            }
            $scope.message = $scope.currentUser.id;
            $scope.info = {e: "Email: ", n: "Phone Number: ", r: "Role: "};
            $http({
                method: "POST",
                url: "/People/GetTasks",
                data: $.param({ "userId": $scope.currentUser.id }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                $scope.userTasks = response.data.Tasks;

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

    $scope.updateUser = function (id) {
        switch ($scope.role) {
            case "admin":
                $scope.roleNumber = 0;
                break;
            case "headMaster":
                $scope.roleNumber = 1;
                $scope.role = "";
                break;
            case "seniorDev":
                $scope.roleNumber = 2;
                break;
            case "juniorDev":
                $scope.roleNumber = 3;
                break;
            case "intern":
                $scope.roleNumber = 4;
                break;
            case  "tester":
                $scope.roleNumber = 5;
                break;
            case "client":
                $scope.roleNumber = 6;
                break;
        }
        if ($scope.passwords.newPassword == $scope.passwords.confirmPassword && $scope.passwords.newPassword != "") {
            $http({
                method: "POST",
                url: "/People/updateUserPassword",
                data: $.param({ "id": $scope.currentUser.id, "oldPassword": $scope.password, "newPassword": $scope.newPassword }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                $scope.message = "Success";
                $http({
                    method: "POST",
                    url: "/People/updateUserData",
                    data: $.param({ "id": $scope.currentUser.id, "name": $scope.currentUser.name, "login": $scope.currentUser.login, "email": $scope.currentUser.email, "phone": $scope.currentUser.phone, "role": $scope.roleNumber }),
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

                }).then(function mySucces(response) {
                    $scope.userTeams = response.data.Teams;

                }, function myError(response) {
                    $scope.message = "Error in displaying User Stories.";
                })

            }, function myError(response) {
                $scope.message = "Error in passwords.";
            })

        } else if ($scope.passwords.newPassword == "") {

            $http({
                method: "POST",
                url: "/People/updateUserData",
                data: $.param({ "id": $scope.currentUser.id, "name": $scope.currentUser.name, "login": $scope.currentUser.login, "email": $scope.currentUser.email, "phone": $scope.currentUser.phone, "role": $scope.roleNumber }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

            }).then(function mySucces(response) {
                $scope.message = "Success";



            }, function myError(response) {
                $scope.message = "Error in displaying User Stories.";
            })
        }
        else
            $scope.message = "Passwords don't match";
        $scope.passwords.oldPassword = "";
        $scope.passwords.newPassword = "";
        $scope.passwords.confirmPassword = "";
        $scope.showUser($scope.currentUser.id);
        $scope.userName = $scope.currentUser.name;

    }

    $scope.deleteUser = function (id) {
        $http({
            method: "POST",
            url: "/People/deleteUser",
            data: $.param({ "id": $scope.currentUser.id }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }

        }).then(function mySucces(response) {
            $scope.message = "Success";
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
                $scope.message = "Error";
            })


        }, function myError(response) {
            $scope.message = "Error in displaying User Stories.";
        })
    }


    $scope.dropCallback = function (item, x) {
        $scope.message = "DropCallback: " + x;
        $http({
            method: "POST",
            url: "/IterationPlan/MoveTask",
            data: $.param({ "taskId": item.id, "newIterationId": x }),
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



}]);