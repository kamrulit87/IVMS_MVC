using DAL.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces.Setup;
using BLL.Interfaces;
using BLL.Common;

namespace BLL.Factory.Setup
{
    public class EmployeeFactory : GenericFactory<IVMS_DBEntities, Employee>
    {

    }
    public class EmployeeFactorys : IEmployeeFactory
    {

        private IVMS_DBEntities context;
        private IGenericFactory<Employee> _employeeFactory;
        string tableName = "Employee";

        public Result SaveEmployee(Employee emp)
        {
            Result _result = new Result();
            try
            {
                _employeeFactory = new EmployeeFactory();
                if (emp.EmployeeID < 1)
                {
                    _employeeFactory.Add(emp);
                    _result = _employeeFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.SaveSuccessfull(tableName);
                    }
                }
                else
                {
                    _employeeFactory.Edit(emp);
                    _result = _employeeFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.UpdateSuccessfull(tableName);
                    }
                }
            }
            catch (Exception ex)
            {
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }

        public List<Employee> SearchEmployee(int? EmployeeID)
        {
            try
            {
                _employeeFactory = new EmployeeFactory();
                var list = new List<Employee>();
                if (EmployeeID > 0)
                {
                    list = _employeeFactory.FindBy(x => x.EmployeeID == EmployeeID).ToList();
                }
                else
                {
                    list = _employeeFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
