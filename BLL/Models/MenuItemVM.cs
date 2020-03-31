
namespace BLL.Models
{
    public class MenuItemVM
    {
        public int MenuId { get; set; }
        public string ApplicationId { get; set; }
        public int ModuleId { get; set; }
        public int PageId { get; set; }
        public string ModuleName { get; set; }
        public string URL { get; set; }
        public string IconClass { get; set; }
        public bool? Select { get; set; }
        public bool? Edit { get; set; }
        public bool? Create { get; set; }
        public bool? Delete { get; set; }
        public string Name { get; set; }
        public int ID { get; set; }
        public bool ShowHide { get; set; }
    }
}
