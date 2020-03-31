using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BLL.Common
{
    public class HC_FormPermissionList
    {
        public string PermissionNumber { get; set; }
        public string ObjecTHead { get; set; }
        public string MenuHead { get; set; }
        public string MenuCode { get; set; }
        public string UserGroupHead { get; set; }
    }
    
    public class HC_FormList
    { 
        public int? ObjecTabId { get; set; }
        public string ObjecTHead { get; set; }
        public string MenuHead { get; set; }
        public string MenuCode { get; set; }
        public string SL { get; set; }
        public int ObjecTPermissionId { get; set; }
        public string PermissionNumber { get; set; } 
    }
    public class HC_Grap
    {  
        public string label = null;
        public Nullable<double> y = null; 
        public Nullable<double> x = null; 
        public Nullable<double> z = null;
    }
}