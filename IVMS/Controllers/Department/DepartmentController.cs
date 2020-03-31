using BLL.Common;
using BLL.Factory.Setup;
using BLL.Interfaces.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IVMS.Controllers.Department
{
  
    public class DepartmentController : Controller
    {

        IDepartmentFactory DepartmentFactory;
        Result result;

        public ActionResult DepartmentCreate()
        {
            DefaultLoad();
            return View();
        }

        public ActionResult DepartmentList()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = " Department";
            ViewBag.CallingViewPage = "#";
            return View();
        }

        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = " Department";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/Department";
        }

        [HttpPost]
        public JsonResult DepartmentCreate(DAL.db.Department dept)
        {
            result = new Result();

            DepartmentFactory = new DepartmentFactorys();
            result = DepartmentFactory.SaveDepartment(dept);

            return Json(result);

        }
        [HttpPost]
        public JsonResult DepartmentLists(int? DepartmentID)
        {
            result = new Result();

            DepartmentFactory = new DepartmentFactorys();
            List<DAL.db.Department> list = DepartmentFactory.SearchDepartment(DepartmentID);
            var deptList = list.Select(x => new { x.DepartmentID, x.DepartmentName});
            return Json(deptList, JsonRequestBehavior.AllowGet);


        }



    }
}