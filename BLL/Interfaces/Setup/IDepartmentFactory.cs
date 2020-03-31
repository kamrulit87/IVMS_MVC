using BLL.Common;
using DAL.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Setup
{
    public interface IDepartmentFactory
    {

        Result SaveDepartment(Department dept);
        List<Department> SearchDepartment(int? DepartmentID);
    }
}
