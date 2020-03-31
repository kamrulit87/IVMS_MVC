using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Factory.Security;
using BLL.Interfaces.Security;
using BLL.Models;
using DAL.Helper;

namespace IVMS.Areas.Reports.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports/Reports
        Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
        public ActionResult VisitorINOut()
        {
            
            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = securityLogInFactory.GetCrudPermission(userGroupId, "VisitorINOut");
                if (tblUserActionMapping.Select == true)
                {
                    ViewBag.CallingForm = "Security";
                    ViewBag.CallingForm1 = "Visitor In Out";
                    ViewBag.CallingViewPage = "#";
                    return View();
                }
            }
            Session["logInSession"] = null;
            return Redirect("/#!/");
        }
    }
}