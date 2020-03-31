using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BLL.Common;
using BLL.Interfaces;
using BLL.Interfaces.Appointment;
using DAL.db;

namespace BLL.Factory.Appointment
{
    public class UnScheduleAppointmentFactory : GenericFactory<IVMS_DBEntities, DAL.db.Appointment>
    {
    }
    public class UnScheduleAppointmentFactorys : IUnScheduleAppointmentFactory
    {

        private IGenericFactory<DAL.db.Appointment> _unScheduleAppointment;
        Result _result = new Result();
        private string tableName = "Appointment";
        public Result SaveAppointment(DAL.db.Appointment appointment)
        {
            _unScheduleAppointment = new UnScheduleAppointmentFactory();
            try
            {
                if (appointment.AppointmentID > 0)
                {
                    _unScheduleAppointment.Edit(appointment);
                    _result = _unScheduleAppointment.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.UpdateSuccessfull(tableName);
                    }
                }
                else
                {
                    _unScheduleAppointment.Add(appointment);
                    _result = _unScheduleAppointment.Save();
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

        public List<DAL.db.Appointment> SearchAppointment(string status)
        {
            _unScheduleAppointment = new UnScheduleAppointmentFactory();
            try
            {
                var list = new List<DAL.db.Appointment>();
                list = _unScheduleAppointment.FindBy(x => x.Status == status || x.Status == "N" || x.Status == "AP" || x.Status == "P" || x.Status == "B").OrderByDescending(x=> x.AppointmentID).Take(100).ToList();
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //public List<DAL.db.Appointment> SearchCardWiseAppointment(string cardNo, string status)
        //{
        //    _unScheduleAppointment = new UnScheduleAppointmentFactory();
        //    try
        //    {
        //        var list = new List<DAL.db.Appointment>();
        //        list = _unScheduleAppointment.FindBy(x => x.CardNO == cardNo && x.Status == status).ToList();
        //        return list;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public List<DAL.db.Appointment> SearchCheckIn(string status)
        {
            _unScheduleAppointment = new UnScheduleAppointmentFactory();
            try
            {
                var list = new List<DAL.db.Appointment>();
                list = _unScheduleAppointment.FindBy(x => x.Status == status).OrderByDescending(x => x.AppointmentID).Take(100).ToList();
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<DAL.db.Appointment> SearchCheckBreak(string status)
        {
            _unScheduleAppointment = new UnScheduleAppointmentFactory();
            try
            {
                var list = new List<DAL.db.Appointment>();
                list = _unScheduleAppointment.FindBy(x => x.Status == status).OrderByDescending(x => x.AppointmentID).Take(100).ToList();
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<DAL.db.Appointment> SearchCheckOut(string status)
        {
            _unScheduleAppointment = new UnScheduleAppointmentFactory();
            try
            {
                var list = new List<DAL.db.Appointment>();
                list = _unScheduleAppointment.FindBy(x => x.Status == status).OrderByDescending(x => x.AppointmentID).Take(100).ToList();
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<DAL.db.Appointment> SearchCancel(string status)
        {
            _unScheduleAppointment = new UnScheduleAppointmentFactory();
            try
            {
                var list = new List<DAL.db.Appointment>();
                list = _unScheduleAppointment.FindBy(x => x.Status == status).OrderByDescending(x => x.AppointmentID).Take(100).ToList();
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DAL.db.Appointment SearchCardWiseAppointmentData(string cardNO)
        {
            _unScheduleAppointment = new UnScheduleAppointmentFactory();
            try
            {
                var list = new DAL.db.Appointment();
                list = _unScheduleAppointment.FindBy(x => (x.CardNO == cardNO) && (x.Status == "I")).FirstOrDefault();
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
