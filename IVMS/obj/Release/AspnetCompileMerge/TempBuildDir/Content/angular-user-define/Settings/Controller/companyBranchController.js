angular.module('app').controller('companyBranchController', function ($scope, $filter, $location, $route, $templateCache, $cookieStore, settingsRepository, commonRepository) {

    //start :: ng-idel => when user idel specific time then it will be auto logout
    $scope.$on('IdleStart', function () {
        toastr.warning('your session will be end soon!!');
    });

    $scope.$on('IdleTimeout', function () {
        window.location = "/Security/LogOff#!/";
    });
    //End :: ng-idel
    //Start :: Common Function
    $scope.removeCookies = function () {
        $scope.companyBranch = {};
        $cookieStore.put('companyBranch', $scope.companyBranch);
    }
    $scope.Reload = function () {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }
    //End :: Common Function
    $scope.companyBranch = {};
    $scope.loadDropdowns = function () {
        $scope.loadCompany();
        $scope.statusList = commonRepository.getAStatus();
        $scope.companyBranch = $cookieStore.get('companyBranch');
    }
    $scope.loadCompany = function () {
        settingsRepository.loadAllCompany().then(function (response) {
            if (response.data) {
                $scope.companyList = response.data;
                var company = $filter('filter')($scope.companyList, function (d) { return d.Code === "NCC" })[0];
                $scope.companyBranch.CompanyID = company.CompanyID;
            }
        }).catch(function (response) {
            toastr.warning('No Data found!!');
        });
    }

    $scope.saveCompanyBranch = function () {
        debugger
        if ($scope.CreateBranchForm.$valid) {
            var createpage = settingsRepository.saveCompanyBranch($scope.companyBranch);
            createpage.then(function (response) {
                if (response.data.isSucess) {
                    toastr.success(response.data.message);
                    $scope.clearData();
                } else {
                    if (response.data.message === "LogOut") {
                        $location.path('#!/LogIn');
                    }
                    toastr.error(response.data.message);
                }
            }).catch(function (response) {
                toastr.error($scope.name += response.data + '!!');
            });
        } else {
            toastr.error("All input field are not valid");
        }
    }
    $scope.editRow = function (row) {
        $cookieStore.put('companyBranch', row);
        $location.path('/CreateCompanyBranch');
    }

    $scope.loadCompanyBranch = function () {
        settingsRepository.loadCompanyBranch().then(function (response) {
            if (response.data) {
                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].CreatedDate) {
                        var createdDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(createdDate, "dd-MMM-yyyy");
                    }
                }
                $scope.companyBranchList = response.data;
            }
        }).catch(function (response) {
            toastr.warning('No Data found!!');
        });
    }
    $scope.clearData = function () {
        debugger 
        $scope.companyBranch = {};
        $scope.Reload();
        $cookieStore.put('companyBranch', $scope.companyBranch);
    }

});
