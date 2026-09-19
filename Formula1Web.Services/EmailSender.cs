using MailKit.Net.Smtp;
using MimeKit;

namespace Formula1Web.Services;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _settings;

    public EmailSender(EmailSettings settings)
    {
        _settings = settings;
    }

    public async Task SendEmailAsync(
        string toEmail,
        string subject,
        string message)
    {
        var email = new MimeMessage();

        email.From.Add(
            new MailboxAddress(
                _settings.SenderName,
                _settings.SenderEmail));

        email.To.Add(
            MailboxAddress.Parse(toEmail));

        email.Subject = subject;

        email.Body = new TextPart("plain")
        {
            Text = message
        };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            _settings.SmtpServer,
            _settings.SmtpPort,
            MailKit.Security.SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(
            _settings.Username,
            _settings.Password);

        await smtp.SendAsync(email);

        await smtp.DisconnectAsync(true);
    }
}