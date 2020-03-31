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
    public class PageController : Controller
    {
        // GET: Page
        private ISecurityFactory securityFactory;
        Result result = new Result();
        public ActionResult Page()
        {
            
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = securityLogInFactory.GetCrudPermission(userGroupId, "Page");
                if (tblUserActionMapping.Select)
                {
                    ViewBag.CallingForm = "Security";
                    ViewBag.CallingForm1 = "Page";
                    ViewBag.CallingViewPage = "#";
                    return View();
                }
            }
            Session["logInSession"] = null;
            return Redirect("/#!/");
        }
        [HttpPost]
        public JsonResult LoadPage(int? pageID)
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
                if (userGroupId > 0)
                {
                    securityFactory = new SecurityFactorys();
                    List<SEC_UIPage> list = securityFactory.SearchUiPages(pageID);
                    var pageList = list.Select(x => new { x.ID, x.ModuleID, ModuleName = x.SEC_UIModule.Name, x.Name, x.UrlID, x.UIPageName }).OrderBy(x=> x.ModuleName);
                    return Json(pageList, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetModuleData(int? moduleID)
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
                if (userGroupId > 0)
                {
                    securityFactory = new SecurityFactorys();
                    List<SEC_UIModule> list = securityFactory.SearchUiModule(moduleID);
                    var pageList = list.Select(x => new { x.ID, x.Name });
                    return Json(pageList, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DeletePage(int id)
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
                if (userGroupId != 0)
                {

                    ISecurityFactory _securityLogInFactory = new SecurityFactorys();
                    PagePermissionVM tblUserActionMapping = _securityLogInFactory.GetCrudPermission(userGroupId, "Page");
                    if (tblUserActionMapping.Delete)
                    {
                        securityFactory = new SecurityFactorys();
                        result = securityFactory.DeleteUiPage(id);
                        if (result.isSucess)
                        {
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { isSucess = false, message = "You cant delete this another one use this User Group" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { isSucess = false, message = "You are not permitted for this action" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSucess = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSucess = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PageCreate()
        {
            
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = securityLogInFactory.GetCrudPermission(userGroupId, "Page");
                if (tblUserActionMapping.Create)
                {
                    DefaultLoad();
                    return View();
                }
            }
            Session["logInSession"] = null;
            return Redirect("/#!/");
        }
        [HttpPost]
        public JsonResult PageSave(SEC_UIPage page)
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userId = Convert.ToInt32(dictionary[3].Id);
                if (userId != 0)
                {
                    securityFactory = new SecurityFactorys();
                    result = securityFactory.UiPageSave(page);
                    if (result.isSucess)
                    {
                        return Json(result);
                    }
                    return Json(result);
                }
                Session["logInSession"] = null;
                return Json(result);
            }
            catch (Exception exception)
            {
                return Json(new { isSucess = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Security";
            ViewBag.CallingForm1 = "Page";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/Page";
        }
    }
}