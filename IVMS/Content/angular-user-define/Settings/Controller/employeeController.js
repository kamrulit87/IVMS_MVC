angular.module("app").controller('employeeController', function ($scope, $filter,$cookieStore, settingsRepository, appointmentRepository, securityRepository) {


    $scope.emp = {};
    $scope.branchlist = [];
    $scope.empID = '';

    $scope.loadDefault = function () {
        $scope.empList();
        $scope.deptList();        
        $scope.getBranchList();
        $scope.designationList();
        $scope.loadUserGroup();
        $scope.emp = $cookieStore.get('editEmpData');
        if ($scope.emp.OnBehalfEmployeeID != null) {
            $scope.getDepWiseDropdownEmployee();
            $scope.empOnbeHalf = true;
        } else {
            $scope.empOnbeHalf = false;
        }
    }

    $scope.loadUserGroup = function () {
        var loadDropdownUserGroup = securityRepository.getDropdownUserGroupData();
        loadDropdownUserGroup.then(function (response) {
            if (response.data) {
                $scope.userGroupList = response.data;
            }
        });
    }

    $scope.saveEmp = function () {
        debugger
        if ($scope.beHalf == "BEHALF") {
            if ($scope.beHalfEmpList.length < 1) {
                toastr.error("Please Add Your Employee!!");
                return;
            }

            if ($scope.emp.OnBehalfEmployeeID > 0) {
                toastr.error("Please Click ON (+) Sign To Add On Behalf Employee!!");
                return;
            }

            if ($scope.isCreateUser & !$scope.user.UserGroupID > 0) {
                toastr.error("Please Select User Group!!");
                return;
            }

        }
        if ($scope.empForm.$valid) {
            
            var data = settingsRepository.saveEmp($scope.emp, $scope.beHalfEmpList, $scope.user).then(function (response) {
                debugger
                if (response.data.isSucess) {
                    toastr.success(response.data.message);
                    $scope.emp = {};
                    $cookieStore.put('editEmpData', $scope.emp);
                    $scope.emp.BranchID = $scope.branchlist[0].BranchID;
                } else {
                    toastr.error(response.data.message);
                }
            })

        }
        else {
            toastr.error("Please fill the required field !!");
        }


    }
    $scope.empOnbeHalf = false;
    $scope.empSubmitDisabled = false;
    $scope.getOnBeHalfEmp = function (did) {
        debugger
        var dlist = $filter('filter')($scope.designationList, function (d) { return d.DesignationID === did; })[0];
        if (dlist.Code == "BEHALF") {
            $scope.beHalf = dlist.Code;
            $scope.empOnbeHalf = true;
            $scope.empSubmitDisabled = true;
        } else {
            $scope.empOnbeHalf = false;
            $scope.emp.OnBehalfEmployeeID = null;
            $scope.empSubmitDisabled = false;
            $scope.beHalf = "";
        }

    }
    $scope.deptList = function () {
        $scope.deptList = [];
        var data = settingsRepository.deptList().then(function (response) {

            if (response.data) {
                $scope.deptList = response.data;
            }
        });
    }



    $scope.getDepWiseDropdownEmployee = function () {
        if ($scope.emp.DepartmentID > 0) {
            appointmentRepository.getDepWiseEmployee($scope.emp.DepartmentID).then(function (response) {
                if (response.data) {
                    debugger
                    $scope.depEmployeeList = response.data;               

                }
            });
        } else {
            $scope.depEmployeeList = [];
        }

    }
    $scope.designationList = function () {
        $scope.designationList = [];
        var data = settingsRepository.designationList().then(function (response) {

            if (response.data) {
                $scope.designationList = response.data;
            }


        });

    }

    $scope.getBranchList = function () {
        var data = settingsRepository.branchList().then(function (response) {
            if (response.data) {
                debugger
                
                $scope.branchlist = response.data;

                var tmp = $cookieStore.get('editEmpData');
                if (!tmp) {
                    $scope.emp = {};
                    $scope.emp.BranchID = $scope.branchlist[0].BranchID;
                }
               
            }

        })


    }

    $scope.empList = function () {
        $scope.empList = [];
        var data = settingsRepository.empList().then(function (response) {
            if (response.data) {
                $scope.empList = response.data;
            }
        }); 
    }

    $scope.viewOnBehalf = function (empID) {
        debugger
        $scope.onBehalfEmpList = [];
        var data = settingsRepository.onBehalfEmpList(empID).then(function (response) {
            if (response.data) {
                $scope.onBehalfEmpList = response.data;
            }
        });
    }

    $scope.isModal = false;
    $scope.modalOpen = function (empID) {
        //$scope.empID = empID;
        $scope.viewOnBehalf(empID);
        $scope.isModal = !$scope.isModal;
    }

    $scope.modalClose = function () {
        $scope.isModal = !$scope.isModal;
    }

    $scope.editRow = function (row) {
        
        window.location.replace("#!/EmployeeCreate");
        $scope.emp = row;
        $scope.getDepWiseDropdownEmployee();
     
        $cookieStore.put('editEmpData', $scope.emp);
    }

    $scope.userInfo = false;
    $scope.showUserInfo = function () {
        $scope.user = {};
        $scope.userInfo = false;
        if ($scope.isCreateUser) {
            $scope.userInfo = true;            
            $scope.user.UserName = $scope.emp.EmpCode;
            $scope.user.Password = 'admin123';
            $scope.user.ConfirmPassword = 'admin123';
        } 
    }



    $scope.clearData = function () {
        $scope.emp = {};
        $scope.emp = $cookieStore.put('editEmpData');
    }
    //Add Employee
    $scope.multiEmployeeList = [];
    $scope.beHalfEmpList = [];

    $scope.addEmployee = function (index, emp) {
        debugger

        var employee = $filter('filter')($scope.depEmployeeList, function (d) { return d.EmployeeID === emp.OnBehalfEmployeeID; })[0];

        if (employee.Code == 'BEHALF') {
            toastr.error("You can not add any On Behalf Employee !!");
            return;
        }

        if (emp.OnBehalfEmployeeID > 0) {
            $scope.empSubmitDisabled = false;
            if ($scope.multiEmployeeList.length > 0) {
                for (var i = 0; i < $scope.multiEmployeeList.length; i++) {
                    if ($scope.multiEmployeeList[i].OnBehalfEmployeeID == emp.OnBehalfEmployeeID) {
                        toastr.error("All Ready Exsit!!");
                        return;
                    }
                }
                $scope.multiEmployeeList.push({
                    OnBehalfEmployeeID: emp.OnBehalfEmployeeID,
                    selected: true
                });
                $scope.beHalfEmpList.push(emp.OnBehalfEmployeeID);
                emp.OnBehalfEmployeeID = 0;
            } else {
                $scope.multiEmployeeList.push({
                    OnBehalfEmployeeID: emp.OnBehalfEmployeeID,
                    selected: true
                });
                $scope.beHalfEmpList.push(emp.OnBehalfEmployeeID);
            }
            emp.OnBehalfEmployeeID = 0;
        }
    }

    $scope.removeEmployee = function (index, emp) {
        $scope.multiEmployeeList.splice(index, 1);
        if ($scope.multiEmployeeList.length < 1) {
            $scope.empSubmitDisabled = true;
        }
    }

});