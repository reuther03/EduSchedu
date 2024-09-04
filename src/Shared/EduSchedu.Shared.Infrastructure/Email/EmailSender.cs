using EduSchedu.Shared.Abstractions.Email;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace EduSchedu.Shared.Infrastructure.Email;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _options;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(
        IOptions<EmailSettings> options,
        ILogger<EmailSender> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task Send(EmailMessage request)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_options.Username, _options.FromAddress));
        message.To.Add(new MailboxAddress("", request.Email));
        message.Subject = request.Subject;
        message.Body = new TextPart("html") { Text = request.Body };

        try
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(_options.SmtpServer, _options.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_options.FromAddress, _options.Password);

            _logger.LogInformation("Sending email to {Mail}", request.Email);
            await client.SendAsync(message);
            _logger.LogInformation("Successfully sent email to {Mail}", request.Email);

            await client.DisconnectAsync(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error sending email to {Mail}", request.Email);
            throw;
        }
    }
}