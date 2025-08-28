
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
}
