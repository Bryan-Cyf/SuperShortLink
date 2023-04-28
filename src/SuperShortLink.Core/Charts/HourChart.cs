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
                    output.Access[i / 10] = 1;
                    output.Generate[i / 10] = await CountAsync(hourTime, i);
                    //output.All[i / 10] = await repository.CountAsync(x => x.LongDate >= hourTime.AddMinutes(i) && x.LongDate <= hourTime.AddMinutes(i + 9).AddSeconds(59));
                    //output.Error[i / 10] = await CountAsync(LogLevelConst.Error, repository, hourTime, i);
                    //output.Info[i / 10] = await CountAsync(LogLevelConst.Info, repository, hourTime, i);
                    //output.Debug[i / 10] = await CountAsync(LogLevelConst.Debug, repository, hourTime, i);
                    //output.Fatal[i / 10] = await CountAsync(LogLevelConst.Fatal, repository, hourTime, i);
                    //output.Trace[i / 10] = await CountAsync(LogLevelConst.Trace, repository, hourTime, i);
                    //output.Warn[i / 10] = await CountAsync(LogLevelConst.Warn, repository, hourTime, i);
                }
            }
            return output;
        }

        private async Task<int> CountAsync(DateTime hourTime, int i)
        {
            return await _repository.GetGenerateCountAsync(hourTime.AddMinutes(i), hourTime.AddMinutes(i + 10));
        }
    }
}
