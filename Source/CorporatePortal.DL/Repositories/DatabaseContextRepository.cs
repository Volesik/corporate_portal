using CorporatePortal.DL.Abstractions.Repositories;
using CorporatePortal.DL.EntityFramework;

namespace CorporatePortal.DL.Repositories;

public class DatabaseContextRepository : RepositoryBase<CorporatePortalContext>, IDatabaseContextRepository
{
    public DatabaseContextRepository(CorporatePortalContext context)
        : base(context)
    {
    }
}