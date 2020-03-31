namespace Anko.Models
{
    public class ChangePasswordModel
    {
        public string Answre { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
    }
}