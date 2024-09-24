using CorporatePortal.DL.Abstractions.Models;

namespace CorporatePortal.DL;

public interface IDatabaseContextRepository<T> : IDatabaseContextRepository
    where T : BaseEntity
{
}
