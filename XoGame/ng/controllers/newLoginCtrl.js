var app = angular.module('xoApp')
    .controller('newLoginCtrl', function($http, $scope, $rootScope) {

        $scope.submit = function() {
            $http.post('api/Player/IsAuthorized', { Name: $scope.name, Password: $scope.password })
                .success(function(data) {
                    if (data) {
                        $rootScope.isAuth = 1;
                        $rootScope.currentPlayer = data;
                        $rootScope.hub.invoke('connect', data.Id, data.Name);
                    } else {
                        $rootScope.isAuth = 0;
                    }
                })
                .error(function() {
                    $rootScope.isAuth = 0;
                });
        };

    });