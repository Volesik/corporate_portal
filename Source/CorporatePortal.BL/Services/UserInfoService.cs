using CorporatePortal.BL.Interfaces;
using CorporatePortal.DL;
using CorporatePortal.DL.Abstractions.Specifications;
using CorporatePortal.DL.Abstractions.Specifications.Operators;
using CorporatePortal.DL.EntityFramework.Models;
using CorporatePortal.DL.Specifications;
using CorporatePortal.DL.Specifications.User;

namespace CorporatePortal.BL.Services;

public class UserInfoService : IUserInfoService
{
    private readonly IDatabaseContextRepository<UserInfo> _userInfoRepository;

    public UserInfoService(IDatabaseContextRepository<UserInfo> userInfoRepository)
    {
        _userInfoRepository = userInfoRepository;
    }
    
    public async Task<UserInfo[]> GetAllAsync(string? searchTerm, int skip, CancellationToken token)
    {
        var specification = searchTerm == null
            ? (Specification<UserInfo>)new TrueSpecification<UserInfo>()
            : new SearchUserInfoByFullName(searchTerm);
        var users = await _userInfoRepository.GetArrayAsync(specification, token, skip, take: 20);
        return users;
    }

    public async Task<UserInfo?> GetByIdAsync(long id, CancellationToken token)
    {
        var specification = new FindEntitiesByIds<UserInfo>(id);
        var user = await _userInfoRepository.FirstOrDefaultAsync(specification, token);
        return user;
    }

    public async Task<UserInfo[]> SearchAsync(string? searchTerm, CancellationToken token)
    {
        var specification = new SearchUserInfoByFullName(searchTerm);
        var users = await _userInfoRepository.GetArrayAsync(specification, token, skip: default, take: 5);
        return users;
    }

    public Task<UserInfo[]> GetTodayBirthdayUsersAsync(CancellationToken token)
    {
        var specification = new FindTodayBirthdayUsers();
        var users = _userInfoRepository.GetArrayAsync(specification, token);
        return users;
    }

    public async Task UpsertAsync(UserInfo userInfo, CancellationToken token)
    {
        var searchSpecification = new FindUserInfoByUniqueIds(userInfo.UniqueId);
        var isUserExists = await _userInfoRepository.AnyAsync(searchSpecification, token);
        if (isUserExists)
        {
            await _userInfoRepository.UpdateAsync(
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
                },
                token);
            
            return;
        }
        
        await _userInfoRepository.AddAsync(userInfo, token);
    }

    public async Task<int> CountAsync(string? searchTerm, CancellationToken token)
    {
        var specification = searchTerm == null
            ? (Specification<UserInfo>)new TrueSpecification<UserInfo>()
            : new SearchUserInfoByFullName(searchTerm);
        
        return await _userInfoRepository.CountAsync(specification, token);
    }
}