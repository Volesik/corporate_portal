using CorporatePortal.DL.Abstractions.Repositories;
using CorporatePortal.DL.EntityFramework;

namespace CorporatePortal.DL;

public interface IDatabaseContextRepository : IRepositoryBase<CorporatePortalContext>
{
}
