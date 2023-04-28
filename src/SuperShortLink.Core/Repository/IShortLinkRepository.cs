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
        /// <param name="model"></param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        Task<int> InsertAsync(UrlRecordModel model);

        /// <summary>
        /// 更新短链
        /// </summary>
        /// <param name="model"></param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        Task<int> UpdateShortUrlAsync(UrlRecordModel model);

        /// <summary>
        /// 更新短链访问次数
        /// </summary>
        /// <param name="model"></param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        Task<int> UpdateAccessDataAsync(long id);

        /// <summary>
        /// 查询生成短链的数量
        /// </summary>
        Task<int> GetGenerateCountAsync(DateTime startTime, DateTime endTime);

        /// <summary>
        /// 查询短链信息
        /// </summary>
        Task<UrlRecordModel> GetAsync(long id);

        /// <summary>
        /// 查询原始链接
        /// </summary>
        Task<string> GetOriginUrlAsync(long id);

        /// <summary>
        /// 分页查询短链信息
        /// </summary>
        Task<PageResponseDto<UrlRecordModel>> GetListAsync(RecordListRequest dto);
    }
}
