using CorporatePortal.DL.Abstractions.Extensions;
using CorporatePortal.DL.Abstractions.Models;
using CorporatePortal.DL.Abstractions.Specifications;
using Microsoft.EntityFrameworkCore;

namespace CorporatePortal.DL.Abstractions.Repositories;

public abstract class RepositoryBase<TDbContext> : IRepositoryBase<TDbContext>
    where TDbContext : DbContext
{
    public TDbContext Context { get; }
    
    protected virtual IQueryable<TEntity> Set<TEntity>()
        where TEntity : BaseEntity
    {
        return Context.Set<TEntity>();
    }

    protected RepositoryBase(TDbContext context)
    {
        Context = context;
    }

    public Task<TEntity?> FirstOrDefaultAsync<TEntity>(
        Specification<TEntity> specification,
        CancellationToken cancellationToken) where TEntity : BaseEntity
    {
        var query = Set<TEntity>().GetFilteredQueryWithoutSorting(specification);
        return query.FirstOrDefaultAsync(cancellationToken);
    }

    public Task<TEntity[]> GetArrayAsync<TEntity>(Specification<TEntity> specification, CancellationToken cancellationToken, int skip = default,
        int take = default) where TEntity : BaseEntity
    {
        var query = Set<TEntity>().GetFilteredQueryWithoutSorting(specification, skip, take);
        return query.ToArrayAsync(cancellationToken);
    }
}