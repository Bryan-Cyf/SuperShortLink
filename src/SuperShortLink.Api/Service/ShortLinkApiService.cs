using Microsoft.Extensions.Options;
using SuperShortLink.Api.Models;
using SuperShortLink.Api.Options;
using System;
using System.Threading.Tasks;

namespace SuperShortLink.Api
{
    internal class ShortLinkApiService : IShortLinkApiService
    {
        private readonly ShortLinkHttpRequest _httpRequest;
        private readonly ShortLinkApiOptions _option;

        public ShortLinkApiService(ShortLinkHttpRequest exportHttpRequest
            , IOptionsSnapshot<ShortLinkApiOptions> option)
        {
            _httpRequest = exportHttpRequest;
            _option = option.Value;
        }

        public async Task<string> GenerateAsync(string origin_url)
        {
            try
            {
                var dto = new ShortLinkGenerateDto()
                {
                    generate_url = origin_url
                };

                var response = await _httpRequest.PostAsync<ResponseModel<string>>(dto, ShortLinkApiConsts.ApiUrl.Generate);
                return $"{_option.ApiDomain}/{response.resultData}";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}