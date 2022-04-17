using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MyDbtDiaryCard.Model.Abstractions;
using SQLite;

namespace MyDbtDiaryCard.Services.DataService.Repositories
{
    internal class BaseRepository : IBaseRepository
    {
        protected bool hasBeenInitialized;
        protected readonly SQLiteAsyncConnection connection;
        public BaseRepository(SQLiteAsyncConnection connection)
        {
            this.connection = connection;
        }

        public async Task<bool> CreateAsync<TEntity>(TEntity entity) where TEntity : new()
        {
            return 1 == await connection.InsertAsync(entity);
        }

        public async Task<bool> UpdateAsync<TEntity>(TEntity entity) where TEntity : new()
        {
            return 1 == await connection.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync<TEntity>(TEntity entity) where TEntity : new()
        {
            return 1 == await connection.DeleteAsync(entity);
        }

        public async Task<List<TEntity>> FindAllAsync<TEntity>() where TEntity : new()
        {
            var entities = await connection.Table<TEntity>().ToListAsync();
            return entities;
        }


        public async Task<TEntity> FindByConditionAsync<TEntity>(
            Expression<Func<TEntity, bool>> expression) where TEntity : new()
        {
            var entity = await connection.Table<TEntity>().Where(expression).FirstOrDefaultAsync();
            return entity;
        }

        public async Task<List<TEntity>> FindManyByConditionAsync<TEntity, U>(
            Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, U>> orderExpr,
            bool isByDescending = false) where TEntity : new()
        {

            if (isByDescending)
            {
                var entities = await connection.Table<TEntity>()
                    .Where(expression).OrderByDescending(orderExpr).ToListAsync();
                return entities;
            }
            else
            {
                var entities = await connection.Table<TEntity>()
                    .Where(expression).OrderBy(orderExpr).ToListAsync();
                return entities;
            }
            
        }

    }
}
