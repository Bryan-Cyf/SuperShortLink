using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SuperShortLink.Models
{
    public enum ResponseStatusEnum
    {
        /// <summary>
        /// token无效
        /// </summary>
        [Description("企业未授权")]
        EnterpriseUnAuthorize = 101,

        /// <summary>
        /// 操作成功
        /// </summary>
        [Description("操作成功")]
        Success = 200,

        /// <summary>
        /// 操作失败
        /// </summary>
        [Description("操作失败")]
        Failure = 201,

        /// <summary>
        /// 操作异常
        /// </summary>
        [Description("操作异常")]
        Error = 203,

        /// <summary>
        /// 权限不足
        /// </summary>
        [Description("权限不足")]
        Unauthorized = 204,

        /// <summary>
        /// 登录失效
        /// </summary>
        [Description("登录失效")]
        NoLogin = 205,

        /// <summary>
        /// 参数错误
        /// </summary>
        [Description("参数错误")]
        ParamsError = 206,

        /// <summary>
        /// token无效
        /// </summary>
        [Description("token无效")]
        TokenError = 207,
    }
}
