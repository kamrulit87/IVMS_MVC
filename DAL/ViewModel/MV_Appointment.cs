using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModel
{
    public class MV_Appointment
    {
        public int AppointmentID { get; set; }
        public string VisitorName { get; set; }
        public string VisitorMobile { get; set; }
        public string VisitorEmail { get; set; }
        public string CompanyName { get; set; }
        public int EmployeeID { get; set; }
        public string Purpose { get; set; }
        public System.DateTime AppointmentDate { get; set; }
        public Nullable<System.DateTime> AppointmentTime { get; set; }
        public Nullable<System.DateTime> ReachTime { get; set; }
        public Nullable<System.DateTime> CheckedInTime { get; set; }
        public Nullable<System.DateTime> CheckedOutTime { get; set; }
        public Nullable<System.DateTime> BreakOutTime { get; set; }
        public Nullable<System.DateTime> BreakInTime { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string AppointmentBy { get; set; }
        public string Status { get; set; }
        public string CardNO { get; set; }
        public string BankCardID { get; set; }
        public string NotifyMessage { get; set; }
        public string ReplayMessage { get; set; }
        public string AccessFloors { get; set; }
    }
}
