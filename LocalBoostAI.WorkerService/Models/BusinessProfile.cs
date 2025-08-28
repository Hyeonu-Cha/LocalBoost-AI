
using System;

namespace LocalBoostAI.WebApp.Models;

public class BusinessProfile
{
    public Guid Id { get; set; }
    public string BusinessName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string BusinessDescription { get; set; }
    public string BusinessCategory { get; set; }
    public string TargetLocation { get; set; }
    public string ToneOfVoice { get; set; }

    // Social Media Integration Placeholders
    public bool IsFacebookConnected { get; set; }
    public string FacebookAccessToken { get; set; }
    public bool IsInstagramConnected { get; set; }
    public string InstagramAccessToken { get; set; }
    public bool IsGoogleBusinessProfileConnected { get; set; }
    public string GoogleBusinessProfileAccessToken { get; set; }

    public bool AutoPostEnabled { get; set; }
}
