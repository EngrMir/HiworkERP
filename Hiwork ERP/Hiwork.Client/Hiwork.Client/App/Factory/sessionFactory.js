(function () {
    'use strict'
    angular.module("hiworkApp").factory("sessionFactory", function ($cookies,$window) {
        return {
            SetData: function (key, value) {
                $cookies.remove(key);
                $cookies.put(key, value);
            },
            SetObject: function (key, data) {
                $cookies.putObject(key, data);
            },
            GetData: function (key) {
                return $cookies.get(key);
            },
            GetObject: function (key) {
                return $cookies.getObject(key);
            },
            SetSessionObject : function(key,value){
                $window.localStorage.setItem(key, angular.toJson(value));
            },
            GetSessionObject: function (key) {
                return  angular.fromJson($window.localStorage.getItem(key));
            },
            RemoveByKey: function (key) {
                $cookies.remove(key);
            },
            ClearAll: function () {
                $window.localStorage.clear();
                var cookies = $cookies.getAll();
                angular.forEach(cookies, function (v, k) {
                    $cookies.remove(k);
                });
            }
        };
    });
}());