

/* ******************************************************************************************************************
 * AngularJS 1.5 Directive for common selection window to select single/multiple items from a list of items
 * Date             :   07-July-2017
 * By               :   Ashis Kr. Das
 * *****************************************************************************************************************/


/* ******************************************************************************************************************
 * In this javascript file, there will be two angularjs entity to be programmed
 * First directive  :   The main directive which will be used directly by programmers to incorporate common selection
 * Second Component :   Child component which will be responsible for Add/Edit selection
 * *****************************************************************************************************************/





angular.module("hiworkApp").directive("commSelection", commSelectionDirective);

function commSelectionDirective() {

    function ctlSelectController($scope, $uibModal, $rootScope) {

        // Initialization of the data objects of the directive
        function initializeModel() {
            init();
        }

        $rootScope.$on("listenToManipulateCommSelectionItem", function () {
            init();
        });

        function init() {
            $scope.allnames = "";
            $scope.itemList = [];
            var datatype_1 = typeof $scope.data;
            var datatype_2 = typeof $scope.listento;

            if (typeof $scope.name == "undefined") {
                $scope.name = "Name";
            }
            if (datatype_1 == "string" && $scope.data != "") {
                var Item = {};
                Item.ID = $scope.data;
                Item[$scope.name] = $scope.text;
                $scope.itemList.push(Item);
            }
            else if (Array.isArray($scope.data) == true) {
                $scope.itemList = angular.copy($scope.data);
            }
            if (datatype_2 == "undefined") {
                $scope.listento = "commSelection_".concat((Math.floor((Math.random() * 10000) + 1)).toString());
            }
            updateNameBox();
        }

        // Create a string of comma separated names for available items
        function updateNameBox() {
            var i;
            var str = "";
            var len = $scope.itemList.length;
            for (i = 0; i < len; i += 1) {
                str = str.concat($scope.itemList[i][$scope.name]);
                if (i == len - 1)
                    continue;
                str = str.concat(", ");
            }
            $scope.allnames = str;
            //$rootScope.$broadcast('selectedStaff', str);
        }

        // Initialize necessary data objects
        initializeModel();

        // Opens up a popup dialog for the selection of items among the whole list
        $scope.openSelectionPopupDialog = function () {
            var configdata = {};
            var multiselect_bool = typeof $scope.multiselect != "undefined";
            configdata.component = "selectEntityComponent";
            configdata.resolve = {};
            configdata.resolve.modalData = {};
            configdata.resolve.modalData.itemList = $scope.itemList;
            configdata.resolve.modalData.url = $scope.url;
            configdata.resolve.modalData.listento = $scope.listento;
            configdata.resolve.modalData.multiselect = multiselect_bool;
            configdata.resolve.modalData.name = $scope.name;
            configdata.resolve.modalData.obj = $scope.item;
            configdata.resolve.title = function () { return $scope.title; };
            $uibModal.open(configdata);
        };

        // Event handler to handle events on successful selection of items done in the popup dialog
        // response will be an array of objects which comprises the selected items
        $scope.$on($scope.listento, function (event, response) {
            $scope.itemList = response;
            updateNameBox();
        });
    };


    // Create the Directive Definition Object (DDO) and return it to the angular system
    var ddo = {};
    ddo.restrict = "E";
    ddo.templateUrl = "App/Directives/CommonSelectionControl/Template/viewCommSelectionControl.html";
    ddo.scope = { title: '@dlgTitle', url: '@url', data: '@data', text: '@text', name: '@name', listento: '@listento', multiselect: '@multiselect', item: '@item' };
    ddo.controller = ["$scope", "$uibModal", "$rootScope", ctlSelectController];
    return ddo;
};




angular.module('hiworkApp').component('selectEntityComponent', {

    templateUrl: "App/Directives/CommonSelectionControl/Template/viewSelectionWindow.html",
    bindings: { modalInstance: "<", resolve: "<" },
    controller: ['$rootScope', 'AppStorage', 'appSettings', 'sessionFactory', 'ajaxService', selectEntityController]
});


function selectEntityController($rootScope, AppStorage, appSettings, sessionFactory, ajaxService) {

    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    // load list of all entity data from the server as per the url provided
    $ctrl.GetDataList = function () {
        $ctrl.enableSave = false;
        var BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, $ctrl.modalData.url, onGetData, onErrorToastMessage);
        return;
    };

    // Search for and mark selected to the valid items/entities
    // This search algorithm is exhaustive
    var onGetData = function (response) {
        $ctrl.dataList = [];
        $ctrl.dataList = response;
        var currentid_a, currentid_b;
        var serverdata, userdata;
        var id_x = 'Id', id_y = 'ID';

        _.forEach($ctrl.dataList, function (serverdata) {

            Object.defineProperty(serverdata, 'IsSelected', { value: false, configurable: true, writable: true, enumerable: true });
            serverdata.IsSelected = false;
            if (id_x in serverdata)
                currentid_a = serverdata.Id;
            else if (id_y in serverdata)
                currentid_a = serverdata.ID;

            _.forEach($ctrl.modalData.itemList, function (userdata) {
                if (id_x in userdata)
                    currentid_b = userdata.Id;
                else if (id_y in userdata)
                    currentid_b = userdata.ID;

                if (currentid_a == currentid_b) {
                    serverdata.IsSelected = true;
                    $ctrl.countSelected += 1;         // maintain a counter reflecting the number of current valid selection
                    return;
                }
            });
        });

        // Maintain a mark to tag if each and every item on the list is already selected or not
        if ($ctrl.dataList.length == $ctrl.countSelected) {
            $ctrl.selectEverything = true;
        }
        $ctrl.enableSave = true;
    };

    $ctrl.itemClicked = function (item) {
        if ($ctrl.modalData.multiselect == false) {
            item.IsSelected = !item.IsSelected;
            var i;
            for (i = 0; i < $ctrl.dataList.length; i++) {
                if ($ctrl.dataList[i] != item) {
                    $ctrl.dataList[i].IsSelected = false;
                }
            }
        }
        else {
            item.IsSelected = !item.IsSelected;
            $ctrl.itemSelectionChanged(item);
        }
    };

    $ctrl.allItemsSelectionChanged = function () {
        _.forEach($ctrl.dataList, function (dataitem) {
            dataitem.IsSelected = $ctrl.selectEverything;
        });
        $ctrl.countSelected = $ctrl.selectEverything == true ? $ctrl.dataList.length : 0;
    };

    $ctrl.itemSelectionChanged = function (item) {
        if (item.IsSelected == false) {
            $ctrl.countSelected -= 1;
        }
        else if (item.IsSelected == true) {
            $ctrl.countSelected += 1;
        }
        $ctrl.selectEverything = $ctrl.countSelected == $ctrl.dataList.length;
    };

    // Initialize component
    $ctrl.$onInit = function () {
        $ctrl.enableSave = false;
        $ctrl.countSelected = 0;
        $ctrl.selectEverything = false;
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.name = $ctrl.resolve.modalData.name;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
        $ctrl.modalData.itemList = angular.copy($ctrl.resolve.modalData.itemList);
        $ctrl.GetDataList();
    };

    // Prepare output list which will be propagated to all receivers
    // Run a search algorithm (linear search) to collect each and every item which is selected by user
    $ctrl.ChooseSelected = function (obj) {
        if ($ctrl.modalData.multiselect == true) {
            $ctrl.itemList = new Array($ctrl.countSelected);
            var i, j;
            i = j = 0;
            while (i < $ctrl.dataList.length) {
                if ($ctrl.dataList[i].IsSelected == true) {
                    $ctrl.itemList[j] = $ctrl.dataList[i];
                    delete $ctrl.itemList[j].IsSelected;        // delete unnecessary property
                    j += 1;
                }
                i += 1;
            }
        }
        else {
            var index;
            var NewList = new Array(1);
            for (index = 0; index < $ctrl.dataList.length; index += 1) {
                if ($ctrl.dataList[index].IsSelected == true) {
                    NewList[0] = $ctrl.dataList[index];
                    if (obj) {
                        NewList[0].Item = JSON.parse(obj);
                    }
                    break;
                }
            }
            $ctrl.itemList = NewList;
        }

        // Propagate the output list to all registered listeners
        //$ctrl.$emit("mal", $ctrl.itemList);
        $rootScope.$broadcast($ctrl.modalData.listento, $ctrl.itemList);
        if ($ctrl.itemList[0]) {
            if ($ctrl.itemList[0].Item) {
                $rootScope.$broadcast('selectedStaff', $ctrl.itemList);
            }
        }
        $ctrl.Close();
    };

    $ctrl.Close = function () {
        $ctrl.dataList = null;
        $ctrl.itemList = null;
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}


