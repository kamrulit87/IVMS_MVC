using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
    public class UserGroupController : Controller
    {
        // GET: UserGroup

        private IGenericFactory<SEC_UserGroup> _userGroupFactory;
        private IGenericFactory<SEC_UserInformation> _userFactory;
        private IGenericFactory<SEC_UserActionMapping> _userActionMappingFactory;
        private IGenericFactory<SEC_UIPage> _uiPageFactory;
        private ISecurityFactory _securityFactory;
        private Result result = new Result();
        public ActionResult UserGroup()
        {

            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = securityLogInFactory.GetCrudPermission(userGroupId, "PagePermission");
                if (tblUserActionMapping.Select == true)
                {
                    ViewBag.CallingForm = "Security";
                    ViewBag.CallingForm1 = "User Group";
                    ViewBag.CallingViewPage = "#";
                    return View();
                }
            }
            Session["logInSession"] = null;
            return Redirect("/#!/");
        }


        public ActionResult LoadAllUserGroup()
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
                if (userGroupId != 0)
                {
                    _userGroupFactory = new UserGroupFactory();
                    var userGroup = _userGroupFactory.GetAll().Select(x => new
                    {
                        x.ID,
                        UserGroup = x.Name,
                        IsAdmin = x.IsAdmin,
                        x.CreatedBy,
                        x.CreatedDate
                    }).ToList();
                    return Json(userGroup.OrderBy(x => x.UserGroup).ToList());
                }
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetPage()
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int companyId = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
                _securityFactory = new SecurityFactorys();
                //var menu = _securityFactory.GetPageList(Convert.ToInt32(companyId)); //Page are Common For all the Application  
                var menu = _securityFactory.GetPageList();
                return Json(new { data = menu }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error occured" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UserGroupSave(SEC_UserGroup userGroup)
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userId = Convert.ToInt32(dictionary[3].Id);
                int empId = Convert.ToInt32(dictionary[1].Id);
                if (userId != 0)
                {
                    _securityFactory = new SecurityFactorys();
                    if (userGroup.ID < 1)
                    {
                        userGroup.CreatedBy = empId;
                        userGroup.CreatedDate = DateTime.Now;
                    }
                    result = _securityFactory.SaveUserGroupWithPageMapping(userGroup);
                    if (result.isSucess)
                    {
                        return Json(result);
                    }
                    return Json(result);
                }
                Session["logInSession"] = null;
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteUserGroup(int id)
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
                if (userGroupId != 0)
                {

                    ISecurityFactory _securityLogInFactory = new SecurityFactorys();
                    PagePermissionVM tblUserActionMapping = _securityLogInFactory.GetCrudPermission(userGroupId, "PagePermission");
                    if (tblUserActionMapping.Delete)
                    {
                        _securityFactory = new SecurityFactorys();
                        result = _securityFactory.DeleteUserGroup(id);
                        if (result.isSucess)
                        {
                            return Json(new { success = true, message = "Successfuly deleted the User Group" },
                                JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { success = false, message = "You cant delete this another one use this User Group" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = "You are not permitted for this action" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditUserGroup()
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory _securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = _securityLogInFactory.GetCrudPermission(userGroupId, "PagePermission");
                if (tblUserActionMapping.Edit)
                {
                    DefaultLoad();
                    return View();
                }
            }
            Session["logInSession"] = null;
            return Redirect("/#/");
        }

        public JsonResult LoadUserGroupForEdit(int id)
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int companyId = Convert.ToInt32(dictionary[1].Id);
                if (companyId != 0)
                {
                    _userGroupFactory = new UserGroupFactory();
                    var userGroup = _userGroupFactory.GetAll()
                        .Select(a => new
                        {
                            a.ID,
                            a.Name
                        }).FirstOrDefault();

                    return Json(new { success = true, data = userGroup }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "Error Ocured" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditUserGroupSave(SEC_UserGroup userGroup, List<MenuItemVM> userMappingVm = null)
        {
            try
            {
                _securityFactory = new SecurityFactorys();
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userId = Convert.ToInt32(dictionary[3].Id);
                if (userId != 0)
                {
                    result = _securityFactory.EditUserGroupPagePermission(userGroup, userMappingVm);
                    if (result.isSucess)
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                Session["logInSession"] = null;
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LoadMappingDataForEdit(int id)
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userId = Convert.ToInt32(dictionary[3].Id);
                if (userId != 0)
                {
                    _securityFactory = new SecurityFactorys();
                    var userPagemapping = _securityFactory.GetEditPageList(id);
                    return Json(userPagemapping);
                }
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UserGroupCreate()
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory _securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = _securityLogInFactory.GetCrudPermission(userGroupId, "PagePermission");
                if (tblUserActionMapping.Edit)
                {
                    DefaultLoad();
                    return View();
                }
            }
            Session["logInSession"] = null;
            return Redirect("/#/");
            
        }

        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Security";
            ViewBag.CallingForm1 = "User Group";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/UserGroup";
        }
    }
}