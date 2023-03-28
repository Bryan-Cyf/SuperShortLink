using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SuperShortLink.Models
{
    /// <summary>
    /// 状态枚举
    /// </summary>
    internal enum StatusEnum
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Description("默认")]
        Default = 0,

        /// <summary>
        /// 有效
        /// </summary>
        [Description("有效")]
        Valid = 2,

        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = -99
    }
}
