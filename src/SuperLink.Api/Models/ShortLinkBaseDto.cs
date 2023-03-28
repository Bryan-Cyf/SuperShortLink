using System;
using System.Collections.Generic;
using System.Text;

namespace SuperShortLink.Api.Models
{
    internal class ShortLinkBaseDto
    {
        /// <summary>
        /// 应用code
        /// </summary>
        public string app_code { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; }

    }
}
