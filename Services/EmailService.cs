using System.Net;
using System.Net.Mail;

namespace Blog.Services;

public class EmailService
{

    public bool Send(string toName, string toEmail, string subject, string body,
        string fromName = "Ivan Henriques",
        string fromEmail = "ivanhenriquessilva@gmail.com")
    {

        SmtpClient smtpClient = new(Configuration.Smtp.Host,
        int.Parse(Configuration.Smtp.Port))
        {
            Credentials = new NetworkCredential(Configuration.Smtp.UserName,
            Configuration.Smtp.Password),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            EnableSsl = true
        };


        MailMessage mail = new()
        {
            From = new MailAddress(fromEmail, fromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mail.To.Add(new MailAddress(toEmail, toName));

        try
        {
            smtpClient.Send(mail);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
