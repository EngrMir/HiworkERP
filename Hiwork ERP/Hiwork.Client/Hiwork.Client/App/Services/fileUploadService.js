/**
 * Created by Tomas on 13th June 2017.
 */
angular.module("hiworkApp").service('fileUploadService', ['$http', '$rootScope',
    function ($http, $rootScope) {
        this.uploadFileToUrl = function (file, uploadUrl, userID, culture, successFunction, errorFunction) {
            $http({
                method: 'POST',
                url: uploadUrl,
                headers: { 'Content-Type': undefined },

                transformRequest: function (data) {
                    var formData = new FormData();
                    formData.append("file", data.file);
                    return formData;
                },
                data: { file: file },
                params: { userID: userID, culture: culture}
            })
            .then(function (response) {
                //$rootScope.$broadcast("fileUploaded", response);
                successFunction(response);
            }, function (error) {
                //$rootScope.$broadcast("fileNotUploaded", error);
                errorFunction(error);
            });

            //var fd = new FormData();
            //fd.append('file', file);

            //$http.post(uploadUrl, fd, {dataType: "jsonp"}, {
            //    transformRequest: angular.identity,
            //    headers: { 'Content-Type': undefined, },
            //    params: { userID: userID, culture: culture }
            //})
            //.then(function (response) {
            //    //$rootScope.$broadcast("fileUploaded", response);
            //    successFunction(response);
            //}, function (error) {
            //    //$rootScope.$broadcast("fileNotUploaded", error);
            //    errorFunction(error);
            //});
        }
    }]);