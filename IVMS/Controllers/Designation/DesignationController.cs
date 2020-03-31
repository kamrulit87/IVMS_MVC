using BLL.Common;
using BLL.Factory.Setup;
using BLL.Interfaces.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IVMS.Controllers.Designation
{

    public class DesignationController : Controller
    {
        IDesignationFactory DesignationFactory;

        Result result;
        public ActionResult DesignationCreate()
        {
            DefaultLoad();
            return View();
        }

        public ActionResult DesignationList()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = " Designation";
            ViewBag.CallingViewPage = "#";
            return View();
        }

        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = " Designation";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/Designation";
        }

        [HttpPost]
        public JsonResult DesignationCreate(DAL.db.Designation  designation)
        {
            result = new Result();
            DesignationFactory = new DesignationFactorys();
            result=DesignationFactory.SaveDesignation(designation);
            return Json(result);

        }
        [HttpPost]
        public JsonResult DesignationLists(int? id)
        {
            result = new Result();
            DesignationFactory = new DesignationFactorys();
            List<DAL.db.Designation> list = DesignationFactory.SearchDesignation(id);

            var designationList = list.Select(x => new { x.DesignationID, x.DesignationName,x.Code });

            return Json(designationList, JsonRequestBehavior.AllowGet);
        }



    }
}