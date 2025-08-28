using LocalBoostAI.WorkerService.Services;

namespace LocalBoostAI.WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IContentGenerationService _contentGenerationService;

    public Worker(ILogger<Worker> logger, IContentGenerationService contentGenerationService)
    {
        _logger = logger;
        _contentGenerationService = contentGenerationService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await _contentGenerationService.GenerateContentAsync();

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Run once a day
        }
    }
}
