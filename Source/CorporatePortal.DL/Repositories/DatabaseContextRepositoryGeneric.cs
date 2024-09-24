using CorporatePortal.DL.Abstractions.Models;
using CorporatePortal.DL.Abstractions.Repositories;
using CorporatePortal.DL.EntityFramework;

namespace CorporatePortal.DL.Repositories;

public class DatabaseContextRepository<T> : RepositoryBase<CorporatePortalContext>, IDatabaseContextRepository<T>
    where T : BaseEntity
{
    public DatabaseContextRepository(CorporatePortalContext context)
        : base(context)
    {
    }
}