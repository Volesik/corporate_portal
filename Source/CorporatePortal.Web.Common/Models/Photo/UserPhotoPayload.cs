using System.Text.Json.Serialization;

namespace CorporatePortal.Web.Common.Models.Photo;

public class UserPhotoPayload : BaseUserDataPayload
{
    public string? Kod { get; set; }
    
    [JsonPropertyName("NameFL")]
    public string? NameFl { get; set; }
}