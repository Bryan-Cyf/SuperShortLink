using SuperShortLink.Helpers;
using SuperShortLink.Models;
using SuperShortLink.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperShortLink
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _repository;

        public ApplicationService(IApplicationRepository repository)
        {
            _repository = repository;
        }

        public async Task<PageResponseDto<ApplicationModel>> QueryPageAsync(ApplicationListRequest request)
        {
            return await _repository.QueryPageAsync(request);
        }

        public async Task<ApplicationModel> GetByCodeAsync(string appCode)
        {
            return await _repository.GetByCodeAsync(appCode);
        }

        public async Task<ApplicationModel> GetByIdAsync(int appId)
        {
            return await _repository.GetByIdAsync(appId);
        }

        public async Task<bool> AddAsync(ApplicationModel model)
        {
            model.create_time = DateTime.Now;
            model.update_time = DateTime.Now;
            model.status = StatusEnum.Valid.GetHashCode();
            var appInfo = await GetByCodeAsync(model.app_code);
            if (appInfo != null)
            {
                return false;
            }

            model.app_secret = CreateAppSecret(model.app_code);
            return await _repository.InsertAsync(model);
        }

        public async Task<bool> DeleteAsync(int appId)
        {
            var appInfo = await GetByIdAsync(appId);
            if (appInfo == null)
            {
                return false;
            }

            return await _repository.UpdateStatusAsync(appId, StatusEnum.Delete.GetHashCode());
        }

        public async Task<bool> UpdateSecretAsync(AppUpdateRequest request)
        {
            var appInfo = await GetByIdAsync(request.appId);
            if (appInfo == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(request.appSecret))
            {
                appInfo.app_secret = CreateAppSecret(appInfo.app_code);
            }
            else
            {
                appInfo.app_secret = request.appSecret;
            }

            return await _repository.UpdateSecretAsync(request.appId, appInfo.app_secret);
        }

        private string CreateAppSecret(string appCode)
        {
            return (appCode + DateTime.Now.ToUnixTimestamp()).ToMd5();
        }
    }
}
