using Dapper;
using Microsoft.Extensions.Options;
using SuperShortLink.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SuperShortLink.Repository
{
    public class BaseRepository
    {
        protected readonly IDbConnection _dbConnection;
        protected readonly DatabaseType _dbType;

        public BaseRepository(IOptionsSnapshot<ShortLinkOptions> options)
        {
            _dbType = options.Value.DbType;
            _dbConnection = ConnectionFactory.CreateConnection(options.Value.DbType, options.Value.ConnectionString);
        }

        /// <summary>
        /// 分页通用方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="param"></param>
        /// <param name="orderSql"></param>
        /// <returns></returns>
        protected async Task<PageResponseDto<T>> PageQueryAsync<T>(string sql, PageRequestDto page, string orderSql = "", object param = null)
        {
            PageResponseDto<T> result = new PageResponseDto<T>();

            string pageSql = _dbType == DatabaseType.SqlServer ?
                $"{sql} {orderSql} offset {(page.page_index - 1) * page.page_size} rows fetch next {page.page_size} rows only" :
                $"{sql} {orderSql} limit {page.page_size} offset {(page.page_index - 1) * page.page_size}";

            string countSql = $"select count(1)  from  {sql.Remove(0, sql.IndexOf("from", StringComparison.Ordinal) + 4)} ";

            result.total = await _dbConnection.ExecuteScalarAsync<long>(countSql, param);
            result.results = await _dbConnection.QueryAsync<T>(pageSql, param);
            result.currentPage = page.page_index;
            result.pages = (long)Math.Ceiling(result.total * 1.0 / page.page_size);
            return result;
        }
    }
}