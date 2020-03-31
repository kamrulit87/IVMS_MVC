
angular.module('app').controller('menuController', function ($scope, $http, $rootScope) {
    //start :: ng-idel => when user idel specific time then it will be auto logout
    $scope.$on('IdleStart', function () {
        toastr.warning('your session will be end soon!!');
    });

    $scope.$on('IdleTimeout', function () {
        window.location = "/Security/LogOff#!/";
    });
    //End :: ng-idel

    ////Hide all menu on first time page load     
    //All Module
    $scope.groupID = 0;
    $scope.Security = false;
    $scope.VMS = false;
    $scope.Setup = false;
    $scope.Reports = false;
    
    //Hr Page
    //Security
    $scope.SecurityMaster = false;
    $scope.Dashboard = false;
    $scope.User = false;
    $scope.Page = false;
    $scope.PagePermission = false;
    $scope.ChangePasswordByAdmin = false;
    $scope.ChangeOwnPassword = false;

    //Settings
    $scope.Department = false;
    $scope.Designation = false;
    $scope.CompanyBranch = false;
    $scope.Company = false;
    $scope.Employee = false;
    $scope.Branch = false;

    //VMS
    $scope.UnSchedule = false;
    $scope.Schedule = false;
    $scope.MeetingRoom = false;
    $scope.MeetingRoomReq = false;
    $scope.MeetingRoomApproval = false;


    //Reports
    $scope.VisitorINOut = false;

    $http.get('/Security/GetSiteMenu').then(function (response) {
        //When login SuperAdmin        
        $scope.groupID = response.data.userGroupID;
        if ($scope.groupID === 2) { // groupID 2 = SuperAdmin
            //Main Module Show
            $scope.Security = true;
            $scope.Setup = true;
            //Sub Module Show
            $scope.User = true;
            $scope.Page = true;
            $scope.PagePermission = true;
            $scope.ChangePasswordByAdmin = true;
            $scope.ChangeOwnPassword = true;
        }
        else {
            $scope.menu = response.data.menu;
            ////Module and Sub Module Show
            for (var i = 0; i < $scope.menu.length; i++) {
                if ($scope.menu[i].Module == "Security")
                    $scope.Security = true;
                if ($scope.menu[i].Module == "VMS")
                    $scope.VMS = true;
                if ($scope.menu[i].Module == "Setup")
                    $scope.Setup = true;
                if ($scope.menu[i].Module == "Reports")
                    $scope.Reports = true;
            }

            ////Page Show
            for (var i = 0; i < $scope.menu.length; i++) {
                if ($scope.menu[i].Page == "SecurityMaster")
                    $scope.SecurityMaster = true;
                if ($scope.menu[i].Page == "Dashboard")
                    $scope.Dashboard = true;
                if ($scope.menu[i].Page == "User")
                    $scope.User = true;
                if ($scope.menu[i].Page == "Page")
                    $scope.Page = true;
                if ($scope.menu[i].Page == "PagePermission")
                    $scope.PagePermission = true;
                if ($scope.menu[i].Page == "ChangePasswordByAdmin")
                    $scope.ChangePasswordByAdmin = true;
                if ($scope.menu[i].Page == "ChangeOwnPassword")
                    $scope.ChangeOwnPassword = true;
                //VMS Settings
                if ($scope.menu[i].Page == "Department")
                    $scope.Department = true;
                if ($scope.menu[i].Page == "Designation")
                    $scope.Designation = true;
                if ($scope.menu[i].Page == "CompanyBranch")
                    $scope.CompanyBranch = true;
                if ($scope.menu[i].Page == "Company")
                    $scope.Company = true;
                if ($scope.menu[i].Page == "Employee")
                    $scope.Employee = true;
                if ($scope.menu[i].Page == "Branch")
                    $scope.Branch = true;
                //VMS
                if ($scope.menu[i].Page == "UnSchedule")
                    $scope.UnSchedule = true;
                if ($scope.menu[i].Page == "Schedule")
                    $scope.Schedule = true;
                if ($scope.menu[i].Page == "MeetingRoom")
                    $scope.MeetingRoom = true;
                if ($scope.menu[i].Page == "MeetingRoomReq")
                    $scope.MeetingRoomReq = true;
                if ($scope.menu[i].Page == "MeetingRoomApproval")
                    $scope.MeetingRoomApproval = true;
                //VMS Reporst
                if ($scope.menu[i].Page == "VisitorINOut")
                    $scope.VisitorINOut = true;
                
            }
        }
    });


});


