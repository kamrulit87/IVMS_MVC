using BLL.Common;
using DAL.Common;
using DAL.db;
using Newtonsoft.Json;
using IVMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using DAL.Helper;

//using Presentation.Models;
//using BAL.Master;
//using BAL.Common;

namespace Application.Controllers
{
    [Utility.SessionExpire]
    [Authorize]
    public class mstr_basicController : Controller
    {
        IVMS_DBEntities db = new IVMS_DBEntities();
       // DBOrganization objDBOrganization = new DBOrganization();
        //SQL objDBCosTCentreTab = new SQL();
        //DBSetting objDBSetting = new DBSetting();
        Function objFunction = new Function();
        public bool isCompleted = false;
        public string sMessage = "";

        //***************************** Start:: Index ****************************
        #region Index
        //--------------------------- Start:: Function ---------------------------
        public void ChartOfAccounts()
        {
            //using (ReportClass rptH = new ReportClass())
            //{
            //    DataTable rptDt = new DataTable();
            //    string strSQL = "";
            //    strSQL = "Select NodeId,NodeNumber ,space(3*Lvl)+NodeHead AS NodeHeadDisplay,Lvl,NodeMode From NodeMatrix where NodeMode = 0 order by Hierarchy,NodeId";

            //    try
            //    {
            //        rptDt = DataManager.ExecuteQuery(strSQL);
            //        rptH.FileName = Server.MapPath("~/Views/report/CrystalReport/RptChartOfAcc.rpt");
            //        rptH.Database.Tables[0].SetDataSource(rptDt);
            //        Response.Buffer = false;
            //        Response.ClearContent();
            //        Response.ClearHeaders();
            //        rptH.Load();
            //        Stream stream = rptH.ExportToStream(ExportFormatType.CrystalReport);
            //        rptH.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
            //    }
            //    catch (Exception ex)
            //    {
            //        ViewBag.Validation = "Report open Error";
            //    }
            //    finally
            //    {

            //    }
            //}
        }

        //---------------------------- End:: Function ---------------------------- 
        //------------------------- Start:: ActionResult -------------------------
        [Authorize]
        public ActionResult index()
        {
            // var infoQuery = (from cust in db.CosTCentreTabs select new { cust.CosTCentreId }).Union(from emp in db.ProjectTabs select new { emp.ProjectTabId }).ToList();

            //Select Sum(GrandTotal) As GrandTotal
            //From SalesMaster 
            //Where Year(BillDate) = Year('01-Jan-2017') Group By Month(BillDate)  Order By Month(BillDate)


            ViewBag.CallingForm1 = "index";
            ViewBag.DataPoints = JsonConvert.SerializeObject(GetRandomDataForCategoryAxis(), _jsonSetting);
            //var list = db.Projects.ToList();
            //list.Select(x => new { x.ProjectName, x.NumberOfFloors, x.StartDate, x.AprxEndDate });
            return View();
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
                var groupCode = dc.UserGroups.Where(x => x.UserGroupId == userGroupID).Select(x => x.GroupCode).FirstOrDefault();
                if (groupCode == "RECEPTION")
                {
                    var events = dc.Appointments.Where(x => (x.Status == "A" || x.Status == "N" || x.Status == "AP" || x.Status == "P")).Select(x => new { x.AppointmentBy, x.VisitorName, x.Purpose, x.AppointmentDate, x.AppointmentTime, x.CompanyName }).ToList();
                    return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    var events = dc.Appointments.Where(x => (x.EmployeeID == employeeID) && (x.Status == "A" || x.Status == "N" || x.Status == "AP" || x.Status == "P")).Select(x => new { x.AppointmentBy, x.VisitorName, x.Purpose, x.AppointmentDate, x.AppointmentTime, x.CompanyName }).ToList();
                    return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }


            }
        }

        [Authorize]
        public ActionResult keepalive()
        {
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public List<HC_Grap> GetRandomDataForCategoryAxis()
        {
            List<HC_Grap> _dataPoints = new List<HC_Grap>();
            _dataPoints = (from a in db.Employees
                           //where a.BillDate.Year == System.DateTime.Now.Year 
                           select new HC_Grap
                           {
                               label = a.EmpName.ToString()
                           }).ToList();

            return _dataPoints;
        }

        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };


        //-------------------------- End:: ActionResult -------------------------- 

        #endregion
        //***************************** End:: Index ****************************** 

        //************************* Start:: Organization *************************
        #region Organization
        //--------------------------- Start:: Function ---------------------------
        //[HttpPost]
        //public JsonResult FxOrganizationSave()
        //{
        //    var fileInfo = Request.Form["fileInfo"];
        //    var objOrganization = JsonConvert.DeserializeObject<OrganizationCore>(fileInfo);

        //    //Start:: Convert Image To Byte To Save In DB
        //    var PicUploadFile = Request.Files["PicUploadFile"];
        //    if (PicUploadFile != null)
        //    {
        //        System.Drawing.Image imag = System.Drawing.Image.FromStream(PicUploadFile.InputStream);
        //        byte[] byteImage;
        //        byteImage = ConvertImageToByteArray(imag, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        objOrganization.OrganizationLogo = byteImage;
        //    }
        //    //End:: Convert Image To Byte To Save In DB

        //    int i = -1;
        //    int OrganizationId = -1;

        //    if (!ModelState.IsValid)
        //    {
        //        return Json(new { isCompleted = isCompleted, Errors = Function.FillModelStateError(ModelState) }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        if (objOrganization.OrganizationId > 0)
        //        {
        //            OrganizationCore organizationCore = db.OrganizationCores.Find(objOrganization.OrganizationId);

        //            if (PicUploadFile != null)
        //            {
        //                organizationCore.OrganizationLogo = objOrganization.OrganizationLogo;
        //            }
        //            organizationCore.OrganizationName = objOrganization.OrganizationName;
        //            organizationCore.RegisteredPerson = objOrganization.RegisteredPerson;
        //            organizationCore.OrganizationMode = objOrganization.OrganizationMode;
        //            organizationCore.OrganizationAddress = objOrganization.OrganizationAddress;
        //            organizationCore.PosTCode = objOrganization.PosTCode;
        //            organizationCore.City = objOrganization.City;
        //            organizationCore.Country = objOrganization.Country;
        //            organizationCore.Phone = objOrganization.Phone;
        //            organizationCore.Fax = objOrganization.Fax;
        //            organizationCore.Email = objOrganization.Email;
        //            organizationCore.Web = objOrganization.Web;

        //            i = objDBOrganization.Save(organizationCore);
        //            OrganizationId = (int)db.OrganizationCores.Max(org => org.OrganizationId);
        //        }
        //        else
        //        {
        //            i = objDBOrganization.Submit(objOrganization);
        //            OrganizationId = (int)db.OrganizationCores.Max(org => org.OrganizationId);
        //        }
        //        if (i > 0)
        //        {
        //            isCompleted = true;
        //        }
        //        else
        //        {
        //            isCompleted = false;
        //        }
        //    }

        //    return Json(new { isCompleted = isCompleted, OrganizationId = OrganizationId }, JsonRequestBehavior.AllowGet);
        //}

        //private byte[] ConvertImageToByteArray(System.Drawing.Image imageToConvert, System.Drawing.Imaging.ImageFormat formatOfImage)
        //{
        //    byte[] byteArray;
        //    try
        //    {
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            imageToConvert.Save(ms, formatOfImage);
        //            byteArray = ms.ToArray();
        //        }
        //    }
        //    catch (Exception) { throw; }
        //    return byteArray;
        //}
        ////---------------------------- End:: Function ---------------------------- 

        ////------------------------- Start:: ActionResult -------------------------  
        //public ActionResult organization()
        //{
        //    // Form Permission Set
        //    if (!objFunction.CanView("SETTINGS_0001"))
        //    {
        //        return RedirectToAction("login", "security", new { area = "" });
        //    }

        //    ViewBag.CallingForm = this.ControllerContext.RouteData.Values["controller"].ToString(); ;
        //    ViewBag.CallingForm1 = "Organization";
        //    ViewBag.CallingViewPage = "#";
        //    ViewBag.TxtOrgSave = "Submit";
        //    ViewBag.BtnOrgSave = "Submit";

        //    var organizationcore = db.OrganizationCores.FirstOrDefault();
        //    if (organizationcore == null)
        //    {
        //        ViewBag.cboMode = "";
        //        return View();
        //    }
        //    else
        //    {
        //        ViewBag.TxtOrgSave = "Save";
        //        ViewBag.BtnOrgSave = "Save";
        //        ViewBag.cboMode = organizationcore.OrganizationMode.ToString();

        //        return View(organizationcore);
        //    }
        //}



        //-------------------------- End:: ActionResult -------------------------- 
        #endregion
        //************************** End:: Organization **************************

        //************************* Start:: Project *************************
        #region Project

        //------------------------- Start:: ActionResult -------------------------  
        public ActionResult project()
        {
            // Form Permission Set
            if (!objFunction.CanView("SETTINGS_0001"))
            {
                return RedirectToAction("login", "security", new { area = "" });
            }

            ViewBag.CallingForm = this.ControllerContext.RouteData.Values["controller"].ToString(); ;
            ViewBag.CallingForm1 = "Project";
            ViewBag.CallingViewPage = "#";
            ViewBag.TxtOrgSave = "Submit";
            ViewBag.BtnOrgSave = "Submit";
            return View();
        }



        //-------------------------- End:: ActionResult -------------------------- 
        #endregion
        //************************** End:: Project **************************


        //**************************** Start:: Branch ****************************
        #region Branch

        //--------------------------- Start:: Function ---------------------------
        public void FrmBranchDefaultLoad()
        {
            ViewBag.CallingForm = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.CallingForm1 = "Branch";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "branch";
            ViewBag.btnVsbSubmit = "visibility: hidden";
            ViewBag.btnVsbSave = "visibility: hidden";
        }
       
        //--------------------------- Start:: Function ---------------------------
        public void FrmCostCenterDefaultLoad()
        {
            ViewBag.CallingForm = this.ControllerContext.RouteData.Values["controller"].ToString(); ;
            ViewBag.CallingForm1 = "Cost Center";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "/mstr_basic/cost_center";

            ViewBag.btnVsbSubmit = "visibility: hidden";
            ViewBag.btnVsbSave = "visibility: hidden";
            ViewBag.Status = "none";
        }
        #endregion
        
    }

}