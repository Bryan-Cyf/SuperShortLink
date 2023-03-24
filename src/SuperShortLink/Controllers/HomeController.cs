using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SuperShortLink.Cache;
using SuperShortLink.Helpers;
using SuperShortLink.Models;
using SuperShortLink.Repository;

namespace SuperShortLink
{
    [Route("[controller]/[Action]")]
    public class HomeController : Controller
    {
        private readonly IMemoryCaching _memory;
        private readonly IShortLinkService _shortLinkService;
        private readonly IShortLinkRepository _shortLinkRepository;

        public HomeController(IShortLinkService shortLinkService, 
            IShortLinkRepository shortLinkRepository, 
            IMemoryCaching memory)
        {
            _shortLinkService = shortLinkService;
            _shortLinkRepository = shortLinkRepository;
            _memory = memory;
        }

        #region Auth

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Cookies["token"];

            var ignore = context.HttpContext.GetEndpoint().Metadata.GetMetadata<AllowAnonymousAttribute>();

            var isRedirect = true;
            if (!string.IsNullOrEmpty(token) || ignore != null)
            {
                var loginCache = _memory.Get<string>(LoginConst.CacheKey);
                if(!loginCache.IsNull && loginCache.Value == token)
                {
                    isRedirect = false;
                    base.OnActionExecuting(context);
                }
            }

            if (isRedirect)
            {
                context.HttpContext.Response.Redirect("/Login/Index");
            }
        }

        #endregion

        #region Page

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Transfer()
        {
            return View();
        }

        #endregion

        #region API

        /// <summary>
        /// 查询短链列表
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> List([FromBody] RecordListRequest request)
        {
            var pageData = await _shortLinkRepository.GetListAsync(request);

            return base.Json(new { pageData }, DateTimeConverter.Serializer);
        }

        /// <summary>
        /// 解析生成短网址，并保存到数据库
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Generate([FromBody] GenerateRequest request)
        {
            var shortURL = await _shortLinkService.GenerateAsync(request.generate_url);
            return base.Json(new { short_url = shortURL, origin_url = request.generate_url });
        }

        #endregion
    }
}