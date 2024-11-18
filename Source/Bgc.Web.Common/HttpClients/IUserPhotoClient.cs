using Refit;
using Bgc.Web.Common.Models;
using CorporatePortal.Common.Constants;

namespace Bgc.Web.Common.HttpClients;

public interface IUsersPhotoClient
{
    [Post("/mis/hs/api")]
    Task<UserPhotoResponseModel> SendRequestAsync(
        CreateUserPhotoRequestPayload payload,
        [Header(AuthConstants.Authorization)] string auth);
}