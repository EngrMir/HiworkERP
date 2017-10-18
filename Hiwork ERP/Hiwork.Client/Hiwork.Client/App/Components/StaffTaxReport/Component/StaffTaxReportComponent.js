angular.module("hiworkApp").component("stafftaxreport", {

    templateUrl: 'app/Components/StaffTaxReport/Template/StaffTaxReport.html',
    controller: stafftaxreportController

})

function stafftaxreportController($scope) {

    $scope.$on("selectedstaff", function (event, response) {
        $scope.vm.StaffMemberID = response[0].ID;
        $scope.vm.StaffMemberName = response[0].Name;
    });
}