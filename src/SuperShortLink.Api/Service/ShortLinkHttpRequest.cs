using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SuperShortLink.Api.Helpers;
using SuperShortLink.Api.Models;
using SuperShortLink.Api.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SuperShortLink.Api
{
    internal class ShortLinkHttpRequest
    {
        private static readonly HttpClient _client = new HttpClient();

        private readonly ShortLinkApiOptions _option;

        public ShortLinkHttpRequest(IOptionsSnapshot<ShortLinkApiOptions> option)
        {
            _option = option.Value;
        }

        public async Task<T> PostAsync<T>(ShortLinkBaseDto dto, string url) where T : ResponseModel
        {
            var response = default(T);
            dto.app_code = _option.AppCode;
            dto.timestamp = DateTime.Now.ToUnixTimestamp().ToString();

            var requestApiUrl = _option.ApiDomain + url;
            Dictionary<string, string> headers = GetHeader(dto);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestApiUrl);
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            foreach (var header in headers)
            {
                requestMessage.Headers.Add(header.Key, header.Value);
            }
            var httpResponseMessage = await _client.SendAsync(requestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var resultContent = await httpResponseMessage.Content.ReadAsStringAsync();
                response = resultContent.ToObj<T>();
            }

            return response;
        }

        /// <summary>
        /// 获取请求报文头
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetHeader(ShortLinkBaseDto dto)
        {
            var token = GenerateToken(dto);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("token", token);
            return headers;
        }

        /// <summary>
        /// 生成token
        /// </summary>
        /// <returns></returns>
        private string GenerateToken(ShortLinkBaseDto dto)
        {
            var originStr = $"timestamp={dto.timestamp}&&app_secret={_option.AppSecret}";
            var token = originStr.ToMd5();
            return token;
        }
    }
}
