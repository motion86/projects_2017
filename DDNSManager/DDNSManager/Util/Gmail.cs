using Google.Apis.Gmail.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DDNSManager.Util
{
    public class Gmail
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FromAddress { get; set; }

        public Gmail(string username, string password, string fromAddress)
        {
            UserName = username;
            Password = password;
            FromAddress = fromAddress;
        }

        public Gmail()
        {
            UserName = "versacalltest@gmail.com";
            Password = "test!123";
            FromAddress = UserName;
        }

        public bool SendMail(string sendTo, string subject, string body)
        {
            try
            {
                using (var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(FromAddress, Password),
                    EnableSsl = true
                })
                {
                    client.Send(FromAddress, sendTo, subject, body);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// Delete a Message.
        /// </summary>
        /// <param name="service">Gmail API service instance.</param>
        /// <param name="userId">User's email address. The special value "me"
        /// can be used to indicate the authenticated user.</param>
        /// <param name="messageId">ID of the Message to delete.</param>
        public static void DeleteMessage(GmailService service, String userId, String messageId)
        {
            try
            {
                service.Users.Messages.Delete(userId, messageId).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
        }


    }
}
