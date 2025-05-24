using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string smtpHost = "smtp.gmail.com";
    private readonly int smtpPort = 587;
    private readonly string smtpUser = "testlicenta063@gmail.com";    // put your email here
    private readonly string smtpPass = "cyfjcbpxvwolrazn";       // put your app password here

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MailMessage
        {
            From = new MailAddress(smtpUser),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        message.To.Add(new MailAddress(toEmail));

        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUser, smtpPass),
            EnableSsl = true,
            UseDefaultCredentials = false
        };

        await client.SendMailAsync(message);
    }
}
