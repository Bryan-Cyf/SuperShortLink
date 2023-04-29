using SuperShortLink.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShortLink
{
    public interface IShortLinkService
    {
        /// <summary>
        /// 分页查询短链信息
        /// </summary>
        Task<PageResponseDto<LinkModel>> GetListAsync(RecordListRequest dto);

        /// <summary>
        /// 生成短链
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<string> GenerateAsync(string url);

        /// <summary>
        /// 访问短链
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> AccessAsync(string key);
    }
}
