angular.module('app').controller('userController', function ($scope, $filter, $location, $route, $templateCache, $cookieStore, securityRepository, settingsRepository) {

        //start :: ng-idel => when user idel specific time then it will be auto logout
        $scope.$on('IdleStart', function () {
            toastr.warning('your session will be end soon!!');
        });

        $scope.$on('IdleTimeout', function () {
            window.location = "/Security/LogOff#!/";
        });
        //End :: ng-idel

        $scope.SecurityQuestions = [];
        $scope.SecurityQuestions = ["Which one is your favorite sports Team?", "Where is your birth Place?", "What is the name of your first School?", "Who is your favorite Athlete?"];


        $scope.Reload = function () {
            var currentPageTemplate = $route.current.templateUrl;
            $templateCache.remove(currentPageTemplate);
            $route.reload();
        }

        $scope.rowCollection = [];
        $scope.user = {};

        $scope.loadDropdowns = function () {
            //$scope.loadDepartment();
            //$scope.loadCompany();
            var loadDropdownUserGroup = securityRepository.getDropdownUserGroupData();
            loadDropdownUserGroup.then(function (response) {
                if (response.data) {
                    $scope.userGroupList = response.data;
                }
                
            });
            
        }
        $scope.loadCompany = function () {
            $scope.companyList = [];
            securityRepository.loadCompany().then(function (response) {
                if (response.data) {
                    $scope.companyList = response.data;
                    var company = $filter('filter')($scope.companyList, function (d) { return d.Code === "NCC"})[0];
                    $scope.user.CompanyID = company.CompanyID;
                    $scope.companyWiseBranch($scope.user.CompanyID);
                }
            }).catch(function (response) {
                toastr.warning('No Data Found!!');
            });
        }

        $scope.companyWiseBranch = function (companyID) {
            debugger 
            var branchesList = securityRepository.companyWiseBranch(companyID);
            branchesList.then(function (response) {
                $scope.branchList = response.data;
            }).catch(function (response) {
                toastr.warning('No Data Found!!');
            });
        }
        $scope.loadDepartment = function () {
            $scope.deptList = [];
            var data = settingsRepository.deptList().then(function (response) {
                if (response.data) {
                    $scope.deptList = response.data;
                }

            });
        }
        $scope.getDeptWiseEmployee = function (id) {
            var getUser = securityRepository.getDeptWiseEmployee(id);
            getUser.then(function (response) {
                if (response.data) {
                    $scope.employeeList = response.data;
                }
            }).catch(function (response) {
                toastr.error("Data Not Found!!");
            });
        }
        $scope.getEmployeeName = function () {
            debugger 
            if ($scope.editUser.ID > 0) {
                var emp = $filter('filter')($scope.employeeList, function (d) { return d.EmployeeID === $scope.editUser.EmployeeID; })[0];
                var personnelName = emp.EmpName;
                hyphen = personnelName.split(" ");
                if (hyphen.length > 1) {
                    hyphen = hyphen[0] + " " + hyphen[1] + "";
                } else {
                    hyphen = hyphen[0];
                }
                $scope.editUser.UserFullName = hyphen;
            } else {
                var emp = $filter('filter')($scope.employeeList, function (d) { return d.EmployeeID === $scope.user.EmployeeID; })[0];
                var personnelName = emp.EmpName;
                hyphen = personnelName.split(" ");
                if (hyphen.length > 1) {
                    hyphen = hyphen[0] + " " + hyphen[1] + "";
                } else {
                    hyphen = hyphen[0];
                }
                $scope.user.UserFullName = hyphen;
            }
            
        }
        $scope.getUser = function () {
            var getUser = securityRepository.getAllUserData();
            getUser.then(function (response) {
                if (response.data) {
                    $scope.rowCollection = response.data;
                }
            }).catch(function (response) {
                $scope.name += response.data.toUpperCase() + '!!';
            });
            $scope.itemsByPage = 10;
        }

        $scope.deleteRow = function (id) {
            var deleteUser = securityRepository.deleteUser(id);
            deleteUser.then(function (response) {
                if (response.data.success === true) {
                    $scope.Reload();
                    toastr.success(response.data.message);
                } else {
                    toastr.error(response.data.message);
                }
            });
        }

        $scope.activeOrInActive = function (id, status) {
            var userStatus = securityRepository.activeDeActiveUser(id, status);
            userStatus.then(function (response) {
                if (response.data.success === true) {
                    $scope.Reload();
                    toastr.success(response.data.message);
                } else {
                    toastr.error(response.data.message);
                }
            });
        }

        $scope.filterValueForNumberOnly = function (event) {
            var key = window.event ? event.keyCode : event.which;
            if ((key >= 48 && key <= 57)|| key === 8 || key === 46) {
                return true;
            }
            else {
                return;
            }
        }
        

        //Create User
        $scope.saveUser = function () {
            debugger
            $scope.user.SecurityQuestion = $scope.SecurityQuestions[0];
            $scope.user.SecurityQueAns = 'BD';
            //$scope.user.BranchID = $scope.branchList[0].BranchID;

            debugger;
            var newPassword = $scope.user.Password;
            var confirmPassword = $scope.user.ConfirmPassword;
            if (newPassword.length < 6 || confirmPassword < 6) {
                toastr.error("Your password will be minimum 6 character");
                return;
            }
            if ($scope.user.Password !== $scope.user.ConfirmPassword) {
                toastr.error("New and Confirm Password are not matched");
                return;
            }
            if ($scope.CreateUserForm.$valid) {
                var createUser = securityRepository.saveUserData($scope.user);
                createUser.then(function (response) {
                    if (response.data.isSucess) {
                        toastr.success(response.data.message);
                        $scope.clierForm();
                    } else {
                        if (response.data.message === "LogOut") {
                            $location.path('#!/LogIn');
                        }
                        toastr.error(response.data.message);
                    }
                }).catch(function (response) {
                    toastr.error($scope.name += response.data + '!!');
                });
            } else {
                toastr.error("All input field are not valid");
            }
        }

        $scope.editRow = function (row) {
            $cookieStore.put('user', row);
            $location.path('/EditUser');
        }
        $scope.editUser = {};
        $scope.editUserByID = function () {
            $scope.loadDropdowns();
            $scope.editUser = $cookieStore.get('user');
            debugger 
            if ($scope.editUser.CompanyID) {
                $scope.companyWiseBranch($scope.editUser.CompanyID);
                if ($scope.editUser.DepartmentID) {
                    $scope.getDeptWiseEmployee($scope.editUser.DepartmentID);
                }
            }
        }
        $scope.clierForm = function () {
            $scope.user = {};
            $scope.editUser = {};
            $cookieStore.put('user', $scope.editUser);
        }


        $scope.updateUser = function () {
            if ($scope.UpdateUserForm.$valid) {
                $scope.data = {
                    ID: $scope.editUser.ID,            
                    SecurityQuestion: $scope.SecurityQuestions[0],
                    SecutiryAnswer: 'BD'
                };

                var editUser = securityRepository.updateUserData($scope.editUser);
                editUser.then(function(response) {
                    if (response.data.isSucess) {
                        toastr.success(response.data.message);
                        $scope.clierForm();
                    } else {
                        if (response.data.message === "LogOut") {
                            $location.path('#!/LogIn');
                        }
                        toastr.error(response.data.message);
                    }
                });
            } else {
                toastr.error("All input field are not valid");
            }
        }


    });
