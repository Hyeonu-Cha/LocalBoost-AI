using LocalBoostAI.WorkerService.Models;
using LocalBoostAI.WorkerService.Services;
using LocalBoostAI.WorkerService.Services.Email;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace LocalBoostAI.WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IContentGenerationService _contentGenerationService;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private const string ContentFilePath = "..\\..\\..\\content.json";

    public Worker(ILogger<Worker> logger, IContentGenerationService contentGenerationService, IEmailService emailService, IConfiguration configuration)
    {
        _logger = logger;
        _contentGenerationService = contentGenerationService;
        _emailService = emailService;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            // Generate new content
            await _contentGenerationService.GenerateContentAsync();

            // Check for unapproved content and send reminder
            if (File.Exists(ContentFilePath))
            {
                var json = await File.ReadAllTextAsync(ContentFilePath);
                var generatedContent = JsonSerializer.Deserialize<GeneratedContent>(json);

                if (generatedContent != null && (DateTime.UtcNow - generatedContent.GenerationDate).TotalDays >= 7)
                {
                    var toEmail = _configuration["SendGrid:SenderEmail"]; // Assuming sender email is also recipient for reminders
                    var subject = "Reminder: Content Awaiting Approval!";
                    var webAppBaseUrl = "https://localhost:5001"; // Hardcoded for now
                    var message = $"Your content generated on {generatedContent.GenerationDate.ToShortDateString()} is still awaiting your approval. Please visit {webAppBaseUrl}/content-review to review and approve it.";
                    await _emailService.SendEmailAsync(toEmail, subject, message);
                    _logger.LogInformation("Sent reminder email for unapproved content.");
                }
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Run once a day
        }
    }
}
