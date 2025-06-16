using System.Linq.Expressions;
using CorporatePortal.DL.Abstractions.Specifications;
using CorporatePortal.DL.EntityFramework.Models;

namespace CorporatePortal.DL.Specifications.User;

public class FindActiveUserInfo : Specification<UserInfo>
{
    private readonly bool _isActive;
    
    public FindActiveUserInfo(bool isActive = true)
    {
        _isActive = isActive;
    }

    public override Expression<Func<UserInfo, bool>> Expression
        => result => result.IsActive == _isActive;
}