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
            var now = DateTime.Now;
            var minute = now.Minute;
            var hourTime = DateTime.Now.Date.AddHours(now.Hour);
            var output = new GetChartsOutput(6);

            for (var i = 0; i < 60; i += 10)
            {
                if (i > minute)
                {
                    output.Access[i / 10] = 0;
                    output.Generate[i / 10] = 0;
                }
                else
                {
                    output.Access[i / 10] = 0;
                    output.Generate[i / 10] = await CountAsync(hourTime, i);
                }
                output.Labels[i / 10] = hourTime.AddMinutes(i).ToString("HH:mm:ss");
            }
            return output;
        }

        private async Task<int> CountAsync(DateTime time, int i)
        {
            return await _repository.GetGenerateCountAsync(time.AddMinutes(i), time.AddMinutes(i + 10));
        }
    }
}
