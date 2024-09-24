using System.Linq.Expressions;
using CorporatePortal.DL.Abstractions.Models;
using CorporatePortal.DL.Abstractions.Specifications;

namespace CorporatePortal.DL.Specifications;

public class FindEntitiesByIds<T> : Specification<T>
    where T : BaseEntity
{
    private readonly IEnumerable<long> _ids;

    public FindEntitiesByIds(long id)
    {
        _ids = new[] { id };
    }

    public FindEntitiesByIds(IEnumerable<long> ids)
    {
        _ids = ids;
    }

    public override Expression<Func<T, bool>> Expression
    {
        get
        {
            return result => _ids.Contains(result.Id);
        }
    }
}
