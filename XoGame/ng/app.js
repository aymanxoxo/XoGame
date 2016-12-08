var app = angular.module('xoApp', ['ui.router', 'ngMessages'])
    .config(function ($stateProvider, $urlRouterProvider, $locationProvider) {

        $urlRouterProvider.otherwise('/newMain');

        $stateProvider
            .state('newMain', {
                url: '/newMain',

                views: {
                    '': {
                        templateUrl: '/ng/views/newViews/newMain.html'
                    }
                }
            });
        $locationProvider.html5Mode(true);
    })
    .controller('appCtrl', function ($scope, $rootScope) {
        
        $scope.initForm = function (isAuth, currentPlayer) {

            $rootScope.isAuth = isAuth;
            $rootScope.currentPlayer = currentPlayer;
        }
    });