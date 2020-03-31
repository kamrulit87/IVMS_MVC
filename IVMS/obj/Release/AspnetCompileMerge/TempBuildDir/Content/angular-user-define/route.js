angular.module('app', ['ngTouch', 'ngRoute', 'ngResource', 'ngCookies', 'smart-table', 'ngDialog', 'ngAnimate', 'ngSanitize', 'rt.select2', 'treeGrid', 'angucomplete-alt', 'ui.bootstrap', 'dndLists', 'ngIdle'],
function ($routeProvider, $compileProvider) {

    $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|file|ftp|blob):|data:image\//);
    $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|ftp|mailto|javascript):/);
    $routeProvider.when('/', {
        templateUrl: '/Security/Index'
    }).when('/UnSchedule', {
        templateUrl: '/UnScheduleAppointment/UnScheduleAppointmentList#!/'
    }).when('/UnScheduleCreate', {
        templateUrl: '/UnScheduleAppointment/UnScheduleAppointmentCreate#!/'
    }).when('/SearchAppointment', {
        templateUrl: '/UnScheduleAppointment/SearchAppointment#!/'
    }).when('/Schedule', {
        templateUrl: '/ScheduleAppointment/ScheduleAppointmentList#!/'
    }).when('/ScheduleCreate', {
        templateUrl: '/ScheduleAppointment/ScheduleAppointmentCreate#!/'
    }).when('/MeetingRoom', {
        templateUrl: '/MeetingRoom/MeetingRoomList#!/'
    }).when('/MeetingRoomCreate', {
        templateUrl: '/MeetingRoom/MeetingRoomCreate#!/'
    }).when('/MeetingRoomReq', {
        templateUrl: '/MeetingRoom/MeetingRoomRequisitionList#!/'
    }).when('/MeetingRoomReqList', {
        templateUrl: '/MeetingRoom/MeetingRoomReqList#!/'
    }).when('/MeetingRoomReqApproval', {
        templateUrl: '/MeetingRoom/MeetingRoomReqApproval#!/'
    }).when('/Department', {
        templateUrl: '/Department/DepartmentList#!/'
    }).when('/DepartmentCreate', {
        templateUrl: '/Department/DepartmentCreate#!/'
    }).when('/Designation', {
        templateUrl: '/Designation/DesignationList#!/'
    }).when('/DesignationCreate', {
        templateUrl: '/Designation/DesignationCreate#!/'
    }).when('/Employee', {
        templateUrl: '/Employee/EmployeeList#!/'
    }).when('/EmployeeCreate', {
        templateUrl: '/Employee/EmployeeCreate#!/'
    }).when('/Branch', {
        templateUrl: '/Branch/BranchList#!/'
    }).when('/BranchCreate', {
        templateUrl: '/Branch/BranchCreate#!/'
    }).when('/Company', {
        templateUrl: '/HRM/Company/CompanyList#!/'
    }).when('/CreateCompany', {
        templateUrl: '/Company/CreateCompany#!/'
    }).when('/CompanyBranch', {
        templateUrl: '/CompanyBranch/CompanyBranchList#!/'
    }).when('/CreateCompanyBranch', {
        templateUrl: '/CompanyBranch/CreateCompanyBranch#!/'
    }).when('/Page', { // Security
        templateUrl: '/Page/Page#!/'
    }).when('/AddPage', {
        templateUrl: '/Page/PageCreate#!/'
    }).when('/User', {
        templateUrl: '/User/User#!/'
    }).when('/AddUser', {
        templateUrl: '/User/UserCreate#!/'
    }).when('/EditUser', {
        templateUrl: '/User/EditUser#!/'
    }).when('/UserGroup', {
        templateUrl: '/UserGroup/UserGroup#!/'
    }).when('/CreateUserGroup', {
        templateUrl: '/UserGroup/UserGroupCreate#!/'
    }).when('/EditUserGroup', {
        templateUrl: '/UserGroup/EditUserGroup#!/'
    }).when('/ChangePassword', {
        templateUrl: '/ChangePassword/ChangePassword#!/'
    }).when('/PasswordChangeByAdmin', {
        templateUrl: '/ChangePassword/PasswordChangeByAdmin#!/'
    }).when('/VisitorINOut', { //Reports
        templateUrl: '/Reports/Reports/VisitorINOut#!/'
    }).when('/LogIn', {
        templateUrl: '/Security/Login#!/'
    }).when('/LogOut', {
        templateUrl: '/Security/LogOff#!/'
    }).otherwise('/', {
        templateUrl: '/Security/LogOff#!/'
    });


    toastr.options = {
        "closeButton": true,
        "debug": true,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-bottom-right",
        //"positionClass": "toast-top-full-width",
        "preventDuplicates": true,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }


}).config(['KeepaliveProvider', 'IdleProvider', function (KeepaliveProvider, IdleProvider) {
    IdleProvider.idle(1800);
    IdleProvider.timeout(1);
    KeepaliveProvider.interval(20);
}]);

// Following code use for ng-idle logout option
var myApp = angular.module('app');
myApp.run(['Idle', function (Idle) {
    Idle.watch();
}]);
