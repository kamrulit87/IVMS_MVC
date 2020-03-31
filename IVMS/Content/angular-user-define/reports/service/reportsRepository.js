angular.module("app").factory('reportsRepository', function ($http) {
    return {
        //Company 
        loadCompany: function (id) {
            return $http.post('/Common/LoadCompany', id);
        },
        loadCompanyWiseBranch: function (id) {
            return $http.post('/Common/CompanyWiseBranch', { "companyID": id });
        },
        deptWiseEmployee: function (id) {
            return $http.post('/Common/DeptWiseEmployee', { "deptID": id });
        },

    }

})