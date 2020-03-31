angular.module("app").controller('departmentController', function ($scope, $cookieStore, settingsRepository) {


    $scope.dept = {};
   

    $scope.LoadDefault = function () {
        $scope.deptLists();
        $scope.dept = $cookieStore.get('editdeptData');
    }

    $scope.saveDept = function () {

        if ($scope.deptForm.$valid) {
            var data = settingsRepository.savedept($scope.dept).then(function (response) {
                if (response.data.isSucess) {
                    toastr.success(response.data.message);
                    $scope.dept = {};
                    $cookieStore.put('editdeptData', $scope.branch);
                } else {
                    toastr.error(response.data.message);
                }
            })

        }
        else {
            toastr.error("Please fill the required field !!");
        }


    }
    $scope.deptLists = function () {
        $scope.deptList = [];
        var data = settingsRepository.deptList().then(function (response) {

            if (response.data) {
                $scope.deptList = response.data;
            }

        });
    }

    $scope.editRow = function (row) {
        window.location.replace("#!/DepartmentCreate");
        $scope.branch = row;
        $cookieStore.put('editdeptData', $scope.branch);
    }

    $scope.clearData = function () {
        $scope.dept = {};
        $scope.dept = $cookieStore.put('editdeptData');
    }


})