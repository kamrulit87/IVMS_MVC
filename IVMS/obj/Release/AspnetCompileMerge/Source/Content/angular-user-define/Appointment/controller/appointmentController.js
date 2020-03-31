angular.module('app').controller('appointmentController', function ($scope, $http, $filter, $cookieStore, $window, appointmentRepository, commonRepository) {
    $scope.removeCookies = function () {
        $scope.appointment = {};
        $cookieStore.put('editDataAppointment', $scope.appointment);
    }

    // Start Tab
    $scope.tab = 1;
    $scope.setTab = function (tabId) {
        $scope.tab = tabId;
        if ($scope.tab == 2) {
            if (!$scope.appointment.AppointmentID) {
                $scope.appointment = {};
                $scope.getDepWiseFloor();                
                $scope.selfDeptEmployeeList = [];
                $scope.appointment.AppointmentDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
            }
        } else {
            $scope.loadSelfDepartment();
            $scope.getSelfDepWiseDropdownEmployee();
        }
        
    };
    $scope.isSet = function (tabId) {
        return $scope.tab === tabId;

    };
    // End Tab

    $scope.appointment = {};
    $scope.selfDeptEmployeeList = [];
    $scope.depEmployeeList = [];
    $scope.appointment.AppointmentDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');

    $scope.loadDropdowns = function () {
        $scope.getLoadStatus();
        $scope.getDtlsAppointment();
        $scope.loadDepartment();
        $scope.loadSelfDepartment();
        $scope.loadEmployee();
        //$scope.loadDepWiseFloor();
        $scope.appointment.AppointmentDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        $scope.appointment = $cookieStore.get('editDataAppointment');
        if ($scope.appointment.DepartmentID) {
            $scope.getDepWiseDropdownEmployee();
            $scope.getDepWiseEmployeeBehalf();
            $scope.getDepWiseEmployee();
        }
    }
    $scope.search={};
    $scope.loadDropdownsUnschedule = function () {
        //$scope.getLoadStatus();
        //$scope.getDtlsAppointment();
        debugger
        $scope.search.FromDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        $scope.search.ToDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        if(($scope.search.FromDate) && ( $scope.search.ToDate)){
            $scope.searchAppointment();
        }

        $scope.loadDepartment();
        //$scope.loadSelfDepartment();
        //$scope.loadEmployee();
        //$scope.loadDepWiseFloor();
        $scope.appointment.AppointmentDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
   
        $scope.appointment = $cookieStore.get('editDataAppointment');
        if ($scope.appointment.DepartmentID) {
            $scope.getDepWiseDropdownEmployee();
            $scope.getDepWiseEmployeeBehalf();
            $scope.getDepWiseEmployee();
        }
    }



    $scope.Reload = function () {
        $window.location.reload();
    }
    $scope.loadNotifyDropdowns = function () {
        $scope.loadNotifyList();
        $scope.loadEmployee();
        $scope.getDepWiseEmployee();
        $scope.loadCalendarDropdowns();
    }

    //Cookies Remove Function
    $scope.removeCookies = function () {
        $cookieStore.put('editDataAppointment', $scope.appointment);
    }
    $scope.loadScheduleDropdowns = function () {
        $scope.loadScheduleAppointment();
        $scope.loadEmployee();
    }
    $scope.loadUnScheduleDropdowns = function () {
        $scope.getDtlsAppointment();
        $scope.loadEmployee();

    }

    $scope.getLoadStatus = function () {
        $scope.statusList = commonRepository.getAppoStatus();
    }
    $scope.clearSearch = function () {
        $scope.search = {};
        $scope.appointmentList=[];
        $scope.appointment.DepartmentID = null;
    }
    $scope.clearData = function () {
     
        $scope.appointment = {};
        $scope.appointment.AppointmentDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        $scope.CheckFloor = [];
        $scope.multiEmployeeList = [];
        $scope.depEmployeeList = [];
        $scope.noOfFloorList = [];
        $cookieStore.put('editDataAppointment', $scope.appointment);
    }
    $scope.loadEmployee = function () {
        appointmentRepository.loadEmployee().then(function (response) {
            if (response.data) {
                $scope.employeeList = response.data;
            }
        });
    }
    $scope.loadDepartment = function () {        
        appointmentRepository.loadDepartment().then(function (response) {
            if (response.data) {
                debugger
                $scope.departmentList = response.data;
            }
        })
    }

    $scope.getDepWiseDropdownEmployee = function () {
        $scope.depEmployeeList = [];
        if ($scope.appointment.DepartmentID > 0) {
            appointmentRepository.getDepWiseEmployee($scope.appointment.DepartmentID).then(function (response) {
                if (response.data) {
                    debugger 
                    $scope.depEmployeeList = response.data;
                    if ($scope.appointment.EmployeeID) {
                        $scope.getDepWiseFloor();
                    }
                    
                }
            });
        } else {
            $scope.depEmployeeList = [];
        }

    }

    $scope.getDepWiseEmployeeBehalf = function () {
        $scope.depEmployeeList = [];
        if ($scope.appointment.DepartmentID > 0) {
            appointmentRepository.getDepWiseEmployeeBehalf($scope.appointment.DepartmentID).then(function (response) {
                if (response.data) {
                    debugger 
                    $scope.depEmployeeList = response.data;
                    if ($scope.appointment.EmployeeID) {
                        $scope.getDepWiseFloor();
                    }
                    
                }
            });
        } else {
            $scope.depEmployeeList = [];
        }

    }
    $scope.loadSelfDepartment = function () {
        $scope.selfDepartmentList = [];
        appointmentRepository.loadSelfDepartment().then(function (response) {
            if (response.data) {
                debugger
                $scope.selfDepartmentList = response.data;
                $scope.appointment.DepartmentID = $scope.selfDepartmentList[0].DepartmentID;
                $scope.getSelfDepWiseDropdownEmployee();
            }
        })
    }
    $scope.getSelfDepWiseDropdownEmployee = function () {
        $scope.selfDeptEmployeeList = [];
        if ($scope.appointment.DepartmentID > 0) {
            appointmentRepository.getSelfDepWiseDropdownEmployee($scope.appointment.DepartmentID).then(function (response) {
                if (response.data) {
                    debugger 
                    $scope.selfDeptEmployeeList = response.data;
                    $scope.appointment.EmployeeID = $scope.selfDeptEmployeeList[0].EmployeeID;
                    if ($scope.appointment.EmployeeID) {
                        $scope.getDepWiseFloor();
                    }
                    if (!$scope.appointment.AccessFloors) {
                        if ($scope.selfDeptEmployeeList.length > 0) {
                            var floorId = $filter('filter')($scope.selfDeptEmployeeList, function (d) { return d.EmployeeID === $scope.appointment.EmployeeID; })[0];
                            for (var j = 0; j < $scope.noOfFloorList.length; j++) {
                                if ($scope.noOfFloorList[j].id == floorId.Floor) {
                                    $scope.noOfFloorList[j].FloorID = true;
                                    $scope.changeFloor($scope.noOfFloorList[j].id);
                                }
                            }
                        }
                    } 
                }
            });
        } else {
            $scope.selfDeptEmployeeList = [];
        }

    }
    $scope.getDepWiseEmployee = function () {
        $scope.depEmployeeList = [];
        if ($scope.appointment.EmployeeID > 0) {
            appointmentRepository.getEmployeeWiseFloor($scope.appointment.EmployeeID).then(function (response) {
                if (response.data) {
                    debugger 
                    $scope.depEmployeeList = response.data;
                    if ($scope.appointment.EmployeeID) {
                        $scope.getDepWiseFloor();
                    }                    
                }
            });
        } else {
            $scope.depEmployeeList = [];
        }

    }
    //UnSchedule Appointment

    //Add Employee
    $scope.multiEmployeeList = [];

    $scope.addEmployee = function (index, emp) {
        debugger 
        if (emp.EmployeeID > 0) {
            if ($scope.multiEmployeeList.length > 0) {
                for (var i = 0; i < $scope.multiEmployeeList.length; i++) {
                    if ($scope.multiEmployeeList[i].EmployeeID == emp.EmployeeID) {
                        toastr.error("All Ready Exsit!!");
                        return;
                    }
                }
                $scope.multiEmployeeList.push({
                    EmployeeID: emp.EmployeeID,
                    AppointmentID: $scope.appointment.AppointmentID,
                    selected: true
                });
                emp.EmployeeID = 0;
            } else {
                $scope.multiEmployeeList.push({
                    EmployeeID: emp.EmployeeID,
                    AppointmentID: $scope.appointment.AppointmentID,
                    selected: true
                });
            }
            emp.EmployeeID = 0;

        }
    }

    $scope.removeEmployee = function (index, emp) {
        $scope.multiEmployeeList.splice(index, 1);
        for (var i = 0; i < $scope.CheckFloor.length; i++) {
            if ($scope.CheckFloor[i].EmployeeID == emp.EmployeeID) {
                $scope.noOfFloorList[$scope.CheckFloor[i].Floor].FloorID = false;
            }
        }
        for (var j = 0; j < $scope.CheckFloor.length; j++) {
            if ($scope.CheckFloor[j].EmployeeID == emp.EmployeeID) {
                $scope.CheckFloor.splice(j, 1);
                j--;
            }
        }
    }

    $scope.getDepWiseFloor = function () {
        if ($scope.appointment.EmployeeID > 0) {
            debugger
            appointmentRepository.getDepWiseFloor($scope.appointment.EmployeeID).then(function (response) {
                if (response.data) {
                    $scope.noOfFloorList = response.data;
                    for (var i = 0; i < $scope.noOfFloorList.length; i++) {
                        var noOfFloor = commonRepository.generateFloors($scope.noOfFloorList[i].NoOfFloor);
                    }
                    $scope.noOfFloorList = noOfFloor;
                    if ($scope.appointment.AccessFloors) {
                        $scope.floor = [];
                        var TimeWithoutAmPM = $scope.appointment.AccessFloors;
                        time = TimeWithoutAmPM.split(",");
                        for (var l = 0; l < time.length; l++) {
                            $scope.floor.push(time[l]);
                        }
                        if ($scope.floor.length > 0) {
                            for (var k = 0; k < $scope.floor.length; k++) {
                                if ($scope.floor[k] != "") {
                                    $scope.noOfFloorList[$scope.floor[k]].FloorID = true;
                                    $scope.changeFloor($scope.noOfFloorList[$scope.floor[k]].id);
                                }
                                
                            }
                            return;
                        }
                    }
                    debugger 
                    
                    if ($scope.selfDeptEmployeeList.length > 0) {
                        var floorId = $filter('filter')($scope.selfDeptEmployeeList, function (d) { return d.EmployeeID === $scope.appointment.EmployeeID; })[0];
                        for (var j = 0; j < $scope.noOfFloorList.length; j++) {
                            if ($scope.noOfFloorList[j].id == floorId.Floor) {
                                $scope.noOfFloorList[j].FloorID = true;
                                $scope.changeFloor($scope.noOfFloorList[j].id);
                            }

                        }
                    }

                    if ($scope.depEmployeeList.length > 0) {
                        var floorId = $filter('filter')($scope.depEmployeeList, function (d) { return d.EmployeeID === $scope.appointment.EmployeeID; })[0];
                        for (var j = 0; j < $scope.noOfFloorList.length; j++) {
                            if ($scope.noOfFloorList[j].id == floorId.Floor) {
                                $scope.noOfFloorList[j].FloorID = true;
                                $scope.changeFloor($scope.noOfFloorList[j].id);
                            }

                        }
                    }
                    //debugger
                    //if ($scope.multiEmployeeList.length > 0) {
                    //    if ($scope.CheckFloor.length > 0) {
                    //        for (var k = 0; k < $scope.CheckFloor.length; k++) {
                    //            $scope.noOfFloorList[$scope.CheckFloor[k].Floor].FloorID = true;
                    //        }
                    //    }
                    //}
                    
                }
            });
        } else {
            $scope.noOfFloorList = [];
        }

    }
    $scope.CheckFloor = [];
    $scope.checkedFloorID = function (emp) {
        debugger
        if (emp > 0) {
            for (var i = 0; i <= $scope.CheckFloor.length; i++) {
                if ($scope.CheckFloor[i] != null && $scope.CheckFloor[i].Floor == emp) {
                    $scope.CheckFloor.splice(i, 1);
                    return;
                }
            }
            $scope.CheckFloor.push({
                Floor: emp,
                EmployeeID: $scope.appointment.EmployeeID
            });

        }

    }

    $scope.changeFloor = function (emp) {
        debugger
        if (emp > 0) {
            for (var i = 0; i <= $scope.CheckFloor.length; i++) {
                if ($scope.CheckFloor[i] != null && $scope.CheckFloor[i].Floor == emp) {
                    $scope.CheckFloor[i].Floor = emp;
                    return;
                }
            }
            $scope.CheckFloor.push({
                Floor: emp,
                EmployeeID: $scope.appointment.EmployeeID
            });

        }

    }


    // Details Data
    $scope.getDtlsAppointment = function () {
        appointmentRepository.getDtlsAppointment().then(function (response) {
            if (response.data) {
                for (var i = 0; i < response.data.length; i++) {
                    if (response.data[i].AppointmentTime) {
                        var appointmentTime = response.data[i].AppointmentTime.replace('/Date(', '').replace(')/', '');
                        appointmentTime = $filter('date')(appointmentTime, "dd/MM/yyyy hh:mm a");
                        time = appointmentTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data[i].AppointmentTime = time;
                    }
                    if (response.data[i].AppointmentDate) {
                        var appointmentDate = response.data[i].AppointmentDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].AppointmentDate = $filter('date')(appointmentDate, "dd-MMM-yyyy");
                       // response.data[i].AppointmentDate += " " + (response.data[i].AppointmentTime == null ? "" : response.data[i].AppointmentTime);
                    }
               
                    if (response.data[i].ReachTime) {
                        var reachTime = response.data[i].ReachTime.replace('/Date(', '').replace(')/', '');
                        reachTime = $filter('date')(reachTime, "dd/MM/yyyy hh:mm a");
                        time = reachTime.split(" ");
                        time = time[1] + " " + time[2];
                        response.data[i].ReachTime = time;
                    }

                    if (response.data[i].EndTime) {
                        var endTime = response.data[i].EndTime.replace('/Date(', '').replace(')/', '');
                        endTime = $filter('date')(endTime, "dd/MM/yyyy hh:mm a");
                        time = endTime.split(" ");
                        time = time[1] + " " + time[2];
                        response.data[i].EndTime = time;
                    }


                    if (response.data[i].CreatedDate) {
                        var createdDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(createdDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].Status) {
                        response.data[i].Status = response.data[i].Status.trim();
                    }

                }
                $scope.appointmentDtlsList = response.data;
                for (var j = 0; j < $scope.appointmentDtlsList.length; j++) {
                    $scope.appointmentDtlsList[j].disabledInBtn = true;
                    $scope.appointmentDtlsList[j].disabledCancelBtn = true;

                    if ($scope.appointmentDtlsList[j].Status == 'N') {
                        $scope.appointmentDtlsList[j].disabledNotify = true;
                        $scope.appointmentDtlsList[j].showNotified = true;
                        $scope.appointmentDtlsList[j].hideNotify = true;   
                    }
                    if ($scope.appointmentDtlsList[j].Status == 'P') {
                        $scope.appointmentDtlsList[j].disabledNotify = true;
                        $scope.appointmentDtlsList[j].showPending = true;
                        $scope.appointmentDtlsList[j].showNotified = false;
                        $scope.appointmentDtlsList[j].hideNotify = true;
                    }
                    if ($scope.appointmentDtlsList[j].Status == 'AP') {
                        $scope.appointmentDtlsList[j].disabledNotify = true;
                        $scope.appointmentDtlsList[j].showPending = false;
                        $scope.appointmentDtlsList[j].showNotified = false;
                        $scope.appointmentDtlsList[j].showAccept = true;
                        $scope.appointmentDtlsList[j].hideNotify = true;
                        $scope.appointmentDtlsList[j].disabledInBtn = false;
                        $scope.appointmentDtlsList[j].disabledReBtn = true;
                    }                   
                    if ($scope.appointmentDtlsList[j].FirstWordMsg) {
                        var message = $scope.appointmentDtlsList[j].FirstWordMsg.split(" ");
                        message = message[0] + "...";
                        $scope.appointmentDtlsList[j].FirstWordMsg = message;
                    }
                    if ($scope.appointmentDtlsList[j].Status == 'A' || $scope.appointmentDtlsList[j].Status == 'N') {
                        $scope.appointmentDtlsList[j].disabledCancelBtn = false;
                    }
                    
                    
                }
                $scope.CardNo = "";
            }
        })
    }
    $scope.getDtlsCheckIn = function () {
        debugger
        appointmentRepository.getDtlsCheckIn().then(function (response) {
            if (response.data) {
                for (var i = 0; i < response.data.length; i++) {
                    if (response.data[i].AppointmentDate) {
                        var appointmentDate = response.data[i].AppointmentDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].AppointmentDate = $filter('date')(appointmentDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].AppointmentTime) {
                        var appointmentTime = response.data[i].AppointmentTime.replace('/Date(', '').replace(')/', '');
                        appointmentTime = $filter('date')(appointmentTime, "dd/MM/yyyy hh:mm a");
                        time = appointmentTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data[i].AppointmentTime = time;
                    }
                    if (response.data[i].ReachTime) {
                        var reachTime = response.data[i].ReachTime.replace('/Date(', '').replace(')/', '');
                        reachTime = $filter('date')(reachTime, "dd/MM/yyyy hh:mm a");
                        time = reachTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data[i].ReachTime = time;
                    }

                    if (response.data[i].EndTime) {
                        var endTime = response.data[i].EndTime.replace('/Date(', '').replace(')/', '');
                        endTime = $filter('date')(endTime, "dd/MM/yyyy hh:mm a");
                        time = endTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data[i].EndTime = time;
                    }
                    if (response.data[i].CreatedDate) {
                        var createdDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(createdDate, "dd-MMM-yyyy");
                    }
                }
                debugger 
                $scope.checkInDtlsList = response.data;
            }
        })
    }
    $scope.getDtlsCheckBreak = function () {
        debugger
        appointmentRepository.getDtlsCheckBreak().then(function (response) {
            if (response.data) {
                for (var i = 0; i < response.data.length; i++) {
                    if (response.data[i].AppointmentDate) {
                        var appointmentDate = response.data[i].AppointmentDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].AppointmentDate = $filter('date')(appointmentDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].AppointmentTime) {
                        var appointmentTime = response.data[i].AppointmentTime.replace('/Date(', '').replace(')/', '');
                        appointmentTime = $filter('date')(appointmentTime, "dd/MM/yyyy hh:mm a");
                        time = appointmentTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data[i].AppointmentTime = time;
                    }
                    if (response.data[i].ReachTime) {
                        var reachTime = response.data[i].ReachTime.replace('/Date(', '').replace(')/', '');
                        reachTime = $filter('date')(reachTime, "dd/MM/yyyy hh:mm a");
                        time = reachTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data[i].ReachTime = time;
                    }
                    if (response.data[i].EndTime) {
                        var endTime = response.data[i].EndTime.replace('/Date(', '').replace(')/', '');
                        endTime = $filter('date')(endTime, "dd/MM/yyyy hh:mm a");
                        time = endTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data[i].EndTime = time;
                    }
                    if (response.data[i].CreatedDate) {
                        var createdDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(createdDate, "dd-MMM-yyyy");
                    }
                }
                debugger 
                $scope.checkBreakDtlsList = response.data;
            }
        })
    }

    $scope.getDtlsCheckOut = function () {
        appointmentRepository.getDtlsCheckOut().then(function (response) {
            if (response.data) {
                for (var i = 0; i < response.data.length; i++) {
                    if (response.data[i].AppointmentDate) {
                        var appointmentDate = response.data[i].AppointmentDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].AppointmentDate = $filter('date')(appointmentDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].AppointmentTime) {
                        var appointmentTime = response.data[i].AppointmentTime.replace('/Date(', '').replace(')/', '');
                        appointmentTime = $filter('date')(appointmentTime, "dd/MM/yyyy hh:mm a");
                        time = appointmentTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data[i].AppointmentTime = time;
                    }

                    if (response.data[i].ReachTime) {
                        var reachTime = response.data[i].ReachTime.replace('/Date(', '').replace(')/', '');
                        reachTime = $filter('date')(reachTime, "dd/MM/yyyy hh:mm a");
                        time = reachTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data[i].ReachTime = time;
                    }

                    if (response.data[i].EndTime) {
                        var endTime = response.data[i].EndTime.replace('/Date(', '').replace(')/', '');
                        endTime = $filter('date')(endTime, "dd/MM/yyyy hh:mm a");
                        time = endTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data[i].EndTime = time;
                    }

                    if (response.data[i].CheckedInTime) {
                        var checkInTime = response.data[i].CheckedInTime.replace('/Date(', '').replace(')/', '');
                        checkInTime = $filter('date')(checkInTime, "dd/MM/yyyy hh:mm a");
                        time = checkInTime.split(" ");
                        time = time[1] + " " + time[2];
                        response.data[i].CheckedInTime = time;
                    }
                    if (response.data[i].BreakOutTime) {
                        var breakOutTime = response.data[i].BreakOutTime.replace('/Date(', '').replace(')/', '');
                        breakOutTime = $filter('date')(breakOutTime, "dd/MM/yyyy hh:mm a");
                        time = breakOutTime.split(" ");
                        time = time[1] + " " + time[2];
                        response.data[i].BreakOutTime = time;
                    }
                    if (response.data[i].BreakInTime) {
                        var breakInTime = response.data[i].BreakInTime.replace('/Date(', '').replace(')/', '');
                        breakInTime = $filter('date')(breakInTime, "dd/MM/yyyy hh:mm a");
                        time = breakInTime.split(" ");
                        time = time[1] + " " + time[2];
                        response.data[i].BreakInTime = time;
                    }

                    if (response.data[i].CheckedOutTime) {
                        var checkOutTime = response.data[i].CheckedOutTime.replace('/Date(', '').replace(')/', '');
                        checkOutTime = $filter('date')(checkOutTime, "dd/MM/yyyy hh:mm a");
                        time = checkOutTime.split(" ");
                        time = time[1] + " " + time[2];
                        response.data[i].CheckedOutTime = time;
                    }
                    if (response.data[i].CreatedDate) {
                        var createdDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(createdDate, "dd-MMM-yyyy");
                    }
                }
                $scope.checkOutDtlsList = response.data;
            }
        })
    }

    $scope.getDtlsCancel = function () {
        appointmentRepository.getDtlsCancel().then(function (response) {
            if (response.data) {
                for (var i = 0; i < response.data.length; i++) {
                    if (response.data[i].AppointmentDate) {
                        var appointmentDate = response.data[i].AppointmentDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].AppointmentDate = $filter('date')(appointmentDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].AppointmentTime) {
                        var appointmentTime = response.data[i].AppointmentTime.replace('/Date(', '').replace(')/', '');
                        appointmentTime = $filter('date')(appointmentTime, "dd/MM/yyyy hh:mm a");
                        time = appointmentTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data[i].AppointmentTime = time;
                    }
                    if (response.data[i].ReachTime) {
                        var reachTime = response.data[i].ReachTime.replace('/Date(', '').replace(')/', '');
                        reachTime = $filter('date')(reachTime, "dd/MM/yyyy hh:mm a");
                        time = reachTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data[i].ReachTime = time;
                    }

                    if (response.data[i].EndTime) {
                        var endTime = response.data[i].EndTime.replace('/Date(', '').replace(')/', '');
                        endTime = $filter('date')(endTime, "dd/MM/yyyy hh:mm a");
                        time = endTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data[i].EndTime = time;
                    }

                }
                $scope.cancelDtlsList = response.data;
            }
        })
    }


    // Show Message
    $scope.isShowMsgModal = false;
    $scope.showMsgModalClose = function () {
        $scope.isShowMsgModal = !$scope.isShowMsgModal;
    }

    $scope.showMessage = function (row) {
        $scope.appointment.ReplayMessage = row;
        $scope.isShowMsgModal = !$scope.isShowMsgModal;
    }
    //Notify IN
    $scope.isNotifyModal = false;
    $scope.notifyModalClose = function (row) {
        $scope.isNotifyModal = !$scope.isNotifyModal;
    }

    $scope.notifyRow = function (index, row) {
        debugger
        $scope.appointment = row;
        $scope.appointment.NotifyMessage = "Mr." + " " + row.VisitorName + " " + "wants to meet with you.";
        $scope.isNotifyModal = !$scope.isNotifyModal;
        $scope.indexId = index;
    }

    $scope.saveNotify = function () {
        debugger
        if ($scope.isValidNotify()) {
            $scope.appointment.ReplayMessage = "";
            $scope.appointment.Status = "N";
            appointmentRepository.visitorNotify($scope.appointment).then(function (response) {
                if (response.data.isSucess) {
                    toastr.success(response.data.message);
                    $scope.getDtlsAppointment();
                    $scope.clearData();
                    $scope.loadNotifyList();
                    $scope.isNotifyModal = !$scope.isNotifyModal;
                    debugger
                    $scope.appointmentDtlsList[$scope.indexId].disabledNotify = true;
                } else {
                    toastr.error(response.data.message);
                    $scope.isNotifyModal = $scope.isNotifyModal;
                }
            })
        } else {
            $scope.isNotifyModal = $scope.isNotifyModal;
        }
    }

    //Forward

    $scope.isForwardModal = false;
    $scope.forwardModalClose = function () {
        $scope.isForwardModal = !$scope.isForwardModal;
    };
    $scope.forwardScheduleRow = function (index, row) {
        debugger
        $scope.loadDepartment();
        $scope.appointment = row;
        $scope.appointment.DepartmentID = '';
        $scope.appointment.EmployeeID = '';
        $scope.appID = row.AppointmentID;
        $scope.isForwardModal = !$scope.isForwardModal;
        $scope.indexId = index;
        $scope.getDepWiseDropdownEmployee();
    }
    $scope.saveForward = function () {
        debugger
        if ($scope.isValidForward()) {
            $scope.appointment.Status = "I";
            $scope.appointment.AppointmentBy = "F";
            $scope.appointment.AppointmentID = 0;
            appointmentRepository.visitorForward($scope.appointment, $scope.appID).then(function (response) {
                if (response.data.isSucess) {
                    toastr.success(response.data.message);
                    $scope.loadScheduleAppointment();
                    $scope.clearData();
                    $scope.isForwardModal = !$scope.isForwardModal;
                    $scope.appointmentList[$scope.indexId].disabledForward = true;
                } else {
                    toastr.error(response.data.message);
                    $scope.isForwardModal = $scope.isForwardModal;
                }
            })
        } else {
            $scope.isForwardModal = $scope.isForwardModal;
        }
    }

    //Replay

    $scope.isReplayModal = false;
    $scope.replayModalClose = function () {
        $scope.isReplayModal = !$scope.isReplayModal;
    };
    $scope.replayRow = function (row) {
        debugger 
        $scope.appointment = row;
        $scope.isReplayModal = !$scope.isReplayModal;
    }
    $scope.saveAccept = function () {
        debugger
        if ($scope.isValidReplay()) {
            $scope.appointment.Status = "AP";
            appointmentRepository.acceptedNotify($scope.appointment).then(function (response) {
                if (response.data.isSucess) {
                    toastr.success(response.data.message);
                    $scope.loadNotifyList();
                    $scope.clearData();
                    $scope.isReplayModal = !$scope.isReplayModal;
                } else {
                    toastr.error(response.data.message);
                    $scope.isReplayModal = $scope.isReplayModal;
                }
            })
        } else {
            $scope.isReplayModal = $scope.isReplayModal;
        }
    }
    $scope.savePending = function () {
        debugger
        if ($scope.isValidReplay()) {
            $scope.appointment.Status = "P";
            appointmentRepository.pendingNotify($scope.appointment).then(function (response) {
                if (response.data.isSucess) {
                    toastr.success(response.data.message);
                    $scope.loadNotifyList();
                    $scope.clearData();
                    $scope.isReplayModal = !$scope.isReplayModal;
                } else {
                    toastr.error(response.data.message);
                    $scope.isReplayModal = $scope.isReplayModal;
                }
            })
        } else {
            $scope.isReplayModal = $scope.isReplayModal;
        }
    }

    //Check IN
    $scope.isCheckInModal = false;
    $scope.checkInClose = function () {
        $scope.isCheckInModal = !$scope.isCheckInModal;
    }

    $scope.breakModalClose = function () {
        $scope.isCheckInModal = !$scope.isCheckInModal;
    }

    $scope.checkInRow = function (row) {
        $scope.appointment = row;
        $scope.isCheckInModal = !$scope.isCheckInModal;
    }

    $scope.breakCheckInRow = function (row) {
        $scope.appointment = row;
        $scope.isCheckInModal = !$scope.isCheckInModal;
        $scope.isBreakCheckIn = true;
    }


    $scope.saveCheckedIn = function () {
        debugger
        if ($scope.isBreakCheckIn) {
            $scope.saveBreakIn();
        }
        else {
            $scope.firstCheckedIn();
        }

    }



    $scope.firstCheckedIn = function (row) {
        $scope.appointment.CardNO = $scope.appointment.BankCardID;
        if ($scope.isValidCheckIn()) {
            $scope.appointment.Status = "I";
            appointmentRepository.visitorCheckedIn($scope.appointment).then(function (response) {
                if (response.data.isSucess) {
                    toastr.success(response.data.message);
                    $scope.getDtlsAppointment();
                    $scope.clearData();
                    $scope.isCheckInModal = !$scope.isCheckInModal;
                } else {
                    if (response.data.message == "LogOut") {
                        $location.path('#!/Login')
                    }
                    toastr.error(response.data.message);
                }
            })
        } else {
            $scope.isCheckInModal = $scope.isCheckInModal;
        }
    }

    // Start :: Visitor Break Option
    //$scope.saveBreakOut = function (row) {
    //    debugger
    //    $scope.appointment.CardNO = $scope.appointment.BankCardID;        
    //    $scope.appointment.Status = "B";

    //    appointmentRepository.visitorBreakOut($scope.appointment).then(function (response) {
    //        if (response.data.isSucess) {
    //            toastr.success(response.data.message);
    //            $scope.getDtlsAppointment();
    //            $scope.clearData();
    //            $scope.checkOutModal = $scope.checkOutModal;
    //        } else {
    //            if (response.data.message == "LogOut") {
    //                $location.path('#!/Login')
    //            }
    //            toastr.error(response.data.message);
    //        }
    //    })
    //}

    $scope.saveBreakOut = function () {
        debugger
        $scope.appointment.CardNO = '';
        $scope.appointment.Status = "B";

        appointmentRepository.visitorBreakOut($scope.appointment).then(function (response) {
            if (response.data.isSucess) {
                $scope.breakModalClose();
                toastr.success(response.data.message);
                $scope.getDtlsAppointment();
                $scope.clearData();
            } else {
                if (response.data.message == "LogOut") {
                    $location.path('#!/Login')
                }
                toastr.error(response.data.message);
            }
        })
    }

    $scope.saveBreakIn = function (row) {
        $scope.appointment.CardNO = $scope.appointment.BankCardID;

        $scope.appointment.Status = "I";
        appointmentRepository.visitorBreakIn($scope.appointment).then(function (response) {
            if (response.data.isSucess) {
                toastr.success(response.data.message);
                $scope.getDtlsCheckBreak();
                $scope.clearData();
                $scope.isCheckInModal = !$scope.isCheckInModal;
            } else {
                if (response.data.message == "LogOut") {
                    $location.path('#!/Login')
                }
                toastr.error(response.data.message);
            }
        })
    }

    // End :: Visitor Break Option
    
    
    //Check Out list
    $scope.isCheckOutModal = false;
    $scope.checkOutClose = function () {
        $scope.isCheckOutModal = !$scope.isCheckOutModal;
    }
    $scope.cardWiseAppointmentData = function () {
        //if ($scope.CardNo != "" && $scope.CardNo.length == 10) {
            if ($scope.CardNo != "" && $scope.CardNo.length == 3) {
            appointmentRepository.cardWiseAppointmentData($scope.CardNo).then(function (response) {
                debugger
                if (response.data && response.data != null) {
                    
                    if (response.data.CreatedDate) {
                        var createdDate = response.data.CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data.CreatedDate = $filter('date')(createdDate, "dd-MMM-yyyy");
                    }
                    if (response.data.AppointmentDate) {
                        var appointmentDate = response.data.AppointmentDate.replace('/Date(', '').replace(')/', '');
                        response.data.AppointmentDate = $filter('date')(appointmentDate, "dd-MMM-yyyy");
                    }
                    if (response.data.AppointmentTime) {
                        var appointmentTime = response.data.AppointmentTime.replace('/Date(', '').replace(')/', '');
                        appointmentTime = $filter('date')(appointmentTime, "dd/MM/yyyy hh:mm a");
                        time = appointmentTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data.AppointmentTime = time;
                    }
                    if (response.ReachTime) {
                        var reachTime = response.data.ReachTime.replace('/Date(', '').replace(')/', '');
                        reachTime = $filter('date')(reachTime, "dd/MM/yyyy hh:mm a");
                        time = reachTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data.ReachTime = time;
                    }

                    $scope.appointment = response.data;
                    $scope.isCheckOutModal = !$scope.isCheckOutModal;
                } else {
                    toastr.error("No Checked IN Visitor Found Against This Card");
                }
                $scope.CardNo = "";
            })
        }
    }

    $scope.checkOutByCard = function () {        
        if ($scope.isValidCheckIn()) {
            debugger
            //if ($scope.appointment.CardNO != "" && $scope.appointment.CardNO.length == 10) {
            if ($scope.appointment.CardNO != "" && $scope.appointment.CardNO.length == 3) {
                appointmentRepository.visitorCheckedOut($scope.appointment.CardNO).then(function (response) {
                    if (response.data.isSucess) {
                        toastr.success(response.data.message);
                        $scope.isCheckOutModal = !$scope.isCheckOutModal;
                    } else {
                        if (response.data.message == "LogOut") {
                            $location.path('#!/Login')
                        }
                        toastr.error(response.data.message);
                    }
                    $scope.CardNo = "";
                })
            }
        } else {
            $scope.isCheckOutModal = $scope.isCheckOutModal;
            toastr.error("Please fill-up all required field !!!");
        }
        
    }

    // Break
    //$scope.saveCheckedBreak = function (row) {
    //    debugger
    //    if ($scope.isValidCheckOut()) {
    //        $scope.appointment.Status = "B";
    //        appointmentRepository.visitorCheckedBreak($scope.appointment).then(function (response) {
    //            if (response.data.isSucess) {
    //                toastr.success(response.data.message);
    //                $scope.isCheckOutModal = !$scope.isCheckOutModal;
    //            } else {
    //                if (response.data.message == "LogOut") {
    //                    $location.path('#!/Login')
    //                }
    //                toastr.error(response.data.message);
    //            }
    //        })
    //    } else {
    //        $scope.isCheckOutModal = $scope.isCheckOutModal;
    //        toastr.error("Please fill-up all required field !!!");
    //    }
    //}

    // Cancel UnSchedule
    $scope.isCancelModal = false;
    $scope.cancelModalClose = function () {
        $scope.isCancelModal = !$scope.isCancelModal;
    }

    //$scope.cancelRow = function (row) {
    //    $scope.appointment = row;
    //    $scope.isCancelModal = !$scope.isCancelModal;
    //}

    //$scope.saveUnScheduleAppointment = function () {
       
    //    if ($scope.appointmentForm.$valid) {
    //        if ($scope.CheckFloor.length > 0) {
    //            $scope.appointment.AccessFloors = "";
    //            for (var i = 0; i < $scope.CheckFloor.length; i++) {
    //                $scope.appointment.AccessFloors += $scope.CheckFloor[i].Floor + ",";
    //            }
    //        } else {
    //            toastr.error("Please Select Access Floor NO.");
    //            return
    //        }


    //        appointmentRepository.saveUnScheduleAppointment($scope.appointment).then(function (response) {
    //            if (response.data.isSucess) {
    //                toastr.success(response.data.message);
    //                $scope.getDtlsAppointment();


    //                $scope.clearData();
    //                $scope.isCancelModal = !$scope.isCancelModal;
    //            } else {
    //                if (response.data.message == "LogOut") {
    //                    $location.path('#!/Login');
    //                }
    //                toastr.error(response.data.message);
    //                $scope.isCancelModal = !$scope.isCancelModal;
    //            }
    //        });
    //    } else {
    //        toastr.error("Please fill-up all required field !!!");
    //    }
    //}



    //Cancel Schedule
    $scope.isCancelScheduleModal = false;
    $scope.cancelScheduleModalClose = function () {
        $scope.isCancelScheduleModal = !$scope.isCancelScheduleModal;
    }
    $scope.cancelScheduleRow = function (row) {
        debugger 
        $scope.appointment = row;
        $scope.isCancelScheduleModal = !$scope.isCancelScheduleModal;
    }
    $scope.cancelUnScheduleRow = function (row) {
        debugger 
        $scope.appointment = row;
        $scope.isCancelScheduleModal = !$scope.isCancelScheduleModal;
    }
    
    $scope.saveCancelSchedule = function () {
        debugger
        $scope.appointment.Status = "C";
        appointmentRepository.saveCancelAppointment($scope.appointment).then(function (response) {
            if (response.data.isSucess) {
                toastr.success(response.data.message);
                
                $scope.getDtlsAppointment();
                $scope.loadScheduleAppointment();
                $scope.clearData();
                debugger
                $scope.isCancelScheduleModal = !$scope.isCancelScheduleModal;
                
            } else {
                if (response.data.message == "LogOut") {
                    $location.path('#!/Login')
                }
                toastr.error(response.data.message);

            }
        });
    }
    

    //Schedule Appointment
    $scope.Alert = "nsm";
    $scope.saveScheduleAppointment = function () {

        $scope.appointment.AccessFloors = "";
        debugger
        $scope.appointment.ReachTime = $scope.appointment.AppointmentTime;
        if ($scope.appointmentForm.$valid) {
            appointmentRepository.checkMeetingTime($scope.appointment.AppointmentTime, $scope.appointment.AppointmentDate, $scope.appointment.EmployeeID).then(function (response) {
                if (response.data) {
                    debugger
                    for (var i = 0; i < response.data.length; i++) {
                        if (response.data[i].AppointmentTime) {
                            $scope.Alert = "sm";
                        }
                    }
                }
                debugger
                if ($scope.Alert == "sm") {
                    var isConfirmed = confirm("Another meeting is already set for this time, do you want to save any way ?");
                    if (isConfirmed) {
                       
                        if ($scope.CheckFloor.length > 0) {
                            for (var i = 0; i < $scope.CheckFloor.length; i++) {
                                $scope.appointment.AccessFloors += $scope.CheckFloor[i].Floor + ",";
                            }
                        } else {
                            toastr.error("Please Select Access Floor NO.");
                            return
                        }


                        if ($scope.appointmentForm.$valid) {
                            debugger
                            $scope.appointment.Status = "A";
                            $scope.appointment.AppointmentBy = "E";
                            //$scope.appointment.ReachTime = $scope.appointment.AppointmentTime;
                            appointmentRepository.saveScheduleAppointment($scope.appointment).then(function (response) {
                                if (response.data.isSucess) {
                                    toastr.success(response.data.message);
                                    $scope.clearData();
                                    $scope.Alert = "nsm";


                                } else {
                                    if (response.data.message == "LogOut") {
                                        $location.path('#!/Login')
                                    }
                                    toastr.error(response.data.message);
                                }
                            })
                        } else {
                            toastr.error("Please fill-up all required field !!!");
                        }

                    } else {
                        $scope.Alert = "nsm";
                    }
                }
                else {
                    if ($scope.appointmentForm.$valid) {

                        if ($scope.CheckFloor.length > 0) {
                            for (var i = 0; i < $scope.CheckFloor.length; i++) {
                                $scope.appointment.AccessFloors += $scope.CheckFloor[i].Floor + ",";
                            }
                        } else {
                            toastr.error("Please Select Access Floor NO.");
                            return
                        }

                        $scope.appointment.Status = "A";
                        $scope.appointment.AppointmentBy = "E";
                        appointmentRepository.saveScheduleAppointment($scope.appointment).then(function (response) {
                            if (response.data.isSucess) {
                                toastr.success(response.data.message);
                                $scope.clearData();
                                $scope.Alert = "nsm";

                            } else {
                                if (response.data.message == "LogOut") {
                                    $location.path('#!/Login')
                                }
                                toastr.error(response.data.message);
                            }
                        })
                    } else {
                        toastr.error("Please fill-up all required field !!!");
                    }
                    $scope.Alert = "nsm";
                }
            })
        } else {
            toastr.error("Please fill-up all required field !!!");
        }
    }

    $scope.loadScheduleAppointment = function () {
        appointmentRepository.loadScheduleAppointment().then(function (response) {
            if (response.data) {
                for (var i = 0; i < response.data.length; i++) {
                    if (response.data[i].AppointmentDate) {
                        var appointmentDate = response.data[i].AppointmentDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].AppointmentDate = $filter('date')(appointmentDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].CreatedDate) {
                        var CreatedDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(CreatedDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].AppointmentTime) {
                        var appointmentTime = response.data[i].AppointmentTime.replace('/Date(', '').replace(')/', '');
                        appointmentTime = $filter('date')(appointmentTime, "dd/MM/yyyy hh:mm a");
                        time = appointmentTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data[i].AppointmentTime = time;
                    }
                    if (response.data[i].ReachTime) {
                        var reachTime = response.data[i].ReachTime.replace('/Date(', '').replace(')/', '');
                        reachTime = $filter('date')(reachTime, "dd/MM/yyyy hh:mm a");
                        time = reachTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data[i].ReachTime = time;
                    }
                    if (response.data[i].EndTime) {
                        var endTime = response.data[i].EndTime.replace('/Date(', '').replace(')/', '');
                        endTime = $filter('date')(endTime, "dd/MM/yyyy hh:mm a");
                        time = endTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data[i].EndTime = time;
                    }
                    if (response.data[i].Status) {
                        response.data[i].Status = response.data[i].Status.trim();
                    }
                }
                debugger 
                $scope.appointmentList = response.data;

                for (var j = 0; j < $scope.appointmentList.length; j++) {
                    $scope.appointmentList[j].disabledForward = true;
                    if ($scope.appointmentList[j].Status == 'I' || $scope.appointmentList[j].Status == 'B') {
                        $scope.appointmentList[j].disabledForward = false;
                        $scope.appointmentList[j].hideForward = false;
                        $scope.appointmentList[j].dsiableChe = false;
                        $scope.appointmentList[j].disabledInBtn = true;
                    }                 
                }
            }
        })
    }

    $scope.loadNotifyList = function () {
        debugger
        appointmentRepository.loadNotifyList().then(function (response) {
            if (response.data) {
                for (var i = 0; i < response.data.length; i++) {
                    if (response.data[i].AppointmentDate) {
                        var appointmentDate = response.data[i].AppointmentDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].AppointmentDate = $filter('date')(appointmentDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].AppointmentTime) {
                        var appointmentTime = response.data[i].AppointmentTime.replace('/Date(', '').replace(')/', '');
                        appointmentTime = $filter('date')(appointmentTime, "dd/MM/yyyy hh:mm a");
                        time = appointmentTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data[i].AppointmentTime = time;
                    }
                    if (response.data[i].ReachTime) {
                        var reachTime = response.data[i].ReachTime.replace('/Date(', '').replace(')/', '');
                        reachTime = $filter('date')(reachTime, "dd/MM/yyyy hh:mm a");
                        time = reachTime.split(" ");

                        time = time[1] + " " + time[2];
                        response.data[i].ReachTime = time;
                    }

                    if (response.data[i].Status) {
                        response.data[i].Status = response.data[i].Status.trim();
                    }
                }
                $scope.notifyList = response.data;
                for (var j = 0; j < $scope.notifyList.length; j++) {
                    if ($scope.notifyList[j].Status == 'P') {
                        $scope.notifyList[j].hideReplay = true;
                        $scope.notifyList[j].showPending = true;
                    }
                }
            }
        })
    }
    // Edit Schedule
    $scope.isEditScheduleModal = false;
    $scope.editScheduleModalClose = function () {
        $scope.isEditScheduleModal = !$scope.isEditScheduleModal;
    }
    $scope.editScheduleRow = function (row) {
        $scope.appointment = row;
        $scope.isEditScheduleModal = !$scope.isEditScheduleModal;
    }
    $scope.editScheduleData = function () {
        $scope.isEditScheduleModal = !$scope.isEditScheduleModal;
        $scope.editRow($scope.appointment);
        
    }
    $scope.editRow = function (row) {
        debugger
        //window.location.replace("#!/ScheduleCreate");
        window.open('#!/ScheduleCreate', '_blank');
        $scope.appointment = row;
        $cookieStore.put('editDataAppointment', $scope.appointment);
    }
    //Edit UnSchedule
    $scope.isReUnScheduleModal = false;
    $scope.reUnScheduleModalClose = function () {
        $scope.isReUnScheduleModal = !$scope.isReUnScheduleModal;
    }
    $scope.reUnScheduleModal = function (row) {
        debugger
        $scope.appointment = row;
        $scope.isReUnScheduleModal = !$scope.isReUnScheduleModal;
    }
    $scope.reScheduleData = function () {
        debugger
        $scope.isReUnScheduleModal = !$scope.isReUnScheduleModal;
        $scope.editUnScheduleRow($scope.appointment);
        
    }
    $scope.editUnScheduleRow = function (row) {
        debugger
        if (row.AppointmentBy == 'R') {
            //window.location.replace("#!/UnScheduleCreate");
            window.open('#!/UnScheduleCreate', '_blank');
            $scope.appointment = row;
            if ($scope.appointment.DepartmentID) {
                $scope.getDepWiseEmployee();
            }
            $cookieStore.put('editDataAppointment', $scope.appointment);
        }
        if (row.AppointmentBy == 'E') {
            //window.location.replace("/ScheduleAppointment/ScheduleAppointmentCreate");
            //$scope.appointment = row;
            //$cookieStore.put('editDataAppointment', $scope.appointment);

            toastr.error("Not Re-Schedule!!");
        }

    }

    $scope.saveUnScheduleAppointment = function () {

        if ($scope.unScheduleForm.$valid) {
            $scope.appointment.Status = "A";
            $scope.appointment.AppointmentBy = "R";

            $scope.appointment.AccessFloors = "";
            if ($scope.CheckFloor.length > 0) {
                for (var i = 0; i < $scope.CheckFloor.length; i++) {
                    $scope.appointment.AccessFloors += $scope.CheckFloor[i].Floor + ",";
                }
            } else {
                toastr.error("Please Select Access Floor NO.");
                return
            }

            

            appointmentRepository.checkMeetingTime($scope.appointment.AppointmentTime, $scope.appointment.AppointmentDate, $scope.appointment.EmployeeID).then(function (response) {
                if (response.data) {
                    for (var i = 0; i < response.data.length; i++) {
                        if (response.data[i].AppointmentTime) {
                            $scope.Alert = "sm";
                        }
                    }
                }

                if ($scope.Alert == "sm") {
                    var isConfirmed = confirm("Do you want to set a meeting at the same time ?");
                    if (isConfirmed) {
                        appointmentRepository.saveUnScheduleAppointment($scope.appointment).then(function (response) {
                            if (response.data.isSucess) {
                                toastr.success(response.data.message);
                                $scope.clearData();

                            } else {
                                if (response.data.message == "LogOut") {
                                    $location.path('#!/Login')
                                }
                                toastr.error(response.data.message);
                            }
                        })

                    } else {
                        $scope.Alert = "nsm";
                    }
                }
                else {
                    appointmentRepository.saveUnScheduleAppointment($scope.appointment).then(function (response) {
                        if (response.data.isSucess) {
                            toastr.success(response.data.message);
                            $scope.clearData();
                            debugger
                            $scope.loadScheduleAppointment($scope.appointment.EmployeeID);
                            $scope.Alert = "nsm";

                        } else {
                            if (response.data.message == "LogOut") {
                                $location.path('#!/Login')
                            }
                            toastr.error(response.data.message);
                        }
                    })
                }
            })

            $scope.Alert = "nsm";

        } else {
            toastr.error("Please fill-up all required field !!!");
        }
    }


    $scope.isValidCheckIn = function () {
        var isValid = true;
        if ($scope.appointment.VisitorName == "") {
            isValid = false;
            toastr.error("Please fill-up Visitor Name !!!");
            return isValid;
        }
        else if ($scope.appointment.EmployeeID == null) {
            isValid = false;
            toastr.error("Please fill-up Employee !!!");
            return isValid;
        }
        else if ($scope.appointment.CardNO == "") {
            isValid = false;
            toastr.error("Please fill-up Card NO !!!");
            return isValid;
        }
        else if ($scope.appointment.CardNO.length != 3) {
            isValid = false;
            toastr.error("Card NO Must Be 3 Digit !!!");
            return isValid;
        }
        else {
            return isValid;
        }
    }
    $scope.isValidCheckOut = function () {
        var isValid = true;
        if ($scope.appointment.VisitorName == "") {
            isValid = false;
            toastr.error("Please fill-up Visitor Name !!!");
            return isValid;
        }
        else if ($scope.appointment.EmployeeID == null) {
            isValid = false;
            toastr.error("Please fill-up Employee !!!");
            return isValid;
        }
        else if ($scope.appointment.CardNO == "") {
            isValid = false;
            toastr.error("Please fill-up Card NO !!!");
            return isValid;
        }
        else {
            return isValid;
        }
    }
    $scope.isValidNotify = function () {
        var isValid = true;
        if ($scope.appointment.VisitorName == "") {
            isValid = false;
            toastr.error("Please fill-up Visitor Name !!!");
            return isValid;
        }
        if ($scope.appointment.CompanyName == "") {
            isValid = false;
            toastr.error("Please fill-up Company Name !!!");
            return isValid;
        }
        else if ($scope.appointment.EmployeeID == null) {
            isValid = false;
            toastr.error("Please fill-up Employee !!!");
            return isValid;
        }
        else if ($scope.appointment.NotifyMessage == "") {
            isValid = false;
            toastr.error("Please fill-up Notify Message !!!");
            return isValid;
        }
        else {
            return isValid;
        }
    }
    $scope.isValidReplay = function () {
        var isValid = true;
        if ($scope.appointment.VisitorName == "" || $scope.appointment.VisitorName == null) {
            isValid = false;
            toastr.error("Please fill-up Visitor Name !!!");
            return isValid;
        }
        if ($scope.appointment.CompanyName == "" || $scope.appointment.CompanyName == null) {
            isValid = false;
            toastr.error("Please fill-up Company Name !!!");
            return isValid;
        }
        else if ($scope.appointment.EmployeeID == null || $scope.appointment.EmployeeID == "") {
            isValid = false;
            toastr.error("Please fill-up Employee !!!");
            return isValid;
        }
        else if ($scope.appointment.ReplayMessage == "" || $scope.appointment.ReplayMessage == null) {
            isValid = false;
            toastr.error("Please fill-up Notify !!!");
            return isValid;
        }
        else {
            return isValid;
        }
    }
    $scope.isValidForward = function () {
        var isValid = true;
        if ($scope.appointment.VisitorName == "" || $scope.appointment.VisitorName == null) {
            isValid = false;
            toastr.error("Please fill-up Visitor Name !!!");
            return isValid;
        }
        if ($scope.appointment.CompanyName == "" || $scope.appointment.CompanyName == null) {
            isValid = false;
            toastr.error("Please fill-up Company Name !!!");
            return isValid;
        }
        else if ($scope.appointment.EmployeeID == null || $scope.appointment.EmployeeID == "") {
            isValid = false;
            toastr.error("Please fill-up Employee !!!");
            return isValid;
        }
        else {
            return isValid;
        }
    }
    //=============================================== Search Appointment =========================================

    $scope.searchAppointment = function(){
        debugger
        $scope.appointmentList=[];
        if($scope.search){
            appointmentRepository.searchAppointment($scope.search).then(function (response) {
                if (response.data) {
                    debugger 
                    for(i=0;i<response.data.length ;i++){
                        if (response.data[i].AppointmentDate) {
                            var appointmentDate = response.data[i].AppointmentDate.replace('/Date(', '').replace(')/', '');
                            response.data[i].AppointmentDate = $filter('date')(appointmentDate, "dd-MMM-yyyy");
                    
                        }
                        if (response.data[i].AppointmentTime) {
                            var appointmentTime = response.data[i].AppointmentTime.replace('/Date(', '').replace(')/', '');
                            appointmentTime = $filter('date')(appointmentTime, "dd/MM/yyyy hh:mm a");
                            time = appointmentTime.split(" ");

                            time = time[1] + " " + time[2];
                            response.data[i].AppointmentTime = time;
                        }
                        if (response.data[i].ReachTime) {
                            var ReachTime = response.data[i].ReachTime.replace('/Date(', '').replace(')/', '');
                            ReachTime = $filter('date')(ReachTime, "dd/MM/yyyy hh:mm a");
                            time = ReachTime.split(" ");

                            time = time[1] + " " + time[2];
                            response.data[i].ReachTime = time;
                        }
                        if (response.data[i].EndTime) {
                            var EndTime = response.data[i].EndTime.replace('/Date(', '').replace(')/', '');
                            EndTime = $filter('date')(EndTime, "dd/MM/yyyy hh:mm a");
                            time = EndTime.split(" ");

                            time = time[1] + " " + time[2];
                            response.data[i].EndTime = time;
                        }

                        if (response.data[i].CheckedInTime) {
                            var CheckedInTime = response.data[i].CheckedInTime.replace('/Date(', '').replace(')/', '');
                            CheckedInTime = $filter('date')(CheckedInTime, "dd/MM/yyyy hh:mm a");
                            time = CheckedInTime.split(" ");

                            time = time[1] + " " + time[2];
                            response.data[i].CheckedInTime = time;
                        }

                        if (response.data[i].BreakInTime) {
                            var BreakInTime = response.data[i].BreakInTime.replace('/Date(', '').replace(')/', '');
                            BreakInTime = $filter('date')(BreakInTime, "dd/MM/yyyy hh:mm a");
                            time = BreakInTime.split(" ");

                            time = time[1] + " " + time[2];
                            response.data[i].BreakInTime = time;
                        }

                        if (response.data[i].BreakOutTime) {
                            var BreakOutTime = response.data[i].BreakOutTime.replace('/Date(', '').replace(')/', '');
                            BreakOutTime = $filter('date')(BreakOutTime, "dd/MM/yyyy hh:mm a");
                            time = BreakOutTime.split(" ");

                            time = time[1] + " " + time[2];
                            response.data[i].BreakOutTime = time;
                        }
                        if (response.data[i].CheckedOutTime) {
                            var CheckedOutTime = response.data[i].CheckedOutTime.replace('/Date(', '').replace(')/', '');
                            CheckedOutTime = $filter('date')(CheckedOutTime, "dd/MM/yyyy hh:mm a");
                            time = CheckedOutTime.split(" ");

                            time = time[1] + " " + time[2];
                            response.data[i].CheckedOutTime = time;
                        }
                    }
                  
                    $scope.appointmentList = response.data;
                }
            });
        }else  {
            $scope.disabledSaveBtn = true ;
        }
    }


    //================================================ Check Meeeting Time =======================================
    $scope.disabledSaveBtn2 = true ;
    $scope.checkPrvCard = function(){
      debugger
      if($scope.appointment.BankCardID.length == 3){
            appointmentRepository.checkIsCardFree($scope.appointment.BankCardID).then(function (response) {
                if (response.data) {
                    debugger 
                    $scope.cardNo = response.data;
                    if ($scope.cardNo) {
                        $scope.disabledSaveBtn2 = true ;
                        toastr.error("This card is already allocated!!!");
                    }
                    
                }
                else{
                   // toastr.success("This card is free, may you can check-in  !!!");
                    $scope.disabledSaveBtn2 = false ;
                }
                
            });
      }else  {
          $scope.disabledSaveBtn2 = true ;
      }
    }

    $scope.loadCalendarDropdowns = function () {
        $http.get("/UnScheduleAppointment/ScheduleGetEvents").then(function (response) {
            var events = [];
            $.each(response.data, function (i, v) {
                if (v.AppointmentBy == "E") {
                    events.push({
                        title: 'Meeting With : ' + v.VisitorName,
                        visitorName: v.VisitorName,
                        companyName: v.CompanyName,
                        purpose: v.Purpose,
                        start: moment(v.AppointmentDate),
                        end: v.AppointmentDate != null ? moment(v.AppointmentDate) : null,
                        time: v.AppointmentTime != null ? moment(v.AppointmentTime) : null,
                        appointment: "Schedule",
                        color: "#7986CB",
                        allDay: v.IsFullDay,
                        visitorMobile: v.VisitorMobile

                    });
                }
                if (v.AppointmentBy == "R") {
                    events.push({
                        title: 'Meeting With : ' + v.VisitorName,
                        visitorName: v.VisitorName,
                        companyName: v.CompanyName,
                        purpose: v.Purpose,
                        start: moment(v.AppointmentDate),
                        end: v.AppointmentDate != null ? moment(v.AppointmentDate) : null,
                        time: v.AppointmentTime != null ? moment(v.AppointmentTime) : null,
                        appointment: "Un-Schedule",
                        color: "#B80672",
                        allDay: v.IsFullDay,
                        visitorMobile: v.VisitorMobile
                    });
                }
                if (v.AppointmentBy == "F") {
                    events.push({
                        title: 'Meeting With : ' + v.VisitorName,
                        visitorName: v.VisitorName,
                        companyName: v.CompanyName,
                        purpose: v.Purpose,
                        start: moment(v.AppointmentDate),
                        end: v.AppointmentDate != null ? moment(v.AppointmentDate) : null,
                        time: v.AppointmentTime != null ? moment(v.AppointmentTime) : null,
                        appointment: "Forward",
                        color: "green",
                        allDay: v.IsFullDay,
                        visitorMobile: v.VisitorMobile
                    });
                }

            })

            $scope.GenerateCalender(events);
        });
    }
    $scope.GenerateCalender = function (events) {
        $('#calender').fullCalendar('destroy');
        $('#calender').fullCalendar({
            contentHeight: 500,
            defaultDate: new Date(),
            timeFormat: 'h(:mm)a',
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,basicWeek,basicDay'
            },
            eventLimit: true,
            eventColor: '#378006',
            events: events,
            eventClick: function (calEvent, jsEvent, view) {
                $('#myModal #eventTitle').text(calEvent.title);
                var $purpose = $('<div/>');
                $purpose.append($('<p/>').html(`<b>Visitor's Name: </b>` + calEvent.visitorName));
                $purpose.append($('<p/>').html(`<b>Visitor's Company: </b>` + calEvent.companyName));
                $purpose.append($('<p/>').html(`<b>Visitor's Mobile: </b>` + calEvent.visitorMobile));
                $purpose.append($('<p/>').html('<b>Date: </b>' + calEvent.start.format("DD-MMM-YYYY ")));
                if (calEvent.time != null) {
                    $purpose.append($('<p/>').html('<b>Time: </b>' + calEvent.time.format("HH:mm a")));
                }
                if (calEvent.appointment != null) {
                    $purpose.append($('<p/>').html('<b>Appointment Type: </b>' + calEvent.appointment));
                }
                $purpose.append($('<p/>').html('<b>Meeting Purpose: </b>' + calEvent.purpose));
                

                $('#myModal #pDetails').empty().html($purpose);

                $('#myModal').modal();
            }
        })
    }
})