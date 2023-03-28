using SuperShortLink.Api.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SuperShortLink.Api
{
    /// <summary>
    /// 短链Api接口
    /// </summary>
    public interface IShortLinkApiService
    {
        /// <summary>
        /// 生成短链
        /// </summary>

        Task<string> GenerateAsync(ShortLinkGenerateRequest request);
    }
}
