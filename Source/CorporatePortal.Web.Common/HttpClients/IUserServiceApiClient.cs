using Refit;
using CorporatePortal.Common.Constants;
using CorporatePortal.Web.Common.Models;

namespace CorporatePortal.Web.Common.HttpClients;

public interface IUserServiceApiClient
{
    [Post("/mis/hs/api")]
    Task<UserPhotoResponseModel> SendRequestAsync(
        CreateUserPhotoRequestPayload payload,
        [Header(AuthConstants.Authorization)] string auth);
}