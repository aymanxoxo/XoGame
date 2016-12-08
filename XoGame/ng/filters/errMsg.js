var app = angular.module('xoApp')
.filter('errMsg', function () {
    return function (x, para) {
        switch (x) {
            case 'required':
                {
                    return para.name + ' is mandatory.';
                }
            case 'minlength':
                {
                    return para.name + ' must be at least ' + para.valu;
                }
            case 'maxlength':
                {
                    return para.name + ' must not exceeds ' + para.valu;
                }
            case 'match':
                {
                    return para.name + " doesn't match " + para.valu;
                }
            case 'nameAvailability':
                {
                    return para.name + " is not available.";
                }
        };
    }
})