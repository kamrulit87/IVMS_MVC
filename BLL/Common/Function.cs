using DAL.Common;
using DAL.db;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Text;
using DAL.Helper;

namespace BLL.Common
{
    public class Function
    {
        IVMS_DBEntities db = new IVMS_DBEntities();
        Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
        public static String GetValidationErrorsDtls(DbEntityValidationException ex)
        {
            StringBuilder sb = new StringBuilder("BOQ ", 1000);
            foreach (var eve in ex.EntityValidationErrors)
            {
                sb.AppendLine("Entity has the following validation Error => ");
                foreach (var ve in eve.ValidationErrors)
                {
                    sb.AppendLine(ve.PropertyName + " Error : " + ve.ErrorMessage);
                }
            }
            return sb.ToString();
        }

        //***************************** Start :: Common *****************************
        public bool CanView(string MenuCode)  
        {
            bool IsPermissionFlag = true;
            List<BLL.Common.HC_FormPermissionList> objFormPermissionList = HttpContext.Current.Session["FormPermissionList"] as List<BLL.Common.HC_FormPermissionList>;
            if (HttpContext.Current.Session["UserGroupHead"] !=null && HttpContext.Current.Session["UserGroupHead"].ToString() == "Administrator")
            {

            }
            else
            {
                if (objFormPermissionList != null && objFormPermissionList.Where(stringToCheck => stringToCheck.MenuCode.Contains(MenuCode)).FirstOrDefault() == null)
                {
                    IsPermissionFlag = false;
                }
            }
            return IsPermissionFlag;
        }

        public bool CanInsert(string sMenuCode)
        {
            bool IsActionFlag = true;
            List<BLL.Common.HC_FormPermissionList> objFormPermissionList = HttpContext.Current.Session["FormPermissionList"] as List<BLL.Common.HC_FormPermissionList>;
            if (HttpContext.Current.Session["UserGroupHead"] != null && HttpContext.Current.Session["UserGroupHead"].ToString() == "Administrator")
            {

            }
            else
            {
                DataTable dtPermission = new DataTable();
                string sql = "Select * From ObjecTPermission Where MenuCode = '" + sMenuCode + "' And UserGroupId = '" + HttpContext.Current.Session["UserGroupId"].ToString() + "'   ";
                dtPermission = DataManager.ExecuteQuery(sql);
                if (dtPermission.Rows.Count > 0)
                {
                    DataRow[] drs = dtPermission.Select("MenuCode='" + sMenuCode + "'");
                    if (drs.Length > 0)
                    {
                        if (drs[0]["PermissionNumber"].ToString().IndexOf("I") != -1)
                        {
                            return IsActionFlag = true;
                        }
                    }
                    return IsActionFlag = false;
                }
            }
            return IsActionFlag;
        } 
        public bool CanUpdate(string sMenuCode)
        {
            bool IsActionFlag = true;
            List<BLL.Common.HC_FormPermissionList> objFormPermissionList = HttpContext.Current.Session["FormPermissionList"] as List<BLL.Common.HC_FormPermissionList>;
            if (HttpContext.Current.Session["UserGroupHead"] != null && HttpContext.Current.Session["UserGroupHead"].ToString() == "Administrator")
            {

            }
            else
            {
                DataTable dtPermission = new DataTable();
                string sql = "Select * From ObjecTPermission Where MenuCode = '" + sMenuCode + "' And UserGroupId = '" + HttpContext.Current.Session["UserGroupId"].ToString() + "'   ";
                dtPermission = DataManager.ExecuteQuery(sql);
                if (dtPermission.Rows.Count > 0)
                {
                    DataRow[] drs = dtPermission.Select("MenuCode='" + sMenuCode + "'");
                    if (drs.Length > 0)
                    {
                        if (drs[0]["PermissionNumber"].ToString().IndexOf("U") != -1)
                        {
                            return IsActionFlag = true;
                        }
                    }
                    return IsActionFlag = false;
                }
            }
            return IsActionFlag;
        } 
        public bool CanDelete(string sMenuCode)
        {
            bool IsActionFlag = true;
            List<BLL.Common.HC_FormPermissionList> objFormPermissionList = HttpContext.Current.Session["FormPermissionList"] as List<BLL.Common.HC_FormPermissionList>;
            if (HttpContext.Current.Session["UserGroupHead"] != null && HttpContext.Current.Session["UserGroupHead"].ToString() == "Administrator")
            {

            }
            else
            {
                DataTable dtPermission = new DataTable();
                string sql = "Select * From ObjecTPermission Where MenuCode = '" + sMenuCode + "' And UserGroupId = '" + HttpContext.Current.Session["UserGroupId"].ToString() + "'   ";
                dtPermission = DataManager.ExecuteQuery(sql);
                if (dtPermission.Rows.Count > 0)
                {
                    DataRow[] drs = dtPermission.Select("MenuCode='" + sMenuCode + "'");
                    if (drs.Length > 0)
                    {
                        if (drs[0]["PermissionNumber"].ToString().IndexOf("D") != -1)
                        {
                            return IsActionFlag = true;
                        }
                    }
                    return IsActionFlag = false;
                }
            }
            return IsActionFlag;
        }

             
        public enum Status
        {
            Active = 0,
            Inactive = 1
        }
        public static string GetPrevDate(string sDate)
        {
            string sSql = "Select dbo.FxDateStr(DateAdd(Month,-1,'" + sDate + "')) ";
            string VarNo = "";
            DataTable dt = DataManager.ExecuteQuery(sSql);
            if (dt.Rows.Count > 0)
            {
                VarNo = dt.Rows[0][0].ToString();

            }
            return VarNo;
        } 
        public static string getServerDateTime()
        {
            string sql = "Select Getdate()";
            DataTable dt = new DataTable();
            dt = DataManager.ExecuteQuery(sql);
            return dt.Rows[0][0].ToString();

        } 
        public static string GetMonthNameFromMonthNo(int MonNo)
        {
            string Tm = "";

            switch (MonNo)
            {
                case 1:
                    Tm = "Jan"; break;
                case 2:
                    Tm = "Feb"; break;
                case 3:
                    Tm = "Mar"; break;
                case 4:
                    Tm = "Apr"; break;
                case 5:
                    Tm = "May"; break;
                case 6:
                    Tm = "Jun"; break;
                case 7:
                    Tm = "Jul"; break;
                case 8:
                    Tm = "Aug"; break;
                case 9:
                    Tm = "Sep"; break;
                case 10:
                    Tm = "Oct"; break;
                case 11:
                    Tm = "Nov"; break;
                case 12:
                    Tm = "Dec"; break;
                default:
                    Tm = "";
                    break;
            }
            return Tm;
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
        public static string FormatDatedd_MMM_yyyy(DateTime Dt)
        {
            string BalMonth = "";
            string YearLength = Dt.Year.ToString();
            string MonthLength = GetMonthNameFromMonthNo(Dt.Month);
            if (Dt.Day.ToString().Length == 1)
            {
                BalMonth = "0" + Dt.Day.ToString() + "-" + MonthLength + "-" + YearLength;
            }
            else
                BalMonth = Dt.Day.ToString() + "-" + MonthLength + "-" + YearLength;
            {

            }
            return BalMonth;
        }

        //****************************** End :: Common ******************************

        //------------------------ Start :: Create DataTable ------------------------
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        //------------------------- End :: Create DataTable -------------------------


        //-------------------- Start :: Model Validation Function -------------------
        public static Dictionary<string, object> GetErrorsFromModelState(ModelStateDictionary modelState)
        {
            var errors = new Dictionary<string, object>();
            foreach (var key in modelState.Keys)
            {
                // Only send the errors to the client.
                if (modelState[key].Errors.Count > 0)
                {
                    errors[key] = modelState[key].Errors;
                }
            }

            return errors;
        }
        public static IList<SelectListItem> GetEnumSelectList(Type type)
        {
            var enums = new List<SelectListItem>();
            foreach (int value in Enum.GetValues(type))
            {
                var item = new SelectListItem();
                item.Value = value.ToString();
                item.Text = Enum.GetName(type, value);
                enums.Add(item);
            }
            return enums;
        }

        public static IEnumerable FillModelStateError(ModelStateDictionary modelState)
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

        #region Cookies Get Set
        public void SetCookies()
        {
            //string url = HttpContext.Current.Request.Url.AbsolutePath;
            //url = HttpContext.Current.Request.Url.AbsoluteUri;
            string url = HttpContext.Current.Request.Url.ToString();
            if (HttpContext.Current.Response.Cookies["latestVisit"] == null)
            {
                HttpCookie myCookie = new HttpCookie("latestVisit");
                myCookie.Expires = DateTime.Now.AddHours(2); // Last visit page keept in cookies till 2 hours.
                myCookie.Values["url"] = System.Web.HttpUtility.UrlEncode(url);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
            else
            {
                var myCookie = HttpContext.Current.Request.Cookies["latestVisit"];
                var cookieCollection = myCookie.Values;
                string[] CookieTitles = cookieCollection.AllKeys;

                //mj-y: If the url is reapeated, move it to end(means make it newer by removing it and adding it again)
                string cookieURL = "";
                foreach (string cookTit in CookieTitles)
                {
                    cookieURL = System.Web.HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies["latestVisit"].Values[cookTit]);
                    if (cookieURL == url)
                    {
                        cookieCollection.Remove(cookTit);
                        cookieCollection.Set("url", System.Web.HttpUtility.UrlEncode(url));
                        HttpContext.Current.Response.SetCookie(myCookie);
                        return;
                    }
                }
                //mj-y: If it was not repeated ...
                cookieCollection.Set("url", System.Web.HttpUtility.UrlEncode(url));
                if (cookieCollection.Count > 15) // store just 15 item         
                    cookieCollection.Remove(CookieTitles[0]);
                HttpContext.Current.Response.SetCookie(myCookie);
            }
        }

        public string GetCookies()
        {
            string url = string.Empty;
            if (HttpContext.Current.Response.Cookies["latestVisit"] != null)
            {
                var myCookie = HttpContext.Current.Request.Cookies["latestVisit"];
                url = System.Web.HttpUtility.UrlDecode(myCookie.Values["url"]);
            }
            return url;
        }
        #endregion

        #region Reports
        public string CallReports(string DocType, List<ReportParameter> reportParameters, bool isPortrait, string reportPath, DataTable dataTable, IEnumerable<dynamic> dataObject, string reportDataSetName)
        {
            int companyID = Convert.ToInt32(dictionary[9].Id == "" ? 0 : Convert.ToInt32(dictionary[9].Id));
            int branchID = Convert.ToInt32(dictionary[10].Id == "" ? 0 : Convert.ToInt32(dictionary[10].Id));

            string errorMessage = string.Empty;
            string fileString = string.Empty;
            try
            {
                LocalReport lr = new LocalReport();
                ReportDataSource rd = new ReportDataSource();
                string path = string.Empty;
                if (dataTable != null && dataTable.Rows.Count > 0 || dataObject != null && dataObject.Count() > 0)
                {
                    path = reportPath;
                }
                else
                {
                    if (isPortrait)
                    {
                        path = Path.Combine(HttpContext.Current.Server.MapPath("~/Areas/Reports/RDLC"), "BlankPortrait.rdlc");
                    }
                    else
                    {
                        path = Path.Combine(HttpContext.Current.Server.MapPath("~/Areas/Reports/RDLC"), "BlankLandscape.rdlc");
                    }
                }

                if (System.IO.File.Exists(path))
                {
                    lr.EnableExternalImages = true;
                    lr.ReportPath = path;
                    //Set Default Report Parameter
                    if (dataTable != null && dataTable.Rows.Count > 0 || dataObject != null && dataObject.Count() > 0)
                    {
                        var orgList = from c in db.SET_Company
                                      join cb in db.SET_CompanyBranch on c.CompanyID equals cb.CompanyID
                                      where cb.CompanyID == companyID && cb.BranchID == branchID
                                      select new
                                      {
                                          CompanyName = c.Name,
                                          BranchName = cb.Name
                                      };
                        var org = orgList.FirstOrDefault();
                        if (org != null)
                        {
                            reportParameters.Add(new ReportParameter("companyName", org.CompanyName.ToUpper()));
                            reportParameters.Add(new ReportParameter("branchName", org.BranchName.ToUpper()));
                        }

                        // checking declared report parameters
                        ReportParameterInfoCollection ps;
                        ps = lr.GetParameters();
                        ReportParameter paramV = new ReportParameter();
                        foreach (ReportParameterInfo p in ps)
                        {
                            if (p.Name == "logo")
                            {
                                string logoPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/user_define/logo"), "r_logo.jpg");
                                if (System.IO.File.Exists(logoPath))
                                {
                                    reportParameters.Add(new ReportParameter("logo", "File:\\" + logoPath, true));
                                }
                            }
                        }


                        lr.SetParameters(reportParameters);
                    }
                }
                else
                {
                    errorMessage = "File Not Exists.";
                }
                if (dataTable != null)
                {
                    rd = new ReportDataSource(reportDataSetName, dataTable);
                }
                else if (dataObject != null)
                {
                    rd = new ReportDataSource(reportDataSetName, dataObject);
                }

                lr.DataSources.Add(rd);
                // Convert Report To Base64String
                string reportType = DocType;
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + DocType + "</OutputFormat>" +
                "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                fileString = Convert.ToBase64String(renderedBytes.ToArray());
                return fileString;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return errorMessage;
            }
        }

        public string CallReportsMultipleDS(string DocType, List<ReportParameter> reportParameters, bool isPortrait, string reportPath, IEnumerable<dynamic> dataObject1, IEnumerable<dynamic> dataObject2, string reportDS1, string reportDS2)
        {
            string errorMessage = string.Empty;
            string fileString = string.Empty;
            try
            {
                LocalReport lr = new LocalReport();
                ReportDataSource rd = new ReportDataSource();
                lr.EnableExternalImages = true;
                string path = string.Empty;
                if (dataObject1 != null && dataObject1.Count() > 0 || dataObject2 != null && dataObject2.Count() > 0)
                {
                    path = reportPath;
                }
                else
                {
                    if (isPortrait)
                    {
                        path = Path.Combine(HttpContext.Current.Server.MapPath("~/Areas/Reports/RDLC"), "BlankPortrait.rdlc");
                    }
                    else
                    {
                        path = Path.Combine(HttpContext.Current.Server.MapPath("~/Areas/Reports/RDLC"), "BlankLandscape.rdlc");
                    }
                }

                if (System.IO.File.Exists(path))
                {
                    lr.ReportPath = path;
                    //Set Default Report Parameter
                    if (dataObject1 != null && dataObject1.Count() > 0 || dataObject2 != null && dataObject2.Count() > 0)
                    {
                        var company = db.SET_Company.FirstOrDefault();
                        if (company != null)
                        {
                            string contactInfo = "Phone : " + company.ContactNo + " Email : " + company.EmailID;
                            reportParameters.Add(new ReportParameter("orgName", company.Name));
                            reportParameters.Add(new ReportParameter("orgAddress", company.Address));
                            reportParameters.Add(new ReportParameter("orgContactInfo", contactInfo));
                        }

                        // checking declared report parameters
                        ReportParameterInfoCollection ps;
                        ps = lr.GetParameters();
                        ReportParameter paramV = new ReportParameter();
                        foreach (ReportParameterInfo p in ps)
                        {
                            if (p.Name == "logo")
                            {
                                string logoPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/user_define/logo"), "r_logo.png");
                                if (System.IO.File.Exists(logoPath))
                                {
                                    reportParameters.Add(new ReportParameter("logo", "File:\\" + logoPath, true));
                                }
                            }
                        }

                        lr.SetParameters(reportParameters);
                    }
                }
                else
                {
                    errorMessage = "File Not Exists.";
                }
                //if (dataObjectAsset != null)
                //{
                //    rd = new ReportDataSource(reportDSAsset, dataObjectAsset);                   
                //}
                //if (dataObjectLiabilities != null)
                //{
                //    rd = new ReportDataSource(reportDSLiabilites, dataObjectLiabilities);
                //}
                lr.DataSources.Clear();
                ReportDataSource rd1 = new ReportDataSource(reportDS1, dataObject1);
                ReportDataSource rd2 = new ReportDataSource(reportDS2, dataObject2);
                lr.DataSources.Add(rd1);
                lr.DataSources.Add(rd2);

                // Convert Report To Base64String
                string reportType = DocType;
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + DocType + "</OutputFormat>" +
                "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                fileString = Convert.ToBase64String(renderedBytes.ToArray());
                return fileString;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return errorMessage;
            }
        }
        #endregion

       
    }
}
        
    
