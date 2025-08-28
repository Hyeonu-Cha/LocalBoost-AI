
using System.Threading.Tasks;

namespace LocalBoostAI.WorkerService.Services.Email;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string message);
}
