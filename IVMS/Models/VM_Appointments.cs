﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IVMS.Models
{
    public class VM_Appointments
    {
        public string AppointmentBy { get; set; }
        public int AppointmentID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int EmployeeID { get; set; }
        public string EmpName { get; set; }
        public string CompanyName { get; set; }
        public string Purpose { get; set; }
        public DateTime AppointmentTime { get; set; }
        public DateTime? ReachTime { get; set; }
        public DateTime? CheckedInTime { get; set; }
        public DateTime? CheckedOutTime { get; set; }
        public DateTime? BreakOutTime { get; set; }
        public DateTime? BreakInTime { get; set; }
        public string Status { get; set; }
        public string CardNO { get; set; }
        public string VisitorName { get; set; }
        public string VisitorMobile { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}