using SuperShortLink.Models;
using SuperShortLink.Repository;
using System;
using System.Threading.Tasks;

namespace SuperShortLink.Charts
{
    public class HourChart : IChart
    {
        public ChartTypeEnum ChartType => ChartTypeEnum.Hour;
        private readonly IShortLinkRepository _repository;

        public HourChart(IShortLinkRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetChartsOutput> GetCharts()
        {
            var dtNow = DateTime.Now;
            var output = new GetChartsOutput(6);
            var index = 0;

            for (var i = 60; i > 0; i -= 10)
            {
                output.Access[index] = 1;
                output.Generate[index] = await CountAsync(dtNow, i);
                output.Labels[index] = dtNow.AddMinutes(-i).ToString("HH:mm:ss");
                index++;
            }
            return output;
        }

        private async Task<int> CountAsync(DateTime time, int i)
        {
            return await _repository.GetGenerateCountAsync(time.AddMinutes(-i), time.AddMinutes(-i + 10));
        }
    }
}
