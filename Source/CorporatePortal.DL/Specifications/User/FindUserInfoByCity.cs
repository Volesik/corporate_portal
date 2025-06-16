using System.Linq.Expressions;
using CorporatePortal.DL.Abstractions.Specifications;
using CorporatePortal.DL.EntityFramework.Models;

namespace CorporatePortal.DL.Specifications.User;

public class FindUserInfoByCity : Specification<UserInfo>
{
    private readonly IEnumerable<string?> _cities;

    public FindUserInfoByCity(IEnumerable<string?> cities)
    {
        _cities = cities;
    }

    public override Expression<Func<UserInfo, bool>> Expression
        => result => _cities.Contains(result.City);
}