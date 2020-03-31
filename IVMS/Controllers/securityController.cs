using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BLL.Factory;
using BLL.Factory.Security;
using BLL.Interfaces;
using BLL.Interfaces.Security;
using BLL.Models;
using DAL.db;
using DAL.Helper;
using System.IO;
using BLL.Factory.Setup;

namespace IVMS.Controllers
{
    public class SecurityController : Controller
    {
        private IGenericFactory<SEC_UIPage> _uiPageFactory;
        private IGenericFactory<SEC_UIModule> _uiModuleFactory;
        private IGenericFactory<SEC_UserActionMapping> _uiMappingFactory;

        private ISecurityFactory _securityFactory; 
        private IGenericFactory<SEC_UserInformation> _userInformationFactory;
        private IGenericFactory<SEC_LoginStatus> _loginStatusFactory;
        private IGenericFactory<Employee> _employeeFactory;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SecurityMaster()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LogOnModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool getLan = false;
                    string visitorIpAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (String.IsNullOrEmpty(visitorIpAddress))
                        visitorIpAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    if (string.IsNullOrEmpty(visitorIpAddress))
                        visitorIpAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
                    if (string.IsNullOrEmpty(visitorIpAddress) || visitorIpAddress.Trim() == "::1")
                    {
                        getLan = true;
                        visitorIpAddress = string.Empty;
                    }
                    if (getLan && string.IsNullOrEmpty(visitorIpAddress))
                    {
                        //This is for Local(LAN) Connected ID Address
                        string stringHostName = Dns.GetHostName();
                        //Get Ip Host Entry
                        IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
                        ipHostEntries = System.Net.Dns.GetHostEntry(Request.ServerVariables["REMOTE_HOST"]);

                        //Get Ip Address From The Ip Host Entry Address List
                        IPAddress[] arrIpAddress = ipHostEntries.AddressList;

                        try
                        {
                            visitorIpAddress = arrIpAddress[arrIpAddress.Length - 2].ToString();
                        }
                        catch
                        {
                            try
                            {
                                visitorIpAddress = arrIpAddress[0].ToString();
                            }
                            catch
                            {
                                try
                                {
                                    arrIpAddress = Dns.GetHostAddresses(stringHostName);
                                    visitorIpAddress = arrIpAddress[0].ToString();
                                }
                                catch
                                {
                                    visitorIpAddress = "127.0.0.1";
                                }
                            }
                        }

                    }

                    ////////////////////////////////////
                    _securityFactory = new SecurityFactorys();
                    _userInformationFactory = new UserFactory();
                    _employeeFactory = new EmployeeFactory();

                    model.UserName = model.UserName.ToLower().Trim();

                    var logInStatus = _securityFactory.CheckLogIn(new LogOnModel { CompanyID = model.CompanyID, BranchID = model.BranchID, UserName = model.UserName, Password = model.Password });

                    if (logInStatus.IsAllowed)
                    {
                        var aSecurityUser = _userInformationFactory.FindBy(x => x.UserName.Contains(model.UserName)).FirstOrDefault();
                        var aCompanyUser = _employeeFactory.FindBy(x => x.EmployeeID == aSecurityUser.EmployeeID).FirstOrDefault();

                        if (aSecurityUser != null)
                        {

                            System.Web.HttpContext.Current.Session["LoginEmployee"] = aSecurityUser.EmployeeID;
                            System.Web.HttpContext.Current.Session["LoginCompanyID"] = aCompanyUser.CompanyID;
                            System.Web.HttpContext.Current.Session["LoginBranchID"] = aCompanyUser.BranchID;
                            System.Web.HttpContext.Current.Session["LoginUserID"] = aSecurityUser.ID;
                            System.Web.HttpContext.Current.Session["LoginUserName"] = aSecurityUser.UserName;
                            System.Web.HttpContext.Current.Session["LoginUserFullName"] = aSecurityUser.Employee.EmpName;
                            System.Web.HttpContext.Current.Session["UserGroupID"] = aSecurityUser.UserGroupID;
                            System.Web.HttpContext.Current.Session["IPAddress"] = visitorIpAddress;
                            System.Web.HttpContext.Current.Session["LoginPhoto"] = GetLoginPhoto(aSecurityUser.EmployeeID);
                            string[] computerName = null;
                            //try
                            //{
                            //    computerName = Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName.Split(new Char[] { '.' });
                            //}
                            //catch (Exception)
                            //{

                            //}
                            if (computerName != null)
                            {
                                System.Web.HttpContext.Current.Session["PCName"] = computerName[0];
                            }
                            else
                            {
                                System.Web.HttpContext.Current.Session["PCName"] = "N/A";
                            }


                            if (!String.IsNullOrEmpty(model.UserName))
                            {
                                if (!aSecurityUser.UserName.Equals(model.UserName, StringComparison.Ordinal))
                                {
                                    return Json(new { success = false, message = "Incorrect User Name or Password." }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                System.Web.HttpContext.Current.Session["LoginUserID"] = 0;
                            }

                            if (!logInStatus.IsAllowed)
                            {
                                return Json(new { success = false, message = logInStatus.Message }, JsonRequestBehavior.AllowGet);
                            }
                            //if (String.IsNullOrEmpty(model.UserName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
                            //FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                            SEC_LoginStatus tblLogInStatus = new SEC_LoginStatus();
                            _loginStatusFactory = new LoginStatusFactory();
                            tblLogInStatus.UserID = aSecurityUser.ID;
                            tblLogInStatus.PresentLogInStatus = true;
                            tblLogInStatus.LogInTime = DateTime.Now;
                            tblLogInStatus.LogOutTime = DateTime.Now;
                            tblLogInStatus.ForcedLogOutStatus = false;
                            _loginStatusFactory.Add(tblLogInStatus);
                            _loginStatusFactory.Save();
                            Session["logInSession"] = "true";
                            return Json( new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                    //
                        }
                        return Json(new { success = false, message = "The user name or password provided is incorrect." }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = logInStatus.Message }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "The user name or password provided is incorrect." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                //Route();
                return Json(new { success = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Json(new { success = false, message = "The user name or password provided is incorrect. 4" }, JsonRequestBehavior.AllowGet);
        }

        public string GetLoginPhoto(int personnelID)
        {
            string photoPath = "~/Content/user_files/hr/attach/" + personnelID + "/";
            string path = Server.MapPath("~/Content/user_files/hr/attach/" + personnelID + "/");
            DirectoryInfo directory = new DirectoryInfo(path);
            if (directory.Exists)
            {
                var file = directory.EnumerateFiles().Where(x => x.Name.Contains("Photo")).Select(x => new { x.Name, x.FullName }).FirstOrDefault();
                if (file != null)
                {
                    photoPath = photoPath + file.Name;
                }                
            }
            return photoPath;
        }
        public ActionResult LogOff()
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                if (dictionary[3].Id != null || dictionary[3].Id != "")
                {
                    int userId = Convert.ToInt32(dictionary[3].Id);
                    _loginStatusFactory = new LoginStatusFactory();

                    SEC_LoginStatus loginStatus = _loginStatusFactory.FindBy(x => x.UserID == userId).FirstOrDefault();
                    loginStatus.PresentLogInStatus = false;
                    loginStatus.LogOutTime = DateTime.Now;
                    loginStatus.ForcedLogOutStatus = false;
                    _loginStatusFactory.Edit(loginStatus);
                    _loginStatusFactory.Save();

                    System.Web.HttpContext.Current.Session["LoginUserID"] = 0;
                    System.Web.HttpContext.Current.Session["LoginUserName"] = 0;
                    System.Web.HttpContext.Current.Session["LoginEmployee"] = 0;
                    System.Web.HttpContext.Current.Session["LoginCompanyID"] = 0;
                    System.Web.HttpContext.Current.Session["LoginBranchID"] = 0;
                    System.Web.HttpContext.Current.Session["LoginUserFullName"] = 0;
                    System.Web.HttpContext.Current.Session["UserGroupID"] = 0;
                    System.Web.HttpContext.Current.Session["IPAddress"] = 0;
                    Session["logInSession"] = null;

                    return Redirect("/#!/");
                }
                return Redirect("/#!/");
            }
            catch (Exception)
            {
                return Redirect("/#!/");
            }
        }

        [HttpGet]
        public JsonResult GetSiteMenu()
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int _userGroupID = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            ISecurityFactory _securityLogInFactory = new SecurityFactorys();
            var _menu = _securityLogInFactory.PagePermissedList(_userGroupID);
            return Json(new { menu = _menu, userGroupID = _userGroupID }, JsonRequestBehavior.AllowGet);
        }
    }
}