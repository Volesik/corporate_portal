using System.Linq.Expressions;
using CorporatePortal.DL.Abstractions.Interfaces;

namespace CorporatePortal.DL.Abstractions.Specifications.Operators;

public class NotSpecification<T> : Specification<T>
    where T : IBaseEntity
{
    private readonly Specification<T> _spec1;

    public NotSpecification(Specification<T> spec1)
    {
        _spec1 = spec1;
    }

    public override Expression<Func<T, bool>> Expression => _spec1.Expression.Not();
}
