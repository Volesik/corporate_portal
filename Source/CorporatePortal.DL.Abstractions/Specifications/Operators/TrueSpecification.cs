using System.Linq.Expressions;
using CorporatePortal.DL.Abstractions.Interfaces;

namespace CorporatePortal.DL.Abstractions.Specifications.Operators;

public class TrueSpecification<T> : Specification<T>
    where T : IBaseEntity
{
    public override Expression<Func<T, bool>> Expression => True();
}
