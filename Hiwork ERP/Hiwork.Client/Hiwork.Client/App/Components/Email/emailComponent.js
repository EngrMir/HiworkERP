angular.module('hiworkApp').component('emailWindow', {
    templateUrl: 'app/Components/Email/emailTemplate.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: emailController
    
});

emailController.$inject = ['$scope', '$rootScope', 'appSettings', 'AppStorage', 'sessionFactory', '$filter', 'ajaxService', '$http', '$uibModal'];
function emailController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService, $http, $uibModal) {

    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    $ctrl.sendEmail = function () {
        $scope.isTryToSend = true;
        if (!$ctrl.model.EmailTo || !$ctrl.model.EmailSubject ||
           $ctrl.model.EmailTo == "" || $ctrl.model.EmailSubject == "")
            return;
        $http({
            method: 'POST',
            url: appSettings.API_BASE_URL + "email/send",
            headers: { 'Content-Type': undefined },
            transformRequest: function (data) {
                var formData = new FormData();
                formData.append("model", angular.toJson(data.model));
                formData.append("file", data.file);
                return formData;
            },
            data: { model: $ctrl.model, file: $ctrl.AttachedFile }
        }).then(function (response) {
            toastr.success("Email sent successfully.");
        }, function (error) {
            toastr.error(error);
        });
    }

    $ctrl.$onInit = function () {
        $ctrl.model = {};
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.model.EmailTo = $ctrl.resolve.toaddress;
        $ctrl.model.UserID = currentUser.CurrentUserID;
        $ctrl.AttachedFile = {};
        $ctrl.LoadEmailCategoryList();
        $ctrl.model.demo = null;
        $ctrl.LoadEmailGroupList();
        $ctrl.model.demo2 = null;
        $ctrl.LoadEmailTemplateList();
        
    }
    $ctrl.Close = function () {
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };

    $ctrl.openCreateEmailWindow = function () {
        var title = "Create Template"
        $uibModal.open({
            component: "createemailwindow",
            transclude: true,
            resolve: {
                title: function () {
                    return title;
                }
            },
            size: 'lg'
        });
    };

    $ctrl.openChooseTemplate = function () {
        var title = "Choose Template"
        $uibModal.open({
            component: "chooseemailwindow",
            transclude: true,
            resolve: {
                title: function () {
                    return title;
                }
            },
            size: 'lg'
        });
    }

    $ctrl.LoadEmailCategoryList = function () {
        var BaseModel = new Object();
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "email/categorylist", successOnCategoryList, errorOnCategoryList);
    }

    var successOnCategoryList = function (response) {
        $ctrl.CategoryList = response;
    };

    var errorOnCategoryList = function (message) {
        //Error handling goes here.
        toastr.error($filter('translator')('ERRORDBOPERATION'));
    };
    $ctrl.LoadEmailGroupList = function () {
        var BaseModel = new Object();
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "email/grouplist", successOnGroupList, errorOnGroupList);
    }

    var successOnGroupList = function (response) {
        $ctrl.GroupList = response;
    }

    var errorOnGroupList = function (message) {
        //Error handling goes here.
        toastr.error($filter('translator')('ERRORDBOPERATION'));
    }

        $ctrl.LoadEmailTemplateList = function () {
            var BaseModel = new Object();
            BaseModel.CurrentUserID = currentUser.CurrentUserID;
            BaseModel.CurrentCulture = currentCulture;
            BaseModel.ApplicationId = appSettings.ApplicationId;
            ajaxService.AjaxPostWithData(BaseModel, "email/list", successOnTemplateList, errorOnTemplateList);
        }

        var successOnTemplateList = function (response) {
            $ctrl.TemplateList = response;
            $ctrl.NewList =[];
            var j = 0;
            for (i = 0; i < $ctrl.TemplateList.length; i++)
            {
                if ($ctrl.TemplateList[i].ParentID == null)
                {
                     
                }
                else
                {
                    var obj = new Object();
                    obj.Child = $ctrl.TemplateList[i].Name;
                    obj.Parent = "";
                    obj.EmailBody = $ctrl.TemplateList[i].Body;
                    obj.EmailSubject = $ctrl.TemplateList[i].Subject;
                    $ctrl.NewList[j] = obj;
                    $ctrl.ID = $ctrl.TemplateList[i].ParentID;
                    for(k=0; k< $ctrl.TemplateList.length; k++)
                    {
                        if($ctrl.ID==$ctrl.TemplateList[k].ID)
                        {
                            $ctrl.NewList[j].Parent = $ctrl.TemplateList[k].Name;
                            
                        }
                    }
                    j++;
                }

            }
            
          

        }
        var errorOnTemplateList = function (message) {
            //Error handling goes here.
            toastr.error($filter('translator')('ERRORDBOPERATION'));
        }

        $ctrl.getEmailSubjectBodyList = function () {
            $ctrl.model.EmailBody = $ctrl.model.EmailTemplateList.EmailBody;
            $ctrl.model.EmailSubject = $ctrl.model.EmailTemplateList.EmailSubject;
            
        }

        $ctrl.LoadInitialData = function () {
            var BaseModel = new Object();
            BaseModel.CurrentUserID = currentUser.CurrentUserID;
            BaseModel.CurrentCulture = currentCulture;
            BaseModel.ApplicationId = appSettings.ApplicationId;
            ajaxService.AjaxPostWithData(BaseModel, "email/list", successOnTemplateList, errorOnTemplateList);
            
        }

    }

