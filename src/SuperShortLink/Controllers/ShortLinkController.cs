using Microsoft.AspNetCore.Mvc;

namespace SuperShortLink
{
    [Route("api/[controller]/[Action]")]
    public class ShortLinkController : Controller
    {
        private readonly IShortLinkService _shortLinkService;
        public ShortLinkController(IShortLinkService shortLinkService)
        {
            _shortLinkService = shortLinkService;
        }

        /// <summary>
        /// 解析生成短网址，并保存到数据库
        /// </summary>
        /// <param name="url">长链接</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Generate(string url)
        {
            var short_url = await _shortLinkService.GenerateAsync(url);
            return short_url;
        }
    }
}
