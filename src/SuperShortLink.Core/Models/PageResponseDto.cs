using System.Collections.Generic;

namespace SuperShortLink.Models
{
    /// <summary>
    /// 分页模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResponseDto<T>
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public long currentPage { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public long pages { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public long total { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public IEnumerable<T> results { get; set; }
    }
}
