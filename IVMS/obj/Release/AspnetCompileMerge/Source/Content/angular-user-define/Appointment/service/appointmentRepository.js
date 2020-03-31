angular.module('app').factory('appointmentRepository', function ($http) {
    return {
        //Common
        loadEmployee: function (id) {
            return $http.post('/ScheduleAppointment/LoadEmployee', { "empID": id });
        },
        getDepWiseEmployee: function (id) {
            return $http.post('/ScheduleAppointment/GetDepWiseEmployee', { "depID": id });
        },
        getDepWiseEmployeeBehalf: function (id) {
            return $http.post('/ScheduleAppointment/DepWiseEmployeeBehalf', { "depID": id });
        },
        getEmployeeWiseFloor: function (id) {
            return $http.post('/ScheduleAppointment/GetEmployeeWiseFloor', { "empID": id });
        },
        loadDepartment: function (id) {
            return $http.post('/ScheduleAppointment/LoadDepartment', {"depID": id});
        },
        loadSelfDepartment: function (id) {
            return $http.post('/ScheduleAppointment/LoadSelfDepartment', { "depID": id });
        },
        getSelfDepWiseDropdownEmployee: function (id) {
            return $http.post('/ScheduleAppointment/SelfDeptWiseEmp', { "depID": id });
        },
        getDepWiseFloor: function (id) {
            return $http.post('/ScheduleAppointment/GetDepWiseFloor', { "empID": id });
        },
        //Schedule Appointment
        saveScheduleAppointment: function(data) {
            return $http.post('/ScheduleAppointment/SaveScheduleAppointment', data);
        },
        visitorForward: function (data,apID) {
            debugger 
            return $http.post('/ScheduleAppointment/VisitorForward', { "appointment": data, "apID": apID });
        },
        acceptedNotify: function (data) {
            return $http.post('/ScheduleAppointment/ReplayNotify', data);
        },
        pendingNotify: function (data) {
            return $http.post('/ScheduleAppointment/ReplayNotify', data);
        },
        loadScheduleAppointment: function(id) {
            return $http.post('/ScheduleAppointment/LoadScheduleAppointment', { "appointmentID": id });
        },
        loadNotifyList: function () {
            return $http.post('/ScheduleAppointment/LoadNotifyList');
        },
        //UnSchedule Appointment
        saveUnScheduleAppointment: function (data) {
            debugger 
            return $http.post('/UnScheduleAppointment/SaveUnScheduleAppointment', { "appointment": data });
        },
        checkIsCardFree: function (cardNo) {
            debugger
            return $http.post('/UnScheduleAppointment/CheckIsCardFree', { "cardNo": cardNo });
        },
        searchAppointment: function (search) {
            debugger
            return $http.post('/UnScheduleAppointment/MultipleOptionWiseMeetingScheduleReport', { "meetingReportRequestModel": search });
        },
        saveCancelAppointment: function (data, list) {
            debugger
            return $http.post('/UnScheduleAppointment/SaveCancelAppointment', { "appointment": data, "empList": list });
        },

        visitorCheckedIn: function (data) {
            return $http.post('/UnScheduleAppointment/VisitorIn', data);
        },
        visitorCheckedOut: function (data) {
            return $http.post('/UnScheduleAppointment/VisitorOut', {"cardNO": data});
        },
        visitorBreakOut: function (data) {
            return $http.post('/UnScheduleAppointment/VisitorBreakOut', data);
        },
        visitorBreakIn: function (data) {
            return $http.post('/UnScheduleAppointment/VisitorBreakIn', data);
        },
        visitorNotify: function (data) {
            return $http.post('/UnScheduleAppointment/VisitorNotify', data);
        },
        getDtlsAppointment: function (id) {
            return $http.post('/UnScheduleAppointment/GetDtlsAppointment');
        },
        getSearchCardNo: function (cardNo) {
            return $http.post('/UnScheduleAppointment/GetSearchCardNo', { "cardNo": cardNo });
        },
        getDtlsCheckIn: function (id) {
            return $http.post('/UnScheduleAppointment/GetDtlsCheckIn');
        },
        getDtlsCheckBreak: function () {
            return $http.post('/UnScheduleAppointment/GetDtlsCheckBreak');
        },
        getDtlsCheckOut: function (id) {
            return $http.post('/UnScheduleAppointment/GetDtlsCheckOut');
        },
        getDtlsCancel: function (id) {
            return $http.post('/UnScheduleAppointment/GetDtlsCancel');
        },
        cardWiseAppointmentData: function (id) {
            return $http.post('/UnScheduleAppointment/CardWiseAppointmentData', { "cardNO": id });
        },
        // ===================== Check Meeting Time ========================
        checkMeetingTime: function (time, date,empid) {
            return $http.post('/ScheduleAppointment/CheckMeetingTime', { "time": time, "date": date, "empid": empid });
        },


    }
})