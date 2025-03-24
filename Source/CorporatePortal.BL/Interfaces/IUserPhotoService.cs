using CorporatePortal.Web.Common.Models.Photo;

namespace CorporatePortal.BL.Interfaces;

public interface IUserPhotoService
{
    Task<UserPhotoResponseModel?> SendAsync(string guid);
    
    Task<bool> SavePhotoAsync(UserPhotoResponseModel? userPhotoResponseModel, string guid);
}