using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class PagePermission
    {
        public int UserGroupId { get; set; }
        public int ID { get; set; }
        public bool Select { get; set; }
        public bool Edit { get; set; }
        public bool Create { get; set; }
        public bool Delete { get; set; }

        public string Application { get; set; }
        public string Page { get; set; }
        public string Module { get; set; }
        public int ApplicationID { get; set; }
        public int ModuleID { get; set; }
        public int UIPageID { get; set; }
        public int UIPage { get; set; }
        public int UIPageURL { get; set; }
        public bool ForcedLogOut { get; set; } 
    }
}
