using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Common;
using BLL.Factory.Appointment;
using BLL.Factory.Setup;
using BLL.Interfaces.Appointment;
using BLL.Interfaces.Setup;
using DAL.db;
using DAL.Helper;
using Device.Services;
using WebGrease.Css.Ast.Selectors;
using System.Data.SqlClient;
using IVMS.Models;

namespace IVMS.Controllers.Appointment
{
    public class ScheduleAppointmentController : Controller
    {
        Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
        private IScheduleAppointmentFactory scheduleAppointmentFactory;
        private IDepartmentFactory departmentFactory;
        DateTime toDayTime = DateTime.Now;
        //int employeeID = 0;
        private Result result;
        IVMS_DBEntities db = new IVMS_DBEntities();
        private ICardFactory cardFactory;
        Utility utility;
        string message = string.Empty;
        public ActionResult ScheduleAppointmentList()
        {
            ViewBag.CallingForm = "Appointment";
            ViewBag.CallingForm1 = "Schedule Appointment";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        [HttpPost]
        public JsonResult LoadScheduleAppointment()
        {
            int employeeID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));          
            //var appointmentList = from x in db.Appointments
            //    join e in db.Employees on x.EmployeeID equals e.EmployeeID
            //    join d in db.Departments on e.DepartmentID equals d.DepartmentID
            //    join de in db.Designations on e.DesignationID equals de.DesignationID
            //    where x.EmployeeID == employeeID && x.Status != "C" && x.Status != "O" & x.Employee.OnBehalfEmployeeID > 0
            //    select new
            //    {
            //        x.AppointmentBy, 
            //        x.AppointmentID, 
            //        x.AppointmentDate, 
            //        x.EmployeeID, 
            //        x.VisitorMobile,
            //        d.DepartmentID, 
            //        de.DesignationID, 
            //        e.EmpName,
            //        d.DepartmentName, 
            //        de.DesignationName, 
            //        x.CompanyName,
            //        x.Purpose,
            //        x.AppointmentTime,
            //        x.ReachTime,
            //        x.CardNO,
            //        x.Status,
            //        x.VisitorName,
            //        x.VisitorEmail,
            //        x.CreatedBy,
            //        x.CreatedDate,
            //        x.CheckedInTime, 
            //        x.CheckedOutTime,
            //        x.AccessFloors,
            //        x.NotifyMessage,
            //        x.ReplayMessage,
            //        FirstWordMsg = x.ReplayMessage
            //    };

            SqlParameter p1 = new SqlParameter("@P1", employeeID);
            var list = db.Database.SqlQuery<VM_ScheduleAppointment>("sp_ScheduleAppList @P1", p1).ToList();
            return Json(list.OrderByDescending(x => x.AppointmentDate), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult CheckMeetingTime(DateTime time, DateTime date,int empid)
        {
            if (empid != null && empid >0)
            {
                 
                scheduleAppointmentFactory = new ScheduleAppointmentFactorys();

                List<DAL.db.Appointment> list = scheduleAppointmentFactory.SearchScheduleAppointment(empid);

                var appointmentList = list.Where(x => x.AppointmentTime == time && x.AppointmentDate == date).Select(x => new { x.AppointmentBy, x.AppointmentID, x.AppointmentDate, x.EmployeeID, x.Employee.EmpName, x.Employee.Department.DepartmentName, x.Employee.Designation.DesignationName, x.CompanyName, x.Purpose, x.AppointmentTime, x.ReachTime, x.CardNO, x.Status, x.VisitorName, x.VisitorMobile, x.VisitorEmail, x.CreatedBy, x.CreatedDate, x.CheckedInTime, x.CheckedOutTime }).OrderBy(x => x.AppointmentTime);
                //appointmentList.OrderByDescending(x => x.AppointmentID);
                // var appointmentLists = appointmentList.OrderBy(x =>x.AppointmentTime);
                return Json(appointmentList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int employeeID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
                scheduleAppointmentFactory = new ScheduleAppointmentFactorys();

                List<DAL.db.Appointment> list = scheduleAppointmentFactory.SearchScheduleAppointment(employeeID);

                var appointmentList = list.Where(x => x.AppointmentTime == time && x.AppointmentDate == date).Select(x => new { x.AppointmentBy, x.AppointmentID, x.AppointmentDate, x.EmployeeID, x.Employee.EmpName, x.Employee.Department.DepartmentName, x.Employee.Designation.DesignationName, x.CompanyName, x.Purpose, x.AppointmentTime, x.ReachTime, x.CardNO, x.Status, x.VisitorName, x.VisitorMobile, x.VisitorEmail, x.CreatedBy, x.CreatedDate, x.CheckedInTime, x.CheckedOutTime }).OrderBy(x => x.AppointmentTime);
                //appointmentList.OrderByDescending(x => x.AppointmentID);
                // var appointmentLists = appointmentList.OrderBy(x =>x.AppointmentTime);
                return Json(appointmentList, JsonRequestBehavior.AllowGet);
            }
        }





        //[HttpPost]
        //public JsonResult LoadNotifyList()
        //{
        //    IVMS_DBEntities  db = new IVMS_DBEntities();
        //    int employeeID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
        //    var did = (from e in db.Employees where e.EmployeeID == employeeID select e.DesignationID).FirstOrDefault();
        //    var dcode = (from d in db.Designations where d.DesignationID == did select d.Code).FirstOrDefault();
        //    if (dcode  == "BEHALF")
        //    {
        //        var onbhalfId = (from e in db.Employees where e.EmployeeID == employeeID select e.OnBehalfEmployeeID).FirstOrDefault();
              
        //            scheduleAppointmentFactory = new ScheduleAppointmentFactorys();

        //            List<DAL.db.Appointment> list = scheduleAppointmentFactory.SearchNotifyDataPS(Convert.ToInt32(onbhalfId), employeeID);
        //            var appointmentList = list.Select(x => new { x.AppointmentBy, x.AppointmentID, x.AppointmentDate, x.EmployeeID, x.Employee.EmpName, x.CompanyName, x.Purpose, x.AppointmentTime, x.ReachTime, x.Status, x.VisitorName, x.NotifyMessage, x.ReplayMessage, x.CreatedBy, x.CreatedDate, x.Employee.OnBehalfEmployeeID });
        //            return Json(appointmentList, JsonRequestBehavior.AllowGet);
                
              


        //    }
        //    else
        //    {
        //        scheduleAppointmentFactory = new ScheduleAppointmentFactorys();
        //        List<DAL.db.Appointment> list = scheduleAppointmentFactory.SearchNotifyData(employeeID);
        //        var appointmentList = list.Select(x => new { x.AppointmentBy, x.AppointmentID, x.AppointmentDate, x.EmployeeID, x.Employee.EmpName, x.CompanyName, x.Purpose, x.AppointmentTime, x.ReachTime, x.Status, x.VisitorName, x.NotifyMessage, x.ReplayMessage, x.CreatedBy, x.CreatedDate, x.Employee.OnBehalfEmployeeID });
        //        return Json(appointmentList, JsonRequestBehavior.AllowGet);
        //    }

        //}

        [HttpPost]
        public JsonResult LoadNotifyList()
        {
            IVMS_DBEntities db = new IVMS_DBEntities();
            int employeeID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            var did = (from e in db.Employees where e.EmployeeID == employeeID select e.DesignationID).FirstOrDefault();
            var dcode = (from d in db.Designations where d.DesignationID == did select d.Code).FirstOrDefault();
            if (dcode == "BEHALF")
            {
                //var onbhalfId = (from e in db.Employees where e.EmployeeID == employeeID select e.OnBehalfEmployeeID).FirstOrDefault();
                var appointmentList = from x in db.Appointments
                                      join e in db.Employees on x.EmployeeID equals e.EmployeeID
                                      where e.OnBehalfEmployeeID == employeeID && (x.Status == "N" || x.Status == "P")
                                      select new
                                      {
                                          x.AppointmentBy,
                                          x.AppointmentID,
                                          x.AppointmentDate,
                                          x.EmployeeID,
                                          e.EmpName,
                                          x.CompanyName,
                                          x.Purpose,
                                          x.AppointmentTime,
                                          x.ReachTime,
                                          x.Status,
                                          x.VisitorName,
                                          x.NotifyMessage,
                                          x.ReplayMessage,
                                          x.CreatedBy,
                                          x.CreatedDate,
                                          x.VisitorMobile,
                                          x.VisitorEmail,
                                          e.OnBehalfEmployeeID
                                      };
                //scheduleAppointmentFactory = new ScheduleAppointmentFactorys();

                //List<DAL.db.Appointment> list = scheduleAppointmentFactory.SearchNotifyDataPS((int)onbhalfId);
                //var appointmentList = list.Select(x => new { });
                return Json(appointmentList, JsonRequestBehavior.AllowGet);




            }
            else
            {
                scheduleAppointmentFactory = new ScheduleAppointmentFactorys();
                List<DAL.db.Appointment> list = scheduleAppointmentFactory.SearchNotifyData(employeeID);
                var appointmentList = list.Select(x => new { x.AppointmentBy, x.AppointmentID, x.AppointmentDate, x.EmployeeID, x.Employee.EmpName, x.CompanyName, x.Purpose, x.AppointmentTime, x.ReachTime, x.Status, x.VisitorName, x.VisitorMobile, x.VisitorEmail, x.NotifyMessage, x.ReplayMessage, x.CreatedBy, x.CreatedDate, x.Employee.OnBehalfEmployeeID });
                return Json(appointmentList, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult LoadEmployee(int? empID)
        {
            scheduleAppointmentFactory = new ScheduleAppointmentFactorys();
            List<Employee> list = scheduleAppointmentFactory.SearchEmployee(empID);
            var empList = list.Select(x => new {x.EmployeeID, x.EmpName,x.DesignationID});
            return Json(empList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DepWiseEmployeeBehalf(int depID)
        {
            IVMS_DBEntities db = new IVMS_DBEntities();
            int employeeID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            var did = (from e in db.Employees where e.EmployeeID == employeeID select e.DesignationID).FirstOrDefault();
            var dcode = (from d in db.Designations where d.DesignationID == did select d.Code).FirstOrDefault();

            //var bid = (from e in db.Employees where e.EmployeeID == employeeID select e.OnBehalfEmployeeID).FirstOrDefault();
            scheduleAppointmentFactory = new ScheduleAppointmentFactorys();
            List<Employee> list = scheduleAppointmentFactory.SearchDepWiseEmployeeBehalf(depID, (int)employeeID);
            var empList = list.Select(x => new { x.EmployeeID, EmpName = x.EmpName + " => " + x.Designation.DesignationName + " (Floor-" + x.Floor + ")", x.Floor, x.DesignationID });
            return Json(empList, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetDepWiseEmployee(int depID)
        {

            scheduleAppointmentFactory = new ScheduleAppointmentFactorys();
            List<Employee> list = scheduleAppointmentFactory.SearchDepWiseEmployee(depID);
            var empList = list.Select(x => new { x.EmployeeID, EmpName = x.EmpName + " => " + x.Designation.DesignationName + " (Floor-" + x.Floor + ")", x.Floor, x.DesignationID, x.Designation.Code });
            return Json(empList, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetEmployeeWiseFloor(int empID)
        {
            scheduleAppointmentFactory = new ScheduleAppointmentFactorys();
            List<Employee> list = scheduleAppointmentFactory.SearchEmployeeWiseFloor(empID);
            var empList = list.Select(x => new { x.EmployeeID, EmpName = x.EmpName + " => " + x.Designation.DesignationName + " (Floor-" + x.Floor + ")", x.Floor,x.DesignationID });
            return Json(empList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetDepWiseFloor(int empID)
        {
            //scheduleAppointmentFactory = new ScheduleAppointmentFactorys();
            //List<Employee> list = scheduleAppointmentFactory.GetDepWiseFloor(empID);
            //var empList = list.Select(x => new {x.EmployeeID, x.Branch.NoOfFloor});
            var empList = from e in db.Employees
                join b in db.SET_CompanyBranch on e.BranchID equals b.BranchID
                join d in db.Departments on e.DepartmentID equals d.DepartmentID
                join de in db.Designations on e.DesignationID equals de.DesignationID
                where e.EmployeeID == empID
                select new
                {
                    e.EmployeeID,
                    b.NoOfFloor
                };
            return Json(empList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadDepartment(int? depID)
        {
            departmentFactory = new DepartmentFactorys();
            List<DAL.db.Department> list = departmentFactory.SearchDepartment(depID);
            var empList = list.Select(x => new {x.DepartmentID, x.DepartmentName});
            return Json(empList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadSelfDepartment(int? depID)
        {
            int empID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            var deptList = (from x in db.Employees
                join d in db.Departments on x.DepartmentID equals d.DepartmentID
                where x.EmployeeID == empID
                select new
                {
                    d.DepartmentID,
                    d.DepartmentName
                }).ToList();
            return Json(deptList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SelfDeptWiseEmp(int depID)
        {
            int empID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            var empList = (from x in db.Employees
                join d in db.Departments on x.DepartmentID equals d.DepartmentID
                where x.EmployeeID == empID && x.DepartmentID == depID
                select new
                {
                    x.EmployeeID,
                    x.EmpName,
                    x.Floor
                }).ToList();
            return Json(empList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ScheduleAppointmentCreate()
        {
            DefaultLoad();
            return View();
        }

        [HttpPost]
        public JsonResult SaveScheduleAppointment(DAL.db.Appointment appointment)
        {
            scheduleAppointmentFactory = new ScheduleAppointmentFactorys();
            int employeeID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            appointment.CreatedBy = employeeID;
            appointment.CreatedDate = toDayTime;
            result = scheduleAppointmentFactory.SaveAppointment(appointment);
            return Json(result);
        }
        [HttpPost]
        public JsonResult VisitorForward(DAL.db.Appointment appointment,int apID)
        {
            int results = 0;
            result = new Result();

            int EmployeeID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            //var appData = db.Appointments.Where(x => (x.EmployeeID == EmployeeID) && (x.Status == "I") && (x.AppointmentID == apID)).FirstOrDefault();
            var appData = db.Appointments.Where(x => x.AppointmentID == apID).FirstOrDefault();
            if (appointment.EmployeeID == appData.EmployeeID)
            {
                result.isSucess = false;
                result.message = "Self forward is not allowed, Please select another employee!!!";
                return Json(result, JsonRequestBehavior.AllowGet);

            }

            appData.CheckedOutTime = DateTime.Now;
            appData.Status = "O";

            appointment.VisitorMobile = appData.VisitorMobile;
            
            db.Entry(appData).State = System.Data.Entity.EntityState.Modified;
            results = db.SaveChanges();

            if (results == 1)
            {
              
                appointment.CheckedInTime = DateTime.Now;
                scheduleAppointmentFactory = new ScheduleAppointmentFactorys();
                result = scheduleAppointmentFactory.SaveAppointment(appointment);
                if (result.isSucess)
                {
                    result.message = "Visitor Forward Successful.";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
            ////Card card = new Card();
            //string cardID = string.Empty;
            //if (appointment.CardNO != string.Empty)
            //{
            //    string ip = "";
            //    int deviceNo = 0;
            //    int portNO = 0;

            //    cardFactory = new CardFactorys();
            //    utility = new Utility();

            //    // Getting device Information
            //    var empInfo = db.Employees.Where(x => x.EmployeeID == appointment.EmployeeID).FirstOrDefault();
            //    var deviceInfo = db.DeviceInfoes.Where(x => x.FloorNO == empInfo.Floor).FirstOrDefault();

            //    //var cardInfo = db.Cards.Where(x => x.CardNO == appointment.CardNO).ToList();

            //    if (deviceInfo != null)
            //    {
            //        ip = deviceInfo.IPAddress;
            //        deviceNo = deviceInfo.DeviceNO;
            //        portNO = deviceInfo.PortNO;

            //        if (appointment.Status == "AP")
            //        {
            //            result = utility.Connect(ip, deviceNo, portNO);

            //            if (result.isSucess)
            //            {
            //                var cardInfo = cardFactory.GetFreeCard(deviceNo);

            //                if (cardInfo != null)
            //                {
            //                    appointment.CheckedInTime = DateTime.Now;
            //                    cardID = cardInfo.CardID.ToString();

            //                    result = utility.AssignToDevice(deviceNo, cardID, appointment.CardNO);

            //                    //var card = new Card { CardID = Convert.ToInt16(cardID), CardNO = appointment.CardNO, DeviceNO = deviceNo };
            //                    cardInfo.CardNO = appointment.CardNO;
            //                    result = cardFactory.SaveCard(cardInfo);

            //                    if (result.isSucess)
            //                    {
            //                        scheduleAppointmentFactory = new ScheduleAppointmentFactorys();
            //                        result = scheduleAppointmentFactory.SaveAppointment(appointment);
            //                        if (result.isSucess)
            //                        {
            //                            result.message = "Visitor Forward Successful.";
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    result.isSucess = false;
            //                    result.message = deviceNo + " NO. Device Not Found.";
            //                }
            //            }
            //            else
            //            {
            //                result.message = deviceNo + " Device Not Connected.";
            //                return Json(result, JsonRequestBehavior.AllowGet);
            //            }
            //        }
            //    }

            //}
            //else
            //{
            //    result.message = "Please Punch Visitor Card";
            //}
          
        }
        [HttpPost]
        public JsonResult ReplayNotify(DAL.db.Appointment appointment)
        {
            int empID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            scheduleAppointmentFactory = new ScheduleAppointmentFactorys();
            appointment.CreatedBy = empID;
            appointment.CreatedDate = toDayTime;

            result = scheduleAppointmentFactory.SaveAppointment(appointment);
            return Json(result);
        }

        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Appointment";
            ViewBag.CallingForm1 = "Schedule Appointment";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/Schedule";
        }

    }
}