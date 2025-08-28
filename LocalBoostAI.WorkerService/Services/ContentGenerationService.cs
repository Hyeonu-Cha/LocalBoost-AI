
using Google.Cloud.AIPlatform.V1;
using Google.Protobuf.WellKnownTypes;
using LocalBoostAI.WorkerService.Models;
using LocalBoostAI.WorkerService.Services.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace LocalBoostAI.WorkerService.Services;

public class ContentGenerationService : IContentGenerationService
{
    private readonly PredictionServiceClient _predictionServiceClient;
    private readonly HttpClient _httpClient;
    private readonly IEmailService _emailService;
    private readonly ILogger<ContentGenerationService> _logger;
    private readonly IConfiguration _configuration;
    private const string ProfileFilePath = "..\\..\\..\\profile.json";
    private const string ContentFilePath = "..\\..\\..\\content.json";

    public ContentGenerationService(PredictionServiceClient predictionServiceClient, HttpClient httpClient, IEmailService emailService, ILogger<ContentGenerationService> logger, IConfiguration configuration)
    {
        _predictionServiceClient = predictionServiceClient;
        _httpClient = httpClient;
        _emailService = emailService;
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

        // 2. Search for local news using Google Custom Search JSON API
        var apiKey = _configuration["GoogleCustomSearch:ApiKey"];
        var searchEngineId = _configuration["GoogleCustomSearch:SearchEngineId"];
        var searchQuery = $"local news in {profile.TargetLocation}";
        var searchUrl = $"https://www.googleapis.com/customsearch/v1?key={apiKey}&cx={searchEngineId}&q={searchQuery}";

        string localNews = null;
        try
        {
            var searchResponse = await _httpClient.GetStringAsync(searchUrl);
            using (JsonDocument doc = JsonDocument.Parse(searchResponse))
            {
                if (doc.RootElement.TryGetProperty("items", out JsonElement itemsElement) && itemsElement.EnumerateArray().Any())
                {
                    localNews = itemsElement.EnumerateArray().First().GetProperty("snippet").GetString();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching for local news.");
        }

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
            SocialMediaPosts = socialMediaPosts,
            GenerationDate = DateTime.UtcNow
        };

        var contentJson = JsonSerializer.Serialize(generatedContent);
        await File.WriteAllTextAsync(ContentFilePath, contentJson);

        // 6. Send email notification
        var toEmail = "test@example.com"; // Hardcoded for now
        var subject = "New Content Ready for Review!";
        var webAppBaseUrl = "https://localhost:5001"; // Hardcoded for now
        var message = $"Your new weekly content is ready for review. Please visit {webAppBaseUrl}/content-review to review and approve it.";
        await _emailService.SendEmailAsync(toEmail, subject, message);

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
