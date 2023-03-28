using SuperShortLink.Models;
using System.Threading.Tasks;

namespace SuperShortLink.Repository
{
    public interface IApplicationRepository
    {
        Task<PageResponseDto<ApplicationModel>> QueryPageAsync(ApplicationListRequest dto);

        Task<ApplicationModel> GetByCodeAsync(string appCode);

        Task<ApplicationModel> GetByIdAsync(int appId);

        Task<bool> InsertAsync(ApplicationModel model);

        Task<bool> UpdateStatusAsync(int appId, int status);

        Task<bool> UpdateSecretAsync(int appId, string secret);
    }
}
