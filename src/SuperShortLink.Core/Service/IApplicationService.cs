using SuperShortLink.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperShortLink
{
    public interface IApplicationService
    {
        Task<PageResponseDto<ApplicationModel>> QueryPageAsync(ApplicationListRequest request);

        Task<ApplicationModel> GetByCodeAsync(string appCode);

        Task<ApplicationModel> GetByIdAsync(int appId);

        Task<bool> AddAsync(ApplicationModel model);

        Task<bool> DeleteAsync(int appId);

        Task<bool> UpdateSecretAsync(AppUpdateRequest request);
    }
}
