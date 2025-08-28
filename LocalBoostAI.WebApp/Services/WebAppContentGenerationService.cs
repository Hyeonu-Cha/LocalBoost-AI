
using System.Threading.Tasks;

namespace LocalBoostAI.WebApp.Services;

public class WebAppContentGenerationService : IContentGenerationService
{
    public Task RegenerateContentAsync()
    {
        // This is a placeholder. In a real application, this would trigger the
        // content generation process in the worker service.
        return Task.CompletedTask;
    }
}
