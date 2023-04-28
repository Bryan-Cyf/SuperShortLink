using SuperShortLink.Models;
using SuperShortLink.Repository;
using System;
using System.Threading.Tasks;

namespace SuperShortLink.Charts
{
    public abstract class ChartAbstract : IChart
    {
        private readonly IShortLinkRepository _repository;

        public ChartAbstract(IShortLinkRepository repository)
        {
            _repository = repository;
        }

        public abstract ChartTypeEnum ChartType { get; }

        public abstract Task<GetChartsOutput> GetCharts();

        protected async Task<int> GetGenerateCountAsync(DateTime start, DateTime end)
        {
            return await _repository.GetGenerateCountAsync(start, end);
        }
    }
}
