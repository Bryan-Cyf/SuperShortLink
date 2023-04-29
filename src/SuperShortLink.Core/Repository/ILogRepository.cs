using SuperShortLink.Models;
using System;
using System.Threading.Tasks;

namespace SuperShortLink.Repository
{
    public interface ILogRepository
    {
        /// <summary>
        /// 插入
        /// </summary>
        Task InsertAsync(LogModel model);

        /// <summary>
        /// 查询短链访问的数量
        /// </summary>
        Task<int> GetCountAsync(DateTime startTime, DateTime endTime);
    }
}
