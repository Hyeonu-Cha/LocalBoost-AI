
using Azure.AI.OpenAI;
using LocalBoostAI.WorkerService;
using LocalBoostAI.WorkerService.Services;
using Microsoft.Azure.CognitiveServices.Search.WebSearch;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton(new OpenAIClient(new Uri(builder.Configuration["AzureOpenAI:Endpoint"]), new Azure.AzureKeyCredential(builder.Configuration["AzureOpenAI:ApiKey"])));
builder.Services.AddSingleton<WebSearchClient>(new WebSearchClient(new ApiKeyServiceClientCredentials(builder.Configuration["BingSearch:ApiKey"])));

builder.Services.AddTransient<IContentGenerationService, ContentGenerationService>();

var host = builder.Build();
host.Run();

