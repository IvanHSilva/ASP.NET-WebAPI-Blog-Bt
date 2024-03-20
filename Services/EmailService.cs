using System.Net;
using System.Net.Mail;

namespace Blog.Services; 

public class EmailService {

    public bool Send(string toName, string toEmail, string subject, string body, 
        string fromName = "Ivan Henriques", string fromEmail = "ivanhenriquessilva@gmail.com"){

        SmtpClient smtpClient = new(Configuration.Smtp.Host, int.Parse(Configuration.Smtp.Port));
        smtpClient.Credentials = new NetworkCredential(Configuration.Smtp.UserName,
            Configuration.Smtp.Password);
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = true;

        MailMessage mail = new();
        mail.From = new MailAddress(fromEmail, fromName);
        mail.To.Add(new MailAddress(toEmail, toName));
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        try {
            smtpClient.Send(mail);
            return true;
        } catch {
            return false;
        }
    }
}
