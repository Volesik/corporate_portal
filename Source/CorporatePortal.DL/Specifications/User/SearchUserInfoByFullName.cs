using System.Linq.Expressions;
using CorporatePortal.DL.Abstractions.Specifications;
using CorporatePortal.DL.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace CorporatePortal.DL.Specifications.User;

public class SearchUserInfoByFullName : Specification<UserInfo>
{
    private readonly string? _fullName;

    public SearchUserInfoByFullName(string? fullName)
    {
        _fullName = fullName;
    }

    public override Expression<Func<UserInfo, bool>> Expression =>
        result => EF.Functions.ILike(result.FullName, $"%{_fullName}%");

}