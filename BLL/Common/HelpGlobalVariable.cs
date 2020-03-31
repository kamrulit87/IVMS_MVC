using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BLL.Common
{
    public class HelpGlobalVariable
    {
        public static string sUserId
        {
            get
            {
                return HttpContext.Current.Session["UserId"] == null ? string.Empty :  HttpContext.Current.Session["UserId"].ToString();
            }
            set { HttpContext.Current.Session["UserId"] = value; }
        }
        public static string sUserName
        {
            get
            {
                return HttpContext.Current.Session["UserName"] == null ?  string.Empty : HttpContext.Current.Session["UserName"].ToString();
            }
            set { HttpContext.Current.Session["UserName"] = value; }
        }
        public static string sMachineNumber
        {
            get
            {
                return HttpContext.Current.Session["MachineNo"] == null ? string.Empty : HttpContext.Current.Session["MachineNo"].ToString();
            }
            set { HttpContext.Current.Session["MachineNo"] = value; }
        }

        public static int sBranch
        {
            get
            {
                return HttpContext.Current.Session["Branch"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["Branch"].ToString());
            }
            set { HttpContext.Current.Session["Branch"] = value; }
        }

    } 
}
