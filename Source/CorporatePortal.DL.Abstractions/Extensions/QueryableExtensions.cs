using System.Linq.Expressions;
using CorporatePortal.DL.Abstractions.Models;
using CorporatePortal.DL.Abstractions.Specifications;
using Microsoft.EntityFrameworkCore;

namespace CorporatePortal.DL.Abstractions.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity?> IncludeAll<TEntity>(this IQueryable<TEntity?> query, IEnumerable<string>? includedProperties = null)
        where TEntity : BaseEntity
    {
        if (query == null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        var includedPropertiesArray = includedProperties?.ToArray();

        if (includedPropertiesArray == null || !includedPropertiesArray.Any())
        {
            return query;
        }

        foreach (var property in includedPropertiesArray)
        {
            query = ((IQueryable<TEntity>)query).Include(property);
        }

        return query;
    }
    
    public static IQueryable<TEntity?> GetFilteredQueryWithoutSorting<TEntity>(
        this IQueryable<TEntity?> query,
        Specification<TEntity> specification,
        int skip = default,
        int take = default)
        where TEntity : BaseEntity
    {
        query = query.GetFilteredQuery(specification);

        if (skip != default)
        {
            query = query.Skip(skip);
        }

        if (take != default)
        {
            query = query.Take(take);
        }

        return query;
    }

    public static IQueryable<TEntity?> GetFilteredQuery<TEntity>(
        this IQueryable<TEntity?> query,
        Specification<TEntity> specification)
        where TEntity : BaseEntity
    {
        query = query.DefaultIfEmpty();

        var expression = specification.Expression as Expression<Func<TEntity?, bool>>;
        query = query.Where(expression);

        return query;
    }
    
    public static IQueryable<TEntity?> GetFilteredQuery<TEntity>(
        this IQueryable<TEntity?> query,
        Specification<TEntity> specification,
        IEnumerable<string> includedProperties,
        bool noTracking,
        bool asSplitQuery)
        where TEntity : BaseEntity
    {
        query = query.IncludeAll(includedProperties);

        if (noTracking)
        {
            query = ((IQueryable<TEntity>)query).AsNoTracking();
        }

        if (asSplitQuery)
        {
            query = ((IQueryable<TEntity>)query).AsSplitQuery();
        }

        query = query.DefaultIfEmpty();

        var expression = specification.Expression as Expression<Func<TEntity?, bool>>;
        query = query.Where(expression);

        return query;
    }
    
    public static IQueryable<TProjection?> GetDistinctFilteredQueryWithSorting<TEntity, TProjection, TGroupKey>(
        this IQueryable<TEntity?> query,
        Specification<TEntity> specification,
        Expression<Func<TEntity?, TGroupKey>> groupByExpression,
        Expression<Func<TGroupKey, TProjection>> projectExpression,
        Expression<Func<TEntity?, object>> sortingExpression,
        int skip,
        int take,
        IEnumerable<string> includedProperties,
        bool noTracking,
        bool asSplitQuery)
        where TEntity : BaseEntity
    {
        query = query.GetFilteredQuery(specification, includedProperties, noTracking, asSplitQuery);

        var projectionQuery = query
            .OrderBy(sortingExpression)
            .GroupBy(groupByExpression)
            .Select(x => x.Key)
            .Select(projectExpression);

        if (skip != default)
        {
            projectionQuery = projectionQuery.Skip(skip);
        }

        if (take != default)
        {
            projectionQuery = projectionQuery.Take(take);
        }

        return projectionQuery;
    }

}