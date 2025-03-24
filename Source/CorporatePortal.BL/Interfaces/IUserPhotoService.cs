using CorporatePortal.Web.Common.Models;

namespace CorporatePortal.BL.Interfaces;

public interface IUserPhotoService
{
    Task<UserPhotoResponseModel?> SendAsync(string guid);
    
    Task<bool> SavePhotoAsync(UserPhotoResponseModel? userPhotoResponseModel, string guid);
}