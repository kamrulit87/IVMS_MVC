angular.module('app').factory('securityRepository', function ($http, $location) {
    
    return {        
        getLogInPermission: function (data) {
            return $http({
                method: 'POST',
                url: '/Account/Login',
                params: data,
                async: true,
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            });
        },
        //getLogInPermission: function (data) {
        //    debugger;
        //    var config = {
        //        async: false
        //    }
        //    return $http.post('/Account/LoginpPrmission', data, config);
        //},
        logIn: function(data){
            return $http.post('/Security/Login', data);
        }, 
        logOff: function() {
            return $http.post('/Security/LogOff');
        },
        getCompanyInfoById: function (id) {            
            return $http.post('/Account/getCompanyInfoByID/' + id);
        },
        getLocationData: function () {
            return $http.post('/Location/LoadAllLocation');
        },
        getDropdownLocationData: function () {
            return $http.post('/Location/GetAllParentLocations');
        },

        postLocationData: function (data) {
            return $http.post('/Location/CreateLocationSave', data);
        },

        putLocation: function (data) {
            return $http.post('/Location/EditLocationSave/', data);
        },

        deleteLocation: function (id) {
            return $http.get('/Location/DeleteLocation/' + id);
        },

        getAllUserData: function () {
            return $http.post('/User/LoadAllUser');
        },

        getDeptWiseEmployee: function (deptID) {
            return $http.post('/User/GetDeptWiseEmployee', { "deptID": deptID });
        },
        allEmployee: function () {
            return $http.post('/User/AllEmployee');
        },

        deleteUser: function (id) {
            return $http.get('/User/Delete/' + id);
        },

        getDropdownUserGroupData: function () {
            return $http.post('/User/GetAllUserGroups');
        },
        loadCompany: function () {
            return $http.post('/Common/LoadAllCompany');
        },
        companyWiseBranch: function (id) {
            return $http.post('/Common/CompanyWiseAllBranch', { "companyID": id });
        },
        getAllCompanyData: function () {
            return $http.post('/User/GetAllCompany');
        },
        saveUserData: function (data) {
            return $http.post('/User/CreateUserSave', data);
        },

        activeDeActiveUser: function (id,status) {
            return $http.get("/User/ActiveDeActiveUser/", { params: { "id": id, "status": status } });
        },

        updateUserData: function (data) {
            var data = $.param({
                user: data
            });

            var config = {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
                }
            }
            return $http.post('/User/UpdateUserForm/', data, config);
        },

        getUserGroupData: function () {
            return $http.post('/UserGroup/LoadAllUserGroup');
        },

        deleteUserGroup: function (id) {
            return $http.post('/UserGroup/DeleteUserGroup/'+id);
        },

        putPasswordData: function (data) {
            return $http.post('/ChangePassword/PasswordChangeByAdminSave/', data);
        },

        putOwnPasswordData: function (data) {
            return $http.post('/ChangePassword/SelfPasswordChange/', data);
        },

        
        generateCompanyCode: function (){
            return $http.get('/Company/GenerateCompanyCode');
        },

        postCompanyData: function (uploadImage, CompanyInfo) {
            
            var formData = new FormData();
            formData.append("Logo", uploadImage);
            formData.append("CompanyInfo", angular.toJson(CompanyInfo));

            var config = {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }
            return $http.post('/Company/CreateCompanySave', formData, config);
        },

        getImage: function (id) {
            return $http.post('/Company/getImage/' + id);
        },

        putCompanyData: function (uploadImage, CompanyInfo) {
            var formData = new FormData();
            formData.append("Logo", uploadImage);
            formData.append("CompanyInfo", angular.toJson(CompanyInfo));

            var config = {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }
            return $http.post('/Company/EditCompanySave', formData, config);
        },

        deleteCompany: function (id) {
            return $http.get('/Company/DeleteCompany/' + id);
        },

        activationCompany: function (id, status) {
            return $http.get('/Company/ActivationCompany/', { params: { "id": id, "status": status } });
        },

        getPageData: function () {
            return $http.post('/UserGroup/GetPage');
        },

        postUsergroupData: function (userGroup) {
            return $http.post("/UserGroup/UserGroupSave", { "userGroup": userGroup });
        },

        getEditUserGroupData: function (id) {
            return $http.post('/UserGroup/LoadUserGroupForEdit/' + id);
        },

        getEditPageMappingData: function (id) {
            return $http.post('/UserGroup/LoadMappingDataForEdit/' + id);
        },

        putUsergroupData: function (userGroup, data) {
            return $http.post('/UserGroup/EditUserGroupSave', { "userGroup": userGroup, "userMappingVm": data });
        },
        // Page
        loadPage: function (id) {
            return $http.post('/Page/LoadPage', { "pageID": id });
        },
        getModuleData: function (id) {
            return $http.post('/Page/GetModuleData', { "moduleID": id });
        },
        postPageData: function (data) {
            return $http.post('/Page/PageSave', { "page": data });
        },
        deletePage: function (id) {
            return $http.post('/Page/DeletePage', { "id": id });
        },

    }
})