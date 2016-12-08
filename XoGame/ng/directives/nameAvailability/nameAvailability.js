
app.directive('nameAvailability', function ($q, $http) {
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {

            ctrl.$asyncValidators.nameAvailability = function (modelValue, viewValue) {

                if (ctrl.$isEmpty(modelValue)) {
                    // consider empty model valid
                    return $q.resolve();
                }

                var def = $q.defer();

                $http.post('/api/Player/NameAvailable', { Name: modelValue })
                    .success(function (data) {
                        console.log(data);
                        if (data)
                            def.resolve();
                        else
                            def.reject();
                    })
                .error(function () {
                    def.reject();
                });
                //return ctrl.$setValidity('valid', def.promise && def.promise.status);
                return def.promise;
            };
        }
    };
});
