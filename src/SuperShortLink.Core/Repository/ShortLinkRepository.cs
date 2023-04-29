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
        public async Task<int> InsertAsync(LinkModel model)
        {
            var sb = new StringBuilder(
              @"insert into short_link
                    (short_url, origin_url, create_time, update_time, access_count)
                values
                    (@short_url, @origin_url, @create_time, @update_time, @access_count);");

            sb.Append($"select {_autoIncrementSql};");

            var result = await base.ExecuteScalarAsync<int>(sb.ToString(), model);
            return result;
        }

        /// <summary>
        /// 更新短链
        /// </summary>
        public async Task<int> UpdateShortUrlAsync(LinkModel model)
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
            var result = await base.ExecuteAsync(sqlstr, param);
            return result;
        }

        /// <summary>
        /// 更新短链访问次数
        /// </summary>
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
            var result = await base.ExecuteAsync(sqlstr, param);
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
            var result = await base.QueryFirstOrDefaultAsync<string>(sqlstr, param);
            return result;
        }

        /// <summary>
        /// 查询生成短链的数量
        /// </summary>
        public async Task<int> GetCountAsync(DateTime startTime, DateTime endTime)
        {
            string sqlstr = @"select count(1)
                              from short_link 
                              where create_time >= @startTime and create_time < @endTime;";
            var param = new
            {
                startTime,
                endTime
            };
            var result = await base.QueryFirstOrDefaultAsync<int>(sqlstr, param);
            return result;
        }

        /// <summary>
        /// 查询短链信息
        /// </summary>
        public async Task<LinkModel> GetAsync(long id)
        {
            string sqlstr = @"select *
                              from short_link 
                              where Id = @Id;";
            var param = new
            {
                Id = id,
            };
            var result = await base.QueryFirstOrDefaultAsync<LinkModel>(sqlstr, param);
            return result;
        }

        /// <summary>
        /// 分页查询短链信息
        /// </summary>
        public async Task<PageResponseDto<LinkModel>> GetListAsync(RecordListRequest dto)
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

            return await base.PageQueryAsync<LinkModel>(sb.ToString(), dto, orderSql, dto);
        }
    }
}