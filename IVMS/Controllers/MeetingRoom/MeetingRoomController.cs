using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Common;
using BLL.Factory.MeetingRoom;
using BLL.Interfaces.MeetingRoom;
using DAL.db;
using DAL.Helper;
using System.Data.SqlClient;
using IVMS.Models;

namespace IVMS.Controllers.MeetingRoom
{
    public class MeetingRoomController : Controller
    {
        Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
        private IMeetingRoomFactory mrFactory;
        private IMeetingRoomReqFactory mrReqFactory;
        IVMS_DBEntities db = new IVMS_DBEntities();
        private Result result;
        DateTime todayTime = DateTime.Now;
        public ActionResult MeetingRoomList()
        {
            ViewBag.CallingForm = "MeetingRoom";
            ViewBag.CallingForm1 = "Meeting Room";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult MeetingRoomRequisitionList()
        {
            ViewBag.CallingForm = "MeetingRoom Requisition";
            ViewBag.CallingForm1 = "Meeting Room Requisition";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult MeetingRoomReqList()
        {
            ViewBag.CallingForm = "MeetingRoom";
            ViewBag.CallingForm1 = "Meeting Room Requisition";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult MeetingRoomReqApproval()
        {
            ViewBag.CallingForm = "MeetingRoom";
            ViewBag.CallingForm1 = "Meeting Room Requisition Approval";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        [HttpPost]
        public JsonResult LoadMeetingRoom(int? meetingRoomID)
        {
            mrFactory = new MeetingRoomFactorys();
            List<DAL.db.MeetingRoom> list = mrFactory.SearchMeetingRoom(meetingRoomID);
            var appointmentList = list.Select(x => new { x.MeetingRoomID, x.RoomNo, x.FloorLocation, x.ProjectorAvailable, x.SeatCapacity });
            return Json(appointmentList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetMeetingReq(int roomID, DateTime date)
        {
            mrReqFactory = new MeetingRoomReqFactorys();
            List<MeetingRoomRequisition> list = mrReqFactory.SearchMeetingRoom(roomID, date);
            var appointmentList = list.Select(x => new { x.RequiredDate, x.MeetingRoomID, x.RequisitionID, x.Remarks, x.FromTime, x.ToTime, Time = x.FromTime, x.Employee.EmpName, x.MeetingRoom.RoomNo }).OrderByDescending(x => x.RequiredDate);
            return Json(appointmentList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetMeetingReq2(int? id)
        {
            mrReqFactory = new MeetingRoomReqFactorys();
            List<MeetingRoomRequisition> list = mrReqFactory.SearchMeetingRoomReq(id);
            var appointmentList = list.Select(x => new { x.RequiredDate, x.MeetingRoomID, x.RequisitionID, x.Remarks, x.FromTime, x.ToTime, Time = x.FromTime, x.Employee.EmpName, x.MeetingRoom.RoomNo }).OrderByDescending(x=> x.RequiredDate);
            return Json(appointmentList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchMeetingRoomParticipant(int? id)
        {
            mrReqFactory = new MeetingRoomReqFactorys();
            List<DAL.db.MeetingParticipant> list = mrReqFactory.SearchMeetingRoomParticipant(id);
            var participantList = list.Select(x => new { x.Employee.EmpName, x.Department.DepartmentName,x.EmployeeID,x.DesignationID,x.DepartmentID,x.ParticipantID,x.RequisitionID });
            return Json(participantList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MeetingRoomCreate()
        {
            DefaultLoad();
            return View();
        }

        [HttpPost]
        public JsonResult SaveMeetingRoom(DAL.db.MeetingRoom meetingRoom)
        {
            mrFactory = new MeetingRoomFactorys();
            result = mrFactory.SaveMeetingRoom(meetingRoom);
            return Json(result);
        }
        [HttpPost]
        public JsonResult SaveMeetingReqRoom(DAL.db.MeetingRoomRequisition meetingRoomReq, List<DAL.db.MeetingParticipant> participantList,List <int> deleteStoreReqDtlsID)
        {
            int empID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            mrReqFactory = new MeetingRoomReqFactorys();
            meetingRoomReq.CreatedBy = empID;
            meetingRoomReq.CreatedDate = todayTime;
            meetingRoomReq.RequisitionBy = empID;
            result = mrReqFactory.SaveMeetingReq(meetingRoomReq, participantList, deleteStoreReqDtlsID);
            return Json(result);
        }
        [HttpPost]
        public JsonResult GetMeetingRoomAppData(string status)
        {
            //var fDate = fromDate.ToString("yyyy-MM-dd");
            //var tDate = toDate.ToString("yyyy-MM-dd");
            //if(fDate==null)
            //{
            //    fDate="";
            //}
            //if (tDate == null)
            //{
            //    tDate = "";
            //}

            if (status == null)
            {
                status = "p";
            }



            int empID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            string UserGroup=(dictionary[6].Id == "" ? "" : (dictionary[6].Id));

            if(UserGroup=="4")
            {
                empID = 0;
            }

            SqlParameter UserId = new SqlParameter("@UserId", empID);
            SqlParameter RoomStatus = new SqlParameter("@RoomStatus", status);
            //SqlParameter FromDate = new SqlParameter("@FromDate", fDate);
            //SqlParameter ToDate = new SqlParameter("@ToDate", tDate);
            var approvalList = db.Database.SqlQuery<VM_MeetingRoomApproved>("[dbo].[USP_LoadMeetingRoomRequisitionInfo] @UserId, @RoomStatus", UserId, RoomStatus).ToList();
            return Json(approvalList, JsonRequestBehavior.AllowGet);
        }

        public void DefaultLoad()
        {
            ViewBag.CallingForm = "MeetingRoom";
            ViewBag.CallingForm1 = "Meeting Room";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/MeetingRoom";
        }
    }
}