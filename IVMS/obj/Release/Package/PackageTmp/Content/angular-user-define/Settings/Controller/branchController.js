angular.module("app").controller('branchController', function ($scope, $cookieStore, settingsRepository, securityRepository) {


    $scope.branch = {};
    $scope.branchList = [];

    $scope.LoadDefault = function () {

        $scope.loadCompany();
        $scope.branchList();
        $scope.branch = $cookieStore.get('editBranchData');
    }
    $scope.loadCompany = function () {
        $scope.companyList = [];
        securityRepository.loadCompany().then(function (response) {
            if (response.data) {
                $scope.companyList = response.data;
            }
        }).catch(function (response) {
            toastr.warning('No Data Found!!');
        });
    }
    $scope.saveBranch = function () {

        if ($scope.branchForm.$valid) {
            var data = settingsRepository.saveBranch($scope.branch).then(function (response) {
                if (response.data.isSucess) {
                    toastr.success(response.data.message);
                    $scope.branch = {};
                    $cookieStore.put('editBranchData', $scope.branch);
                } else {
                    toastr.error(response.data.message);
                }
            })

        }
        else {
            toastr.error("Please fill the required field !!");
        }
    }


    $scope.branchList = function () {
        var data = settingsRepository.branchList().then(function (response) {

            if (response.data) {
                $scope.branchList = response.data;
            }

        });

    }

    $scope.editRow = function (row) {
        window.location.replace("#!/BranchCreate");
        $scope.branch = row;
        $cookieStore.put('editBranchData', $scope.branch);
    }

    $scope.clearData = function () {
        $scope.branch = {};
        $scope.branch = $cookieStore.put('editBranchData');
    }
})