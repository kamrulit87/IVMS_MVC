angular.module('app').directive('ngModelOnblur', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        priority: 1,
        link: function (scope, elm, attr, ngModelCtrl) {
            if (attr.type === 'radio' || attr.type === 'checkbox') return;

            elm.unbind('input').unbind('keydown').unbind('change');
            elm.bind('blur', function () {
                scope.$apply(function () {
                    ngModelCtrl.$setViewValue(elm.val());
                });
            });
        }
    };
});

angular.module('app').directive('myModal', function () {
    return {
        restrict: 'A',
        scope: { myModalIsOpen: '=' },
        link: function (scope, element, attr) {
            scope.$watch(
                function () { return scope.myModalIsOpen; },
                function () { element.modal(scope.myModalIsOpen ? 'show' : 'hide'); }
            );
        }
    };
});

angular.module('app').directive('clockpicker', [
    function () {
        var link;
        link = function (scope, element, attr, ngModel) {
            element.clockpicker({
                autoclose: true,
                dayOfWeekStart: 1,
                lang: 'en',
                formatTime: 'g a',
                step: 05
            });

            element.on('dp.change', function (event) {
            });
        };
        return {
            restrict: 'A',
            link: link,
            require: 'ngModel'
        };
    }
]);