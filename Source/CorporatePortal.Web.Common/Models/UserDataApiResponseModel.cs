namespace CorporatePortal.Web.Common.Models;

public class UserDataApiResponseModel<T> where T : class
{
    public ResponseInfo? Response { get; set; }
    
    public T? Data { get; set; }
}