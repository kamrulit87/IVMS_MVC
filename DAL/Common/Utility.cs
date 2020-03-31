using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace DAL.Common
{
    public class Utility
    {
        public static string sMAC;// = GetMAC();
        public static string GetMAC()
        {
            string sMAC = "";
            System.Net.NetworkInformation.NetworkInterface[] adapters = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            if (adapters.Length > 0)
            {
                System.Net.NetworkInformation.NetworkInterface adapter = adapters[0];
                System.Net.NetworkInformation.PhysicalAddress phadd = adapter.GetPhysicalAddress();
                sMAC = phadd.ToString();
            }
            return sMAC; 
        }

        //public static string sMachineNumber = DataManager.myReader.GetValue("MachineNumber", typeof(System.String)).ToString();

        //public static string sMachineNumber = "1";

        //SessionExpire Function
        public class SessionExpireAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                HttpContext ctx = HttpContext.Current;
                // check  sessions here 
                if (HttpContext.Current.Session["UserId"] == null)
                {
                    filterContext.Result = new RedirectResult("~/security/login");
                    return;
                }

                if (HttpContext.Current.Session["MachineNo"] == null)
                {
                    filterContext.Result = new RedirectResult("~/security/login");
                    return;
                }


                base.OnActionExecuting(filterContext);
            }
        }

        //public enum DemandMode
        //{
        //    Regular = 0,
        //    Cancel = 1
        //}
        //public enum PricingLevel
        //{
        //    Retailer = 0,
        //    Wholesale = 1
        //}
        //public enum Status
        //{
        //    Active = 0,
        //    Inactive = 1
        //}



        //public static string GetMaxRowID(string sSql)
        //{
        //    string VarNo = "";

        //    DataTable dt = DataManager.ExecuteQuery(sSql);
        //    if (dt.Rows.Count > 0)
        //    {
        //        VarNo = dt.Rows[0][0].ToString();

        //    }
        //    return VarNo;
        //}







        // Inventory Utiliti

        public static string GenerateItemCode(string CategoryCode, int ItemCodeLenth)
        {
            CategoryCode = CategoryCode.PadLeft(ItemCodeLenth, '0');
            return CategoryCode;
        }

        public static string GetMaxRowID(string sSql)
        {
            string VarNo = "";

            DataTable dt = DataManager.ExecuteQuery(sSql);
            if (dt.Rows.Count > 0)
            {
                VarNo = dt.Rows[0][0].ToString();

            }
            return VarNo;
        }

        public static string RetCostSQL(string sFlag)
        {
            string SQL;

            if (sFlag == "0")
            {
                SQL = "SELECT CosTCentreId, CosTCentre FROM CosTCentreTab";
            }

            else
            {
                SQL = "SELECT CosTCentreId, CosTCentre FROM CosTCentreTab Where CosTCentreId IN(" + sFlag + ") ";
            }

            return SQL;
        }

        //public Dictionary<string, object> GetErrorsFromModelState()
        //{


        //    var errors = new Dictionary<string, object>();
        //    foreach (var key in ModelState.Keys)
        //    {
        //        // Only send the errors to the client.
        //        if (ModelState[key].Errors.Count > 0)
        //        {
        //            errors[key] = ModelState[key].Errors;
        //        }
        //    }

        //    return errors;
        //}

        //public Dictionary<string, object> GetErrorListFromModelState
        //                                      (ModelStateDictionary modelState)
        //{
        //    // var query = new Dictionary<string, object>();
        //    //var query = from state in ModelState.Values
        //    //            from error in state.Errors
        //    //            select error.ErrorMessage;
        //    //var errors = query.ToArray();
        //    var query = new Dictionary<string, object>();
        //    query = from state in modelState.Values
        //            from error in state.Errors
        //            select new
        //            {
        //                error.ErrorMessage,
        //                error.Exception
        //            };

        //    var errorList = query.ToList();
        //    return errorList;
        //}

        public static IEnumerable Errors(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState.ToDictionary(kvp => kvp.Key,
                    kvp => kvp.Value.Errors
                                    .Select(e => e.ErrorMessage).ToArray())
                                    .Where(m => m.Value.Count() > 0);
            }
            return null;
        }
    }
}