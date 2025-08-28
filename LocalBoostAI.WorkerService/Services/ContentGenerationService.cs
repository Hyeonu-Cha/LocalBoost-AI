
using Google.Cloud.AIPlatform.V1;
using Google.Protobuf.WellKnownTypes;
using LocalBoostAI.WorkerService.Models;
using Microsoft.Azure.CognitiveServices.Search.WebSearch;
using Microsoft.Azure.CognitiveServices.Search.WebSearch.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LocalBoostAI.WorkerService.Services;

public class ContentGenerationService : IContentGenerationService
{
    private readonly PredictionServiceClient _predictionServiceClient;
    private readonly WebSearchClient _webSearchClient;
    private readonly ILogger<ContentGenerationService> _logger;
    private readonly IConfiguration _configuration;
    private const string ProfileFilePath = "..\\..\\..\\profile.json";
    private const string ContentFilePath = "..\\..\\..\\content.json";

    public ContentGenerationService(PredictionServiceClient predictionServiceClient, WebSearchClient webSearchClient, ILogger<ContentGenerationService> logger, IConfiguration configuration)
    {
        _predictionServiceClient = predictionServiceClient;
        _webSearchClient = webSearchClient;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task GenerateContentAsync()
    {
        _logger.LogInformation("Content generation started.");

        if (!File.Exists(ProfileFilePath))
        {
            _logger.LogWarning("Profile file not found. Skipping content generation.");
            return;
        }

        var json = await File.ReadAllTextAsync(ProfileFilePath);
        var profile = JsonSerializer.Deserialize<BusinessProfile>(json);

        // 1. Brainstorm topics
        var topicsPrompt = $"Brainstorm a list of 5 blog post topics for a {profile.BusinessCategory} in {profile.TargetLocation}. The tone should be {profile.ToneOfVoice}. Each topic should be on a new line.";
        var topicsResponse = await GenerateTextAsync(topicsPrompt);
        var topics = topicsResponse.Split('\n').Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();
        var selectedTopic = topics.First();

        // 2. Search for local news
        var newsResult = await _webSearchClient.Web.SearchAsync(query: $"local news in {profile.TargetLocation}");
        var localNews = newsResult.News?.Value?.FirstOrDefault()?.Description;

        // 3. Generate blog post
        var blogPostPrompt = $"Write a 300-400 word blog post about '{selectedTopic}' for a {profile.BusinessCategory} called {profile.BusinessName} in {profile.TargetLocation}. Incorporate the following local news if relevant: {localNews}. The tone should be {profile.ToneOfVoice}.";
        var blogPost = await GenerateTextAsync(blogPostPrompt);

        // 4. Generate social media posts
        var socialMediaPrompt = $"Generate 3-4 social media posts for Facebook and Instagram based on the following blog post: {blogPost}. The tone should be {profile.ToneOfVoice}. Include relevant hashtags.";
        var socialMediaPostsResponse = await GenerateTextAsync(socialMediaPrompt);
        var socialMediaPosts = socialMediaPostsResponse.Split('\n').Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();

        // 5. Save the generated content
        var generatedContent = new GeneratedContent
        {
            BlogPost = blogPost,
            SocialMediaPosts = socialMediaPosts
        };

        var contentJson = JsonSerializer.Serialize(generatedContent);
        await File.WriteAllTextAsync(ContentFilePath, contentJson);

        _logger.LogInformation("Content generation finished.");
    }

    private async Task<string> GenerateTextAsync(string prompt)
    {
        var endpoint = EndpointName.FromProjectLocationModel(_configuration["GoogleCloud:ProjectId"], _configuration["GoogleCloud:Location"], _configuration["GoogleCloud:ModelId"]);
        var instances = new List<Value> { Value.ForString(prompt) };
        var parameters = Value.ForStruct(new Struct());

        var response = await _predictionServiceClient.PredictAsync(endpoint, instances, parameters);
        return response.Predictions.First().StringValue;
    }
}
