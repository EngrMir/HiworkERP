angular.module("hiworkApp").controller("homeController", function ($window, $scope, $state, $rootScope, appMenu, sessionFactory, AppStorage, UserType, translatorService, currentUserToken, ajaxService, appSettings, $http, EstimationType) {
    
    //SELECT dbo.[Order].OrderNo, dbo.Estimation.EstimationType, dbo.[Master.EstimationType].Code, dbo.[Master.EstimationType].Name_en, dbo.Estimation.EstimationNo, dbo.[Order].ID, dbo.[Order].RegistrationID, 
    //dbo.[Order].ApplicationID, dbo.[Order].EstimationID
    //FROM dbo.[Order] INNER JOIN
    //dbo.Estimation ON dbo.[Order].EstimationID = dbo.Estimation.ID INNER JOIN
    //dbo.[Master.EstimationType] ON dbo.Estimation.EstimationType = dbo.[Master.EstimationType].ID

    var Model = {};
    var type;
    var searchValue;
    Model.Search = {};

    var BaseModel = {};
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    BaseModel.CurrentUserID = currentUser.CurrentUserID;
    BaseModel.CurrentCulture = currentCulture;
    BaseModel.ApplicationId = appSettings.ApplicationId;

    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    Model.CurrentUserID = currentUser.CurrentUserID;
    Model.CurrentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    Model.ApplicationId = appSettings.ApplicationId;

    $scope.orderKeyUp = function (e) {
        if (e.keyCode == '13') {
            $scope.search(orderId.value, 'order')
        }
    };
    $scope.staffKeyUp = function (e) {
        if (e.keyCode == '13') {
            $scope.search(staffId.value, 'staff')
        }
    };
    $scope.clientKeyUp = function (e) {
        if(e.keyCode == '13') {
            $scope.search(clientId.value, 'client')
        }
    };
    $scope.search = function (value, field) {
        searchValue = value;
        type = field;
        Model.Search.Value = value;
        Model.Search.Type = field;
        if (field == 'order') {            
            ajaxService.AjaxPostWithData(Model, "homeapi/searchorder", successOrderFunction, errorFunction);
        }
        else if (field == 'staff') {
            ajaxService.AjaxPostWithData(Model, "homeapi/searchstaff", successStaffFunction, errorFunction);
        }
        else if (field == 'client') {
            ajaxService.AjaxPostWithData(Model, "homeapi/searchclient", successClientFunction, errorFunction);
        }
    };

    var successOrderFunction = function (response) {
        if (response.length > 0) {
            var route;
            switch (response[0].TypeCode) {
                case 'TR':
                    route = "TranslationOrderDetails";
                    break;
                case 'PR':
                    route = "TaskQuotationInput";
                    break;
                case 'DT':
                    route = "DTPEstimation";
                    break;
                case 'ST':
                    route = "OrderDetails";
                    break;
                case 'TC':
                    route = "TranscriptionOrderDetails";
                    break;
                case 'OC':
                    route = "OverheadCostQuotation";
                    break;
            }
            $state.go(route, { "id": response[0].ID, "Estimation": response[0] });
        }
        else {
            onErrorToastMessage("Order Number Not Found.");
        }
    };
    var successStaffFunction = function (response) {
        if (response.length > 0) {
            $state.go("StaffRegistration", { "ID": response[0].Id });
        }
        else {
            onErrorToastMessage("Staff Number Not Found.");
        }
    };
    var successClientFunction = function (response) {
        if (response.length > 0) {
            $state.go("CompanyRegistration.companyInfo", { "ID": response[0].Id });
        }
        else {
            onErrorToastMessage("Client Number Not Found.");
        }
    };
    var errorFunction = function (response) {
        onErrorToastMessage("System Error. Please Contact Administrator.");
    };

    if (currentUserToken.profile.token === ""
        || currentUserToken.profile.token === 'undefined') {
        $state.go("login");
        return;
    };

    $scope.selectedMenu = appMenu.Dashboard;
    $scope.UserType = UserType;
    $scope.mobileMenuActive = false;
    $scope.currentUser = sessionFactory.GetObject(AppStorage.userData);
    $scope.languages = sessionFactory.GetSessionObject(AppStorage.cultureData);
    $scope.appMenu = appMenu;
    $scope.setMenu = function (menu) {
        $scope.selectedMenu = menu;
    };
    $scope.logout = function () {
        sessionFactory.ClearAll();
        currentUserToken.profile.username = "";
        currentUserToken.profile.token = "";
        currentUserToken.remove();

        $state.go("login");
    };
    $scope.languageChanged = function (selectLanguage) {
        translatorService.setCurrentLanguage(selectLanguage.Code);
    };

    /* Notification block */
    $scope.loadNotifications = function () {
        $http({ method: 'POST', url: appSettings.API_BASE_URL + 'notification/list', data: BaseModel }).then(function (r) {
            $scope.notifications = r.data;
            //Update notifications as read
            $http({ method: 'POST', url: appSettings.API_BASE_URL + 'notification/updateasread', data: BaseModel }).then(function (r) {
                $scope.unreadcount = null;
            }, function (error) { });
        }, function (error) {

        });
    }

    $scope.traceOutNotification = function ($event, item) {
        $event.stopPropagation();
        
        ajaxService.AjaxPostWithData(BaseModel, 'estimation/getspecificestimation/' + item.EstimationID, function (r) {
            if (r) {
                var route;
                switch (r.EstimationType) {
                    case EstimationType.Translation:
                        route = 'TranslationEstimation';
                        break;
                    case EstimationType.Project:
                        route = 'TaskQuotationInput';
                        break;
                    case EstimationType.DTP:
                        route = 'DTPEstimation';
                        break;
                    case EstimationType.ShortTermDispatch:
                        route = 'ShortTermEstimation';
                        break;
                    case EstimationType.Transcription:
                        route = 'TranscriptionEstimation';
                        break;
                    case EstimationType.OverheadCost:
                        route = 'OverheadCostQuotation';
                        break;
                }
                $state.go(route, { "id": r.ID, "Estimation": r });
            } else {}
        }, function () {});
    }

    $scope.approvalEstimations = function () {
        $state.go(route, { "id": r.ID, "Estimation": r });
    }

    var countUnreadNotification = function () {
        $http({ method: 'POST', url: appSettings.API_BASE_URL + 'notification/unreadcount', data: BaseModel }).then(function (r) {
            if (r.data === 0) {
                r.data = null;
            }
            $scope.unreadcount = r.data;
        }, function (error) {});
    }

    var countUnapprovedNotification = function () {
        $http({ method: 'POST', url: appSettings.API_BASE_URL + 'notification/unapprovedcount', data: BaseModel }).then(function (r) {
            $scope.unapprovedcount = r.data;
        }, function (error) {});
    }

    countUnapprovedNotification();
    countUnreadNotification();
});