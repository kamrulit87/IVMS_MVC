using System;
using DAL.db;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.EntityClient;
using System.Data;
using System.Data.SqlClient;
using BLL.Interfaces;
using BLL.Models;

namespace BLL.Factory
{
    public class PaginationFactory : IPaginationFactory
    {
        public static string s = System.Configuration.ConfigurationManager.ConnectionStrings["HrDBEntities"].ConnectionString;
        public static EntityConnectionStringBuilder e = new EntityConnectionStringBuilder(s);
        public static string ProviderConnectionString = e.ProviderConnectionString;
        public static SqlConnection cnConnection = new SqlConnection(ProviderConnectionString);
        //public VM_Pagination WorksPagination(string name, int pageSize, int pageIndex)
        //{
        //    VM_Pagination vmWork = new VM_Pagination();
        //    SqlCommand cmd = new SqlCommand("SP_WorkSearchPagination");
        //    cmd.Parameters.AddWithValue("@iNamevalue", name);
        //    cmd.Parameters.AddWithValue("@iPageindex", pageIndex);
        //    cmd.Parameters.AddWithValue("@iPagesize", pageSize);
            

        //    if (cnConnection.State == ConnectionState.Closed)
        //    {
        //        cnConnection.Open();
        //    }
        //    try
        //    {
        //        List<WorkList> workList = new List<WorkList>();
        //        cmd.Connection = cnConnection;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        IDataReader dr = null;
        //        dr = cmd.ExecuteReader();
                
        //        while (dr.Read())
        //        {
        //            WorkList work = new WorkList();
        //            work.WorkID = (int)dr["WorkID"];
        //            work.UnitID = (int)dr["UnitID"];
        //            work.WorkStageID = (int)dr["WorkStageID"];
        //            work.WorkCategoryID = (int)dr["WorkCategoryID"];
        //            work.WorkSubCategoryID = (int)dr["WorkSubCategoryID"];
        //            work.Name = dr["WorkName"].ToString();
        //            work.WDescription = dr["WDescription"].ToString();
        //            work.LabourRate = Convert.ToDecimal(dr["LabourRate"]);
        //            work.CalculateBoqFor = dr["CalculateBoqFor"].ToString();
        //            work.WorkCategoryName = dr["WorkCategoryName"].ToString();
        //            work.WorkSubCategoryName = dr["WorkSubCategoryName"].ToString();
        //            work.UnitName = dr["UnitName"].ToString();
        //            workList.Add(work);
        //        }
        //        dr.NextResult();
        //        while (dr.Read())
        //        {
        //            vmWork.TotalCount = (int)dr["totalCount"];
        //        }
        //        vmWork.WorkData = workList;
                
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        cnConnection.Close();
        //    }
        //    return vmWork;
            
        //}

        //public List<Inv_Unit> SearchUnitDropDown(string name, int pageSize, int pageIndex)
        //{
        //    List<Inv_Unit> unitList = new List<Inv_Unit>();
        //    SqlCommand cmd = new SqlCommand("SP_SearchUnitDropdown");
        //    cmd.Parameters.AddWithValue("@iNamevalue", name);
        //    cmd.Parameters.AddWithValue("@iPageindex", pageIndex);
        //    cmd.Parameters.AddWithValue("@iPagesize", pageSize);


        //    if (cnConnection.State == ConnectionState.Closed)
        //    {
        //        cnConnection.Open();
        //    }
        //    try
        //    {
        //        cmd.Connection = cnConnection;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        IDataReader dr = null;
        //        dr = cmd.ExecuteReader();

        //        while (dr.Read())
        //        {
        //            Inv_Unit unit = new Inv_Unit();
        //            unit.UnitID = (int)dr["UnitID"];
        //            unit.Name = dr["Name"].ToString();
        //            unitList.Add(unit);
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        cnConnection.Close();
        //    }
        //    return unitList;
        //}
    }
}
