using System.Linq.Expressions;
using CorporatePortal.DL.Abstractions.Specifications;
using CorporatePortal.DL.EntityFramework.Models;

namespace CorporatePortal.DL.Specifications.User;

public class FindTodayBirthdayUsers : Specification<UserInfo>
{
    public override Expression<Func<UserInfo, bool>> Expression
        => result => result.Birthday != null
                     && result.Birthday.Value.Day == DateTime.Now.Day
                     && result.Birthday.Value.Month == DateTime.Now.Month;
}