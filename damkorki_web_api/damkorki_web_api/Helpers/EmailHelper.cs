using System.Net.Mail;
using System.Net;
using System;

namespace DamkorkiWebApi.Helpers { 

    class EmailHelper { 

        public static bool Send(string address, string subject, string body) 
        { 
            try {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"); 
                smtpClient.UseDefaultCredentials = false; 
                smtpClient.Credentials = new NetworkCredential("damkorki.webapi@gmail.com", "PaSSword12!"); 
                smtpClient.EnableSsl = true; 
                
                MailMessage mailMessage = new MailMessage(); 
                mailMessage.From = new MailAddress("admin@damkorki.pl"); 
                mailMessage.To.Add(address);
                mailMessage.Body = body; 
                mailMessage.Subject = subject; 

                smtpClient.Send(mailMessage); 
                return true; 
            } catch(Exception e) { 
                return false; 
            }
        }
    }
    
}