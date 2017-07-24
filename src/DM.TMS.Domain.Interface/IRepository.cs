using NPoco;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DM.TMS.Domain.Interface
{
    public interface IRepository<T>
    {
        #region 增

        Task<object> InsertAsync(T poco);

        Task<object> InsertAsync(string tableName, string primaryKeyName, T poco);
        #endregion

        #region 删
        Task<int> DeleteAsync(T poco);

        Task<int> DeleteAsync(string tableName, string primaryKeyName, T poco);

        #endregion

        #region 改
        Task<int> UpdateAsync(T poco, Expression<Func<T, object>> fields);

        Task<int> UpdateAsync(T poco);

        Task<int> UpdateAsync(T poco, IEnumerable<string> columns);

        Task<int> UpdateAsync(T poco, object primaryKeyValue, IEnumerable<string> columns);

        #endregion

        #region 查
        Task<T> SingleOrDefaultByIdAsync(object primaryKey);

        Task<List<T>> FetchAsync();

        Task<List<T>> FetchAsync(Sql sql);

        Task<List<T>> FetchAsync(string sql, params object[] args);

        Task<List<T>> FetchAsync(long page, long itemsPerPage, Sql sql);

        Task<List<T>> FetchAsync(long page, long itemsPerPage, string sql, params object[] args);

        Task<Page<T>> PageAsync(long page, long itemsPerPage, Sql sql);

        Task<Page<T>> PageAsync(long page, long itemsPerPage, string sql, params object[] args);

        Task<List<T>> SkipTakeAsync(long skip, long take, Sql sql);

        Task<List<T>> SkipTakeAsync(long skip, long take, string sql, params object[] args);
        #endregion

        Task<int> ExecuteAsync(Sql Sql);

        Task<int> ExecuteAsync(string sql, params object[] args);

        Task<K> ExecuteScalarAsync<K>(Sql Sql);

        Task<K> ExecuteScalarAsync<K>(string sql, params object[] args);
    }
}
