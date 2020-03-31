using BLL.Common;
using BLL.Factory;
using BLL.Interfaces;
using DAL.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Helper;

namespace IVMS.Controllers
{
    public class CommonController : Controller
    {
        Result result; 
        ISQLFactory sqlFactory;
        Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
        IVMS_DBEntities db = new IVMS_DBEntities();
        public JsonResult GenerateCode(string tableName, string fieldName, string prefix)
        {
            result = new Result();
            sqlFactory = new SQLFactory();
            result.message = sqlFactory.ExecuteSP_GnCode(DateTime.Now.Date, tableName, fieldName, prefix);
            return Json(result);
        }
        [HttpPost]
        public JsonResult LoadCompany(int? id)
        {
            int companyID = Convert.ToInt32(dictionary[9].Id == "" ? 0 : Convert.ToInt32(dictionary[9].Id));
            var companyList = from u in db.SET_Company
                where u.CompanyID == companyID
                select new
                {
                    u.CompanyID,
                    u.Name,
                    u.Code
                };
            return Json(companyList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CompanyWiseBranch(int companyID)
        {
            int branchID = Convert.ToInt32(dictionary[10].Id == "" ? 0 : Convert.ToInt32(dictionary[10].Id));
            var branchList = from u in db.SET_Company
                             join cb in db.SET_CompanyBranch on u.CompanyID equals cb.CompanyID
                             where cb.CompanyID == companyID && cb.BranchID == branchID
                select new
                {
                    cb.BranchID,
                    Name = cb.Name
                };
            return Json(branchList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadAllCompany(int? id)
        {
            var companyList = (from c in db.SET_Company
                select new
                {
                    c.CompanyID,
                    c.Name,
                    c.Code
                }).ToList();
            return Json(companyList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CompanyWiseAllBranch(int companyID)
        {
            var branchList = (from c in db.SET_Company
                join cb in db.SET_CompanyBranch on c.CompanyID equals cb.CompanyID
                where c.CompanyID == companyID
                select new
                {
                    cb.BranchID,
                    Name = cb.Name
                }).ToList();
            return Json(branchList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AllBranch()
        {
            var branchList = from c in db.SET_CompanyBranch
                             
                             select new
                             {
                                 c.BranchID,
                                 Name = c.Name,
                                 c.Code
                             };
            return Json(branchList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AllComWiseEmployee()
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int companyID = Convert.ToInt32(dictionary[9].Id == "" ? 0 : Convert.ToInt32(dictionary[9].Id));
            int branchID = Convert.ToInt32(dictionary[10].Id == "" ? 0 : Convert.ToInt32(dictionary[10].Id));
            try
            {
                var employee = from b in db.Employees
                    where b.CompanyID == companyID && b.BranchID == branchID
                    select new
                    {
                        b.EmployeeID,
                        b.EmpName
                    };
                return Json(employee, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ComWiseEmployee()
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int companyID = Convert.ToInt32(dictionary[9].Id == "" ? 0 : Convert.ToInt32(dictionary[9].Id));
            int branchID = Convert.ToInt32(dictionary[10].Id == "" ? 0 : Convert.ToInt32(dictionary[10].Id));
            try
            {
                var employee = from b in db.Employees
                    where b.CompanyID == companyID && b.BranchID == branchID && b.Status == 1
                    select new
                    {
                        b.EmployeeID,
                        b.EmpName
                    };
                return Json(employee, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeptWiseEmployee(int deptID)
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int companyID = Convert.ToInt32(dictionary[9].Id == "" ? 0 : Convert.ToInt32(dictionary[9].Id));
            int branchID = Convert.ToInt32(dictionary[10].Id == "" ? 0 : Convert.ToInt32(dictionary[10].Id));
            try
            {
                var employee = from b in db.Employees
                    where b.CompanyID == companyID && b.BranchID == branchID && b.Status == 1 && b.DepartmentID == deptID
                    select new
                    {
                        b.EmployeeID,
                        b.EmpName
                    };
                return Json(employee, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //[HttpPost]
        //public JsonResult LoadBank()
        //{
        //    var bankList = db.Banks.Select(x=> new {x.BankId, x.BankName}).ToList();
        //    return Json(bankList, JsonRequestBehavior.AllowGet);
        //}
        //[HttpPost]
        //public JsonResult LoadEmployee()
        //{
        //    int companyID = Convert.ToInt32(dictionary[9].Id == "" ? 0 : Convert.ToInt32(dictionary[9].Id));
        //    int branchID = Convert.ToInt32(dictionary[10].Id == "" ? 0 : Convert.ToInt32(dictionary[10].Id));
        //    var bankList = from b in db.Hr_PersonnelBasics
        //        join o in db.Hr_PersonnelOfficial on b.PersonnelId equals o.PersonnelID
        //        where o.CompanyID == companyID && o.BranchID == branchID && o.PersonnelStatus == 1
        //        select new
        //        {
        //            b.PersonnelId,
        //            PersonnelName = b.PersonnelName + " - " + b.PersonnelCode
        //        };
        //    return Json(bankList, JsonRequestBehavior.AllowGet);
        //}
        //[HttpPost]
        //public JsonResult LoadDepartment()
        //{
        //    int companyID = Convert.ToInt32(dictionary[9].Id == "" ? 0 : Convert.ToInt32(dictionary[9].Id));
        //    int branchID = Convert.ToInt32(dictionary[10].Id == "" ? 0 : Convert.ToInt32(dictionary[10].Id));
        //    var deptList = from d in db.Hr_Department
        //                   join o in db.Hr_PersonnelOfficial on d.DepartmentId equals o.DepartmentID
        //        where o.CompanyID == companyID && o.BranchID == branchID
        //        select new
        //        {
        //            d.DepartmentId,
        //            d.DepartmentHead
        //        };
        //    return Json(deptList, JsonRequestBehavior.AllowGet);
        //}
    }
}