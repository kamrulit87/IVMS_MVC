angular.module("app").controller('designationController', function ($scope, $cookieStore, settingsRepository) {


    $scope.designation = {};


    $scope.LoadDefault = function () {

        $scope.designationLists();
        $scope.designation = $cookieStore.get('editdesignationData');
    }

    $scope.saveDesignation = function () {
        if ($scope.designationForm.$valid) {
            var data = settingsRepository.saveDesignation($scope.designation).then(function (response) {
                if (response.data.isSucess) {
                    toastr.success(response.data.message);
                    $scope.designation = {};
                    $cookieStore.put('editdesignationData', $scope.designation);
                } else {
                    toastr.error(response.data.message);
                }
            })

        }
        else {
            toastr.error("Please fill the required field !!");
        }

    }


    $scope.designationLists = function () {
        $scope.designationList = [];
        var data = settingsRepository.designationList().then(function (response) {

            if (response.data) {
                $scope.designationList = response.data;
            }


        });

    }

    $scope.editRow = function (row) {
        window.location.replace("#!/DesignationCreate");
        $scope.designation = row;
        $cookieStore.put('editdesignationData', $scope.designation);
    }

    $scope.clearData = function () {
        $scope.designation = {};
        $scope.designation = $cookieStore.put('editdesignationData');
    }


})