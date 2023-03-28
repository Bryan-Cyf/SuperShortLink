using Microsoft.Extensions.Options;
using SuperShortLink.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SuperShortLink.Repository
{
    public class ApplicationRepository : BaseRepository, IApplicationRepository
    {
        public ApplicationRepository(IOptionsSnapshot<ShortLinkOptions> options) : base(options)
        {
        }

        public async Task<PageResponseDto<ApplicationModel>> QueryPageAsync(ApplicationListRequest dto)
        {
            var sb = new StringBuilder($"select * from short_link_appication where status={StatusEnum.Valid.GetHashCode()} ");

            if (!string.IsNullOrEmpty(dto.app_code))
            {
                sb.Append(" and app_code like @app_code ");
                dto.app_name = $"%{dto.app_code}%";
            }

            if (!string.IsNullOrEmpty(dto.app_name))
            {
                sb.Append(" and app_name like @app_name ");
                dto.app_name = $"%{dto.app_name}%";
            }

            var orderSql = " order by app_id desc";

            return await base.PageQueryAsync<ApplicationModel>(sb.ToString(), dto, orderSql, dto);
        }

        public async Task<ApplicationModel> GetByCodeAsync(string appCode)
        {
            var sql = "select * from short_link_appication where  app_code = @appCode and status = @status limit 1";
            return await base.QueryFirstOrDefaultAsync<ApplicationModel>(sql, new { appCode, status = StatusEnum.Valid.GetHashCode() });
        }

        public async Task<ApplicationModel> GetByIdAsync(int appId)
        {
            var sql = "select * from short_link_appication where  app_id = @appId and status = @status limit 1";
            return await QueryFirstOrDefaultAsync<ApplicationModel>(sql, new { appId, status = StatusEnum.Valid.GetHashCode() });
        }

        public async Task<bool> InsertAsync(ApplicationModel model)
        {
            var sql = @"insert into short_link_appication
                                         (app_code, app_name, remark,create_time,status,app_secret,update_time) 
                                   values
                                         (@app_code, @app_name, @remark, @create_time, @status, @app_secret, @update_time);";
            return await base.ExecuteScalarAsync<bool>(sql, model);
        }

        public async Task<bool> UpdateStatusAsync(int appId, int status)
        {
            var sql = $"update short_link_appication set update_time =@update_time,status=@status  where app_id = @appId";
            return await base.ExecuteAsync(sql, new { appId, status = status, update_time = DateTime.Now }) > 0;
        }

        public async Task<bool> UpdateSecretAsync(int appId, string secret)
        {
            var sql = $"update short_link_appication set update_time =@update_time,app_secret=@secret  where app_id = @appId";
            return await base.ExecuteAsync(sql, new { appId, secret, update_time = DateTime.Now }) > 0;
        }
    }
}
