using System.Linq.Expressions;
using CorporatePortal.DL.Abstractions.Specifications;
using CorporatePortal.DL.EntityFramework.Models;

namespace CorporatePortal.DL.Specifications.User;

public class FindUpcomingBirthdayUsers : Specification<UserInfo>
{
    private readonly DateTime _today = DateTime.Today;
    private readonly DateTime _tomorrow = DateTime.Today.AddDays(1);
    private readonly DateTime _dayAfterTomorrow = DateTime.Today.AddDays(2);
    
    public override Expression<Func<UserInfo, bool>> Expression
        => result => result.Birthday != null &&
            (
                (result.Birthday.Value.Month == _today.Month && result.Birthday.Value.Day == _today.Day) ||
                (result.Birthday.Value.Month == _tomorrow.Month && result.Birthday.Value.Day == _tomorrow.Day) ||
                (result.Birthday.Value.Month == _dayAfterTomorrow.Month && result.Birthday.Value.Day == _dayAfterTomorrow.Day)
            );
}