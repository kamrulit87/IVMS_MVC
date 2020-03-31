using DAL.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using BLL.Common;
using BLL.Models;

namespace BLL.Interfaces.Appointment
{
    public interface IScheduleAppointmentFactory
    {
        Result SaveAppointment(DAL.db.Appointment appointment);
        List<DAL.db.Appointment> SearchScheduleAppointment(int employeeID);
        List<DAL.db.Appointment> SearchNotifyData(int employeeID);
        List<DAL.db.Appointment> SearchNotifyDataPS(int id, int eid);
        List<Employee> SearchDepWiseEmployee(int depID);
        List<Employee> SearchEmployeeWiseFloor(int empID);
        List<Employee> SearchEmployee(int? empID);
        List<Employee> GetDepWiseFloor(int empID);
        List<Employee> SearchDepWiseEmployeeBehalf(int did, int id);

    }
    public interface IUnScheduleAppointmentFactory
    {
        Result SaveAppointment(DAL.db.Appointment appointment);
        List<DAL.db.Appointment> SearchAppointment(string status);
        //List<DAL.db.Appointment> SearchCardWiseAppointment(string cardNo, string status);
        List<DAL.db.Appointment> SearchCheckIn(string status);
        List<DAL.db.Appointment> SearchCheckBreak(string status);
        List<DAL.db.Appointment> SearchCheckOut(string status);
        List<DAL.db.Appointment> SearchCancel(string status);
        DAL.db.Appointment SearchCardWiseAppointmentData(string cardNO);

    }
    public interface ICardFactory
    {
        Card GetFreeCard(int deviceID);
        Result UnassignCard(int deviceID, string cardNO);
        int GetAssignCardID(int deviceID, string cardNO);
        Result SaveCard(Card card);
    }
}
