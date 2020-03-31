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
    public class MeetingRoomReqFactory : GenericFactory<IVMS_DBEntities, MeetingRoomRequisition>
    {
    }
    public class ParticipantFactory : GenericFactory<IVMS_DBEntities, MeetingParticipant>
    {
    }
    public class MeetingRoomReqFactorys : IMeetingRoomReqFactory
    {
        private IGenericFactory<MeetingRoomRequisition> _mrReqFactory;
        private IGenericFactory<DAL.db.MeetingParticipant> _mrPrFactory;
        Result _result = new Result();
        private string tableName = "Meeting Room Requisition";

        public Result SaveMeetingReq(MeetingRoomRequisition meetingRoomReq,List<DAL.db.MeetingParticipant> participantList,List<int> deleteStoreReqDtlsID)
        {
            _mrReqFactory = new MeetingRoomReqFactory();
            _mrPrFactory = new ParticipantFactory(); 
            try
            {
                if (meetingRoomReq.RequisitionID > 0)
                {
                    _mrReqFactory.Edit(meetingRoomReq);
                    _result = _mrReqFactory.Save();
                    if (_result.isSucess)
                    {
                        if (deleteStoreReqDtlsID != null)
                        {
                            foreach (var detailsID in deleteStoreReqDtlsID)
                            {
                                _mrPrFactory.Delete(x => x.ParticipantID == detailsID);
                                _result = _mrPrFactory.Save();
                            }
                        }

                        if (participantList != null)
                        {
                            foreach (var list in participantList)
                            {
                                if (list.ParticipantID < 1)
                                {
                                    list.RequisitionID = meetingRoomReq.RequisitionID;
                                    _mrPrFactory.Add(list);
                                    _result = _mrPrFactory.Save();

                                    if (_result.isSucess)
                                    {
                                        _result.message = _result.UpdateSuccessfull(tableName);
                                    }
                                }
                                else
                                {
                                    _mrPrFactory.Edit(list);
                                    _result = _mrPrFactory.Save();

                                    if (_result.isSucess)
                                    {
                                        _result.message = _result.UpdateSuccessfull(tableName);
                                    }
                                }
                            }
                        } 

                    }
                    
                }
                else
                {
                    _mrReqFactory.Add(meetingRoomReq);
                    _result = _mrReqFactory.Save();
                    if (_result.isSucess)
                    {
                        if (participantList != null && participantList.Count > 0)
                        {
                            foreach (var list in participantList)
                            {
                                if (list.ParticipantID < 1)
                                {

                                    list.RequisitionID = meetingRoomReq.RequisitionID;
                                    _mrPrFactory.Add(list);
                                    _result = _mrPrFactory.Save();

                                    if (_result.isSucess)
                                    {
                                        _result.message = _result.UpdateSuccessfull(tableName);
                                    }

                                }
                            }
                        }
                        
                    }

                }
            }
            catch (Exception e)
            {
                _mrReqFactory.Delete(meetingRoomReq);
                _result = _mrReqFactory.Save();
                _result.isSucess = false;
                _result.message = e.Message;
            }

            return _result;
        }

        public List<MeetingRoomRequisition> SearchMeetingRoom(int roomId, DateTime date)
        {
            _mrReqFactory = new MeetingRoomReqFactory();
            try
            {
                var list = new List<MeetingRoomRequisition>();
                list = _mrReqFactory.FindBy(x => x.MeetingRoomID == roomId && x.RequiredDate == date).ToList();
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<MeetingRoomRequisition> SearchMeetingRoomReq(int ? id)
        {
            _mrReqFactory = new MeetingRoomReqFactory();
            try
            {
                var list = new List<MeetingRoomRequisition>();
                if (id > 0)
                {
                    list = _mrReqFactory.FindBy(x => x.MeetingRoomID == id).ToList();
                }
                else
                {
                    list = _mrReqFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<DAL.db.MeetingParticipant> SearchMeetingRoomParticipant(int? id)
        {
            _mrPrFactory = new ParticipantFactory(); 
            try
            {
                var list = new List<MeetingParticipant>();
                if(id !=null){
                    list = _mrPrFactory.FindBy(x => x.RequisitionID == id).ToList();
                }
                else
                {
                    list = _mrPrFactory.GetAll().ToList();
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
