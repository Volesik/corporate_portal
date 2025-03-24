namespace CorporatePortal.Web.Common.Models;

public abstract class ResponseInfo
{
    public bool Status { get; set; }
    
    public string? CodeError { get; set; }
    
    public string? Message { get; set; }
}