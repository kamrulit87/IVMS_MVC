using BLL.Common;
using BLL.Interfaces;
using BLL.Interfaces.Setup;
using DAL.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Factory.Setup
{

    public class DesignationFactory : GenericFactory<IVMS_DBEntities, Designation>
    {

    }
    public class DesignationFactorys : IDesignationFactory
    {

        private IVMS_DBEntities context;
        private IGenericFactory<Designation> _esignationFactory;
        string tableName = "Designation";

        public Result SaveDesignation(Designation designation)
        {
            Result _result = new Result();
            try
            {
                _esignationFactory = new DesignationFactory();
                if (designation.DesignationID < 1)
                {
                    _esignationFactory.Add(designation);
                    _result = _esignationFactory.Save();

                    if (_result.isSucess)
                    {
                        _result.isSucess = true;
                        _result.message = _result.SaveSuccessfull(tableName);
                    }
                }
                else
                {
                    _esignationFactory.Edit(designation);
                    _result = _esignationFactory.Save();
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

        public List<Designation> SearchDesignation(int? DesignationID)
        {
            try
            {
                _esignationFactory = new DesignationFactory();
                var list = new List<Designation>();
                if (DesignationID > 0)
                {
                    list = _esignationFactory.FindBy(x => x.DesignationID == DesignationID).ToList();
                }
                else
                {
                    list = _esignationFactory.GetAll().ToList();
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
