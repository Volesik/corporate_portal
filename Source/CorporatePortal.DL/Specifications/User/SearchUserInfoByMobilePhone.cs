using System.Linq.Expressions;
using System.Text.RegularExpressions;
using CorporatePortal.DL.Abstractions.Specifications;
using CorporatePortal.DL.EntityFramework;
using CorporatePortal.DL.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace CorporatePortal.DL.Specifications.User;

public class SearchUserInfoByMobilePhone : Specification<UserInfo>
{
    private readonly string? _mobilePhone;

    public SearchUserInfoByMobilePhone(string? mobilePhone)
    {
        _mobilePhone = !string.IsNullOrEmpty(mobilePhone)
            ? Regex.Replace(mobilePhone, "[^0-9]", "")
            : null;
    }

    public override Expression<Func<UserInfo, bool>> Expression =>
        string.IsNullOrEmpty(_mobilePhone)
            ? user => false // no condition if no search
            : user => user.MobilePhone != null &&
                      EF.Functions.ILike(
                          CorporatePortalContext.RegexReplace(user.MobilePhone, "[^0-9]", "", "g"),
                          $"%{_mobilePhone}%");

}