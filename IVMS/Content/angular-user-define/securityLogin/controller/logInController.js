angular.module('app').controller('logInController', function ($scope, $filter, $cookies, $location, securityRepository, settingsRepository) {
    //start :: ng-idel => when user idel specific time then it will be auto logout
    $scope.$on('IdleStart', function () {
        toastr.warning('your session will be end soon!!');
    });

    $scope.$on('IdleTimeout', function () {
        window.location = "/Security/LogOff#!/";
    });

    $scope.login = {};

    

    $scope.loadDropdowns = function () {
        $scope.loadCompany(); 
        if ($scope.login.CompanyID = $cookies.get('CompanyID')) {
            $scope.companyWiseBranch();
        }
        $scope.login.BranchID = $cookies.get('BranchID');        
    }

    $scope.loadCompany = function () {
        $scope.companyList = [];
        settingsRepository.loadAllCompany().then(function (response) {
            if (response.data) {
                $scope.companyList = response.data;
                
                var company = $filter('filter')($scope.companyList, function (d) { return d.Code === "NCC"; })[0];
                $scope.login.CompanyID = company.CompanyID;
                $scope.companyWiseBranch();
            }
        }).catch(function (response) {
            toastr.warning('No Data Found!!');
        });
    }

    $scope.companyWiseBranch = function () {
        var branchesList = settingsRepository.loadCompanyWiseAllBranch($scope.login.CompanyID);
        branchesList.then(function (response) {
            if (response.data) {
                $scope.branchList = response.data;
                $scope.login.BranchID = $scope.branchList[0].BranchID
            }
            
        }).catch(function (response) {
            toastr.warning('No Data Found!!');
        });
    }
    $scope.onEnterKeyPress = function (event) {
        if (event.charCode == 13) //if enter is hot then call ValidateInputvalue().
            $scope.loginUser();
    }
   
    $scope.loginUser = function () {
        var loginStatus = securityRepository.logIn($scope.login);
        loginStatus.then(function (response) {
            if (response.data.success === true) {

                $cookies.put("CompanyID", $scope.login.CompanyID);
                $cookies.put("BranchID", $scope.login.BranchID);

                window.location.replace('/');
            } else {
                toastr.error(response.data.message);
            }
        });
    } 
       
    $scope.LogOut = function () {
        var logOff = securityRepository.logOff();
        logOff.then(function (data) {
            if (data.success === true) {
                window.location = "/";
            } else {
                window.location = "/";
            }
        });
    };

});
