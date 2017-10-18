
/**
 * Created by Tomas on 26/05/2017.
 */
angular.module('hiworkApp').directive('onlyDigits', function () {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function (scope, element, attr, ctrl) {
            function inputValue(val) {
                if (val) {
                    var digits = val.replace(/[^0-9]/g, '');

                    if (digits !== val) {
                        ctrl.$setViewValue(digits);
                        ctrl.$render();
                    }
                    return parseInt(digits, 10);
                }
                return undefined;
            }
            ctrl.$parsers.push(inputValue);
        }
    };
});


angular.module('hiworkApp').directive('onlyDecimal', function () {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function (scope, element, attr, ctrl) {
            function inputValue(val) {
                if (val) {
                    var digits = val.replace(/[^\d.]/g, '')             // numbers and decimals only
                                    .replace(/(^[\d]{*})[\d]/g, '$1')   // ulimited digits before decimal point is acceptable
                                    .replace(/(\..*)\./g, '$1')         // decimal can't exist more than once
                                    .replace(/(\.[\d]{2})./g, '$1');    // not more than 2 digits after decimal

                    if (digits !== val) {
                        ctrl.$setViewValue(digits);
                        ctrl.$render();
                    }
                    return parseFloat(digits, 10);
                }
                return undefined;
            }
            ctrl.$parsers.push(inputValue);
        }
    };
});

