using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Anko.Models;
using BLL.Common;
using BLL.Factory;
using BLL.Factory.Security;
using BLL.Interfaces;
using BLL.Interfaces.Security;
using BLL.Models;
using DAL.db;
using DAL.Helper;

namespace IVMS.Controllers
{
    public class ChangePasswordController : Controller
    {
        // GET: ChangePassword
        private IChangePasswordFactory _changePasswordFactory;
        Result result = new Result();
        public ActionResult ChangePassword()
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory _securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = _securityLogInFactory.GetCrudPermission(userGroupId, "ChangeOwnPassword");
                if (tblUserActionMapping.Select)
                {
                    ViewBag.CallingForm = "Security";
                    ViewBag.CallingForm1 = "Change Password";
                    ViewBag.CallingViewPage = "#";
                    return View();
                }
            }
            Session["logInSession"] = null;
            return Redirect("/#/");
        }

        public JsonResult SelfPasswordChange(ChangePasswordModel changePassword)
        {
            _changePasswordFactory = new ChangePasswordFactorys();
            result = _changePasswordFactory.SelfPasswordChange(changePassword);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PasswordChangeByAdmin()
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory _securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = _securityLogInFactory.GetCrudPermission(userGroupId, "ChangePasswordByAdmin");
                if (tblUserActionMapping.Select)
                {
                    ViewBag.CallingForm = "Security";
                    ViewBag.CallingForm1 = "Reset Password";
                    ViewBag.CallingViewPage = "#";
                    return View();
                }
            }
            return Redirect("/Login");
        }

        public JsonResult LoadAllFullName()
        {
            _changePasswordFactory = new ChangePasswordFactorys();
            List<SEC_UserInformation> list = _changePasswordFactory.LoadAllFullName();
            var userFullName = list.Select(a => new { a.UserFullName });
            return Json(userFullName, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadAllUserName()
        {
            _changePasswordFactory = new ChangePasswordFactorys();
            List<SEC_UserInformation> list = _changePasswordFactory.LoadAllUserName();
            var userName = list.Select(a => new { a.UserName });
            return Json(userName, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PasswordChangeByAdminSave(ChangePasswordModel changePassword)
        {
            _changePasswordFactory = new ChangePasswordFactorys();
            result = _changePasswordFactory.PasswordChangeByAdminSave(changePassword);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}