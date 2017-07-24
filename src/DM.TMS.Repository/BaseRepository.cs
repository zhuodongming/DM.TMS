using DM.TMS.Domain.Interface;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DM.TMS.Repository
{
    public abstract class BaseRepository<T> : IRepository<T>
    {
        protected Database db = null;

        #region 增

        public Task<object> InsertAsync(T poco)
        {
            return db.InsertAsync(poco);
        }

        public Task<object> InsertAsync(string tableName, string primaryKeyName, T poco)
        {
            return db.InsertAsync(tableName, primaryKeyName, poco);
        }
        #endregion

        #region 删
        public Task<int> DeleteAsync(T poco)
        {
            return db.DeleteAsync(poco);
        }

        public Task<int> DeleteAsync(string tableName, string primaryKeyName, T poco)
        {
            return db.DeleteAsync(tableName, primaryKeyName, poco);
        }

        #endregion

        #region 改
        public Task<int> UpdateAsync(T poco, Expression<Func<T, object>> fields)
        {
            return db.UpdateAsync(poco, fields);
        }

        public Task<int> UpdateAsync(T poco)
        {
            return db.UpdateAsync(poco);
        }

        public Task<int> UpdateAsync(T poco, IEnumerable<string> columns)
        {
            return db.UpdateAsync(poco, columns);
        }

        public Task<int> UpdateAsync(T poco, object primaryKeyValue, IEnumerable<string> columns)
        {
            return db.UpdateAsync(poco, poco, columns);
        }

        #endregion

        #region 查
        public Task<T> SingleOrDefaultByIdAsync(object primaryKey)
        {
            return db.SingleOrDefaultByIdAsync<T>(primaryKey);
        }

        public Task<List<T>> FetchAsync()
        {
            return db.FetchAsync<T>();
        }

        public Task<List<T>> FetchAsync(Sql sql)
        {
            return db.FetchAsync<T>(sql);
        }

        public Task<List<T>> FetchAsync(string sql, params object[] args)
        {
            return db.FetchAsync<T>(sql, args);
        }

        public Task<List<T>> FetchAsync(long page, long itemsPerPage, Sql sql)
        {
            return db.FetchAsync<T>(page, itemsPerPage, sql);
        }

        public Task<List<T>> FetchAsync(long page, long itemsPerPage, string sql, params object[] args)
        {
            return db.FetchAsync<T>(page, itemsPerPage, sql, args);
        }

        public Task<Page<T>> PageAsync(long page, long itemsPerPage, Sql sql)
        {
            return db.PageAsync<T>(page, itemsPerPage, sql);
        }

        public Task<Page<T>> PageAsync(long page, long itemsPerPage, string sql, params object[] args)
        {
            return db.PageAsync<T>(page, itemsPerPage, sql, args);
        }

        public Task<List<T>> SkipTakeAsync(long skip, long take, Sql sql)
        {
            return db.SkipTakeAsync<T>(skip, take, sql);
        }

        public Task<List<T>> SkipTakeAsync(long skip, long take, string sql, params object[] args)
        {
            return db.SkipTakeAsync<T>(skip, take, sql, args);
        }
        #endregion

        public Task<int> ExecuteAsync(Sql Sql)
        {
            return db.ExecuteAsync(Sql);
        }

        public Task<int> ExecuteAsync(string sql, params object[] args)
        {
            return db.ExecuteAsync(sql, args);
        }

        public Task<K> ExecuteScalarAsync<K>(Sql Sql)
        {
            return db.ExecuteScalarAsync<K>(Sql);
        }

        public Task<K> ExecuteScalarAsync<K>(string sql, params object[] args)
        {
            return db.ExecuteScalarAsync<K>(sql, args);
        }
    }
}
