using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyDbtDiaryCard.Model.Abstractions
{
    internal interface IBaseRepository
    {
        Task<bool> CreateAsync<TEntity>(TEntity entity) where TEntity : new();
        Task<bool> UpdateAsync<TEntity>(TEntity entity) where TEntity : new();
        Task<bool> DeleteAsync<TEntity>(TEntity entity) where TEntity : new();
        Task<TEntity> FindByConditionAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : new();
        Task<List<TEntity>> FindManyByConditionAsync<TEntity, U>(
            Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, U>> orderExpr,
            bool isByDescending = false) where TEntity : new();
        Task<List<TEntity>> FindAllAsync<TEntity>() where TEntity : new();
    }
}
