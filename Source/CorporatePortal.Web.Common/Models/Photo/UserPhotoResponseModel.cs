namespace CorporatePortal.Web.Common.Models.Photo;

public abstract class UserPhotoResponseModel
{
    public ResponseInfo? Response { get; set; }
    
    public List<ResponseData>? Data { get; set; }
}