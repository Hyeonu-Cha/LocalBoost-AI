
using Azure.AI.OpenAI;
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
    private readonly OpenAIClient _openAIClient;
    private readonly WebSearchClient _webSearchClient;
    private readonly ILogger<ContentGenerationService> _logger;
    private readonly IConfiguration _configuration;
    private const string ProfileFilePath = "..\\..\\..\\profile.json";
    private const string ContentFilePath = "..\\..\\..\\content.json";

    public ContentGenerationService(OpenAIClient openAIClient, WebSearchClient webSearchClient, ILogger<ContentGenerationService> logger, IConfiguration configuration)
    {
        _openAIClient = openAIClient;
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
        var deploymentName = _configuration["AzureOpenAI:DeploymentName"];
        var completionsOptions = new CompletionsOptions() { MaxTokens = 200 };
        var response = await _openAIClient.GetCompletionsAsync(deploymentName, topicsPrompt);
        var topics = response.Value.Choices.First().Text.Split('\n').Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();
        var selectedTopic = topics.First();

        // 2. Search for local news
        var newsResult = await _webSearchClient.Web.SearchAsync(query: $"local news in {profile.TargetLocation}");
        var localNews = newsResult.News?.Value?.FirstOrDefault()?.Description;

        // 3. Generate blog post
        var blogPostPrompt = $"Write a 300-400 word blog post about '{selectedTopic}' for a {profile.BusinessCategory} called {profile.BusinessName} in {profile.TargetLocation}. Incorporate the following local news if relevant: {localNews}. The tone should be {profile.ToneOfVoice}.";
        completionsOptions.MaxTokens = 500;
        var blogPostResponse = await _openAIClient.GetCompletionsAsync(deploymentName, blogPostPrompt);
        var blogPost = blogPostResponse.Value.Choices.First().Text;

        // 4. Generate social media posts
        var socialMediaPrompt = $"Generate 3-4 social media posts for Facebook and Instagram based on the following blog post: {blogPost}. The tone should be {profile.ToneOfVoice}. Include relevant hashtags.";
        completionsOptions.MaxTokens = 300;
        var socialMediaResponse = await _openAIClient.GetCompletionsAsync(deploymentName, socialMediaPrompt);
        var socialMediaPosts = socialMediaResponse.Value.Choices.First().Text.Split('\n').Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();

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
}
