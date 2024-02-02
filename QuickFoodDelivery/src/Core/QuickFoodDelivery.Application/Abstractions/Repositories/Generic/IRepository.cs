using QuickFoodDelivery.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.Abstractions.Repositories
{
    public interface IRepository<T> where T : BaseEntity , new()
    {
        IQueryable<T> GetAll(bool ignoreQuery = false, bool isTracking = false, params string[] includes);
        IQueryable<T> GetAllWhere(Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>? orderExpression = null,
            bool isDescending = false,
            int skip = 0, int take = 0,
            bool isTracking = false,
            bool ignoreQuery = false,
            params string[] includes);
        Task<T> GetByIdAsync(int id, bool isTracking = false, bool ignoreQuery = false, params string[] includes);
        Task<T> GetByExpressionAsync(Expression<Func<T, bool>> expression, bool isTracking = false, bool ignoreQuery = false, params string[] includes);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SoftDelete(T entity);
        void ReverseDelete(T entity);
        Task SaveChangesAsync();
        Task<bool> Cheeck(Expression<Func<T, bool>> expression);
    }
}
