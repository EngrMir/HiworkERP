angular.module('hiworkApp')
    .directive('pageSelect', function () {
        return {
            restrict: 'E',
            template: '<input type="text" class="select-page" style="width:50px;" ng-model="inputPage" ng-change="selectPage(inputPage)" only-digits>',
            link: function (scope, element, attrs) {
                scope.$watch('currentPage', function (c) {
                    scope.inputPage = c;
                });
            }
        }
    });