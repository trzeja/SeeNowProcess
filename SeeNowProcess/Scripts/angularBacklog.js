'use strict';
var backlogAngular = angular.module('backlog', ['ngRoute']);

//backlogAngular.config(['$routeProvider', 'srvFactory', function ($routeProvider, srvFactory) {
//    $routeProvider.when('/BacklogIndex', {
//        templateUrl: 'Backlog/BacklogIndex.cshtml',
//        controller: 'backlogCtrl',
//        resolve: {
//            httpCurrentProject: function (srvFactory) {
//                return srvFactory.getProject();
//            }
//        }
//    })
//}]);

backlogAngular.factory('srvFactory', ['$http', function ($http) {
    var projectdata = {
        getProject: function () {
            var promise = $http({
                method: 'GET',
                url: '/Backlog/GetCurrentProject',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
            }).success(function (data, status, header, config) { return data; })
            return promise;
        }
    }
    return projectdata;
}])

backlogAngular.controller('backlogCtrl', ['$scope', '$http', 'srvFactory',function ($scope, $http, srvFactory) {
    $scope.userstory = 'Select user story to show content';
    $scope.currentUS;
    $scope.tasksUS;
    $scope.info;


    $scope.show = 0;
    $scope.currentProject = srvFactory.getProject().data;
    $scope.projects;

    $scope.$watch('currentProject', function () {
        if ($scope.show != 0) {
            $http({
                method: 'POST',
                url: '/Backlog/ChangeCurrentProject',
                data: $.param({ id: $scope.currentProject }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
            }).then(function mySucces(response) {
                if (response.data == 'Change') {
                    window.location.href = '/Backlog/BacklogIndex';
                }
            }, function myError(response) {
            })

        } else {
            $scope.show = 1;
            //za pierwszym razem wysyła zawsze, gdy nie mamy ustalonego jeszcze currentProject - dlatego są błędy. 
        }
    });

    $http({
        method: 'GET',
        url: '/Backlog/GetProjects',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.projects = response.data;
    }, function myError(response) {
    })

    //$http({
    //    method: "GET",
    //    url: "/Backlog/GetCurrentProject",
    //    headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    //}).then(function mySucces(response) {
    //    $scope.currentProject = response.data;
    //}, function myError(response) {
    //})

    $http({
        method: 'GET',
        url: '/Backlog/GetUserStories',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
    }).then(function mySucces(response) {
        $scope.userstories = response.data;
    }, function myError(response) {    
    })

    $scope.showUserStory = function (id, userstory) {
        $scope.userstory = userstory;
        
        $http({
            method: 'POST',
            url: '/Backlog/GetUserStory',
            data: $.param({ 'id': id }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(result) {
            $scope.currentUS = result.data[0];
        }).catch(function fail(result) {
            $scope.problems = ':(';
        })
        $http({
            method: 'POST',
            url: '/Backlog/GetUSTasks',
            data: $.param({ 'id': id }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' }
        }).then(function success(result) {
            $scope.tasksUS = result.data;
        }).catch(function fail(result) {
            $scope.problems = ':(';
        })
        $scope.info = {
            description: 'Description',
            notes: 'Notes',
            criteria: 'Criteria',
            owner: 'Owner: ',
            project: 'Project: ',
            size: 'Size: '
        };
        
    }
}]);