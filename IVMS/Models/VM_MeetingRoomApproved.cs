using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IVMS.Models
{
    public class VM_MeetingRoomApproved
    {
        public int RequisitionID { get; set; }
        public int MeetingRoomID { get; set; }
        public string RoomNo { get; set; }
        public string FloorLocation { get; set; }
        public int SeatCapacity { get; set; }
        public string ProjectorAvailable { get; set; }
        public int RequisitionBy { get; set; }
        public string RequisitionByName { get; set; }
        public DateTime RequiredDate { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public string Remarks { get; set; }
        public string RoomStatus { get; set; }
    }
}