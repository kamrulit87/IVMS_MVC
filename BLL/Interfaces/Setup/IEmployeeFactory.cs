using BLL.Common;
using DAL.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Setup
{
    public interface IEmployeeFactory
    {
        Result SaveEmployee(Employee emp);
        List<Employee> SearchEmployee(int? EmployeeID);
    }
}
