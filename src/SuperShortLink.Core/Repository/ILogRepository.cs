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
    }
}
