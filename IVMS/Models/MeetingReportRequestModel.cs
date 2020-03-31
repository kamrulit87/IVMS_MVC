using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IVMS.Models
{
    public class MeetingReportRequestModel
    {
        public int EmployeeID { get; set; }
        public string VisitorName { get; set; }
        public string VisitorMobile { get; set; }
        public string VisitorCard { get; set; }
        public string CompanyName { get; set; }
        public string Status { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}