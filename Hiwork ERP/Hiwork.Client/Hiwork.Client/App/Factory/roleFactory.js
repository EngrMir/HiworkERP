/**
 * Created by Kayes on 16-May-2017.
 */
angular.module("hiworkApp").factory("roleFactory", function () {
    return {
        SaveRoleData: function (role) {
            roleService.saveRole(role);
            // loginService.getUserTypeList(user,onGetLoginData, loginError);
        }
    };
});