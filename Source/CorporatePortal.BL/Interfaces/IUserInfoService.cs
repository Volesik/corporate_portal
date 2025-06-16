using CorporatePortal.Common.Models;
using CorporatePortal.DL.EntityFramework.Models;

namespace CorporatePortal.BL.Interfaces;

public interface IUserInfoService
{
    Task<UserInfo[]> GetAsync(SearchParameter searchParameter, CancellationToken token);
    
    Task<UserInfo?> GetAsync(long id, CancellationToken token);
    
    Task<UserInfo[]> SearchAsync(string? searchTerm, CancellationToken token);
    
    Task<UserInfo[]> GetUpcomingBirthdayUsersAsync(CancellationToken token);

    Task UpsertAsync(UserInfo userInfo, CancellationToken token);
    
    Task<int> CountAsync(SearchParameter searchParameter, CancellationToken token);

    Task DisableUser(string guid, CancellationToken token);
}