using CorporatePortal.BL.Interfaces;
using CorporatePortal.DL;
using CorporatePortal.DL.Abstractions.Specifications;
using CorporatePortal.DL.Abstractions.Specifications.Operators;
using CorporatePortal.DL.EntityFramework.Models;
using CorporatePortal.DL.Specifications;
using CorporatePortal.DL.Specifications.User;

namespace CorporatePortal.BL.Services;

public class UserInfoService(IDatabaseContextRepository<UserInfo> userInfoRepository) : IUserInfoService
{
    public async Task<UserInfo[]> GetAsync(string? searchTerm, CancellationToken token, int skip = 0, int take = 0)
    {
        Specification<UserInfo> specification = new FindUserInfoByActive(true);

        if (searchTerm != null)
        {
            specification &= new SearchUserInfoByFullName(searchTerm);
        }
        
        var users = await userInfoRepository.GetArrayAsync(specification, token, skip, take: take);
        
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
        Specification<UserInfo> specification = new FindUserInfoByActive(true);
        
        specification &= new SearchUserInfoByFullName(searchTerm)
                            | new SearchUserInfoByEmail(searchTerm)
                            | new SearchUserInfoByMobilePhone(searchTerm);
        var users = await userInfoRepository.GetArrayAsync(specification, token, skip: default, take: 5);
        return users;
    }

    public Task<UserInfo[]> GetUpcomingBirthdayUsersAsync(CancellationToken token)
    {
        var specification = new FindUserInfoByActive(true) & new FindUpcomingBirthdayUsers();
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

    public async Task<int> CountAsync(string? searchTerm, CancellationToken token)
    {
        Specification<UserInfo> specification = new FindUserInfoByActive(true);

        if (searchTerm != null)
        {
            specification &= new SearchUserInfoByFullName(searchTerm);
        }
        
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
}