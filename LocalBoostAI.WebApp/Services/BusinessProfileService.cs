
using LocalBoostAI.WebApp.Models;
using System.Text.Json;

namespace LocalBoostAI.WebApp.Services;

public interface IBusinessProfileService
{
    Task<BusinessProfile> GetProfileAsync();
    Task SaveProfileAsync(BusinessProfile profile);
}

public class InMemoryBusinessProfileService : IBusinessProfileService
{
    private BusinessProfile _profile;
    private const string ProfileFilePath = "..\\..\\..\\profile.json";

    public InMemoryBusinessProfileService()
    {
        if (File.Exists(ProfileFilePath))
        {
            var json = File.ReadAllText(ProfileFilePath);
            _profile = JsonSerializer.Deserialize<BusinessProfile>(json);
        }
    }

    public Task<BusinessProfile> GetProfileAsync()
    {
        return Task.FromResult(_profile);
    }

    public Task SaveProfileAsync(BusinessProfile profile)
    {
        _profile = profile;
        var json = JsonSerializer.Serialize(_profile);
        File.WriteAllText(ProfileFilePath, json);
        return Task.CompletedTask;
    }
}
