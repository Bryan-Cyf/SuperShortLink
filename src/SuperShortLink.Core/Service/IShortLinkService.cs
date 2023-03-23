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
        /// 【混淆加密】10进制转换为62进制
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string ConfusionConvert(long id);

        /// <summary>
        /// 【恢复混淆】将62进制转为10进制
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long ReConfusionConvert(string key);

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
