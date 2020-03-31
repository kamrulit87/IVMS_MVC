angular.module('app').controller('pageController', function ($scope, $filter, $location, $route, $templateCache, $cookieStore, securityRepository) {

    //start :: ng-idel => when user idel specific time then it will be auto logout
    $scope.$on('IdleStart', function () {
        toastr.warning('your session will be end soon!!');
    });

    $scope.$on('IdleTimeout', function () {
        window.location = "/Security/LogOff#!/";
    });
    //End :: ng-idel
    $scope.page = {};
    $scope.pageList = [];
    $scope.removeCookies = function () {
        $scope.page = {};
        $cookieStore.put('page', $scope.page);
    }
    $scope.Reload = function () {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }
    $scope.loadDropdowns = function() {
        $scope.getModuleData();
        $scope.page = $cookieStore.get('page');
    }
    $scope.getModuleData = function() {
        securityRepository.getModuleData().then(function (response) {
            if (response.data) {
                $scope.moduleList = response.data;
            }
        }).catch(function (response) {
            toastr.warning('No Data found!!');
        });
    }

    $scope.deleteRow = function (id) {
        var deletepage = securityRepository.deletePage(id);
        deletepage.then(function (response) {
            if (response.data.isSucess === true) {
                $scope.Reload();
                toastr.success(response.data.message);
            } else {
                toastr.error(response.data.message);
            }
        });
    }

    $scope.editRow = function (row) {
        $cookieStore.put('page', row);
        $location.path('/AddPage');
    }

    $scope.savePage = function () {
        debugger
        if ($scope.CreatePageForm.$valid) {
            var createpage = securityRepository.postPageData($scope.page);
            createpage.then(function (response) {
                if (response.data.isSucess) {
                    toastr.success(response.data.message);
                    $scope.Reload();
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
    $scope.loadPage = function () {
        securityRepository.loadPage().then(function(response) {
            if (response.data) {
                $scope.pageList = response.data;
            }
        }).catch(function(response) {
            toastr.warning('No Data found!!');
        });
    }
    $scope.clearData = function() {
        $scope.page = {};
        $cookieStore.remove();
    }

});
