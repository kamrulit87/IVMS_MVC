using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ISQLFactory
    {
        DataTable ExecuteQuery(string sQuery);
        String GetSingleValue(string sql);
        String ExecuteSP(SqlCommand sql);
        String ExecuteSP_GnCode(DateTime date, string tableName, string fieldName, string prefix);
    }
}
