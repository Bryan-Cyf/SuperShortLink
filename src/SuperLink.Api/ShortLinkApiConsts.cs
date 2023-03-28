using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SuperShortLink.Api
{
    /// <summary>
    /// 常量
    /// </summary>
    internal class ShortLinkApiConsts
    {
        public static string WaitCheckLog => $"短链-待检查";

        public static string Log => $"短链日志";

        /// <summary>
        /// 配置key
        /// </summary>
        public const string ConfigurationKey = "Export";

        /// <summary>
        /// 请求URL
        /// </summary>
        public class ApiUrl
        {
            public static string Generate = "/api/shortlink/generate";
        }

    }
}
