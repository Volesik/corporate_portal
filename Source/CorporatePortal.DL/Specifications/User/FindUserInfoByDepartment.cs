using System.Linq.Expressions;
using CorporatePortal.DL.Abstractions.Specifications;
using CorporatePortal.DL.EntityFramework.Models;

namespace CorporatePortal.DL.Specifications.User;

public class FindUserInfoByDepartment : Specification<UserInfo>
{
    private readonly IEnumerable<string?> _departments;

    public FindUserInfoByDepartment(IEnumerable<string?> departments)
    {
        _departments = departments;
    }

    public override Expression<Func<UserInfo, bool>> Expression
        => result => _departments.Contains(result.Department);
}