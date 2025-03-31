namespace CorporatePortal.Web.Common.Models.UserData;

public class UserDataDismissResponseModel
{
    public ResponseInfo? Response { get; set; }
    
    public List<ResponseData>? Data { get; set; }
}