using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SuperShortLink.Cache;
using SuperShortLink.Models;

namespace SuperShortLink
{
    public class LoginController : Controller
    {
        private readonly IMemoryCaching _memory;
        private readonly IShortLinkService _shortLinkService;
        private readonly LoginModel _loginInfo;

        public LoginController(IShortLinkService shortLinkService,
            IOptionsSnapshot<ShortLinkOptions> option,
            IMemoryCaching memory)
        {
            _shortLinkService = shortLinkService;
            _loginInfo = new LoginModel(option.Value.LoginAcount, option.Value.LoginPassword);
            _memory = memory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string name, string password)
        {
            if (_loginInfo.UserName == name.Trim() && _loginInfo.Password == password.Trim())
            {
                var guid = Guid.NewGuid().ToString();
                _memory.Set(LoginConst.CacheKey, guid);
                HttpContext.Response.Cookies.Append("token", guid,
                    new CookieOptions() { HttpOnly = true, Expires = DateTimeOffset.Now.AddDays(1) });
                return Redirect("/Home/Index");
            }

            return View(nameof(Index));
        }
    }
}