using CorporatePortal.DL.EntityFramework.Models;

namespace CorporatePortal.BL.Interfaces;

public interface IUserInfoService
{
    Task<UserInfo[]> GetAllAsync();
}