using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IVMS.Areas.Reports.Models
{
    public class VM_VisitorINOut
    {
        public string DepartmentName { get; set; }
        public string EmpName { get; set; }
        public string VisitorName { get; set; }
        public string CompanyName { get; set; }
        public string Purpose { get; set; }
        public DateTime? CheckedOutTime { get; set; }
        public DateTime? CheckedInDate { get; set; }
        public DateTime? CheckedInTime { get; set; }
    }
}