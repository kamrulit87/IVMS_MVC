using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class LogInStatus
    {
        public bool IsAllowed { get; set; }
        public string Message { get; set; }

        public Dictionary<string, string> Status = new Dictionary<string, string>();
    }

    public class LogOnModel
    {
        [Required]
        [Display(Name = "Company ID")]
        public int CompanyID { get; set; }

        [Required]
        [Display(Name = "Branch ID")]
        public int BranchID { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
