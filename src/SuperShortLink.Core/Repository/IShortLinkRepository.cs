using SuperShortLink.Models;
using System;
using System.Threading.Tasks;

namespace SuperShortLink.Repository
{
    public interface IShortLinkRepository
    {

        /// <summary>
        /// 插入
        /// </summary>
        Task<int> InsertAsync(LinkModel model);

        /// <summary>
        /// 更新短链
        /// </summary>
        Task<int> UpdateShortUrlAsync(LinkModel model);

        /// <summary>
        /// 更新短链访问次数
        /// </summary>
        Task<int> UpdateAccessDataAsync(long id);

        /// <summary>
        /// 查询生成短链的数量
        /// </summary>
        Task<int> GetCountAsync(DateTime startTime, DateTime endTime);

        /// <summary>
        /// 查询短链信息
        /// </summary>
        Task<LinkModel> GetAsync(long id);

        /// <summary>
        /// 查询原始链接
        /// </summary>
        Task<string> GetOriginUrlAsync(long id);

        /// <summary>
        /// 分页查询短链信息
        /// </summary>
        Task<PageResponseDto<LinkModel>> GetListAsync(RecordListRequest dto);
    }
}
