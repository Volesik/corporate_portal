using System.Text.Json.Serialization;

namespace Bgc.Web.Common.Models;

public class CreateUserPhotoRequestPayload
{
    public string? KeyAccount { get; set; }
    
    public string? Sign { get; set; }
    
    public string? Request { get; set; }
    
    public string? Type { get; set; }
    
    public string? Name { get; set; }
    
    public string? Kod { get; set; }
    
    [JsonPropertyName("NameFL")]
    public string? NameFl { get; set; }
}