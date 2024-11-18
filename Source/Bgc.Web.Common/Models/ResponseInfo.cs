namespace Bgc.Web.Common.Models;

public class ResponseInfo
{
    public bool Status { get; set; }
    
    public string? CodeError { get; set; }
    
    public string? Message { get; set; }
}