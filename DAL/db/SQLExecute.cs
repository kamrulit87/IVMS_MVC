using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.db
{
    public class SQLExecute
    {
        public static string s = System.Configuration.ConfigurationManager.ConnectionStrings["IVMS_DBEntities"].ConnectionString;
        public static EntityConnectionStringBuilder e = new EntityConnectionStringBuilder(s);
        public static string ProviderConnectionString = e.ProviderConnectionString;
        public static SqlConnection cnConnection = new SqlConnection(ProviderConnectionString);

        public String ExecuteSP(SqlCommand sqlCommand)
        {
            var value = (string)null;
            if (cnConnection.State == ConnectionState.Closed)
            {
                cnConnection.Open();
            }
            try
            {
                sqlCommand.Connection = cnConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.ExecuteNonQuery();
                value = sqlCommand.Parameters["@rMessage"].Value.ToString();
            }
            catch (Exception ex)
            {
                value = ex.Message;
            }
            finally
            {
                cnConnection.Close();
            }
            return value;
        }
    }
}
