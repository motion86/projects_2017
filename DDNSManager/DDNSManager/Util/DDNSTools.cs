using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DDNSManager.Util
{
    public class DDNSTools
    {
        public string ComEmail { get; set; }
        public string CellNumber { get; set; }
        public string MachineName { get; set; }
        public string Pwd { get; set; }
        public string IP { get; set; }


        public static string GetPublicIP()
        {
            string ip;
            try
            {
                using (var client = new HttpClient())
                {
                    ip = client.GetStringAsync(new Uri("http://ipinfo.io/ip")).Result;
                }
            }
            catch (Exception ex)
            {
                ip = "";
            }
            return ip;
        }

        public bool UpdateUser()
        {
            var ip = GetPublicIP();
            if(!ip.Equals(IP))
            {
                var gmail = new Gmail(ComEmail, FileIO.UnscrambleString(Pwd), ComEmail);
                // send sms to user to inform of new ip.
                var status = gmail.SendMail($"{CellNumber}@tmomail.net", "IP Update", $"IP for machine {MachineName} is now {ip}");
                // save new ip.
                IP = ip;

                this.WriteObjectToFile(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "info.dat"));

                if (status)
                    return true;
            }
            return false;
        }

        

        //public bool UpdatePublicIP(string ip)
        //{
        //    string username = Username, pass = Password;

        //    string updateString = $"https://www.dnsdynamic.org/api/?hostname=motion86.x64.me&myip={ip}".Replace("\n","");
        //    var byteArray = Encoding.ASCII.GetBytes($"{username}:{pass}");

        //    string response;
        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        //        response = client.GetStringAsync(new Uri(updateString)).Result;
        //    }
        //    if (response.Contains("good"))
        //        return true;
        //    return false;
        //}
    }
}
