using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SuperShortLink.Models;
using SuperShortLink.Helpers;
namespace SuperShortLink
{
    public class ApiAuthrizeAttribute : TypeFilterAttribute
    {
        public ApiAuthrizeAttribute() : base(typeof(ApiAuthrizeFilter))
        {

        }
    }

    public class ApiAuthrizeFilter : Attribute, IAsyncActionFilter
    {
        private readonly ILogger<ApiAuthrizeFilter> _logger;
        private readonly IApplicationService _applicationService;

        public ApiAuthrizeFilter(IApplicationService applicationService
            , ILogger<ApiAuthrizeFilter> logger)
        {
            _applicationService = applicationService;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var allowAnonymous = context.HttpContext.GetEndpoint().Metadata.GetMetadata<IAllowAnonymous>();
            if (allowAnonymous != null)
            {
                await next();
                return;
            }

            //获取参数
            var dic = GetParamsDictionary(context);
            if (dic == null)
            {
                _logger.LogWarning($"{nameof(ApiAuthrizeFilter)}--获取参数失败");
                return;
            }

            //验证时间
            var isValid = CheckTimestamp(context, dic);
            if (!isValid)
            {
                _logger.LogWarning($"{nameof(ApiAuthrizeFilter)}--验证时间失败--{System.Text.Json.JsonSerializer.Serialize(dic)}");
                return;
            }

            //验证app_code
            var appInfo = await CheckAppCodeAsync(context, dic);
            if (appInfo == null)
            {
                _logger.LogWarning($"{nameof(ApiAuthrizeFilter)}--验证app_code失败--{System.Text.Json.JsonSerializer.Serialize(dic)}");
                return;
            }

            //验证token
            isValid = CheckToken(context, dic, appInfo);
            if (!isValid)
            {
                return;
            }

            await next();
        }

        private bool CheckToken(ActionExecutingContext context, Dictionary<string, string> dic, ApplicationModel appInfo)
        {
            var token = context.HttpContext.Request.Headers["token"].FirstOrDefault();
            var timestampStr = dic[nameof(ApiBaseDto.timestamp)];
            var originStr = $"timestamp={timestampStr}&&app_secret={appInfo.app_secret}";
            var decryMd5 = originStr.ToMd5();

            if (!token.IsNullOrEmpty() && token == decryMd5)
            {
                return true;
            }

            _logger.LogWarning($"{nameof(ApiAuthrizeFilter)}--验证token失败 - 传入token【{token}】，系统生成token【{decryMd5}】,参数字符串【{originStr}】");
            context.Result = new UnauthorizedObjectResult(new ResponseModel { resultMsg = "接口权限验证失败[token]" });
            return false;
        }

        private async Task<ApplicationModel> CheckAppCodeAsync(ActionExecutingContext context, Dictionary<string, string> dic)
        {
            try
            {
                if (dic.ContainsKey(nameof(ApiBaseDto.app_code)))
                {
                    var appInfo = await _applicationService.GetByCodeAsync(dic[nameof(ApiBaseDto.app_code)]);
                    if (appInfo != null)
                    {
                        return appInfo;
                    }
                }

                context.Result = new UnauthorizedObjectResult(new ResponseModel { resultMsg = "接口权限验证失败[app_code]" });
                return null;
            }
            catch (Exception e)
            {

                context.Result = new UnauthorizedObjectResult(new ResponseModel { resultMsg = $"接口权限验证失败[app_code]:{e.Message}" });
                return null;
            }
        }

        private bool CheckTimestamp(ActionExecutingContext context, Dictionary<string, string> dic)
        {
            if (dic.ContainsKey(nameof(ApiBaseDto.timestamp)))
            {
                var timestampStr = dic[nameof(ApiBaseDto.timestamp)];
                var offsetTime = 60; //1分钟内的请求校验

                if (double.TryParse(timestampStr, out var time) &&
                    time <= DateTime.Now.ToUnixTimestamp() &&
                    time > DateTime.Now.AddSeconds(-offsetTime).ToUnixTimestamp())
                {
                    return true;
                }
            }

            context.Result = new UnauthorizedObjectResult(new ResponseModel { resultMsg = "接口权限验证失败[timestamp]" });
            return false;
        }

        private Dictionary<string, string> GetParamsDictionary(ActionExecutingContext context)
        {
            var paramsObj = context.ActionArguments.FirstOrDefault().Value;
            if (paramsObj != null)
            {
                var dic = new Dictionary<string, string>();

                foreach (var c in paramsObj.GetType().GetProperties().OrderBy(c => c.Name))
                {
                    var val = c.GetValue(paramsObj);
                    dic.Add(c.Name, val?.ToString());
                }

                return dic;
            }

            context.Result = new UnauthorizedObjectResult(new ResponseModel() { resultMsg = "接口权限验证失败[参数]" });
            return null;
        }
    }
}
