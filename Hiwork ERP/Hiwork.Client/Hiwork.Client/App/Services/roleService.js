angular.module("hiworkApp").service('roleService', function (role, successFunction, errorFunction) {
    ajaxService.AjaxPostWithRoleData(role, "user/roleAdd", successFunction, errorFunction);
});