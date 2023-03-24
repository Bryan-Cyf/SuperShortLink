using Microsoft.AspNetCore.Mvc;

namespace SuperShortLink
{
    //[Route("[controller]/[Action]")]
    public class ShortLinkController : Controller
    {
        private readonly IShortLinkService _shortLinkService;
        public ShortLinkController(IShortLinkService shortLinkService)
        {
            _shortLinkService = shortLinkService;
        }

        [HttpGet("{key?}")]
        public async Task<IActionResult> Access(string key)
        {
            var url = await _shortLinkService.AccessAsync(key);
            if (!string.IsNullOrWhiteSpace(url))
            {
                return Redirect(url);
            }
            return new NotFoundResult();
        }
    }
}
