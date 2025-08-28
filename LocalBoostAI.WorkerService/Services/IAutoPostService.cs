
using LocalBoostAI.WorkerService.Models;
using System.Threading.Tasks;

namespace LocalBoostAI.WorkerService.Services;

public interface IAutoPostService
{
    Task AutoPostContentAsync(GeneratedContent content, BusinessProfile profile);
}
