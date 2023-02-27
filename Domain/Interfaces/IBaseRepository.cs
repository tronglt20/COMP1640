﻿using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> GetAllQuery();
        IQueryable<T> GetQuery(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        Task InsertAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
    }
}
