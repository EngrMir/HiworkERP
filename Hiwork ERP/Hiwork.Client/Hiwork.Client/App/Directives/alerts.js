/**
 * Created by Mahfuz on 07/24/2017.
 */
/// <reference path="../CommonTemplates/alerts.html" />
(function (module) {

    var alerts = function (alerting) {
        return {
            restrict: "AE",
            templateUrl: "/app/CommonTemplates/alerts.html",
            scope: true,
            controller: function ($scope) {
                $scope.removeAlert = function (alert) {
                    alerting.removeAlert(alert);
                };
            },
            link: function (scope) {
                scope.currentAlerts = alerting.currentAlerts;
            }
        };
    };

    module.directive("alerts", alerts);

}(angular.module("hiworkApp")));