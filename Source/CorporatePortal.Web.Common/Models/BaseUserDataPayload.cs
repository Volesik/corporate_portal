namespace CorporatePortal.Web.Common.Models;

public class BaseUserDataPayload
{
    public string? KeyAccount { get; set; }
    
    public string? Sign { get; set; }
    
    public string? Request { get; set; }
    
    public string? Type { get; set; }
    
    public string? Name { get; set; }
}