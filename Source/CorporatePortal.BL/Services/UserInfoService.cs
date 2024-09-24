using CorporatePortal.BL.Interfaces;
using CorporatePortal.DL;
using CorporatePortal.DL.Abstractions.Specifications.Operators;
using CorporatePortal.DL.EntityFramework.Models;

namespace CorporatePortal.BL.Services;

public class UserInfoService : IUserInfoService
{
    private readonly IDatabaseContextRepository<UserInfo> _repository;

    public UserInfoService(IDatabaseContextRepository<UserInfo> repository)
    {
        _repository = repository;
    }
    
    public async Task<UserInfo[]> GetAllAsync()
    {
        var specification = new TrueSpecification<UserInfo>();
        var users = await _repository.GetArrayAsync(specification, CancellationToken.None);
        return users;
    }
}