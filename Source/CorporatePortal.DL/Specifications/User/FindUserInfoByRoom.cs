using System.Linq.Expressions;
using CorporatePortal.DL.Abstractions.Specifications;
using CorporatePortal.DL.EntityFramework.Models;

namespace CorporatePortal.DL.Specifications.User;

public class FindUserInfoByRoom : Specification<UserInfo>
{
    private readonly IEnumerable<string?> _rooms;

    public FindUserInfoByRoom(IEnumerable<string?> rooms)
    {
        _rooms = rooms;
    }

    public override Expression<Func<UserInfo, bool>> Expression
        => result => _rooms.Contains(result.Room);
}