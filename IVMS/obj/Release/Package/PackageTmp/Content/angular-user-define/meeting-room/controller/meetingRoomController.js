angular.module('app').controller('meetingRoomController', function ($scope, $filter, $cookieStore, meetingRoomRepository, appointmentRepository,settingsRepository, commonRepository) {
    $scope.meetingRoom = {};
    $scope.meetingRoomReq = {};
    $scope.loadDropdowns = function () {
        //$scope.getLoadAvailableStatus();
        debugger
        $scope.availableStatusList = commonRepository.getAvailableStatus();
        $scope.getLoadEmployee();
     
        $scope.meetingRoom = $cookieStore.get('editDataMeetingRoom');
    }
    $scope.loadReqDropdowns = function () {
        $scope.getLoadEmployee();
    }
    //Cookies Remove Function
    $scope.removeCookies = function () {
        $cookieStore.put('editDataMeetingRoom', $scope.meetingRoom);
    }
    // Start Tab
    $scope.tab = 1;
    $scope.approved = {};
    //$scope.approved.FDate = $filter('date')(Date.now(), 'yyyy-MM-dd');
    //$scope.approved.TDate = $filter('date')(Date.now(), 'yyyy-MM-dd');
    $scope.setTab = function (tabId) {
        $scope.tab = tabId;
        if ($scope.tab == 2) {
            $scope.approved.Status = "a";
        }
        else if ($scope.tab == 3) {
            $scope.approved.Status = "c";
        }
        else {
            $scope.approved.Status = "p";
        }

    };
    $scope.isSet = function (tabId) {
        return $scope.tab === tabId;

    };
    // End Tab


    $scope.getLoadAvailableStatus = function () {
        debugger
        $scope.availableStatusList = commonRepository.getAvailableStatus();
    }
    $scope.clearData = function () {
        $scope.meetingRoom = {};
        $cookieStore.put('editDataMeetingRoom', $scope.meetingRoom);
    }
    $scope.getLoadEmployee = function () {
        appointmentRepository.loadEmployee().then(function (response) {
            if (response.data) {
                $scope.employeeList = response.data;
            }
        })
    }
    $scope.isModal = false;
    $scope.modalClose = function () {
        debugger
        $scope.isModal = !$scope.isModal;
  

    }
    $scope.isModal2 = false;
    $scope.modalClose2 = function () {
        debugger
        $scope.isModal2 = !$scope.isModal2;


    }
    $scope.modalOpen = function (row) {
        debugger
        $scope.searchMeetingRoomParticipant(row.RequisitionID);
        $scope.isModal2 = !$scope.isModal2;


    }
    //Meeting Room
    $scope.saveMeetingRoom = function () {        
        $scope.checkTime($scope.meetingRoom.FromTime, $scope.meetingRoom.ToTime);


        if ($scope.meetingRoomForm.$valid) {
            meetingRoomRepository.saveMeetingRoom($scope.meetingRoom).then(function (response) {
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
            toastr.error("Please fill-up all required field !!!");
        }
    }
    $scope.loadMeetingRoom = function () {
        meetingRoomRepository.loadMeetingRoom().then(function (response) {
            if (response.data) {
                for (var i = 0; i < response.data.length; i++) {
                    if (response.data[i].ProjectorAvailable) {
                        response.data[i].ProjectorAvailable = commonRepository.convertAvailableStatusText(response.data[i].ProjectorAvailable);
                    }
                }
                $scope.meetingRoomList = response.data;
            }
        })
    }
    $scope.removeRow = function (index, details) {
        debugger
        $scope.dataDtls.splice(index, 1);
        if (details.ParticipantID > 0) {
            $scope.deleteStoreReqDtlsID.push(details.ParticipantID);
        }

    }
    //  add Data Add
    $scope.dataDtls = [];
    $scope.deleteStoreReqDtlsID = [];
    var indexID = 0;
    $scope.addStoreReqRow = function (details) {
        debugger
        //if (details.EmployeeID != null && details.DepartmentID != null && details.DesignationID != null) {
        if (details.EmployeeID != null && details.DepartmentID != null) {
            $scope.dataDtls.push({
                EmployeeID: details.EmployeeID,
                DepartmentID: details.DepartmentID,
                //DesignationID: details.DesignationID,
                IndexID: indexID
            });
            details.EmployeeID = null;
            details.DepartmentID = null;
            //details.DesignationID = null;
            ++indexID;
        }
    }
    $scope.updateDetailsData = function (details) {
        for (var i = 0; i < $scope.dataDtls.length; i++) {
            if ($scope.dataDtls[i].IndexID == details.IndexID) {
                $scope.dataDtls[i].EmployeeID = details.EmployeeID;
                $scope.dataDtls[i].DepartmentID = details.DepartmentID;
                //$scope.dataDtls[i].DesignationID = details.DesignationID;
            }
        }
    }
    $scope.editRow = function (row) {
        debugger
        window.location.replace("#!/MeetingRoomCreate");
        $scope.meetingRoom = row;
        if (row.ProjectorAvailable == "Yes") {
            $scope.meetingRoom.ProjectorAvailable = 1;
        } else {
            $scope.meetingRoom.ProjectorAvailable = 0;
        }
       
        $cookieStore.put('editDataMeetingRoom', $scope.meetingRoom);
    }
    $scope.editRowRoomReq = function (row) {
        debugger
        $scope.dataDtls = [];
        $scope.detail = {};
        $scope.deleteStoreReqDtlsID = [];
        $scope.meetingRoomReq = row;
        $scope.getMeetingReq();
        $scope.loadEmployee();
        $scope.loadDepartment();
        $scope.designationLists();
        //$scope.searchMeetingRoomParticipant();
        $scope.searchMeetingRoomParticipant(row.RequisitionID);
        $scope.isModal = !$scope.isModal;
       // $scope.designationbyEmp();
       

    }
    //Meeting Requisition Room
    $scope.clearReqData = function () {
        $scope.meetingRoomReq = {};
    }
    $scope.reqRow = function (row) {
        debugger
        $scope.dataDtls = [];
        $scope.detail = {};

        $scope.isModal = !$scope.isModal;
        $scope.designationLists();
        $scope.loadEmployee();
        $scope.loadDepartment();
      
        $scope.meetingRoomReq = {};
        $scope.reqList = [];
        $scope.meetingRoomReq.MeetingRoomID = row.MeetingRoomID;
        
    }
    $scope.getMeetingReq2 = function () {
        debugger
        // $scope.meetingRoomReq.MeetingRoomID, $scope.meetingRoomReq.RequiredDate
        meetingRoomRepository.getMeetingReq2().then(function (response) {
            if (response.data) {
                debugger 
                for (var i = 0; i < response.data.length; i++) {
                    if (response.data[i].RequiredDate) {
                        var requiredDate = response.data[i].RequiredDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].RequiredDate = $filter('date')(requiredDate, "dd-MMM-yyyy");
                    }

                    if (response.data[i].FromTime) {
                        var hours = response.data[i].FromTime.Hours;
                        var minits = response.data[i].FromTime.Minutes;

                        //response.data[i].FromTime = hours.toString().padStart(2, '0') + ":" + minits.toString().padStart(2, '0');
                        response.data[i].FromTime = hours.toString() + ":" + minits.toString();
                    }
                    if (response.data[i].ToTime) {
                        var hours = response.data[i].ToTime.Hours;
                        var minits = response.data[i].ToTime.Minutes;

                        response.data[i].ToTime = hours.toString() + ":" + minits.toString();
                    }
                    response.data[i].Time = response.data[i].FromTime + "-" + response.data[i].ToTime;
                }
                $scope.reqList2 = response.data;
            }
        })
    }
    $scope.disabledSaveBtn = true;
    $scope.checkTime = function (meeting) {
        debugger
        if ($scope.meetingRoomReq.FromTime && $scope.meetingRoomReq.ToTime) {
            $scope.disabledSaveBtn = false;
            var date = $filter('date')(Date.now(), 'dd-MMM-yyyy');
            var time = date + " " + $scope.meetingRoomReq.FromTime;
            var time2 = date + " " + $scope.meetingRoomReq.ToTime;
            var fromTime = new Date(time).getTime();
            var toTime = new Date(time2).getTime();
            if (fromTime >= toTime) {
                toastr.error("From time can not be greater then to time !!!");
                $scope.disabledSaveBtn = true;
                return;
            }
            for (var i = 0; i < $scope.reqList.length; i++) {
                var FTime = date + " " + $scope.reqList[i].FromTime;
                var TTime = date + " " + $scope.reqList[i].ToTime;
                var FromTime = new Date(FTime).getTime();
                var ToTime = new Date(TTime).getTime();
                if ((fromTime >= FromTime) && (toTime <= ToTime)) {
                    toastr.error("This meeting time conflict with another meeting, select another time.!!!");
                    $scope.disabledSaveBtn = true;
                } else if ((fromTime <= FromTime) && (toTime >= ToTime)) {
                    toastr.error("This meeting time conflict with another meeting, select another time.!!!");
                    $scope.disabledSaveBtn = true;

                } else if ((fromTime >= FromTime) && (fromTime <= ToTime)) {
                    toastr.error("This meeting time conflict with another meeting, select another time.!!!");
                    $scope.disabledSaveBtn = true;

                } else if ((toTime >= FromTime) && (toTime <= ToTime)) {
                    toastr.error("This meeting time conflict with another meeting, select another time.!!!");
                    $scope.disabledSaveBtn = true;

                }
                else {
                    $scope.disabledSaveBtn = false;
                }
            }
        }
        
    }
    $scope.designationLists = function () {
        debugger
        $scope.designationList = [];
        var data = settingsRepository.designationList().then(function (response) {

            if (response.data) {
                $scope.designationList = response.data;
               
            }
        });

    }
    $scope.getMeetingReq = function () {
        debugger
        
        meetingRoomRepository.getMeetingReq($scope.meetingRoomReq.MeetingRoomID, $scope.meetingRoomReq.RequiredDate).then(function (response) {
            if (response.data) {
                debugger
                for (var i = 0; i < response.data.length; i++) {
                    if (response.data[i].RequiredDate) {
                        var requiredDate = response.data[i].RequiredDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].RequiredDate = $filter('date')(requiredDate, "dd-MMM-yyyy");
                    }

                    if (response.data[i].FromTime) {
                        var hours = response.data[i].FromTime.Hours;
                        var minits = response.data[i].FromTime.Minutes;

                        //response.data[i].FromTime = hours.toString().padStart(2, '0') + ":" + minits.toString().padStart(2, '0');
                        response.data[i].FromTime = hours.toString() + ":" + minits.toString();
                    }
                    if (response.data[i].ToTime) {
                        var hours = response.data[i].ToTime.Hours;
                        var minits = response.data[i].ToTime.Minutes;

                        response.data[i].ToTime = hours.toString() + ":" + minits.toString();
                    }
                    response.data[i].Time = response.data[i].FromTime + "-" + response.data[i].ToTime;
                }
                $scope.reqList = response.data;
            }
        })
    }

    $scope.searchMeetingRoomParticipant = function (id) {
        debugger
        $scope.meetingRoomParticipantList = [];
        var data = meetingRoomRepository.searchMeetingRoomParticipant(id).then(function (response) {
            debugger
            if (response.data) {
                $scope.meetingRoomParticipantList = response.data;
                $scope.dataDtls = response.data;
                if ($scope.meetingRoomParticipantList.length > 0) {
                    $scope.dataShow = true;
                    $scope.msgShow = false;
                } else {
                    $scope.dataShow = false;
                    $scope.msgShow = true;
                }


            }
        });

    }
    
    $scope.designationbyEmp = function (eid) {
        $scope.designationListsbyEmp = [];
        debugger


           var id  = $filter('filter')($scope.depEmployeeList, function (d) { return d.EmployeeID === eid ; });
           var did = id[0].DesignationID;

           $scope.designationListsbyEmp = $filter('filter')($scope.designationList, function (d) { return d.DesignationID === did; });
      


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
    $scope.detail = {};
    $scope.getDepWiseEmployee = function () {
        debugger
        if ($scope.detail.DepartmentID > 0) {
            appointmentRepository.getDepWiseEmployee($scope.detail.DepartmentID).then(function (response) {
                if (response.data) {
                    debugger
                    $scope.depEmployeeList = response.data;
                    //if ($scope.detail.EmployeeID) {
                    //    $scope.getDepWiseFloor();
                    //}

                }
            });
        } else {
            $scope.depEmployeeList = [];
        }

    }

    //Meeting Room Approval
    $scope.getMeetingRoomAppData = function () {
        debugger
        $scope.meetingRoomApprovedList = [];
        meetingRoomRepository.getMeetingRoomAppData($scope.approved).then(function (response) {
            if (response.data) {
                debugger
                for (i = 0; i < response.data.length ; i++) {
                    if (response.data[i].RequiredDate) {
                        var requiredDate = response.data[i].RequiredDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].RequiredDate = $filter('date')(requiredDate, "dd-MMM-yyyy");
                    }

                    if (response.data[i].FromTime) {
                        var hours = response.data[i].FromTime.Hours;
                        var minits = response.data[i].FromTime.Minutes;

                        //response.data[i].FromTime = hours.toString().padStart(2, '0') + ":" + minits.toString().padStart(2, '0');
                        response.data[i].FromTime = hours.toString() + ":" + minits.toString();
                    }
                    if (response.data[i].ToTime) {
                        var hours = response.data[i].ToTime.Hours;
                        var minits = response.data[i].ToTime.Minutes;

                        response.data[i].ToTime = hours.toString() + ":" + minits.toString();
                    }
                }
                $scope.meetingRoomApprovedList = response.data;

            }
        });

    }
    $scope.saveMeetingReqRoom = function () {
        
   
        //if ($scope.checkTime($scope.meetingRoomReq.FromTime, $scope.meetingRoomReq.ToTime)) {
        //    //toastr.error("This meeting time conflict with another meeting, select another time.!!!");
        //} 
    if ($scope.meetingRoomReqForm.$valid) {
        debugger
        $scope.meetingRoomReq.RoomStatus = "p";
                meetingRoomRepository.saveMeetingReqRoom($scope.meetingRoomReq, $scope.dataDtls,$scope.deleteStoreReqDtlsID).then(function (response) {
                    if (response.data.isSucess) {
                        toastr.success(response.data.message);
                        $scope.clearReqData();
                        $scope.modalClose();
                        $scope.reqList = [];
                        $scope.dataDtls = [];

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
        }
    $scope.approvedMeetingRoom = function (row) {
        $scope.meetingRoomReq = row;
        var isConfirmed = confirm("Are you sure, you want to approve this requisition?.");
        if (isConfirmed) {
        debugger
        $scope.meetingRoomReq.RoomStatus = "a";
        meetingRoomRepository.saveMeetingReqRoom($scope.meetingRoomReq, $scope.dataDtls, $scope.deleteStoreReqDtlsID).then(function (response) {
                    if (response.data.isSucess) {
                        toastr.success(response.data.message);
                        $scope.clearReqData();
                        $scope.getMeetingRoomAppData();

                    } else {
                        if (response.data.message == "LogOut") {
                            $location.path('#!/Login')
                        }
                        toastr.error(response.data.message);
                    }
                })
            }
        }
    $scope.CancelMeetingRoom = function (row) {
        $scope.meetingRoomReq = row;
        var isConfirmed = confirm("Are you sure, you want to Cancel this requisition?.");
        if (isConfirmed) {
        debugger
        $scope.meetingRoomReq.RoomStatus = "c";
        meetingRoomRepository.saveMeetingReqRoom($scope.meetingRoomReq, $scope.dataDtls, $scope.deleteStoreReqDtlsID).then(function (response) {
                    if (response.data.isSucess) {
                        toastr.success(response.data.message);
                        $scope.clearReqData();
                        $scope.getMeetingRoomAppData();

                    } else {
                        if (response.data.message == "LogOut") {
                            $location.path('#!/Login')
                        }
                        toastr.error(response.data.message);
                    }
                })
            }
        }
        
    //}


})