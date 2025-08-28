
using System.Threading.Tasks;

namespace LocalBoostAI.WebApp.Services.SocialMedia;

public interface IFacebookService
{
    Task<string> GetAuthorizationUrl();
    Task<string> GetAccessToken(string code);
}
