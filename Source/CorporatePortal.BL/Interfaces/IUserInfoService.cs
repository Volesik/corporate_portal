using CorporatePortal.DL.EntityFramework.Models;

namespace CorporatePortal.BL.Interfaces;

public interface IUserInfoService
{
    Task<UserInfo[]> GetAsync(string? searchTerm, CancellationToken token, int skip = 0, int take = 0);
    
    Task<UserInfo?> GetAsync(long id, CancellationToken token);
    
    Task<UserInfo[]> SearchAsync(string? searchTerm, CancellationToken token);
    
    Task<UserInfo[]> GetUpcomingBirthdayUsersAsync(CancellationToken token);

    Task UpsertAsync(UserInfo userInfo, CancellationToken token);
    
    Task<int> CountAsync(string? searchTerm, CancellationToken token);

    Task DisableUser(string guid, CancellationToken token);
}