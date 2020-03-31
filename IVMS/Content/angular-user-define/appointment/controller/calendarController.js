angular.module('app').controller('calendarController', function ($scope, $http, $filter, $cookieStore, $window, appointmentRepository, commonRepository) {

    $scope.loadCalendarDropdowns = function() {
        $http.get("/UnScheduleAppointment/GetEvents").then(function (response) {
            var events = [];
            debugger 
            $.each(response.data, function (i, v) {
                if (v.AppointmentBy == "E") {
                    events.push({
                        title: 'Meeting With : ' + v.VisitorName,
                        companyName: v.CompanyName,
                        purpose: v.Purpose,
                        start: moment(v.AppointmentDate),
                        end: v.AppointmentDate != null ? moment(v.AppointmentDate) : null,
                        time: v.AppointmentTime != null ? moment(v.AppointmentTime) : null,
                        appointment: "Schedule",
                        color: "#7986CB",
                        allDay: v.IsFullDay,

                    });
                }
                if (v.AppointmentBy == "R") {
                    events.push({
                        title: 'Meeting With : ' + v.VisitorName,
                        companyName: v.CompanyName,
                        purpose: v.Purpose,
                        start: moment(v.AppointmentDate),
                        end: v.AppointmentDate != null ? moment(v.AppointmentDate) : null,
                        time: v.AppointmentTime != null ? moment(v.AppointmentTime) : null,
                        appointment: "Un-Schedule",
                        color: "#B80672",
                        
                        allDay: v.IsFullDay,

                    });
                }
                if (v.AppointmentBy == "F") {
                    events.push({
                        title: 'Meeting With : ' + v.VisitorName,
                        companyName: v.CompanyName,
                        purpose: v.Purpose,
                        start: moment(v.AppointmentDate),
                        end: v.AppointmentDate != null ? moment(v.AppointmentDate) : null,
                        time: v.AppointmentTime != null ? moment(v.AppointmentTime) : null,
                        appointment: "Forward",
                        color: "green",
                        allDay: v.IsFullDay,

                    });
                }

            })

            $scope.GenerateCalender(events);
        });
    }
    $scope.GenerateCalender = function (events) {
        $('#calender').fullCalendar('destroy');
        $('#calender').fullCalendar({
            contentHeight: 400,
            defaultDate: new Date(),
            timeFormat: 'h(:mm)a',
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,basicWeek,basicDay'
            },
            eventLimit: true,
            eventColor: '#378006',
            events: events,
            eventClick: function (calEvent, jsEvent, view) {
                $('#myModal #eventTitle').text(calEvent.title);
                var $purpose = $('<div/>');
                $purpose.append($('<p/>').html('<b>Company: </b>' + calEvent.companyName));
                $purpose.append($('<p/>').html('<b>Date: </b>' + calEvent.start.format("DD-MMM-YYYY ")));
                if (calEvent.time != null) {
                    $purpose.append($('<p/>').html('<b>Time: </b>' + calEvent.time.format("HH:mm a")));
                }
                if (calEvent.appointment != null) {
                    $purpose.append($('<p/>').html('<b>Appointment Type: </b>' + calEvent.appointment));
                }
                $purpose.append($('<p/>').html('<b>Purpose: </b>' + calEvent.purpose));


                $('#myModal #pDetails').empty().html($purpose);

                $('#myModal').modal();
            }
        })
    }

})