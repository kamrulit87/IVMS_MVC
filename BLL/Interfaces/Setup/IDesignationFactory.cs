using BLL.Common;
using DAL.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Setup
{
  

    public interface IDesignationFactory
    {
        Result SaveDesignation(Designation designation);
        List<Designation> SearchDesignation(int? DesignationID);
    }
}
