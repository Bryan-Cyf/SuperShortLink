using System;
using System.Collections.Generic;

namespace SuperShortLink.Api.Models
{
    /// <summary>
    /// 分页模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExportPageResponseDTO<T>
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

    internal class ResponseModel
    {
        public int resultCode { get; set; }

        public string resultMsg { get; set; }
    }

    internal class ResponseModel<T> : ResponseModel
    {
        /// <summary>
        /// API接口 返回结果
        /// </summary>
        public T resultData { get; set; }
    }

    internal static class ResponseExtend
    {
        public static bool IsSuccess(this ResponseModel model)
        {
            return model != null && model.resultCode == 200;
        }
    }
}
