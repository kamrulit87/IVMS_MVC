angular.module("app").factory('settingsRepository', function ($http) {
    return {
        // Branch services
        saveBranch: function (branch) {
            return $http.post('/Branch/SaveBranch', branch);
        },
        branchList: function () {
            return $http.post('/Common/AllBranch');
        },

        //Employee services
        saveEmp: function (emp, list, user) {
            return $http.post('/Employee/EmployeeCreate', { "emp": emp, "beHalfEmpList": list, "user": user });
        },

        empList: function () {
            return $http.post('/Employee/EmployeeList');
        },
        //Department Services
        savedept: function (dept) {
            return $http.post('/Department/DepartmentCreate', dept);
        },
        deptList: function () {
            return $http.post('/Department/DepartmentLists');
        },
        //Designation Services
        saveDesignation: function (designation) {
            return $http.post('/Designation/DesignationCreate', designation);
        },
        designationList: function (id) {
            return $http.post('/Designation/DesignationLists',id);
        },
        //Company 
        loadAllCompany: function (id) {
            return $http.post('/Common/LoadAllCompany', id);
        },
        loadCompanyWiseAllBranch: function (id) {
            return $http.post('/Common/CompanyWiseAllBranch', { "companyID": id });
        },
        // Company Branch
        saveCompanyBranch: function (branch) {
            return $http.post('/CompanyBranch/SaveCompanyBranch', { 'companyBranch': branch});
        },
        loadCompanyBranch: function (id) {
            return $http.post('/CompanyBranch/LoadCompanyBranch', { "id": id });
        },

        onBehalfEmpList: function (empID) {
            return $http.post('/Employee/OnBehalfEmployee', { "employeeID": empID });
        }

    }

    })