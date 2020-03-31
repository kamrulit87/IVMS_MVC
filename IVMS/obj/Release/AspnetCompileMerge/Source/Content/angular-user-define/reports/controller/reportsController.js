angular.module('app').controller('reportsController', function ($scope, $filter, $location, $route, $templateCache, $cookieStore, reportsRepository, settingsRepository, commonRepository) {

    //start :: ng-idel => when user idel specific time then it will be auto logout
    $scope.$on('IdleStart', function () {
        toastr.warning('your session will be end soon!!');
    });

    $scope.$on('IdleTimeout', function () {
        window.location = "/Security/LogOff#!/";
    });
    //End :: ng-idel
    //Start :: Common Function
    $scope.Reload = function () {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }
    //End :: Common Function
    $scope.inOut = {};
    $scope.loadDropdowns = function () {
        $scope.loadCompany();
        $scope.loadDepartment();
    }
    //Reports
    $scope.loadCompany = function () {
        reportsRepository.loadCompany().then(function (response) {
            if (response.data) {
                $scope.companyList = response.data;
                var company = $filter('filter')($scope.companyList, function (d) { return d.Code === "NCC" })[0];
                $scope.inOut.CompanyID = company.CompanyID;
                $scope.loadCompanyBranch();
            }
        }).catch(function (response) {
            toastr.warning('No Data found!!');
        });
    }
    $scope.loadCompanyBranch = function () {
        reportsRepository.loadCompanyWiseBranch($scope.inOut.CompanyID).then(function (response) {
            if (response.data) {
                $scope.branchList = response.data;
                $scope.inOut.BranchID = $scope.branchList[0].BranchID;
            }
        }).catch(function (response) {
            toastr.warning('No Data found!!');
        });
    }
    $scope.loadDepartment = function () {
        $scope.deptList = [];
        var data = settingsRepository.deptList().then(function (response) {
            if (response.data) {
                $scope.deptList = response.data;
            }

        });
    }
    $scope.deptWiseEmployee = function (deptID) {
        var data = reportsRepository.deptWiseEmployee(deptID).then(function (response) {
            if (response.data) {
                $scope.employeeList = response.data;
            }

        });
    }
    //Visitor In Out
    $scope.getDeptName = function (id) {
        debugger
        var dept = $filter('filter')($scope.deptList, function (d) { return d.DepartmentID === id })[0];
        $scope.DepartmentName = dept.DepartmentName;
    }
    $scope.getEmpName = function (id) {
        debugger 
        var dept = $filter('filter')($scope.employeeList, function (d) { return d.EmployeeID === id })[0];
        $scope.EmpName = dept.EmpName;
    }
    $scope.visitorINOutReport = function () {
        if ($scope.inOut.TDate && $scope.inOut.FDate) {
            var url = '/Reports/VisitorReport/VisitorINOutReport';
            $scope.parameters = { FDate: $scope.inOut.FDate, TDate: $scope.inOut.TDate, DepartmentID: $scope.inOut.DepartmentID, EmployeeID: $scope.inOut.EmployeeID, DepartmentName: $scope.DepartmentName, EmpName: $scope.EmpName }
            commonRepository.viewComMultiParameterDocument($scope.parameters, 'Do you want to view this Report ?', url);
        } else {
            toastr.error("Please fill-up all required field !!!");
        }

    }
});
