using System.Linq.Expressions;
using CorporatePortal.DL.Abstractions.Models;
using CorporatePortal.DL.Abstractions.Specifications;
using Microsoft.EntityFrameworkCore;

namespace CorporatePortal.DL.Abstractions.Repositories;

public interface IRepositoryBase<out TDbContext>
    where TDbContext : DbContext

{
    TDbContext Context { get; }

    Task<long> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken)
        where TEntity : BaseEntity;
    
    Task<bool> AnyAsync<TEntity>(
        Specification<TEntity> specification,
        CancellationToken cancellationToken)
        where TEntity : BaseEntity;
    
    Task<TEntity?> FirstOrDefaultAsync<TEntity>(
        Specification<TEntity> specification,
        CancellationToken cancellationToken)
        where TEntity : BaseEntity;

    
    Task<TEntity[]> GetArrayAsync<TEntity>(
        Specification<TEntity> specification,
        CancellationToken cancellationToken,
        int skip = default,
        int take = default)
        where TEntity : BaseEntity;
    
    Task UpdateAsync<TEntity>(
        Specification<TEntity> specification,
        Action<TEntity> updateAction,
        CancellationToken cancellationToken)
        where TEntity : BaseEntity;

    Task<int> CountAsync<TEntity>(
        Specification<TEntity> specification,
        CancellationToken cancellationToken,
        bool asSplitQuery = false,
        bool useDistinct = false)
        where TEntity : BaseEntity;

}