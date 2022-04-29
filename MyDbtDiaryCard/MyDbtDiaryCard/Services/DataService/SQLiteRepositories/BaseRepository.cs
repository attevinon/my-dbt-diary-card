using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MyDbtDiaryCard.Model.Abstractions;
using SQLite;

namespace MyDbtDiaryCard.Services.DataService.SQLiteRepositories
{
    internal class BaseRepository : IBaseRepository
    {
        protected bool hasBeenInitialized;
        public bool HasBeenInitialized { get; protected set; }
        protected readonly SQLiteAsyncConnection connection;
        public BaseRepository(SQLiteAsyncConnection connection)
        {
            this.connection = connection;
        }

        public async Task<bool> CreateAsync<TEntity>(TEntity entity) where TEntity : new()
        {
            return 1 == await connection.InsertAsync(entity);
        }

        public async Task<bool> CreateManyAsync<TEntity>(IList<TEntity> entities) where TEntity : new()
        {
            bool result = true;

            foreach (var entity in entities)
            {
                result = result && await CreateAsync(entity);
            }

            return result;
        }

        public async Task<bool> UpdateAsync<TEntity>(TEntity entity) where TEntity : new()
        {
            return 1 == await connection.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync<TEntity>(TEntity entity) where TEntity : new()
        {
            return 1 == await connection.DeleteAsync(entity);
        }

        public async Task<bool> DeleteManyAsync<TEntity>(
            Expression<Func<TEntity, bool>> expression) where TEntity : new()
        {
            try
            {
                await connection.Table<TEntity>().Where(expression).DeleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
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

        public async Task<List<TEntity>> FindManyByConditionAsync<TEntity>(
            Expression<Func<TEntity, bool>> expression) where TEntity : new()
        {
            try
            {
                var entities = await connection.Table<TEntity>().Where(expression).ToListAsync();
                return entities;
            }
            catch (Exception ex)
            {
                return null;
            }

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
                try
                {
                    var entities = await connection?.Table<TEntity>()?.Where(expression)?.OrderBy(orderExpr).ToListAsync();
                    return entities;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return null;
            }
        }

    }
}
