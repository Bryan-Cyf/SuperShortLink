using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SuperShortLink.Cache;
using SuperShortLink.Models;

namespace SuperShortLink
{
    public class LoginController : Controller
    {
        private readonly IShortLinkService _shortLinkService;
        private readonly LoginModel _loginInfo;

        public LoginController(IShortLinkService shortLinkService,
            IOptionsSnapshot<ShortLinkOptions> option)
        {
            _shortLinkService = shortLinkService;
            _loginInfo = new LoginModel(option.Value.LoginAcount, option.Value.LoginPassword);
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
                HttpContext.Response.Cookies.Append("token", LoginConst.GetToken(_loginInfo.UserName, _loginInfo.Password),
                    new CookieOptions() { HttpOnly = true, Expires = DateTimeOffset.Now.AddHours(2) });
                return Redirect("/Home/Index");
            }

            return View(nameof(Index));
        }
    }
}