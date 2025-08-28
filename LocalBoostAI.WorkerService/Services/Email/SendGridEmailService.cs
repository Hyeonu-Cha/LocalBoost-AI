
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace LocalBoostAI.WorkerService.Services.Email;

public class SendGridEmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public SendGridEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var apiKey = _configuration["SendGrid:ApiKey"];
        var senderEmail = _configuration["SendGrid:SenderEmail"];

        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(senderEmail, "LocalBoost AI");
        var to = new EmailAddress(toEmail);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, "", message);
        var response = await client.SendEmailAsync(msg);
    }
}
