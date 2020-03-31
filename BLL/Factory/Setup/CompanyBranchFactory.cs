using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Common;
using BLL.Interfaces;
using BLL.Interfaces.Setup;
using DAL.db;
using DAL.Helper;

namespace BLL.Factory.Setup
{
    public class CompanyBranchFactory : GenericFactory<IVMS_DBEntities, SET_CompanyBranch>
    {

    }
    public class CompanyBranchFactorys : ICompanyBranchFactory
    {
        private IVMS_DBEntities db;
        private IGenericFactory<SET_CompanyBranch> _companyBranchFactory;
        private Result _result;
        public CompanyBranchFactorys()
        {
            db = new IVMS_DBEntities();
        }
        public Result SaveCompanyBranch(SET_CompanyBranch companyBranch)
        {
            _result = new Result();
            _companyBranchFactory = new CompanyBranchFactory();
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
                if (userGroupId != 0)
                {
                    if (companyBranch.BranchID > 0)
                    {
                        _companyBranchFactory.Edit(companyBranch);
                        _result = _companyBranchFactory.Save();

                    }
                    else
                    {
                        int branchID = 1;
                        var prvBranchID = _companyBranchFactory.GetLastRecord().OrderByDescending(x => x.BranchID).FirstOrDefault();
                        if (prvBranchID != null)
                        {
                            branchID = prvBranchID.BranchID + 1;
                        }

                        companyBranch.BranchID = branchID;
                        _companyBranchFactory.Add(companyBranch);
                        _result = _companyBranchFactory.Save();
                        
                    }
                }
                else
                {
                    _result.isSucess = false;
                    _result.message = "Logout";
                }

            }
            catch (Exception e)
            {
                _result.isSucess = false;
                _result.message = e.Message;
            }

            return _result;
        }

        public List<SET_CompanyBranch> SearchCompanyBranch(int? id)
        {
            try
            {
                _companyBranchFactory = new CompanyBranchFactory();
                var list = new List<SET_CompanyBranch>();
                if (id > 0)
                {
                    list = _companyBranchFactory.FindBy(x => x.BranchID == id).ToList();
                }
                else
                {
                    list = _companyBranchFactory.GetAll().ToList();
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
