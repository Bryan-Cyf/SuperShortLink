using Microsoft.AspNetCore.Http;
using SuperShortLink.Models;
using SuperShortLink.Repository;
using System.Threading.Tasks;
using System;
using Tools.HttpCore;

namespace SuperShortLink
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogService(ILogRepository repository,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 插入访问记录
        /// </summary>
        public async Task Insert(long id)
        {
            var model = new LogModel
            {
                link_id = id,
                create_time = DateTime.Now,
                ip = _httpContextAccessor.HttpContext.GetIpAddress(),
                os_type = (int)_httpContextAccessor.HttpContext.GetSystemType(),
                browser_type = (int)_httpContextAccessor.HttpContext.GetBrowserType()
            };

            await _repository.InsertAsync(model);
        }
    }
}
