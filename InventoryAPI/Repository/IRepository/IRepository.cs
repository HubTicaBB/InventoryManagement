using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InventoryAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);

        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,                              
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
            );

        T GetFirstOrDefault(Expression<Func<T, bool>> filter = null);

        void Add(T entity);
        void Remove(int id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
