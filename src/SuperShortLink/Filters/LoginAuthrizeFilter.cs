using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SuperShortLink.Cache;
using SuperShortLink.Models;

namespace SuperShortLink
{
    public class LoginAuthrizeAttribute : TypeFilterAttribute
    {
        public LoginAuthrizeAttribute() : base(typeof(LoginAuthrizeFilter))
        {

        }
    }

    public class LoginAuthrizeFilter : IAuthorizationFilter
    {
        private readonly IMemoryCaching _memory;

        public LoginAuthrizeFilter(IMemoryCaching memory)
        {
            _memory = memory;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (IsAllowAnonymous(context))
            {
                return;
            }

            var token = context.HttpContext.Request.Cookies["token"];
            if (!string.IsNullOrEmpty(token))
            {
                var loginCache = _memory.Get<string>(LoginConst.CacheKey);
                if (!loginCache.IsNull && loginCache.Value == token)
                {
                    return;
                }
            }

            context.HttpContext.Response.Redirect("/Login/Index");
            context.Result = new ObjectResult("无效凭证");
        }


        private bool IsAllowAnonymous(AuthorizationFilterContext context)
        {
            var hasAttribute = false;
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                hasAttribute =
                    controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true).Any(a => a.GetType() == typeof(AllowAnonymousAttribute))
                    || controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(inherit: true).Any(a => a.GetType() == typeof(AllowAnonymousAttribute));
            }
            return hasAttribute;
        }
    }
}
