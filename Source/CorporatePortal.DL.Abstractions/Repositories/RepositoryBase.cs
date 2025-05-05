using System.Linq.Expressions;
using CorporatePortal.DL.Abstractions.Extensions;
using CorporatePortal.DL.Abstractions.Interfaces;
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
    
    public virtual async Task<long> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken)
        where TEntity : BaseEntity
    {
        var entry = Context.Add(entity);

        await SaveChangesAsync(cancellationToken);

        return entry.Entity.Id;
    }


    public Task<bool> AnyAsync<TEntity>(Specification<TEntity> specification, CancellationToken cancellationToken) where TEntity : BaseEntity
    {
        var query = Set<TEntity>();
        
        return query.AnyAsync(specification.Expression, cancellationToken);
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

    public async Task UpdateAsync<TEntity>(
        Specification<TEntity> specification,
        Action<TEntity> updateAction,
        CancellationToken cancellationToken)
        where TEntity : BaseEntity
    {
        var originalEntity = await FirstOrDefaultAsync(specification, cancellationToken)
                             ?? throw new Exception($"Could not find entity {typeof(TEntity).Name} by specification {specification.GetType().Name}");

        updateAction(originalEntity);

        await SaveChangesAsync(cancellationToken);
    }
    
    public Task<int> CountAsync<TEntity>(
        Specification<TEntity> specification,
        CancellationToken cancellationToken,
        bool asSplitQuery = false,
        bool useDistinct = false)
        where TEntity : BaseEntity
    {
        var query = Set<TEntity>().GetFilteredQueryWithoutSorting(specification);
        return query
            .CountAsync(cancellationToken);
    }

    
    private Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;

        var modifiedAuditedEntities = Context.ChangeTracker
            .Entries<IBaseEntity>()
            .Where(entity => entity.State == EntityState.Modified)
            .Select(entity => entity.Entity)
            .ToList();

        modifiedAuditedEntities.ForEach(entity =>
        {
            entity.UpdatedWhen = now;
        });

        return Context.SaveChangesAsync(cancellationToken);
    }



}