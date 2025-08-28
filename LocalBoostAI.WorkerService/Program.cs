
using Google.Cloud.AIPlatform.V1;
using LocalBoostAI.WorkerService;
using LocalBoostAI.WorkerService.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton(new PredictionServiceClientBuilder
{
    Endpoint = $"{builder.Configuration["GoogleCloud:Location"]}-aiplatform.googleapis.com"
}.Build());

builder.Services.AddHttpClient(); // Register HttpClient

builder.Services.AddTransient<IContentGenerationService, ContentGenerationService>();
builder.Services.AddTransient<IEmailService, SendGridEmailService>();

var host = builder.Build();
host.Run();

