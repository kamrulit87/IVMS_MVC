using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.EntityClient;
 

namespace DAL.Common
{   
    public class DataManager 
    {
        public static string s = System.Configuration.ConfigurationManager.ConnectionStrings["IVMS_DBEntities"].ConnectionString;
        public static  EntityConnectionStringBuilder e = new EntityConnectionStringBuilder(s);
        public static string ProviderConnectionString = e.ProviderConnectionString;
        public static SqlConnection cnConnection = new SqlConnection(ProviderConnectionString);  

        public static string gCompanyName = "ABC ";
        public static string gCompanyAdd = "Dhaka";
        public static string gCompanyRegAdd = "Gulshan1"; 
        public static int ConnectionTimeOut = 0; 
        public static DataTable GetCompanyInfo()
        {
            string sQuery = "select * from OrganizationCore";
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter myAdapter = new SqlDataAdapter(sQuery, cnConnection);
                myAdapter.Fill(dt);
            }
            catch
            {

            }
            return dt;
        }
       

        public static DataTable ExecuteQuery(string sQuery)
        {
            string error = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter myAdapter = new SqlDataAdapter(sQuery, cnConnection);
                myAdapter.Fill(dt);
            }
            catch(Exception ex)
            {
               error = ex.Message;
            }

            return dt;
        }      

        public static int ExecuteScalar(string sQuery)      //Returns -1 if failed.
        {
            SqlCommand cmd = new SqlCommand(sQuery, cnConnection);
            int i = 0;
            try
            {
                cnConnection.Open();
                i = (int)cmd.ExecuteScalar();
            }
            catch
            {
                i = -1;
            }
            finally
            {
                if (cnConnection.State != ConnectionState.Closed)
                {
                    cnConnection.Close();
                }
            }
            return i;
        }

        public static int ExecuteNonQuery(string sQuery)
        {
            int i = 0;
            try
            {
                cnConnection.Open();
                SqlCommand myCommand = new SqlCommand(sQuery, cnConnection);
                i = myCommand.ExecuteNonQuery();
            }
            catch
            {
                i = -1;
            }
            finally
            {
                if (cnConnection.State != ConnectionState.Closed)
                {
                    cnConnection.Close();
                }
            }
            return i;
        }

        public static DataTable SlowExecuteQuery(string sQuery)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand myCommand = new SqlCommand(sQuery, cnConnection);
                myCommand.CommandTimeout = ConnectionTimeOut;

                SqlDataAdapter myAdapter = new SqlDataAdapter(myCommand);
                myAdapter.Fill(dt);
            }
            catch
            {
            }

            return dt;
        } 
    }
}