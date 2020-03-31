using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IVMS.Areas.Reports.Models
{
    public class Parameters
    {
        public DateTime? FDate { get; set; }
        public DateTime? TDate { get; set; }
        public int? DepartmentID { get; set; }
        public int? EmployeeID { get; set; }
        public string DepartmentName { get; set; }
        public string EmpName { get; set; }
    }
}