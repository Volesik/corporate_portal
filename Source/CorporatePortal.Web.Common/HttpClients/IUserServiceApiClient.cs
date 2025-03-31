using Refit;
using CorporatePortal.Common.Constants;
using CorporatePortal.Web.Common.Models;
using CorporatePortal.Web.Common.Models.Photo;
using CorporatePortal.Web.Common.Models.UserData;

namespace CorporatePortal.Web.Common.HttpClients;

public interface IUserServiceApiClient
{
    [Post("")]
    Task<UserPhotoResponseModel> SendPhotoRequestAsync(UserPhotoPayload payload, [Header(AuthConstants.Authorization)] string auth);
    
    [Post("")]
    Task<string> SendUserDataRequestAsync(BaseUserDataPayload payload, [Header(AuthConstants.Authorization)] string auth);
    
    [Post("")]
    Task<UserDataDismissResponseModel> SendUserDismissDataRequestAsync(BaseUserDataPayload payload, [Header(AuthConstants.Authorization)] string auth);
}