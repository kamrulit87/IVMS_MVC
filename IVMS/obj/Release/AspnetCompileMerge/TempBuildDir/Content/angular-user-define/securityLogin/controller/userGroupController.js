angular.module('app').controller('userGroupController', function ($scope, $filter, $location, $route, $templateCache, $cookieStore, securityRepository) {

    //start :: ng-idel => when user idel specific time then it will be auto logout
    $scope.$on('IdleStart', function () {
        toastr.warning('your session will be end soon!!');
    });

    $scope.$on('IdleTimeout', function () {
        window.location = "/Security/LogOff#!/";
    });
    //End :: ng-idel

        $scope.Reload = function () {
            var currentPageTemplate = $route.current.templateUrl;
            $templateCache.remove(currentPageTemplate);
            $route.reload();
        }

        $scope.rowCollection = [];

        $scope.getUserGroup = function () {
            var getUserGroup = securityRepository.getUserGroupData();
            getUserGroup.then(function (response) {
                debugger 
                if (response.data) {
                    for (var i = 0; i < response.data.length; i++) {
                        if (response.data[i].CreatedDate) {
                            var createdDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                            response.data[i].CreatedDate = $filter('date')(createdDate, "dd-MMM-yyyy");
                        }
                    }
                    $scope.rowCollection = response.data;
                }
            }).catch(function (response) {
                toastr.error("Data No Found!!");
            });
            $scope.itemsByPage = 10;
        }

        $scope.deleteRow = function (id) {
            var deleteUserGroup = securityRepository.deleteUserGroup(id);
            deleteUserGroup.then(function (response) {
                if (response.data.success === true) {
                    $scope.Reload();
                    toastr.success(response.data.message);
                } else {
                    toastr.error(response.data.message);
                }
            });
        }

        $scope.editRow = function (row) {
            $cookieStore.put('userGroup', row);
            $location.path('/EditUserGroup');
            //$scope.loadUserGroupById(row.ID);
        }


        $scope.loadUserGroupById = function () {
            $scope.editUserGroup = $cookieStore.get('userGroup');
            $id = $scope.editUserGroup.ID;
            $IsAdmin = $scope.editUserGroup.IsAdmin;
            $cookieStore.remove("userGroup");
            if ($id > 0) {
                $scope.LoadMappingDataForEdit($id);
            }
        }

        $scope.LoadMappingDataForEdit = function (id) {
            var getEditPageMappingData = securityRepository.getEditPageMappingData(id);
            getEditPageMappingData.then(function (response) {
                if (response.data) {
                    debugger 
                    $scope.tree_data = response.data;
                }
            }).catch(function (response) {
                toastr.error($scope.name += response.data + '!!');
            });
        }

        $scope.updateUserGroup = function () {
            debugger;
            $scope.editUserGroup.Name = $scope.editUserGroup.UserGroup;
            if ($scope.EditUserGroupForm.$valid) {
                var editUserGroup = securityRepository.putUsergroupData($scope.editUserGroup, $scope.pageActionData);
                editUserGroup.then(function (response) {
                    if (response.data.isSucess) {
                        toastr.success(response.data.message);
                        $scope.clearData = {};
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

        $scope.tree_data = [];
        $scope.pageActionData = [];
        $scope.clearData = function () {
            $scope.userGroup = {};
            $scope.pageActionData = [];
            $cookieStore.remove("userGroup");
            $scope.editUserGroup = {};
        }


        $scope.col_defs = [
            {
                field: "Name" ,
                displayName: "Module Name"
            },
            {
                field: "ModuleName",
                displayName: "Page Name"
            },
            {
                field: "Create",
                displayName: "Create",
                cellTemplate: "<input type='checkbox' ng-show='row.branch.ShowHide == true' ng-model = row.branch.Create ng-click='cellTemplateScope.click(row.branch.PageId, row.branch.Create, row.branch.ModuleId )'/>",
                cellTemplateScope: {
                    click: function (id, status, moduleId) {
                        debugger;
                        $scope.setPageData(id, status, 'C', moduleId);
                    }
               }
            },
            {
                field: "Select",
                displayName: "Select",
                cellTemplate: "<input type='checkbox' ng-show='row.branch.ShowHide == true' ng-model = row.branch.Select ng-click='cellTemplateScope.click(row.branch.PageId, row.branch.Select, row.branch.ModuleId  )'/>",
                cellTemplateScope: {
                    click: function (id, status, moduleId) {
                        $scope.setPageData(id, status, 'R', moduleId);
                    }
                }
            },
            {
                field: "Edit",
                displayName: "Edit",
                cellTemplate: "<input type='checkbox' ng-show='row.branch.ShowHide == true' ng-model = row.branch.Edit ng-click='cellTemplateScope.click(row.branch.PageId, row.branch.Edit, row.branch.ModuleId  )'/>",
                cellTemplateScope: {
                    click: function (id, status, moduleId) {
                        $scope.setPageData(id, status, 'U', moduleId);
                    }
                }
            },
            {
                field: "Delete",
                displayName: "Delete",
                cellTemplate: "<input type='checkbox' ng-show='row.branch.ShowHide == true' ng-model = row.branch.Delete ng-click='cellTemplateScope.click(row.branch.PageId, row.branch.Delete, row.branch.ModuleId )'/>",
                cellTemplateScope: {
                    click: function (id, status, moduleId) {
                        $scope.setPageData(id, status, 'D', moduleId);
                    }
                }
            }
        ];

        $scope.Status = true;
        $scope.setPageData = function (id, status, action, moduleId) {
            debugger;
            if (status === false) {
                status = true;
            } else {
                status = false;
            }
            var arrayData = $scope.pageActionData;
            for (var i = 0; i < arrayData.length; i++) {
                if ($scope.pageActionData[i].PageId === id) {
                    if(action === 'C')
                        $scope.pageActionData[i].Create = status;
                    if (action === 'R')
                        $scope.pageActionData[i].Select = status;
                    if (action === 'U')
                        $scope.pageActionData[i].Edit = status;
                    if (action === 'D')
                        $scope.pageActionData[i].Delete = status;
                    $scope.Status = false;
                }
            }
            if ($scope.Status === true) {
                $scope.pageActionData.push({
                    PageId: id,
                    ModuleId: moduleId,
                    Create: action === 'C' ? status : null,
                    Select: action === 'R' ? status : null,
                    Edit: action === 'U' ? status : null,
                    Delete: action === 'D' ? status : null
                });
            }
            $scope.Status = true;
        }

        $scope.saveUserGroup = function () {
            debugger 
            if ($scope.CreateUserGroupForm.$valid) {
                var createUserGroup = securityRepository.postUsergroupData($scope.userGroup);
                createUserGroup.then(function (response) {
                    if (response.data.isSucess) {
                        toastr.success(response.data.message);
                        $scope.clearData();
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


    });
