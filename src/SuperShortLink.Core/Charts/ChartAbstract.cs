using SuperShortLink.Models;
using SuperShortLink.Repository;
using System;
using System.Threading.Tasks;

namespace SuperShortLink.Charts
{
    public abstract class ChartAbstract : IChart
    {
        private readonly IShortLinkRepository _linkRepository;
        private readonly ILogRepository _logRepository;

        public ChartAbstract(IShortLinkRepository inkRepository, 
            ILogRepository logRepository)
        {
            _linkRepository = inkRepository;
            _logRepository = logRepository;
        }

        public abstract ChartTypeEnum ChartType { get; }

        public abstract Task<GetChartsOutput> GetCharts();

        protected async Task<int> GetGenerateCountAsync(DateTime start, DateTime end)
        {
            return await _linkRepository.GetCountAsync(start, end);
        }

        protected async Task<int> GetAccessCountAsync(DateTime start, DateTime end)
        {
            return await _logRepository.GetCountAsync(start, end);
        }
    }
}
