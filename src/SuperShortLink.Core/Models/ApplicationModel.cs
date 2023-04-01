using System;

namespace SuperShortLink.Models
{
    /// <summary>
    /// 应用账号
    /// </summary>
    public class ApplicationModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int app_id { get; set; }

        /// <summary>
        /// 应用code
        /// </summary>
        public string app_code { get; set; }

        /// <summary>
        /// 应用秘钥
        /// </summary>
        public string app_secret { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string app_name { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string remark { get; set; } = string.Empty;

        public DateTime create_time { get; set; }

        public DateTime update_time { get; set; }
        public int status { get; set; }
    }
}
