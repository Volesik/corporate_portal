using System.Linq.Expressions;
using CorporatePortal.DL.Abstractions.Models;
using CorporatePortal.DL.Abstractions.Specifications;
using Microsoft.EntityFrameworkCore;

namespace CorporatePortal.DL.Abstractions.Repositories;

public interface IRepositoryBase<out TDbContext>
    where TDbContext : DbContext

{
    TDbContext Context { get; }
    
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

}