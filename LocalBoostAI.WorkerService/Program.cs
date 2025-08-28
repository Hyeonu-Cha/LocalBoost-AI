
using Google.Cloud.AIPlatform.V1;
using LocalBoostAI.WorkerService;
using LocalBoostAI.WorkerService.Services;
using Microsoft.Azure.CognitiveServices.Search.WebSearch;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton(new PredictionServiceClientBuilder
{
    Endpoint = $"{builder.Configuration["GoogleCloud:Location"]}-aiplatform.googleapis.com"
}.Build());

builder.Services.AddSingleton<WebSearchClient>(new WebSearchClient(new ApiKeyServiceClientCredentials(builder.Configuration["BingSearch:ApiKey"])));

builder.Services.AddTransient<IContentGenerationService, ContentGenerationService>();

var host = builder.Build();
host.Run();

