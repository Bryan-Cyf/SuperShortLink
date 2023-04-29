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
        private readonly DatabaseType _dbType;
        private readonly string _connectionString;

        public BaseRepository(IOptionsSnapshot<ShortLinkOptions> options)
        {
            _dbType = options.Value.DbType;
            _connectionString = options.Value.ConnectionString;
        }

        /// <summary>
        /// 分页通用方法
        /// </summary>
        protected async Task<PageResponseDto<T>> PageQueryAsync<T>(string sql, PageRequestDto page, string orderSql = "", object param = null)
        {
            PageResponseDto<T> result = new PageResponseDto<T>();

            string pageSql = _dbType == DatabaseType.SqlServer ?
                $"{sql} {orderSql} offset {(page.page_index - 1) * page.page_size} rows fetch next {page.page_size} rows only" :
                $"{sql} {orderSql} limit {page.page_size} offset {(page.page_index - 1) * page.page_size}";

            string countSql = $"select count(1)  from  {sql.Remove(0, sql.IndexOf("from", StringComparison.Ordinal) + 4)} ";

            using (var conn = ConnectionFactory.CreateConnection(_dbType, _connectionString))
            {
                result.total = await conn.ExecuteScalarAsync<long>(countSql, param);
                result.results = await conn.QueryAsync<T>(pageSql, param);
            }
            result.currentPage = page.page_index;
            result.pages = (long)Math.Ceiling(result.total * 1.0 / page.page_size);
            return result;
        }

        protected async Task<T> ExecuteScalarAsync<T>(string sql, object param = null)
        {
            using (var conn = ConnectionFactory.CreateConnection(_dbType, _connectionString))
            {
                return await conn.ExecuteScalarAsync<T>(sql, param);
            }
        }

        protected async Task<int> ExecuteAsync(string sql, object param = null)
        {
            using (var conn = ConnectionFactory.CreateConnection(_dbType, _connectionString))
            {
                return await conn.ExecuteAsync(sql, param);
            }
        }

        protected async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null)
        {
            using (var conn = ConnectionFactory.CreateConnection(_dbType, _connectionString))
            {
                return await conn.QueryFirstOrDefaultAsync<T>(sql, param);
            }
        }
    }
}