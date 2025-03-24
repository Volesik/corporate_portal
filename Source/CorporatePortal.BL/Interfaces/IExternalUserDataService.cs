using CorporatePortal.Web.Common.Models.Photo;
using CorporatePortal.Web.Common.Models.UserData;

namespace CorporatePortal.BL.Interfaces;

public interface IExternalUserDataService
{
    Task<UserPhotoResponseModel?> SendPhotoRequestAsync(string guid);

    Task<bool> SavePhotoAsync(UserPhotoResponseModel? userPhotoResponseModel, string name);

    Task<UserDataResponseModel?> SendUserDataRequestAsync();

    Task<UserDataDismissResponseModel?> SendUserDataDismissAsync();
}