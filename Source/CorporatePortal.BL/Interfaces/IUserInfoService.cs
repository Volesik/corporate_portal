using CorporatePortal.DL.EntityFramework.Models;

namespace CorporatePortal.BL.Interfaces;

public interface IUserInfoService
{
    Task<UserInfo[]> GetAllAsync(string? searchTerm, int skip, CancellationToken token);
    
    Task<UserInfo?> GetByIdAsync(long id, CancellationToken token);
    
    Task<UserInfo[]> SearchAsync(string? searchTerm, CancellationToken token);
    
    Task<UserInfo[]> GetTodayBirthdayUsersAsync(CancellationToken token);

    Task UpsertAsync(UserInfo userInfo, CancellationToken token);
    
    Task<int> CountAsync(string? searchTerm, CancellationToken token);
}