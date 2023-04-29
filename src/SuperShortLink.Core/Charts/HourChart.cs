using SuperShortLink.Models;
using SuperShortLink.Repository;
using System;
using System.Threading.Tasks;

namespace SuperShortLink.Charts
{
    public class HourChart : ChartAbstract
    {
        public override ChartTypeEnum ChartType => ChartTypeEnum.Hour;

        public HourChart(IShortLinkRepository linkRepository,
            ILogRepository logRepository) : base(linkRepository, logRepository)
        {

        }

        public override async Task<GetChartsOutput> GetCharts()
        {
            var now = DateTime.Now;
            var minute = now.Minute;
            var hourTime = DateTime.Now.Date.AddHours(now.Hour);
            var output = new GetChartsOutput(6);
            output.Title = hourTime.ToString("yyyy-MM-dd HH:mm");
            for (var i = 0; i < 6; i += 1)
            {
                if (i > minute)
                {
                    output.Access[i] = 0;
                    output.Generate[i] = 0;
                }
                else
                {
                    var start = hourTime.AddMinutes(i * 10);
                    var end = hourTime.AddMinutes(i * 10 + 10);
                    output.Access[i] = await GetAccessCountAsync(start, end);
                    output.Generate[i] = await GetGenerateCountAsync(start, end);
                }

                output.Labels[i] = hourTime.AddMinutes(i * 10).ToString("HH:mm");
            }
            return output;
        }
    }
}
