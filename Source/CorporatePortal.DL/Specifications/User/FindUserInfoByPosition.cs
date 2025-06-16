using System.Linq.Expressions;
using CorporatePortal.DL.Abstractions.Specifications;
using CorporatePortal.DL.EntityFramework.Models;

namespace CorporatePortal.DL.Specifications.User;

public class FindUserInfoByPosition : Specification<UserInfo>
{
    private readonly IEnumerable<string?> _positions;

    public FindUserInfoByPosition(IEnumerable<string?> positions)
    {
        _positions = positions;
    }

    public override Expression<Func<UserInfo, bool>> Expression
        => result => _positions.Contains(result.Position);
}