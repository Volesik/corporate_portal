using System.Linq.Expressions;
using CorporatePortal.DL.Abstractions.Specifications;
using CorporatePortal.DL.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace CorporatePortal.DL.Specifications.User;

public class SearchUserInfoByEmail : Specification<UserInfo>
{
    private readonly string? _email;

    public SearchUserInfoByEmail(string? email)
    {
        _email = email;
    }

    public override Expression<Func<UserInfo, bool>> Expression =>
        result => EF.Functions.ILike(result.Email!, $"%{_email}%");

}