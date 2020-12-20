using Microsoft.AspNetCore.Hosting;
using SpotOnAccountServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SpotOnAccountServer.Services
{
    public class MailService
    {
        [Obsolete]
        private static IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public MailService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public static object ConfirmCode(string email, string code)
        {
            using (var _context = new DB_A5DE44_RussellContext())
            {
                var user = _context.Users.FirstOrDefault(c => c.EmailAddress.Equals(email));
                if (user.ConfirmationCode.Equals(code))
                {
                    return "valid";// users.ToList();
                }
                return "not valid";// users.ToList();
            }
        }

        public static object SendMail(string email)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;


            string fullPath = Path.Combine(webRootPath, "mailer.html");
            StreamReader reader = new StreamReader(fullPath);
            string readFile = reader.ReadToEnd();
            string myString = "";
            myString = readFile;

            RandomGenerator generator = new RandomGenerator();
            string str = generator.RandomString(10, false);

            myString = myString.Replace("$$Link$$", str);

            MailMessage m = new MailMessage();
            SmtpClient sc = new SmtpClient();
            m.From = new MailAddress("recovery@medicallnigeria.com");
            m.To.Add(email);
            m.Subject = "MediCall Password Recovery";
            m.Body = myString.ToString();
            sc.Host = "mail.medicallng.com";
            m.IsBodyHtml = true;
            try
            {
                sc.Port = 25;
                sc.Credentials = new System.Net.NetworkCredential("recovery@medicallng.com", "Rhechoverhy@1");
                sc.EnableSsl = false;
                sc.Send(m);

                using (var _context = new DB_A5DE44_RussellContext())
                {
                    var user = _context.Users.FirstOrDefault(c => c.EmailAddress.Equals(email));

                    if (user != null)
                    {
                        user.ConfirmationCode = str;

                        _context.Users.Update(user);
                        _context.SaveChanges();
                    }
                    var users = _context.Users.Select(c => new { code = "done" });
                    return users.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                using (var _context = new DB_A5DE44_RussellContext())
                {
                    var users = _context.Users.Select(c => new { code = "done" });
                    return users.FirstOrDefault();
                }
            }

        }


        public void SendSuccessMail(string email, string dateexp)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;


            string fullPath = Path.Combine(webRootPath, "thanks.html");
            StreamReader reader = new StreamReader(fullPath);
            string readFile = reader.ReadToEnd();
            string myString = "";
            myString = readFile;

            RandomGenerator generator = new RandomGenerator();

            myString = myString.Replace("$$Link$$", dateexp);

            MailMessage m = new MailMessage();
            SmtpClient sc = new SmtpClient();
            m.From = new MailAddress("payment@medicallnigeria.com");
            m.To.Add(email);
            m.Subject = "MediCall Subscription";
            m.Body = myString.ToString();
            sc.Host = "mail.medicallng.com";
            m.IsBodyHtml = true;
            try
            {
                sc.Port = 25;
                sc.Credentials = new System.Net.NetworkCredential("payment@medicallng.com", "Rexonery@1");
                sc.EnableSsl = false;
                sc.Send(m);


            }
            catch (Exception ex)
            {

            }

        }

    }
}
