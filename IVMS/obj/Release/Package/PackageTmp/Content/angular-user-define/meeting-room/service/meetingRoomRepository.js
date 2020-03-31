angular.module('app').factory('meetingRoomRepository', function ($http) {
    return {
        //Common

        //Meeting Room
        saveMeetingRoom: function (data) {
            return $http.post('/MeetingRoom/SaveMeetingRoom', data);
        },
        loadMeetingRoom: function (id) {
            return $http.post('/MeetingRoom/LoadMeetingRoom', { "meetingRoomID": id });
        },
        getMeetingReq: function (id, date) {
            return $http.post('/MeetingRoom/GetMeetingReq', {"roomID": id, "date": date });
        },
        getMeetingReq2: function (id) {
            return $http.post('/MeetingRoom/GetMeetingReq2', { "id": id });
        },
        //Meeting Room Requisition
        saveMeetingReqRoom: function (data, dataDtls,deleteStoreReqDtlsID) {
            return $http.post('/MeetingRoom/SaveMeetingReqRoom', { "meetingRoomReq": data, "participantList": dataDtls, "deleteStoreReqDtlsID": deleteStoreReqDtlsID });
        },
        searchMeetingRoomParticipant: function (id) {
            return $http.post('/MeetingRoom/SearchMeetingRoomParticipant', { "id": id });
        },
        getMeetingRoomAppData: function (data) {
            return $http.post('/MeetingRoom/GetMeetingRoomAppData', { "status": data.Status});
        }
        
    }
})