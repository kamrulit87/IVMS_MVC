//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL.db
{
    using System;
    using System.Collections.Generic;
    
    public partial class PasswordCore
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PasswordCore()
        {
            this.PasswordHistories = new HashSet<PasswordHistory>();
            this.UserTabs = new HashSet<UserTab>();
        }
    
        public int PasswordId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordH { get; set; }
        public byte PasswordMode { get; set; }
        public System.DateTime PassworDateLine { get; set; }
        public Nullable<int> BranchId { get; set; }
        public int EmployeeID { get; set; }
    
        public virtual Employee Employee { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PasswordHistory> PasswordHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserTab> UserTabs { get; set; }
    }
}
