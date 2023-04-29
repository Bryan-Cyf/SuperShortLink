using SuperShortLink.Cache;
using SuperShortLink.Helpers;
using SuperShortLink.Models;
using SuperShortLink.Repository;
using System.Threading.Tasks;

namespace SuperShortLink
{
    public class ShortLinkService : IShortLinkService
    {
        private readonly IShortLinkRepository _repository;
        private readonly IMemoryCaching _memory;
        private readonly Base62Converter _converter;
        private readonly ILogService _logService;
        public ShortLinkService(IShortLinkRepository repository,
            IMemoryCaching memory,
            Base62Converter converter,
            ILogService logService)
        {
            _repository = repository;
            _memory = memory;
            _converter = converter;
            _logService = logService;
        }

        /// <summary>
        /// 分页查询短链信息
        /// </summary>
        public async Task<PageResponseDto<LinkModel>> GetListAsync(RecordListRequest dto)
        {
            return await _repository.GetListAsync(dto);
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

            var model = LinkModel.GenDefault(originUrl);

            var id = await _repository.InsertAsync(model);
            var shortKey = _converter.Confuse(id);

            model.id = id;
            model.short_url = shortKey;

            await _repository.UpdateShortUrlAsync(model);
            _memory.Set(originUrl, shortKey);
            _memory.Set(shortKey, originUrl);
            return shortKey;
        }

        /// <summary>
        /// 访问短链
        /// </summary>
        /// <param name="shortKey"></param>
        /// <returns></returns>
        public async Task<string> AccessAsync(string shortKey)
        {
            var id = _converter.ReCoverConfuse(shortKey);

            if (id == 0)
            {
                return string.Empty;
            }

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
                await _logService.Insert(id);
            }
            else
            {
                //防止缓存击穿
                _memory.Set(shortKey, string.Empty);
            }

            return originUrl;
        }
    }
}
