using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using BLL.Common;
using BLL.Factory.Appointment;
using BLL.Factory.Security;
using BLL.Interfaces.Appointment;
using BLL.Interfaces.Security;
using BLL.Models;
using DAL.db;
using DAL.Helper;
using Device.Services;
using IVMS.Models;
using System.Data.Entity;
using DAL.ViewModel;
using System.Data.SqlClient;

namespace IVMS.Controllers.Appointment
{
    public class UnScheduleAppointmentController : Controller
    {
        Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
        IVMS_DBEntities db = new IVMS_DBEntities();
        private IUnScheduleAppointmentFactory unScheduleAppointmentFactory;
        private ICardFactory cardFactory;
        private EmailUtility emailUtility;
        DateTime toDayTime = DateTime.Now;
        private Result result = new Result();
        Utility utility;
        string message = string.Empty;


        public ActionResult UnScheduleAppointmentList()
        {
            
            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = securityLogInFactory.GetCrudPermission(userGroupId, "UnSchedule");
                if (tblUserActionMapping.Select)
                {
                    ViewBag.CallingForm = "Appointment";
                    ViewBag.CallingForm1 = "UnSchedule Appointment";
                    ViewBag.CallingViewPage = "#";
                    return View();
                }
            }
            Session["logInSession"] = null;
            return Redirect("/#!/");
        }

        [HttpPost]
        public JsonResult GetDtlsAppointment()
        {
            
            //unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
            //List<DAL.db.Appointment> list = unScheduleAppointmentFactory.SearchAppointment("A");
            //var appointmentList = list.Select(x => new { x.AppointmentBy, x.AppointmentID, x.AppointmentDate,x.VisitorMobile, x.Employee.DesignationID, x.EmployeeID, x.Employee.DepartmentID, x.Employee.EmpName, x.Employee.Designation.DesignationName, x.Employee.Department.DepartmentName, x.CompanyName, x.Purpose, x.AppointmentTime, x.ReachTime, x.AccessFloors, x.CheckedInTime, x.CheckedOutTime, x.Status, x.CardNO, x.VisitorName, x.NotifyMessage, x.ReplayMessage, FirstWordMsg = x.ReplayMessage, x.CreatedBy, x.CreatedDate }).OrderByDescending(x => x.AppointmentDate);
            //return Json(appointmentList, JsonRequestBehavior.AllowGet);

            var appointmentList = from x in db.Appointments
                join e in db.Employees on x.EmployeeID equals e.EmployeeID
                join d in db.Departments on e.DepartmentID equals d.DepartmentID
                join de in db.Designations on e.DesignationID equals de.DesignationID
                where x.Status == "A" || x.Status == "N" || x.Status == "AP" || x.Status == "P"
                select new
                {
                    x.AppointmentBy,
                    x.AppointmentID,
                    x.AppointmentDate,
                    x.EmployeeID,
                    x.VisitorMobile,
                    x.VisitorEmail,
                    d.DepartmentID,
                    de.DesignationID,
                    e.EmpName,
                    d.DepartmentName,
                    de.DesignationName,
                    x.CompanyName,
                    x.Purpose,
                    x.AppointmentTime,
                    x.ReachTime,
                    x.EndTime,
                    x.CardNO,
                    x.Status,
                    FirstWordMsg = x.ReplayMessage,
                    x.ReplayMessage,
                    x.VisitorName,
                    x.CreatedBy,
                    x.CreatedDate,
                    x.CheckedInTime,
                    x.CheckedOutTime,
                    x.AccessFloors, x.NotifyMessage
                };
            return Json(appointmentList.OrderByDescending(x => x.AppointmentDate).Take(100).ToList(), JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public JsonResult GetSearchCardNo(string cardNo)
        //{
        //    unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
        //    List<DAL.db.Appointment> list = unScheduleAppointmentFactory.SearchCardWiseAppointment(cardNo,"I");
        //    var appointmentList = list.Select(x => new { x.AppointmentBy, x.AppointmentID, x.AppointmentDate, x.EmployeeID, x.Employee.EmpName, x.CompanyName, x.Purpose, x.AppointmentTime, x.ReachTime, x.Status, x.CardNO, x.VisitorName, x.CreatedBy, x.CreatedDate });
        //    return Json(appointmentList, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult GetDtlsCheckIn()
        {
            unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
            List<DAL.db.Appointment> list = unScheduleAppointmentFactory.SearchCheckIn("I");
            var appointmentList = list.Select(x => new
            {
                x.AppointmentBy, x.AppointmentID, x.AppointmentDate, x.EmployeeID, x.Employee.EmpName, x.CompanyName,
                x.Purpose, x.AppointmentTime, x.ReachTime,x.EndTime,x.CheckedInTime, x.CheckedOutTime, x.Status, x.CardNO,
                x.VisitorName, x.VisitorMobile, x.VisitorEmail, x.CreatedBy, x.CreatedDate
            }).OrderByDescending(x => x.CheckedInTime).Take(10);
            return Json(appointmentList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckIsCardFree(string cardNo)
        {

            var prvCardNo = from x in db.Cards
                                  where x.CardNO == cardNo
                                  select new
                                  {
                                      x.CardNO
                                  };
            return Json(prvCardNo.FirstOrDefault(), JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GetDtlsCheckBreak()
        {
            unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
            List<DAL.db.Appointment> list = unScheduleAppointmentFactory.SearchCheckBreak("B");
            var appointmentList = list.Select(x => new
            {
                x.AppointmentBy, x.AppointmentID, x.AppointmentDate, x.EmployeeID, x.Employee.EmpName, x.CompanyName,
                x.Purpose, x.AppointmentTime, x.ReachTime,
                x.EndTime, x.CheckedInTime, x.CheckedOutTime, x.Status, x.CardNO,
                x.VisitorName,
                x.VisitorMobile,
                x.VisitorEmail,
                x.CreatedBy,
                x.CreatedDate
            }).OrderByDescending(x => x.CheckedInTime).Take(10);
            return Json(appointmentList, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public JsonResult GetDtlsCheckOut()
        //{
        //    unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
        //    List<DAL.db.Appointment> list = unScheduleAppointmentFactory.SearchCheckOut("O");
        //    var appointmentList = list.Select(x => new { x.AppointmentBy, x.AppointmentID, x.AppointmentDate, x.EmployeeID, x.Employee.EmpName, x.CompanyName, x.Purpose, x.AppointmentTime, x.ReachTime, x.CheckedInTime, x.CheckedOutTime, x.Status, x.CardNO, x.VisitorName, x.VisitorMobile, x.VisitorEmail, x.CreatedBy, x.CreatedDate }).OrderByDescending(x => x.CheckedOutTime).Take(10);
        //    return Json(appointmentList, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult GetDtlsCheckOut()
        {
            //unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
            //List<DAL.db.Appointment> list = unScheduleAppointmentFactory.SearchCheckOut("O");
            //var appointmentList = list.Select(x => new { x.AppointmentBy, x.AppointmentID, x.AppointmentDate, x.EmployeeID, x.Employee.EmpName, x.CompanyName, x.Purpose, x.AppointmentTime, x.ReachTime, x.CheckedInTime, x.CheckedOutTime, x.Status, x.CardNO, x.VisitorName, x.VisitorMobile, x.VisitorEmail, x.CreatedBy, x.CreatedDate }).OrderByDescending(x => x.CheckedOutTime).Take(10);
            //return Json(appointmentList, JsonRequestBehavior.AllowGet);

            SqlParameter p1 = new SqlParameter("@P1", "O");

            var appointmentList = db.Database.SqlQuery<VM_Appointments>("sp_AppointmentList @p1", p1).ToList();
            return Json(appointmentList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDtlsCancel()
        {
            unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
            List<DAL.db.Appointment> list = unScheduleAppointmentFactory.SearchCancel("C");
            var appointmentList = list.Select(x => new { x.AppointmentBy, x.AppointmentID, x.AppointmentDate, x.EmployeeID, x.Employee.EmpName, x.CompanyName, x.Purpose, x.AppointmentTime, x.ReachTime,x.EndTime, x.Status, x.VisitorName, x.VisitorMobile, x.VisitorEmail, x.CardNO, x.CreatedBy, x.CreatedDate });
            return Json(appointmentList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CardWiseAppointmentData(string cardNO)
        {
            unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
            DAL.db.Appointment list = unScheduleAppointmentFactory.SearchCardWiseAppointmentData(cardNO);
            //var appointmentList = list.Select(x => new { x.AppointmentBy, x.AppointmentID, x.AppointmentDate, x.EmployeeID, x.Employee.EmpName, x.CompanyName, x.Purpose, x.AppointmentTime, x.ReachTime, x.Status, x.VisitorName, x.CardNO, x.CreatedBy, x.CreatedDate, x.ReplayMessage, x.NotifyMessage });            
            if (list != null)
            {
                list = new DAL.db.Appointment { AppointmentID = list.AppointmentID, VisitorName = list.VisitorName, VisitorMobile = list.VisitorMobile, VisitorEmail = list.VisitorEmail, CompanyName = list.CompanyName, EmployeeID = list.EmployeeID, Purpose = list.Purpose, AppointmentDate = list.AppointmentDate, AppointmentTime = list.AppointmentTime, ReachTime = list.ReachTime, CheckedInTime = list.CheckedInTime, CheckedOutTime = list.CheckedOutTime, CreatedBy = list.CreatedBy, CreatedDate = list.CreatedDate, AppointmentBy = list.AppointmentBy, Status = list.Status, CardNO = list.CardNO, BankCardID = list.BankCardID, ReplayMessage = list.ReplayMessage, NotifyMessage = list.NotifyMessage, AccessFloors = list.AccessFloors };
            }  
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UnScheduleAppointmentCreate()
        {
            DefaultLoad();
            return View();
        }
        [HttpPost]
        public JsonResult SaveUnScheduleAppointment(DAL.db.Appointment appointment)
        {
            int empID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            if (empID == 0)
            {
                result.sessionOut = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
            appointment.CreatedBy = empID;
            appointment.CreatedDate = toDayTime;
            appointment.AppointmentTime = appointment.AppointmentTime;
            appointment.ReachTime = toDayTime;
            appointment.AppointmentDate = appointment.AppointmentDate;

            appointment.ReachTime = DateTime.Now;
            appointment.Status = "N";
            appointment.NotifyMessage = "Mr." + " " + appointment.VisitorName + " " + "wants to meet with you.";


            result = unScheduleAppointmentFactory.SaveAppointment(appointment);
            if (result.isSucess)
            {
                result.message = "Notify Successful.";

                //appointment.ReachTime = DateTime.Now;
                //emailUtility = new EmailUtility();
                //unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
                //appointment.Status = "N";
                //appointment.NotifyMessage = "Mr." + " " + appointment.VisitorName + " " + "wants to meet with you.";
                //result = unScheduleAppointmentFactory.SaveAppointment(appointment);
                //if (result.isSucess)
                //{
                //    result.message = "Notify Successful.";
                //}
            }
            
            //else
            //{
            //    foreach (var floor in empList)
            //    {
            //        unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
            //        if (floor.AppointmentID < 1)
            //        {
            //            appointment.AppointmentID = 0;
            //        }
            //        appointment.CreatedBy = empID;
            //        appointment.CreatedDate = toDayTime;
            //        appointment.AppointmentTime = appointment.AppointmentTime;
            //        appointment.AppointmentDate = appointment.AppointmentDate;
            //        appointment.ReachTime = toDayTime;
            //        appointment.EmployeeID = floor.EmployeeID;
            //        result = unScheduleAppointmentFactory.SaveAppointment(appointment);
            //        if (result.isSucess)
            //        {
            //            appointment.ReachTime = DateTime.Now;
            //            emailUtility = new EmailUtility();
            //            unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
            //            result = unScheduleAppointmentFactory.SaveAppointment(appointment);
            //            if (result.isSucess)
            //            {
            //                //if (emailUtility.IsInternetConnected())
            //                //{
            //                //    var email = db.Employees.Where(x => x.EmployeeID == appointment.EmployeeID).FirstOrDefault();
            //                //    if (email != null && email.Email != null)
            //                //    {
            //                //        bool isMail = emailUtility.SentMail(email.Email, appointment.CompanyName, appointment.VisitorName, "", "");
            //                //        if (isMail)
            //                //        {
            //                //            result.message = "Notify And Email Successful.";
            //                //        }
            //                //        return Json(result, JsonRequestBehavior.AllowGet);
            //                //    }
            //                //}
            //                result.message = "Notify Successful.";
            //            }

            //        }
            //    }
            //}
            
            result.lastInsertedID = appointment.EmployeeID;
            return Json(result, JsonRequestBehavior.AllowGet);
           
        }

        [HttpPost]
        public JsonResult SaveCancelAppointment(DAL.db.Appointment appointment, List<VM_Employee> empList)
        {
            int empID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            if (empID == 0)
            {
                result.sessionOut = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            // Testing
            if (empList == null)
            {
                unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
                appointment.CreatedBy = empID;
                appointment.CreatedDate = toDayTime;
                appointment.AppointmentTime = appointment.AppointmentTime;
                appointment.ReachTime = toDayTime;
                appointment.AppointmentDate = appointment.AppointmentDate;
                result = unScheduleAppointmentFactory.SaveAppointment(appointment);
                if (result.isSucess)
                {                    
                    emailUtility = new EmailUtility();
                    unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
                    appointment.Status = "C";
                    result = unScheduleAppointmentFactory.SaveAppointment(appointment);                  

                }
            }
            else
            {
                foreach (var floor in empList)
                {
                    unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
                    if (floor.AppointmentID < 1)
                    {
                        appointment.AppointmentID = 0;
                    }
                    appointment.CreatedBy = empID;
                    appointment.CreatedDate = toDayTime;
                    appointment.AppointmentTime = appointment.AppointmentTime;
                    appointment.AppointmentDate = appointment.AppointmentDate;
                    appointment.ReachTime = toDayTime;
                    appointment.EmployeeID = floor.EmployeeID;
                    result = unScheduleAppointmentFactory.SaveAppointment(appointment);                    
                }
            }

            result.lastInsertedID = appointment.EmployeeID;
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult VisitorIn(DAL.db.Appointment appointment)
        {
            int deviceNo = 1;
            string cardID = string.Empty;
            if (appointment.CardNO != string.Empty)
            { 
                cardFactory = new CardFactorys();
                utility = new Utility();

                // Getting device Information
                //var empInfo = db.Employees.Where(x => x.EmployeeID == appointment.EmployeeID).FirstOrDefault();
                if (appointment.Status == "I")
                {
                    var cardInfo = cardFactory.GetFreeCard(deviceNo);
                    if (cardInfo != null)
                    {
                        appointment.CheckedInTime = DateTime.Now;
                        cardID = cardInfo.CardID.ToString();

                        //result = utility.AssignToDevice(deviceNo, cardID, appointment.CardNO);
                        cardInfo.CardNO = appointment.CardNO;
                        result = cardFactory.SaveCard(cardInfo);

                        if (result.isSucess)
                        {
                            unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
                            result = unScheduleAppointmentFactory.SaveAppointment(appointment);
                            if (result.isSucess)
                            {
                                result.message = "Checked IN Successful.";
                            }
                        }
                    }
                    else
                    {
                        result.isSucess = false;
                        result.message = deviceNo + " NO. Free Card Found.";
                    }
                }
                

            }
            else
            {
                result.message = "Please Punch Visitor Card";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public JsonResult VisitorBreakOut(MV_Appointment appointment)
        {
            //Card card = new Card();
            string cardID = string.Empty;
            if (appointment.CardNO != string.Empty)
            {
                int deviceNo = 1;
                cardFactory = new CardFactorys();
                utility = new Utility();

                // Getting device Information
                //var empInfo = db.Employees.Where(x => x.EmployeeID == appointment.EmployeeID).FirstOrDefault();

                //DAL.db.Appointment appInfo = new DAL.db.Appointment();

                var appInfo = db.Appointments.Where(x => x.AppointmentID == appointment.AppointmentID).FirstOrDefault();
                db.Entry(appInfo).State = EntityState.Detached;
                

                if (appointment.Status == "B")
                {
                    //var cardInfo = cardFactory.GetFreeCard(deviceNo);
                    appInfo.Status = appointment.Status;
                    result = cardFactory.UnassignCard(deviceNo, appInfo.CardNO);
                    if (result.isSucess)
                    {
                        unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
                        appInfo.BreakOutTime = DateTime.Now;
                        result = unScheduleAppointmentFactory.SaveAppointment(appInfo);
                        if (result.isSucess)
                        {
                            result.message = "Visitor Checked Out For Break Successful.";
                        }
                    }
                    else
                    {
                        result.message = appInfo.CardNO + " No card not found for break!!!";
                    }                    
                }
            }
            else
            {
                result.message = "Please Punch Visitor Card";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult VisitorBreakIn(DAL.db.Appointment appointment)
        {
            //Card card = new Card();
            //string cardID = string.Empty;
            if (appointment.CardNO != string.Empty)
            {
                int deviceNo = 1;
                cardFactory = new CardFactorys();
                utility = new Utility();

                // Getting device Information
                //var empInfo = db.Employees.Where(x => x.EmployeeID == appointment.EmployeeID).FirstOrDefault();

                var appInfo = db.Appointments.Where(x => x.AppointmentID == appointment.AppointmentID).FirstOrDefault();
                db.Entry(appInfo).State = EntityState.Detached;

                if (appointment.Status == "I")
                {
                    var cardInfo = cardFactory.GetFreeCard(deviceNo);

                    if (cardInfo != null)
                    {                        
                        //cardID = cardInfo.CardID.ToString();
                        cardInfo.CardNO = appointment.CardNO;
                        result = cardFactory.SaveCard(cardInfo);

                        appInfo.CardNO = appointment.CardNO;
                        appInfo.BreakInTime = DateTime.Now;
                        appInfo.Status = appointment.Status;

                        if (result.isSucess)
                        {
                            unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
                            result = unScheduleAppointmentFactory.SaveAppointment(appInfo);
                            if (result.isSucess)
                            {
                                result.message = "Visitor Checked IN From Break Successful.";
                            }
                        }
                    }
                }
            }
            else
            {
                result.message = "Please Punch Visitor Card";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult VisitorNotify(DAL.db.Appointment appointment)
        {
            appointment.ReachTime = DateTime.Now;
            emailUtility = new EmailUtility();
            unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
            result = unScheduleAppointmentFactory.SaveAppointment(appointment);
            if (result.isSucess)
            {
                //if (emailUtility.IsInternetConnected())
                //{
                //    var email = db.Employees.Where(x => x.EmployeeID == appointment.EmployeeID).FirstOrDefault();
                //    if (email != null && email.Email != null)
                //    {
                //        bool isMail = emailUtility.SentMail(email.Email, appointment.CompanyName, appointment.VisitorName, "", "");
                //        if (isMail)
                //        {
                //            result.message = "Notify And Email Successful.";
                //        }
                //        return Json(result, JsonRequestBehavior.AllowGet);
                //    }
                //}
                result.message = "Notify Successful.";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult VisitorOut(string cardNO)
        {
            
            //string cardID;
            if (cardNO != null)
            {
                cardFactory = new CardFactorys();
                utility = new Utility();

                var appData = db.Appointments.Where(x => (x.CardNO == cardNO) && (x.Status == "I")).FirstOrDefault();

                if (appData != null)
                {
                    db.Entry(appData).State = EntityState.Detached;
                    appData.CheckedOutTime = DateTime.Now;
                    appData.Status = "O";

                    var cardInfo = db.Cards.Where(x => x.CardNO == cardNO).ToList();

                    if (cardInfo.Count > 0)
                    {
                        foreach (var card in cardInfo)
                        {
                            var deviceInfo = db.DeviceInfoes.Where(x => x.DeviceNO == card.DeviceNO).FirstOrDefault();

                            if (deviceInfo != null)
                            {

                                unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
                                result = unScheduleAppointmentFactory.SaveAppointment(appData);
                                if (result.isSucess)
                                {
                                    card.CardNO = null;
                                    result = cardFactory.SaveCard(card);
                                    result.message = "Checked OUT Successful.";
                                }
                            }
                            else
                            {
                                result.message =  card.DeviceNO + " Number Device Not Found!";
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }
                else
                {
                    result.isSucess = false;
                    result.message = "No Checked IN Visitor Found Against This Card";
                }
            }
            else
            {
                result.isSucess = false;
                result.message = "Please Punch Visitor Card";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEvents()
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int employeeID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            int userGroupID = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            //var events = dc.Appointments.Where(x => x.Status != "O" && x.Status != "C" && x.EmployeeID == employeeID).Select(x => new { x.VisitorName, x.Purpose, x.AppointmentDate, x.AppointmentTime, x.CompanyName }).ToList();

            //(x.EmployeeID == employeeID) &&
            using (IVMS_DBEntities dc = new IVMS_DBEntities())
            {
                //var groupCode = dc.UserGroups.Where(x => x.UserGroupId == userGroupID).Select(x => x.GroupCode).FirstOrDefault();
                var groupCode = dc.SEC_UserGroup.Where(x => x.ID == userGroupID).Select(x => x.GroupCode).FirstOrDefault();
                if (groupCode == "RECEPTION" || groupCode == "ADMIN")
                {
                    var events = dc.Appointments.Where(x => (x.Status == "A" || x.Status == "N" || x.Status == "AP" || x.Status == "P" || x.Status == "I")).Select(x => new { x.AppointmentBy, x.VisitorName, x.Purpose, x.AppointmentDate, x.AppointmentTime, x.CompanyName }).ToList();
                    return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    var events = dc.Appointments.Where(x => (x.EmployeeID == employeeID) && (x.Status == "A" || x.Status == "N" || x.Status == "AP" || x.Status == "P" || x.Status == "I")).Select(x => new { x.AppointmentBy, x.VisitorName, x.Purpose, x.AppointmentDate, x.AppointmentTime, x.CompanyName }).ToList();
                    return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }


            }
        }
        public JsonResult ScheduleGetEvents()
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int employeeID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            int userGroupID = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            //var events = dc.Appointments.Where(x => x.Status != "O" && x.Status != "C" && x.EmployeeID == employeeID).Select(x => new { x.VisitorName, x.Purpose, x.AppointmentDate, x.AppointmentTime, x.CompanyName }).ToList();

            //(x.EmployeeID == employeeID) &&
            using (IVMS_DBEntities dc = new IVMS_DBEntities())
            {
                //var groupCode = dc.UserGroups.Where(x => x.UserGroupId == userGroupID).Select(x => x.GroupCode).FirstOrDefault();
                var groupCode = dc.SEC_UserGroup.Where(x => x.ID == userGroupID).Select(x => x.GroupCode).FirstOrDefault();
                var events = dc.Appointments.Where(x => (x.EmployeeID == employeeID) && (x.Status == "A" || x.Status == "N" || x.Status == "AP" || x.Status == "P" || x.Status == "I")).Select(x => new { x.AppointmentBy, x.VisitorName, x.Purpose, x.AppointmentDate, x.AppointmentTime, x.CompanyName,x.VisitorMobile }).ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


            }
        }

        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Appointment";
            ViewBag.CallingForm1 = "UnSchedule Appointment";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/UnSchedule";
        }
        public JsonResult VisitorInWithDevice(DAL.db.Appointment appointment)
        {
            //Card card = new Card();
            string cardID = string.Empty;
            if (appointment.CardNO != string.Empty)
            {
                string ip = "";
                int deviceNo = 0;
                int portNO = 0;

                cardFactory = new CardFactorys();
                utility = new Utility();

                // Getting device Information
                var empInfo = db.Employees.Where(x => x.EmployeeID == appointment.EmployeeID).FirstOrDefault();
                var deviceInfo = db.DeviceInfoes.Where(x => x.FloorNO == empInfo.Floor).FirstOrDefault();

                //var cardInfo = db.Cards.Where(x => x.CardNO == appointment.CardNO).ToList();

                if (deviceInfo != null)
                {
                    ip = deviceInfo.IPAddress;
                    deviceNo = deviceInfo.DeviceNO;
                    portNO = deviceInfo.PortNO;

                    if (appointment.Status == "I")
                    {
                        result = utility.Connect(ip, deviceNo, portNO);

                        if (result.isSucess)
                        {
                            var cardInfo = cardFactory.GetFreeCard(deviceNo);

                            if (cardInfo != null)
                            {
                                appointment.CheckedInTime = DateTime.Now;
                                cardID = cardInfo.CardID.ToString();

                                result = utility.AssignToDevice(deviceNo, cardID, appointment.CardNO);

                                //var card = new Card { CardID = Convert.ToInt16(cardID), CardNO = appointment.CardNO, DeviceNO = deviceNo };
                                cardInfo.CardNO = appointment.CardNO;
                                result = cardFactory.SaveCard(cardInfo);

                                if (result.isSucess)
                                {
                                    unScheduleAppointmentFactory = new UnScheduleAppointmentFactorys();
                                    result = unScheduleAppointmentFactory.SaveAppointment(appointment);
                                    if (result.isSucess)
                                    {
                                        result.message = "Checked IN Successful.";
                                    }
                                }
                            }
                            else
                            {
                                result.isSucess = false;
                                result.message = deviceNo + " NO. Device Not Found.";
                            }
                        }
                        else
                        {
                            result.message = deviceNo + " Device Not Connected.";
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

            }
            else
            {
                result.message = "Please Punch Visitor Card";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //======================================== Search Form ===============================

        public ActionResult SearchAppointment()
        {
            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = securityLogInFactory.GetCrudPermission(userGroupId, "UnSchedule");
                if (tblUserActionMapping.Select)
                {
                    ViewBag.CallingForm = "Visitors List";
                    ViewBag.CallingForm1 = "Visitors List";
                    ViewBag.CallingViewPage = "#";
                    return View();
                }
            }
            Session["logInSession"] = null;
            return Redirect("/#!/");
        }


        [HttpPost]
        public JsonResult MultipleOptionWiseMeetingScheduleReport(MeetingReportRequestModel meetingReportRequestModel)
        {
            try
            {
                if(meetingReportRequestModel.VisitorName==null)
                {
                    meetingReportRequestModel.VisitorName = "";
                }
                if (meetingReportRequestModel.VisitorMobile == null)
                {
                    meetingReportRequestModel.VisitorMobile = "";
                }
                if (meetingReportRequestModel.VisitorCard == null)
                {
                    meetingReportRequestModel.VisitorCard = "";
                }
                if (meetingReportRequestModel.Status == null)
                {
                    meetingReportRequestModel.Status = "";
                }
                if (meetingReportRequestModel.FromDate == null)
                {
                    meetingReportRequestModel.FromDate = "";
                }
                if (meetingReportRequestModel.ToDate == null)
                {
                    meetingReportRequestModel.ToDate = "";
                }
                if (meetingReportRequestModel.CompanyName == null)
                {
                    meetingReportRequestModel.CompanyName = "";
                }

                SqlParameter employeeID = new SqlParameter("@EmployeeId", meetingReportRequestModel.EmployeeID);
                SqlParameter visitorName = new SqlParameter("@VisitorName", meetingReportRequestModel.VisitorName);
                SqlParameter mobileNo = new SqlParameter("@MobileNo", meetingReportRequestModel.VisitorMobile);
                SqlParameter visitorCard = new SqlParameter("@VisitorCard", meetingReportRequestModel.VisitorCard);
                SqlParameter companyName = new SqlParameter("@CompanyName", meetingReportRequestModel.CompanyName);
                SqlParameter status = new SqlParameter("@Status", meetingReportRequestModel.Status);
                SqlParameter fromDate = new SqlParameter("@FromDate", meetingReportRequestModel.FromDate);
                SqlParameter toDate = new SqlParameter("@ToDate", meetingReportRequestModel.ToDate);


                var meetingInfoList = db.Database.SqlQuery<MeetingInfoModel>("[dbo].[USP_MultipleOptionWiseMeetingScheduleReport] @employeeID,@visitorName,@mobileNo,@visitorCard,@companyName,@status,@fromDate,@toDate", employeeID, visitorName, mobileNo, visitorCard, companyName, status, fromDate, toDate).ToList();
                return Json(meetingInfoList, JsonRequestBehavior.AllowGet);
            }
            catch(Exception exception)
            {
                throw exception;
            }
            
        }
    }
}