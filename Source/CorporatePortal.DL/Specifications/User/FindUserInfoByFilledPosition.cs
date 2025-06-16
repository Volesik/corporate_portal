using System.Linq.Expressions;
using CorporatePortal.DL.Abstractions.Specifications;
using CorporatePortal.DL.EntityFramework.Models;

namespace CorporatePortal.DL.Specifications.User;

public class FindUserInfoByFilledPosition : Specification<UserInfo>
{
    public override Expression<Func<UserInfo, bool>> Expression
        => result => !string.IsNullOrWhiteSpace(result.Position);
}