using System;
using DAL.db;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Common;
using BLL.Interfaces;
using BLL.Interfaces.Appointment;

namespace BLL.Factory.Appointment
{
    public class ScheduleAppointmentFactory : GenericFactory<IVMS_DBEntities, DAL.db.Appointment>
    {
    }
    public class ScheduleEmployeeFactory : GenericFactory<IVMS_DBEntities, Employee>
    {
    }

    public class ScheduleAppointmentFactorys : IScheduleAppointmentFactory
    {
        private IGenericFactory<DAL.db.Appointment> _scheduleAppointment;
        private IGenericFactory<Employee> _scheduleEmpFactory;
        Result _result = new Result();
        private string tableName = "Appointment";
        public Result SaveAppointment(DAL.db.Appointment appointment)
        {
            _scheduleAppointment = new ScheduleAppointmentFactory();
            try
            {
                if (appointment.AppointmentID > 0)
                {
                    _scheduleAppointment.Edit(appointment);
                    _result = _scheduleAppointment.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.UpdateSuccessfull(tableName);
                    }
                }
                else
                {
                    _scheduleAppointment.Add(appointment);
                    _result = _scheduleAppointment.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.SaveSuccessfull(tableName);
                    }
                }
            }
            catch (Exception e)
            {
                _result.isSucess = false;
                _result.message = e.Message;
            }

            return _result;
        }

        public List<DAL.db.Appointment> SearchScheduleAppointment(int employeeID)
        {
            _scheduleAppointment = new ScheduleAppointmentFactory();
            try
            {
                var list = new List<DAL.db.Appointment>();
                list = _scheduleAppointment.FindBy(x => (x.EmployeeID == employeeID) && (x.Status == "A" || x.Status == "I" || x.Status == "B" || x.Status == "N" || x.Status == "AP" || x.Status == "P")).ToList();
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<DAL.db.Appointment> SearchNotifyData(int employeeID)
        {
            _scheduleAppointment = new ScheduleAppointmentFactory();
            try
            {
                var list = new List<DAL.db.Appointment>();
                list = _scheduleAppointment.FindBy(x => (x.EmployeeID == employeeID) && (x.Status == "N" || x.Status == "P")).ToList();
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<DAL.db.Appointment> SearchNotifyDataPS(int id,int eid)
        {
            _scheduleAppointment = new ScheduleAppointmentFactory();
            try
            {
                var list = new List<DAL.db.Appointment>();
                list = _scheduleAppointment.FindBy(x => (x.Employee.EmployeeID == id || x.EmployeeID == eid) && (x.Status == "N" || x.Status == "P") || (x.Employee.EmployeeID == id && x.EmployeeID == eid) && (x.Status == "N" || x.Status == "P")).ToList();
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<Employee> SearchEmployee(int? empID)
        {
            _scheduleEmpFactory = new ScheduleEmployeeFactory();
            try
            {
                var list = new List<Employee>();
                if (empID > 0)
                {
                    list = _scheduleEmpFactory.FindBy(x => x.EmployeeID == empID).ToList();
                }
                else
                {
                    list = _scheduleEmpFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<Employee> SearchDepWiseEmployee(int depID)
        {
            _scheduleEmpFactory = new ScheduleEmployeeFactory();
            try
            {
                var list = new List<Employee>();
                if (depID > 0)
                {
                    list = _scheduleEmpFactory.FindBy(x => x.DepartmentID == depID).ToList();
                }
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<Employee> SearchDepWiseEmployeeBehalf(int did, int id)
        {
            _scheduleEmpFactory = new ScheduleEmployeeFactory();
            try
            {
                var list = new List<Employee>();
                if (did > 0)
                {
                    list = _scheduleEmpFactory.FindBy(x => x.DepartmentID == did && x.OnBehalfEmployeeID == id && x.EmployeeID != id).ToList();
                }
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<Employee> SearchEmployeeWiseFloor(int empID)
        {
            _scheduleEmpFactory = new ScheduleEmployeeFactory();
            try
            {
                var list = new List<Employee>();
                if (empID > 0)
                {
                    list = _scheduleEmpFactory.FindBy(x => x.EmployeeID == empID).ToList();
                }
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<Employee> GetDepWiseFloor(int empID)
        {
            _scheduleEmpFactory = new ScheduleEmployeeFactory();
            try
            {
                var list = new List<Employee>();
                if (empID > 0)
                {
                    list = _scheduleEmpFactory.FindBy(x => x.EmployeeID == empID).ToList();
                }
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
