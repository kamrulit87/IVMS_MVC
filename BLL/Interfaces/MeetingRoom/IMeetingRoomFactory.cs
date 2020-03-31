using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Common;
using DAL.db;

namespace BLL.Interfaces.MeetingRoom
{
    public interface IMeetingRoomFactory
    {
        Result SaveMeetingRoom(DAL.db.MeetingRoom meetingRoom);
        List<DAL.db.MeetingRoom> SearchMeetingRoom(int? meetingRoomID);
    }
    public interface IMeetingRoomReqFactory
    {
        Result SaveMeetingReq(MeetingRoomRequisition meetingReq, List<DAL.db.MeetingParticipant> participantList, List<int> deleteStoreReqDtlsID);

        List<MeetingRoomRequisition> SearchMeetingRoom(int roomId, DateTime date);
        List<DAL.db.MeetingParticipant> SearchMeetingRoomParticipant(int? id);
        List<MeetingRoomRequisition> SearchMeetingRoomReq(int? id);
    }

}
