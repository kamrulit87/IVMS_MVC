using BLL.Common;
using BLL.Factory.Setup;
using BLL.Interfaces.Setup;
using DAL.db;
using DAL.Helper;
using ICBS.Controllers;
using IVMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IVMS.Controllers
{
 
    public class EmployeeController : Controller
    {
        IEmployeeFactory EmployeeFactory;
        Result  result;
        Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
        public ActionResult EmployeeCreate()
        {
            DefaultLoad();
            return View();
        }

        public ActionResult EmployeeList()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = " Employee";
            ViewBag.CallingViewPage = "#";
            return View();
        }

        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = " Employee";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/Employee";
        }

        [HttpPost]
        public JsonResult EmployeeCreate(DAL.db.Employee emp, List<int> beHalfEmpList, UserModel user)
        {
            IVMS_DBEntities db = new IVMS_DBEntities();
            result = new Result();
            JsonResult jsonResult = new JsonResult();
            EmployeeFactory= new EmployeeFactorys();
            int companyID = Convert.ToInt32(dictionary[9].Id == "" ? 0 : Convert.ToInt32(dictionary[9].Id));

            emp.CompanyID = companyID;
            emp.Status = 1;
            
            
            result = EmployeeFactory.SaveEmployee(emp);
            if(result.isSucess)
            {
                if (beHalfEmpList != null && beHalfEmpList.Count > 0)
                {
                    // Self OnBehalfEmployeeID
                    var appDataSelf = db.Employees.Where(x => x.EmployeeID == emp.EmployeeID).FirstOrDefault();
                    appDataSelf.OnBehalfEmployeeID = emp.EmployeeID;
                    db.Entry(appDataSelf).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    foreach (var details in beHalfEmpList)
                    {
                        var appData = db.Employees.Where(x => x.EmployeeID == details).FirstOrDefault();
                        appData.OnBehalfEmployeeID = emp.EmployeeID;
                        db.Entry(appData).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }                    
                }
                if (user.UserGroupID > 0)
                {
                    UserController userController = new UserController();
                   jsonResult = userController.CreateUserSave(user);                   
                }    
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult EmployeeList(int? EmployeeID)
        {
            result = new Result();
            EmployeeFactory = new EmployeeFactorys();
            List<DAL.db.Employee> list = EmployeeFactory.SearchEmployee(EmployeeID);
            var empList = list.Select(x => new { x.EmployeeID,x.EmpName, x.BranchID,BranchName=x.SET_CompanyBranch.Name,x.DepartmentID,DepartmentName=x.Department.DepartmentName,x.DesignationID,DesignationName=x.Designation.DesignationName,x.Location,x.Floor,x.EmpCode,x.Mobile,x.PbxNumber,x.Email,x.OnBehalfEmployeeID });

            return Json(empList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OnBehalfEmployee(int? employeeID)
        {
            IVMS_DBEntities db = new IVMS_DBEntities();
            //result = new Result();
            //EmployeeFactory = new EmployeeFactorys();
            //List<DAL.db.Employee> list = EmployeeFactory.SearchEmployee(EmployeeID);

            var emp = db.Employees.Where(x => x.OnBehalfEmployeeID == employeeID && x.EmployeeID != employeeID).ToList();
           var empList = emp.Select(x => new { x.EmployeeID, x.EmpName, x.BranchID, BranchName = x.SET_CompanyBranch.Name, x.DepartmentID, DepartmentName = x.Department.DepartmentName, x.DesignationID, DesignationName = x.Designation.DesignationName, x.Location, x.Floor, x.EmpCode, x.Mobile, x.PbxNumber, x.Email, x.OnBehalfEmployeeID });

            return Json(empList, JsonRequestBehavior.AllowGet);
        }

    }
}