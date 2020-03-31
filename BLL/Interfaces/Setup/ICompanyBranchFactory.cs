using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Common;
using DAL.db;

namespace BLL.Interfaces.Setup
{
    public interface ICompanyBranchFactory
    {
        Result SaveCompanyBranch(SET_CompanyBranch companyBranch);
        List<SET_CompanyBranch> SearchCompanyBranch(int? id);

    }
}
