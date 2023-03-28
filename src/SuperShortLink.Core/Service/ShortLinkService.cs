using Microsoft.Extensions.Options;
using SuperShortLink.Cache;
using SuperShortLink.Helpers;
using SuperShortLink.Models;
using SuperShortLink.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShortLink
{
    /// <summary>
    /// 转短码：
    /// 1、根据自增主键id前面补0，如：00000123
    /// 2、倒转32100000
    /// 3、把倒转后的十进制转六十二进制（乱序后）
    /// 解析短码：
    /// 1、六十二进制转十进制，得到如：32100000
    /// 2、倒转00000123，得到123
    /// 3、根据123作为主键去数据库查询映射对象
    /// 
    /// https://www.cnblogs.com/xmlnode/p/4544302.html
    /// </summary>
    public class ShortLinkService : IShortLinkService
    {
        private readonly ShortLinkOptions _option;
        private readonly IShortLinkRepository _repository;
        private readonly IMemoryCaching _memory;

        public ShortLinkService(IOptionsSnapshot<ShortLinkOptions> option,
            IShortLinkRepository repository,
            IMemoryCaching memory)
        {
            _option = option.Value;
            _repository = repository;
            _memory = memory;
        }

        /// <summary>
        /// 生成短链
        /// </summary>
        public async Task<string> GenerateAsync(string originUrl)
        {
            if (!UrlHelper.CheckVaild(originUrl))
            {
                return null;
            }

            var cacheUrl = _memory.Get<string>(originUrl);
            if (!cacheUrl.IsNull)
            {
                return cacheUrl.Value;
            }

            var model = UrlRecordModel.GenDefault(originUrl);

            var id = await _repository.InsertAsync(model);
            var shortKey = ConfusionConvert(id);

            model.id = id;
            model.short_url = shortKey;

            await _repository.UpdateShortUrlAsync(model);
            _memory.Set(originUrl, shortKey);
            _memory.Set(shortKey, originUrl);
            return shortKey;
        }

        /// <summary>
        /// 分页查询短链信息
        /// </summary>
        public async Task<PageResponseDto<UrlRecordModel>> GetListAsync(RecordListRequest dto)
        {
            return await _repository.GetListAsync(dto);
        }

        /// <summary>
        /// 访问短链
        /// </summary>
        /// <param name="shortKey"></param>
        /// <returns></returns>
        public async Task<string> AccessAsync(string shortKey)
        {
            var id = ReConfusionConvert(shortKey);

            var cacheUrl = _memory.Get<string>(shortKey);
            var originUrl = !cacheUrl.IsNull ?
                            cacheUrl.Value :
                            await _repository.GetOriginUrlAsync(id);

            if (!string.IsNullOrEmpty(originUrl))
            {
                if (cacheUrl.IsNull)
                {
                    _memory.Set(shortKey, originUrl);
                }

                await _repository.UpdateAccessDataAsync(id);
            }

            return originUrl;
        }

        /// <summary>
        /// 【混淆加密】10进制转换为62进制
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string ConfusionConvert(long id)
        {
            if (id.ToString().Length > MaxLength)
                throw new Exception($"转换值不能超过最大位数{MaxLength}");

            var key = id.ToString()
                   .PadLeft(MaxLength, '0')
                   .ToCharArray()
                   .Reverse();
            return Convert(long.Parse(string.Join("", key))).PadLeft(_option.CodeLength, _option.Secrect.First());
        }

        /// <summary>
        /// 【恢复混淆】将62进制转为10进制
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long ReConfusionConvert(string key)
        {
            if (key.Length > _option.CodeLength + 1)
                throw new Exception($"转换值不能超过最大位数{_option.CodeLength + 1}");

            var id = Convert(key).ToString().PadLeft(MaxLength, '0')
                .ToCharArray()
                .Reverse();

            return long.Parse(string.Join("", id));
        }

        /// <summary>
        /// 注意：超过设定的长度可能会有异常数据
        /// </summary>
        private int MaxLength
        {
            get
            {
                return Convert("".PadLeft(_option.CodeLength, _option.Secrect.Last())).ToString().Length;
            }
        }

        /// <summary>
        /// 10进制转换为62进制【简单】
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string Convert(long id)
        {
            if (id < 62)
            {
                return _option.Secrect[(int)id].ToString();
            }
            int y = (int)(id % 62);
            long x = id / 62;

            return Convert(x) + _option.Secrect[y];
        }

        /// <summary>
        /// 将62进制转为10进制【简单】
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private long Convert(string key)
        {
            long v = 0;
            int Len = key.Length;
            for (int i = Len - 1; i >= 0; i--)
            {
                int t = _option.Secrect.IndexOf(key[i]);
                double s = (Len - i) - 1;
                long m = (long)(Math.Pow(62, s) * t);
                v += m;
            }
            return v;
        }

    }
}
