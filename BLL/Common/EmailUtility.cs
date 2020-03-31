using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DAL.db;

namespace BLL.Common
{
    public class EmailUtility
    {
        public bool SentMail(string toEmail, string companyName, string visitorName, string subject, string body)
        {
            string result = string.Empty;
            using (IVMS_DBEntities db = new IVMS_DBEntities())
            {
                var emailConfig = db.EmailConfigs.ToList().LastOrDefault();
                try
                {
                    SmtpClient client = new SmtpClient();
                    client.Timeout = 30000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(emailConfig.FromMailID, emailConfig.Pass);
                    client.Port = emailConfig.Port;
                    client.Host = emailConfig.Host;
                    client.EnableSsl = true;

                    string fromMail = emailConfig.FromMailID;
                    if (subject == string.Empty)
                    {
                        subject = "Visitor Waiting Notification";
                    }

                    if (body == string.Empty)
                    {
                        body = visitorName.Trim() + " " + "From" + " " + companyName.Trim() + " " + "wants to meet with you.";

                    }
                    MailMessage mail = new MailMessage(fromMail, toEmail, subject, body);
                    mail.BodyEncoding = UTF8Encoding.UTF8;
                    mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                    client.Send(mail);
                    return true;
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                    return false;
                }
            }

        }
        public bool IsInternetConnected()
        {
            string message = string.Empty;
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                //MessageBox.Show(" No Internet Connection " + DateTime.Now.ToShortTimeString());
                return false;
            }
        }
    }
}
