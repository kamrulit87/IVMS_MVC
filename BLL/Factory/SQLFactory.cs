using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.EntityClient;
using System.Data;
using System.Data.SqlClient;
using BLL.Interfaces;

namespace BLL.Factory
{
    public class SQLFactory : ISQLFactory
    {
        public static string s = System.Configuration.ConfigurationManager.ConnectionStrings["RealEstateDBEntities"].ConnectionString;
        public static EntityConnectionStringBuilder e = new EntityConnectionStringBuilder(s);
        public static string ProviderConnectionString = e.ProviderConnectionString;
        public static SqlConnection cnConnection = new SqlConnection(ProviderConnectionString);
        public DataTable ExecuteQuery(string sQuery)
        {
            string error = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter myAdapter = new SqlDataAdapter(sQuery, cnConnection);
                myAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return dt;
        }

        public String GetSingleValue(string sql)
        {
            var value = (string)null;           
            if (cnConnection.State == ConnectionState.Closed)
            {
                cnConnection.Open();
            }
            try
            {
                SqlCommand cmd = new SqlCommand(sql, cnConnection);
                value = cmd.ExecuteScalar().ToString();
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
                value = "1";
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

        public String ExecuteSP_GnCode(DateTime date, string tableName, string fieldName, string prefix)
        {

            SqlCommand cmd = new SqlCommand("sp_GenerateCode");
            SqlParameter code = new SqlParameter("@oCode", SqlDbType.VarChar,30);
            code.Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@iDate", date);
            cmd.Parameters.AddWithValue("@iTbl", tableName);
            cmd.Parameters.AddWithValue("@iFieldName", fieldName);
            cmd.Parameters.AddWithValue("@iPrefix", prefix);

            cmd.Parameters.Add(code);

            var value = (string)null;
            if (cnConnection.State == ConnectionState.Closed)
            {
                cnConnection.Open();
            }
            try
            {
                cmd.Connection = cnConnection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                value = cmd.Parameters["@oCode"].Value.ToString(); 
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
