using Microsoft.AspNetCore.Mvc;
using SuperShortLink.Models;

namespace SuperShortLink
{
    [ApiAuthrize]
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
        [HttpPost]
        public async Task<IActionResult> Generate([FromBody] ApiGenerateRequest request)
        {
            var result = new ResponseModel<string>();
            result.resultData = await _shortLinkService.GenerateAsync(request.generate_url);
            if (!string.IsNullOrEmpty(result.resultData))
            {
                result.SetSuccess();
            }
            else
            {
                result.SetError();
            }

            return base.Json(result);
        }
    }
}
