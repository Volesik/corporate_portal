using CorporatePortal.Common.Models;

namespace CorporatePortal.BL.Interfaces;

public interface IUserInfoListSearchService
{
    Task<string[]> SearchAsync(UserSearchParameters searchParameters, CancellationToken token);
}