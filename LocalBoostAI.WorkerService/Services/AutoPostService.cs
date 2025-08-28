
using LocalBoostAI.WorkerService.Models;
using LocalBoostAI.WorkerService.Services.Email;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LocalBoostAI.WorkerService.Services;

public class AutoPostService : IAutoPostService
{
    private readonly ILogger<AutoPostService> _logger;
    private readonly IEmailService _emailService;

    public AutoPostService(ILogger<AutoPostService> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public async Task AutoPostContentAsync(GeneratedContent content, BusinessProfile profile)
    {
        _logger.LogInformation("Attempting to auto-post content.");

        bool success = true;
        string errorMessage = string.Empty;

        try
        {
            _logger.LogInformation("Posting at optimal time (immediately after approval).");

            if (profile.IsFacebookConnected)
            {
                _logger.LogInformation("Posting to Facebook...");
                // TODO: Implement actual Facebook posting logic
            }

            if (profile.IsInstagramConnected)
            {
                _logger.LogInformation("Posting to Instagram...");
                // TODO: Implement actual Instagram posting logic
            }

            if (profile.IsGoogleBusinessProfileConnected)
            {
                _logger.LogInformation("Posting to Google Business Profile...");
                // TODO: Implement actual Google Business Profile posting logic
            }
        }
        catch (Exception ex)
        {
            success = false;
            errorMessage = ex.Message;
            _logger.LogError(ex, "Error during auto-post attempt.");
        }

        if (success)
        {
            _logger.LogInformation("Auto-post attempt finished successfully.");
            await _emailService.SendEmailAsync("test@example.com", "Content Posted Successfully!", "Your content has been successfully posted to your connected social media accounts.");
        }
        else
        {
            _logger.LogError($"Auto-post attempt failed: {errorMessage}");
            await _emailService.SendEmailAsync("test@example.com", "Content Posting Failed!", $"Your content could not be posted to your connected social media accounts. Error: {errorMessage}");
        }
    }
}
