using Dapper;
using Microsoft.Extensions.Options;
using SuperShortLink.Models;
using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SuperShortLink.Repository
{
    public class LogRepository : BaseRepository, ILogRepository
    {
        private readonly string _autoIncrementSql;
        public LogRepository(IOptionsSnapshot<ShortLinkOptions> options) : base(options)
        {
            _autoIncrementSql = options.Value.AutoIncrementSQL;
        }

        /// <summary>
        /// 插入记录
        /// </summary>
        public async Task InsertAsync(LogModel model)
        {
            var sb = new StringBuilder(
              @"insert into short_link_log
                    (link_id, ip, os_type, browser_type, create_time)
                values
                    (@link_id, @ip, @os_type, @browser_type, @create_time);");

            await base.ExecuteAsync(sb.ToString(), model);
        }

        /// <summary>
        /// 查询短链访问的数量
        /// </summary>
        public async Task<int> GetCountAsync(DateTime startTime, DateTime endTime)
        {
            string sqlstr = @"select count(1)
                              from short_link_log 
                              where create_time >= @startTime and create_time < @endTime;";
            var param = new
            {
                startTime,
                endTime
            };
            var result = await base.QueryFirstOrDefaultAsync<int>(sqlstr, param);
            return result;
        }
    }
}