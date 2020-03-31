using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IVMS.Models
{
    public class VM_ScheduleAppointment
    {
        public string AppointmentBy { get; set; }
         public int AppointmentID {get; set;}
         public DateTime AppointmentDate {get; set;}
         public int EmployeeID {get; set;}
         public string VisitorMobile {get; set;}
         public int DepartmentID {get; set;}
         public int DesignationID {get; set;}
         public string EmpName { get; set; }
         public string DepartmentName { get; set; }
         public string DesignationName { get; set; }
         public string CompanyName { get; set; }
         public string Purpose { get; set; }
         public DateTime AppointmentTime { get; set; }
         public DateTime? ReachTime { get; set; }
         public DateTime? EndTime { get; set; }
         public string CardNO { get; set; }
         public string Status { get; set; }
        public string VisitorName { get; set; }
        public string VisitorEmail { get; set; }
        public int CreatedBy {get; set;}
        public DateTime CreatedDate { get; set; }
        public DateTime? CheckedInTime { get; set; }
        public DateTime? CheckedOutTime { get; set; }
        public string AccessFloors { get; set; }
        public string NotifyMessage { get; set; }
        public string ReplayMessage { get; set; }
        public string FirstWordMsg { get; set; }
    }
}