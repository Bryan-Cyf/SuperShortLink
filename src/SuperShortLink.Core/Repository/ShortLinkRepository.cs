using Dapper;
using Microsoft.Extensions.Options;
using SuperShortLink.Models;
using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SuperShortLink.Repository
{
    public class ShortLinkRepository : BaseRepository, IShortLinkRepository
    {
        private readonly string _autoIncrementSql;
        public ShortLinkRepository(IOptionsSnapshot<ShortLinkOptions> options) : base(options)
        {
            _autoIncrementSql = options.Value.AutoIncrementSQL;
        }

        /// <summary>
        /// 插入短链
        /// </summary>
        /// <param name="model"></param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(UrlRecordModel model)
        {
            var sb = new StringBuilder(
              @"insert into short_link
                    (short_url, origin_url, create_time, update_time, access_count)
                values
                    (@short_url, @origin_url, @create_time, @update_time, @access_count);");

            sb.Append($"select {_autoIncrementSql};");

            var result = await _dbConnection.ExecuteScalarAsync<int>(sb.ToString(), model);
            return result;
        }

        /// <summary>
        /// 更新短链
        /// </summary>
        /// <param name="model"></param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public async Task<int> UpdateShortUrlAsync(UrlRecordModel model)
        {
            string sqlstr = @"update short_link
                              set short_url=@short_url,update_time=@update_time
                              where id=@Id;";
            var param = new
            {
                Id = model.id,
                short_url = model.short_url,
                update_time = DateTime.Now
            };
            var result = await _dbConnection.ExecuteAsync(sqlstr, param);
            return result;
        }

        /// <summary>
        /// 更新短链访问次数
        /// </summary>
        /// <param name="model"></param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public async Task<int> UpdateAccessDataAsync(long id)
        {
            string sqlstr = @"update short_link
                              set access_count=access_count + 1,update_time=@update_time
                              where Id=@Id;";
            var param = new
            {
                Id = id,
                update_time = DateTime.Now
            };
            var result = await _dbConnection.ExecuteAsync(sqlstr, param);
            return result;
        }

        /// <summary>
        /// 查询原始链接
        /// </summary>
        public async Task<string> GetOriginUrlAsync(long id)
        {
            string sqlstr = @"select origin_url
                              from short_link 
                              where Id = @Id;";
            var param = new
            {
                Id = id,
            };
            var result = await _dbConnection.QueryFirstOrDefaultAsync<string>(sqlstr, param);
            return result;
        }

        /// <summary>
        /// 查询短链信息
        /// </summary>
        public async Task<UrlRecordModel> GetAsync(long id)
        {
            string sqlstr = @"select *
                              from short_link 
                              where Id = @Id;";
            var param = new
            {
                Id = id,
            };
            var result = await _dbConnection.QueryFirstOrDefaultAsync<UrlRecordModel>(sqlstr, param);
            return result;
        }

        /// <summary>
        /// 分页查询短链信息
        /// </summary>
        public async Task<PageResponseDto<UrlRecordModel>> GetListAsync(RecordListRequest dto)
        {
            var sb = new StringBuilder(@"select *
                                         from short_link 
                                         where 1 = 1 ");

            if (!string.IsNullOrEmpty(dto.origin_url))
            {
                sb.Append(" and origin_url like @origin_url ");
                dto.origin_url = $"%{dto.origin_url}%";
            }
            if (!string.IsNullOrEmpty(dto.short_url))
            {
                sb.Append(" and short_url like @short_url ");
                dto.short_url = $"%{dto.short_url}%";
            }

            var orderSql = " order by id desc";

            return await base.PageQueryAsync<UrlRecordModel>(sb.ToString(), dto, orderSql, dto);
        }
    }
}