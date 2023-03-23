using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShortLink.Models
{
    public class UrlRecordModel
    {
        public int id { get; set; }

        /// <summary>
        /// 短连接
        /// </summary>
        public string short_url { get; set; }

        /// <summary>
        /// 原始链接
        /// </summary>
        public string origin_url { get; set; }

        /// <summary>
        /// 访问次数
        /// </summary>
        public int access_count { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime update_time { get; set; }

        /// <summary>
        /// 生成默认模型
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static UrlRecordModel GenDefault(string url)
        {
            return new UrlRecordModel()
            {
                create_time = DateTime.Now,
                update_time = DateTime.Now,
                short_url = string.Empty,
                origin_url = url,
            };
        }
    }
}
