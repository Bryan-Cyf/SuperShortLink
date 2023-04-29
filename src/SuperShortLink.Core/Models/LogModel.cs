using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShortLink.Models
{
    public class LogModel
    {
        public int log_id { get; set; }

        /// <summary>
        /// 短链Id
        /// </summary>
        public long link_id { get; set; }

        /// <summary>
        /// 访问IP
        /// </summary>
        public string ip { get; set; }

        /// <summary>
        /// 访问系统类型
        /// </summary>
        public int os_type { get; set; }

        /// <summary>
        /// 访问浏览器类型
        /// </summary>
        public int browser_type { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { get; set; }

    }
}
