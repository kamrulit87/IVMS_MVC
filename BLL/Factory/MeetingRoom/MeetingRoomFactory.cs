using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Common;
using BLL.Interfaces;
using BLL.Interfaces.MeetingRoom;
using DAL.db;

namespace BLL.Factory.MeetingRoom
{
    public class MeetingRoomFactory : GenericFactory<IVMS_DBEntities, DAL.db.MeetingRoom>
    {
    }

    public class MeetingRoomFactorys : IMeetingRoomFactory
    {
        private IGenericFactory<DAL.db.MeetingRoom> _mrFactory;
        private IGenericFactory<MeetingRoomRequisition> _mrReqFactory;
        Result _result = new Result();
        private string tableName = "Meeting Room";
        public Result SaveMeetingRoom(DAL.db.MeetingRoom meetingRoom)
        {
            _mrFactory = new MeetingRoomFactory();
            try
            {
                if (meetingRoom.MeetingRoomID > 0)
                {
                    _mrFactory.Edit(meetingRoom);
                    _result = _mrFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.UpdateSuccessfull(tableName);
                    }
                }
                else
                {
                    _mrFactory.Add(meetingRoom);
                    _result = _mrFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.SaveSuccessfull(tableName);
                    }
                }
            }
            catch (Exception e)
            {
                _result.isSucess = false;
                _result.message = e.Message;
            }
            return _result;
        }

        public List<DAL.db.MeetingRoom> SearchMeetingRoom(int? meetingRoomID)
        {
            _mrFactory = new MeetingRoomFactory();
            try
            {
                var list = new List<DAL.db.MeetingRoom>();
                if (meetingRoomID> 0)
                {
                    list = _mrFactory.FindBy(x => x.MeetingRoomID == meetingRoomID).ToList();
                }
                else
                {
                    list = _mrFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
