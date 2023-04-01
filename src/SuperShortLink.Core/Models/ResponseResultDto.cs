using SuperShortLink.Helpers;
using System;
using System.Collections.Generic;

namespace SuperShortLink.Models
{
    /// <summary>
    /// 分页模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResponseDTO<T>
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

    public class ResponseModel
    {
        public int resultCode { get; set; }
        public string resultMsg { get; set; }

        //public int resultCode { get; set; }
        //public string resultMsg { get; set; }
    }

    public class ResponseModel<T> : ResponseModel
    {
        /// <summary>
        /// API接口 返回结果
        /// </summary>
        public T resultData { get; set; }
    }


    public static class ResponseModelExtend
    {
        public static int ResponseSuccess = ResponseStatusEnum.Success.GetHashCode();
        public static string ResponseSuccessMsg = ResponseStatusEnum.Success.GetDescription();

        public static int ResponseTokenError = ResponseStatusEnum.TokenError.GetHashCode();

        public static bool IsSuccess<T>(this T model) where T : ResponseModel
        {
            return model.resultCode.Equals(ResponseSuccess);
        }

        public static void SetSuccess<T>(this T model) where T : ResponseModel
        {
            model.resultCode = ResponseSuccess;
            model.resultMsg = ResponseSuccessMsg;
        }
        public static void SetSuccess<T>(this ResponseModel<T> model, T resultData)
        {
            model.SetSuccess();
            model.resultData = resultData;
        }

        public static void SetResult<T>(this T model, Enum resultEnum, string msg = "") where T : ResponseModel
        {
            model.resultCode = resultEnum.GetHashCode();
            if (string.IsNullOrEmpty(msg))
            {
                model.resultMsg = resultEnum.GetDescription();
            }
            else
            {
                model.resultMsg = msg;
            }
        }

        public static void SetResult<T>(this T model, ResponseModel result) where T : ResponseModel
        {
            model.resultCode = result.resultCode;
            model.resultMsg = result.resultMsg;
        }

        public static void SetResult<T>(this T model, int resultCode, string msg) where T : ResponseModel
        {
            model.resultCode = resultCode;
            model.resultMsg = msg;
        }

        public static void SetError<T>(this T model, ResponseStatusEnum code = ResponseStatusEnum.Error) where T : ResponseModel
        {
            model.SetResult(code);
        }

        public static void SetError<T>(this T model, string msg, ResponseStatusEnum code = ResponseStatusEnum.Error) where T : ResponseModel
        {
            model.SetResult(code, msg);
        }

        public static bool IsTokenInvalid<T>(this T model) where T : ResponseModel
        {
            return model.resultCode.Equals(ResponseTokenError);
        }
    }

    public class ResponseListModel<T>
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int currentPage { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int pages { get; set; }

        /// <summary>
        /// 记录总数
        /// </summary>
        public int total { get; set; }

        public long scrollId { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public IEnumerable<T> results { get; set; }
    }
}
