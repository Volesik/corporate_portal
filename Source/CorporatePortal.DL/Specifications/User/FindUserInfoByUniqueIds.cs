using System.Linq.Expressions;
using CorporatePortal.DL.Abstractions.Specifications;
using CorporatePortal.DL.EntityFramework.Models;


namespace CorporatePortal.DL.Specifications.User;

public class FindUserInfoByUniqueIds : Specification<UserInfo>
{
    private readonly IEnumerable<Guid?> _uniqueIds;
    
    public FindUserInfoByUniqueIds(Guid? uniqueIds)
    {
        _uniqueIds = new[] { uniqueIds };
    }

    public FindUserInfoByUniqueIds(IEnumerable<Guid?> uniqueIds)
    {
        _uniqueIds = uniqueIds;
    }

    public override Expression<Func<UserInfo, bool>> Expression
        => result => _uniqueIds.Contains(result.UniqueId);
}