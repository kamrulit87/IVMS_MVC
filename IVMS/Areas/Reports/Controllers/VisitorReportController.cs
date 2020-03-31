using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Common;
using BLL.Factory;
using DAL.db;
using DAL.Helper;
using IVMS.Areas.Reports.Models;
using Microsoft.Reporting.WebForms;

namespace IVMS.Areas.Reports.Controllers
{
    public class VisitorReportController : Controller
    {
        // GET: Reports/VisitorReport
        Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
        Function _function;
        private SQLFactory _sqlFactory;
        List<ReportParameter> reportParameters = new List<ReportParameter>();
        string strSQL = "";
        string path = string.Empty;
        string fileString = string.Empty;
        string reportDataSetName = string.Empty;
        string reportDataSetName2 = string.Empty;
        DataTable dataTable = new DataTable();
        IVMS_DBEntities db = new IVMS_DBEntities();
        [HttpPost]
        public JsonResult VisitorINOutReport(Parameters parameters)
        {
            if (parameters.DepartmentID == null)
            {
                parameters.DepartmentID = 0;
            }
            if (parameters.EmployeeID == null)
            {
                parameters.EmployeeID = 0;
            }
            int empID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            _function = new Function();
            string docType = "pdf";
            SqlParameter c1 = new SqlParameter("@C1", parameters.FDate);
            SqlParameter c2 = new SqlParameter("@C2", parameters.TDate);
            SqlParameter c3 = new SqlParameter("@C3", parameters.DepartmentID);
            SqlParameter c4 = new SqlParameter("@C4", parameters.EmployeeID);
            var visitor = db.Database.SqlQuery<VM_VisitorINOut>("sp_VisitorInOutList @C1, @C2, @C3, @C4", c1, c2, c3, c4).ToList();
            dataTable = _function.ToDataTable(visitor);

            path = Path.Combine(Server.MapPath("~/Areas/Reports/RDLC"), "VisitorInOutReport.rdlc");
            reportDataSetName = "DS_VisitorINOut";
            var fDate = Convert.ToDateTime(parameters.FDate).ToString("dd-MM-yyyy");
            var tDate = Convert.ToDateTime(parameters.TDate).ToString("dd-MM-yyyy");
            reportParameters.Add(new ReportParameter("FDate", fDate));
            reportParameters.Add(new ReportParameter("TDate", tDate));
            reportParameters.Add(new ReportParameter("DeptName", parameters.DepartmentName));
            reportParameters.Add(new ReportParameter("EmpName", parameters.EmpName));
            fileString = _function.CallReports(docType, reportParameters, true, path, dataTable, null, reportDataSetName);
            return Json(fileString);

        }
    }
}