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


    public class DepartmentFactory : GenericFactory<IVMS_DBEntities, Department>
    {

    }
    public class DepartmentFactorys : IDepartmentFactory
    {

        private IVMS_DBEntities context;
        private IGenericFactory<Department> _departmentFactory;
        string tableName = "Department";

        public Result SaveDepartment(Department dept)
        {
            Result _result = new Result();
            try
            {
                _departmentFactory = new DepartmentFactory();
                if (dept.DepartmentID < 1)
                {
                    _departmentFactory.Add(dept);
                    _result = _departmentFactory.Save();

                    if (_result.isSucess)
                    {
                        _result.isSucess = true;
                        _result.message = _result.SaveSuccessfull(tableName);
                    }
                }
                else
                {
                    _departmentFactory.Edit(dept);
                    _result = _departmentFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.isSucess = true;
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

        public List<Department> SearchDepartment(int? DepartmentID)
        {
            try
            {
                _departmentFactory = new DepartmentFactory();
                var list = new List<Department>();
                if (DepartmentID > 0)
                {
                    list = _departmentFactory.FindBy(x => x.DepartmentID == DepartmentID).ToList();
                }
                else
                {
                    list = _departmentFactory.GetAll().ToList();
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
