var app = angular.module('xoApp')
    .directive('validateMsg', function () {
        return {
            scope: {
                control: '=',
                display: '=',
                validators: '='
            },
            link: (scope) => {
                var tempValidators = scope.validators.split(",");
                scope.vs = [];
                for (var i = 0; i < tempValidators.length; i++) {
                    scope.vs.push({ name: tempValidators[i].split("=")[0], valu: tempValidators[i].split("=")[1] });
                };
            },
            templateUrl: '/ng/Directives/validator/validator.html'
        };
    });