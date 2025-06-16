using CorporatePortal.BL.Interfaces;
using CorporatePortal.Common.Enums;
using CorporatePortal.Common.Models;
using CorporatePortal.DL;
using CorporatePortal.DL.EntityFramework.Models;
using CorporatePortal.DL.Specifications.User;

namespace CorporatePortal.BL.Services;

public class UserInfoListSearchService(IDatabaseContextRepository<UserInfo> userInfoRepository) : IUserInfoListSearchService
{
    public Task<string[]> SearchAsync(UserSearchParameters searchParameters, CancellationToken token)
    {
        var searchFunc = GetSearchFunction(searchParameters.SearchType);
        
        return searchFunc(searchParameters, token);
    }

    private Func<UserSearchParameters, CancellationToken, Task<string[]>> GetSearchFunction(UserDashboardSearchParameters searchType)
    {
        return searchType switch
        {
            UserDashboardSearchParameters.City => GetUserInfoCitiesAsync,
            UserDashboardSearchParameters.Department => GetUserInfoDepartmentsAsync,
            UserDashboardSearchParameters.Position => GetUserInfoPositionsAsync,
            UserDashboardSearchParameters.Room => GetUserInfoRoomsAsync,
            _ => throw new InvalidOperationException($"Invalid search type: {searchType}"),
        };
    }

    private Task<string[]> GetUserInfoCitiesAsync(UserSearchParameters searchParameters, CancellationToken token)
    {
        var specification = new FindActiveUserInfo() & new FindUserInfoByFilledCity();
        
        return userInfoRepository.GetDistinctItemsArrayAsync(
            specification,
            groupBy => new { groupBy!.City },
            projection => projection.City!,
            order => order!.City!,
            token);
    }
    
    private Task<string[]> GetUserInfoDepartmentsAsync(UserSearchParameters searchParameters, CancellationToken token)
    {
        var specification = new FindActiveUserInfo() & new FindUserInfoByFilledDepartment();
        
        return userInfoRepository.GetDistinctItemsArrayAsync(
            specification,
            groupBy => new { groupBy!.Department },
            projection => projection.Department!,
            order => order!.Department!,
            token);
    }
    
    private Task<string[]> GetUserInfoPositionsAsync(UserSearchParameters searchParameters, CancellationToken token)
    {
        var specification = new FindActiveUserInfo() & new FindUserInfoByFilledPosition();
        
        return userInfoRepository.GetDistinctItemsArrayAsync(
            specification,
            groupBy => new { groupBy!.Position },
            projection => projection.Position!,
            order => order!.Position!,
            token);
    }
    
    private Task<string[]> GetUserInfoRoomsAsync(UserSearchParameters searchParameters, CancellationToken token)
    {
        var specification = new FindActiveUserInfo();
        
        return userInfoRepository.GetDistinctItemsArrayAsync(
            specification,
            groupBy => new { groupBy!.Room },
            projection => projection.Room!,
            order => order!.Room!,
            token);
    }
}