using System.Text.Json;
using System.Text.Json.Serialization;
using CorporatePortal.BL.Interfaces;
using CorporatePortal.Common.Models;
using CorporatePortal.DL;
using CorporatePortal.DL.Abstractions.Specifications;
using CorporatePortal.DL.EntityFramework.Models;
using CorporatePortal.DL.Specifications;
using CorporatePortal.DL.Specifications.User;
using Refit;

namespace CorporatePortal.BL.Services;

public class UserInfoService(IDatabaseContextRepository<UserInfo> userInfoRepository) : IUserInfoService
{
    public async Task<UserInfo[]> GetAsync(SearchParameter searchParameter, CancellationToken token)
    {
        var specification = BuildFilteredUserInfoSpecification(searchParameter.SearchTerm, searchParameter.Filter);
        
        var users = await userInfoRepository.GetArrayAsync(specification, token, searchParameter.Skip, searchParameter.Take);
        
        return users;
    }

    public async Task<UserInfo?> GetAsync(long id, CancellationToken token)
    {
        var specification = new FindEntitiesByIds<UserInfo>(id);
        var user = await userInfoRepository.FirstOrDefaultAsync(specification, token);
        return user;
    }

    public async Task<UserInfo[]> SearchAsync(string? searchTerm, CancellationToken token)
    {
        Specification<UserInfo> specification = new FindActiveUserInfo();
        
        specification &= new SearchUserInfoByFullName(searchTerm)
                            | new SearchUserInfoByEmail(searchTerm)
                            | new SearchUserInfoByMobilePhone(searchTerm);
        var users = await userInfoRepository.GetArrayAsync(specification, token, skip: default, take: 5);
        return users;
    }

    public Task<UserInfo[]> GetUpcomingBirthdayUsersAsync(CancellationToken token)
    {
        var specification = new FindActiveUserInfo() & new FindUpcomingBirthdayUsers();
        var users = userInfoRepository.GetArrayAsync(specification, token);
        return users;
    }

    public async Task UpsertAsync(UserInfo userInfo, CancellationToken token)
    {
        var searchSpecification = new FindUserInfoByUniqueIds(userInfo.UniqueId);
        var isUserExists = await userInfoRepository.AnyAsync(searchSpecification, token);
        if (isUserExists)
        {
            await userInfoRepository.UpdateAsync(
                searchSpecification,
                dbUser =>
                {
                    dbUser.Birthday = userInfo.Birthday;
                    dbUser.Email = userInfo.Email;
                    dbUser.FullName = userInfo.FullName;
                    dbUser.Department = userInfo.Department;
                    dbUser.City = userInfo.City;
                    dbUser.MobilePhone = userInfo.MobilePhone;
                    dbUser.Organizations = userInfo.Organizations;
                    dbUser.Position = userInfo.Position;
                    dbUser.SubDepartment = userInfo.SubDepartment;
                    dbUser.WorkPhone = userInfo.WorkPhone;
                    dbUser.ChiefFullName = userInfo.ChiefFullName;
                    dbUser.Room = userInfo.Room;
                    dbUser.EmploymentDate = userInfo.EmploymentDate;
                    dbUser.AlternativeName = userInfo.AlternativeName;
                    dbUser.InternalPhone = userInfo.InternalPhone;
                    dbUser.IsActive = true;
                },
                token);
            
            return;
        }
        
        await userInfoRepository.AddAsync(userInfo, token);
    }

    public async Task<int> CountAsync(SearchParameter searchParameter, CancellationToken token)
    {
            var specification = BuildFilteredUserInfoSpecification(searchParameter.SearchTerm, searchParameter.Filter);
        
        return await userInfoRepository.CountAsync(specification, token);
    }

    public async Task DisableUser(string guid, CancellationToken token)
    {
        if (Guid.TryParse(guid, out var uniqueId))
        {
            var specification = new FindUserInfoByUniqueIds(uniqueId);
            var isUserExists = await userInfoRepository.AnyAsync(specification, token);
            if (isUserExists)
            {
                await userInfoRepository.UpdateAsync(
                    specification,
                    dbUser =>
                    {
                        dbUser.IsActive = false;
                    },
                    token);
            }
        }
        
    }

    private Specification<UserInfo> BuildFilteredUserInfoSpecification(string? searchTerm, string? parameters)
    {
        Specification<UserInfo> specification = new FindActiveUserInfo();
        
        specification &= new SearchUserInfoByFullName(searchTerm)
                         | new SearchUserInfoByEmail(searchTerm)
                         | new SearchUserInfoByMobilePhone(searchTerm);

        if (string.IsNullOrWhiteSpace(parameters))
        {
            return specification;
        }
        
        var defaultSerizaliztionOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new ObjectToInferredTypesConverter()
            }
        };
        
        var filter = JsonSerializer.Deserialize<UserInfoFilter>(parameters, defaultSerizaliztionOptions);
        if (filter?.Rooms != null && filter!.Rooms!.Length > 0)
        {
            specification &= new FindUserInfoByRoom(filter.Rooms);
        }
    
        if (filter?.Departments != null && filter!.Departments!.Length > 0)
        {
            specification &= new FindUserInfoByDepartment(filter.Departments);
        }

        if (filter?.Positions != null && filter!.Positions!.Length > 0)
        {
            specification &= new FindUserInfoByPosition(filter.Positions);
        }

        if (filter?.Cities != null && filter!.Cities!.Length > 0)
        {
            specification &= new FindUserInfoByCity(filter.Cities);
        }

        return specification;
    }
}