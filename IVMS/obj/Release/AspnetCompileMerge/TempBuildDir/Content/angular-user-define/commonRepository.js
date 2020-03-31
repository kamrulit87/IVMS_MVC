angular.module('app').factory('commonRepository', function ($http) {
    return {

        //loadProject: function () {
        //    return $http.post('/Project/LoadProjects');
        //},

        generateCode: function(tableName, fieldName, prefix){
            return $http.post('/Common/GenerateCode', { "tableName": tableName, "fieldName": fieldName, "prefix": prefix });
        },
        generateFloors: function (noOfFloor) {
            var floors = [];
            for (var i = 0; i < noOfFloor; i++) {
                var floor = {
                    id: i,
                    label: i
                };
                floors.push(floor);
            }

            return floors;
        },
        getAppoStatus: function () {
            var statusValue = [{
                Status: 'A',
                StatusLabel: 'Appointment'
            }, {
                Status: 'I',
                StatusLabel: 'IN'
            }, {
                Status: 'O',
                StatusLabel: 'OUT'
            }, {
                Status: 'R',
                StatusLabel: 'Re.Schedule'
            }, {
                Status: 'C',
                StatusLabel: 'Cancel'
            }];
            return statusValue;
        },
        getAStatus: function () {
            var AStatus = [{
                    id: 1,
                    label: "Active"
                },
                {
                    id: 0,
                    label: "Inactive"
                }];
            return AStatus;
        },
        convertStatusText: function (statusID) {
            var status = '';
            switch (statusID) {
                case '1':
                    status = 'Create';
                    break
                case '2':
                    status = 'Approved';
                    break
                case '3':
                    status = 'Pending';
                    break
                case '4':
                    status = 'canceled';
                    break
                case '5':
                    status = 'Updated';
                    break
            }
            return status
        },
        getFloorNames: function (noOfFloors) {

            var floors = [];
            var floor = {};

            floor = {
                id: 0,
                label: 'Ground Floor'
            }
            floors.push(floor);

            for (var i = 1; i < noOfFloors; i++) {
                var suffix = 'th';

                var lastone = i.toString().split('').pop();

                if (lastone == 1 && i != 11) {
                    suffix = 'st';
                }

                if (lastone == 2 && i != 12) {
                    suffix = 'nd';
                }

                if (lastone == 3 && i != 13) {
                    suffix = 'rd';
                }

                floor = {
                    id: i,
                    label: i + suffix + ' Floor'
                }

                floors.push(floor);

            }
            return floors;
        },
        getAvailableStatus: function () {
            var availableStatus = [{
                Status: 1,
                StatusLabel: 'Yes'
            }, {
                Status: 0,
                StatusLabel: 'No'
            }];
            return availableStatus;
        },
        convertAvailableStatusText: function (statusID) {
            var status = '';
            switch (statusID) {
            case '1':
                status = 'Yes';
                break
            case '0':
                status = 'No';
                break
            }
            return status
        },
        
        viewDocument: function (id, alertMessage) {
            var confirmatonMsg = alertMessage;
            var reportUrl = '/Reports/BoqReports/ViewBoqDtlsReport/';
            var transactionID = id;
            printDocument(reportUrl, transactionID, confirmatonMsg); //function location asset/custom.js       
        },
        viewMultiParameterDocument: function (monthID, yearID, confirmationMsg, url) {
            var reportUrl = url;
            var parameters = { MonthID: monthID, YearID: yearID };
            printBaseOnMultiParameter(reportUrl, parameters, confirmationMsg);
        },
        viewComMultiParameterDocument: function (parameters, confirmationMsg, url) {
            var reportUrl = url;
            var parameters = parameters;
            printBaseOnMultiParameter(reportUrl, parameters, confirmationMsg);
        },
    }
})