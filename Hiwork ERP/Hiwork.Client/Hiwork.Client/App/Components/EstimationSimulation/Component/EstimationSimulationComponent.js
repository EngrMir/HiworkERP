
angular.module('hiworkApp').component('estimationSimulation', {

    templateUrl: 'App/Components/EstimationSimulation/Template/EstimationSimulation.html',
    bindings: { modalInstance: "<", resolve: "<" },
    controller: ["$rootScope", "sessionFactory", "AppStorage", "appSettings", ctrlEstimationSimulation]
});


function ctrlEstimationSimulation($rootScope, sessionFactory, AppStorage, appSettings) {

    var $ctrl = this;
    $ctrl.DetailsList = [];
    $ctrl.Number = [0, 1, 2, 3, 4];
    $ctrl.Currency;

    $ctrl.$onInit = function () {
        var Details;
        var i, PhaseCount;
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.Currency = $ctrl.resolve.modalData.Currency;
        var EstimationItems = $ctrl.resolve.modalData.EstimationDetailsList;

        for (i = 0; i < EstimationItems.length; i += 1) {
            Details = new Object();
            Details.UnitPrice = new Array(5);
            Details.PageCount = new Array(5);
            Details.Discount = new Array(5);
            Details.TotalAmount = new Array(0, 0, 0, 0, 0);
            Details.UnitPriceAfter = new Array(0, 0, 0, 0, 0);
            Details.TotalEstimation = new Array(0, 0, 0, 0, 0);
            Details.StaffPayment = new Array(0, 0, 0, 0, 0);
            Details.CostPercentage = new Array(0, 0, 0, 0, 0);
            Details.Cost = new Array(0, 0, 0, 0, 0);
            Details.ProfitPercentage = new Array(0, 0, 0, 0, 0);
            Details.Profit = new Array(0, 0, 0, 0, 0);
            Details.Phase = new Array(false, false, false, false, false);
            Details.PhaseNumber = new Array(-1, -1, -1, -1, -1);
            Details.First = -1;
            Details.Last = -1;
            
            PhaseCount = 0;
            if (EstimationItems[i].UnitPrice1 > 0) {
                Details.UnitPrice[0] = EstimationItems[i].UnitPrice1;
                Details.PageCount[0] = EstimationItems[i].PageCount1;
                Details.Discount[0] = EstimationItems[i].Discount1;
                Details.Phase[0] = true;
                Details.PhaseNumber[0] = 0;
                PhaseCount += 1;
            }
            if (EstimationItems[i].UnitPrice2 > 0) {
                Details.UnitPrice[1] = EstimationItems[i].UnitPrice2;
                Details.PageCount[1] = EstimationItems[i].PageCount2;
                Details.Discount[1] = EstimationItems[i].Discount2;
                Details.Phase[1] = true;
                Details.PhaseNumber[1] = 1;
                PhaseCount += 1;
            }
            if (EstimationItems[i].UnitPrice3 > 0) {
                Details.UnitPrice[2] = EstimationItems[i].UnitPrice3;
                Details.PageCount[2] = EstimationItems[i].PageCount3;
                Details.Discount[2] = EstimationItems[i].Discount3;
                Details.Phase[2] = true;
                Details.PhaseNumber[2] = 2;
                PhaseCount += 1;
            }
            if (EstimationItems[i].UnitPrice4 > 0) {
                Details.UnitPrice[3] = EstimationItems[i].UnitPrice4;
                Details.PageCount[3] = EstimationItems[i].PageCount4;
                Details.Discount[3] = EstimationItems[i].Discount4;
                Details.Phase[3] = true;
                Details.PhaseNumber[3] = 3;
                PhaseCount += 1;
            }
            if (EstimationItems[i].UnitPrice5 > 0) {
                Details.UnitPrice[4] = EstimationItems[i].UnitPrice5;
                Details.PageCount[4] = EstimationItems[i].PageCount5;
                Details.Discount[4] = EstimationItems[i].Discount5;
                Details.Phase[4] = true;
                Details.PhaseNumber[4] = 4;
                PhaseCount += 1;
            }
            Details.PhaseCount = PhaseCount;
            Details.SourceLanguageName = EstimationItems[i].SourceLanguageName;
            Details.TargetLanguageName = EstimationItems[i].TargetLanguageName;
            $ctrl.DetailsList.push(Details);
        }

        $ctrl.SimulateEstimation();
    };


    $ctrl.SimulateEstimation = function () {
        for (i = 0; i < $ctrl.DetailsList.length; i += 1) {
            for (j = 0; j < 5; j += 1) {
                if ($ctrl.DetailsList[i].Phase[j] == true) {
                    if ($ctrl.DetailsList[i].First == -1) {
                        $ctrl.DetailsList[i].First = j;
                    }
                    if ($ctrl.DetailsList[i].Last < j) {
                        $ctrl.DetailsList[i].Last = j;
                    }
                    $ctrl.DetailsList[i].TotalAmount[j] = $ctrl.DetailsList[i].UnitPrice[j] * $ctrl.DetailsList[i].PageCount[j];
                    $ctrl.DetailsList[i].UnitPriceAfter[j] = $ctrl.DetailsList[i].UnitPrice[j] -
                                                            ($ctrl.DetailsList[i].Discount[j] / 100 * $ctrl.DetailsList[i].UnitPrice[j]);
                    $ctrl.DetailsList[i].TotalEstimation[j] = $ctrl.DetailsList[i].TotalAmount[j] -
                                                        ($ctrl.DetailsList[i].TotalAmount[j] * $ctrl.DetailsList[i].Discount[j] / 100);
                    $ctrl.DetailsList[i].CostPercentage[j] = $ctrl.DetailsList[i].StaffPayment[j] / $ctrl.DetailsList[i].UnitPriceAfter[j] * 100;
                    $ctrl.DetailsList[i].Cost[j] = $ctrl.DetailsList[i].StaffPayment[j] * $ctrl.DetailsList[i].PageCount[j];
                    $ctrl.DetailsList[i].ProfitPercentage[j] = 100 - $ctrl.DetailsList[i].CostPercentage[j];
                    $ctrl.DetailsList[i].Profit[j] = $ctrl.DetailsList[i].TotalEstimation[j] - $ctrl.DetailsList[i].Cost[j];
                }
            }
        }
    };


    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}