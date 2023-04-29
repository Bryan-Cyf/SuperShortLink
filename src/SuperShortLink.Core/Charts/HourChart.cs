using SuperShortLink.Models;
using SuperShortLink.Repository;
using System;
using System.Threading.Tasks;

namespace SuperShortLink.Charts
{
    public class HourChart : ChartAbstract
    {
        public override ChartTypeEnum ChartType => ChartTypeEnum.Hour;

        public HourChart(IShortLinkRepository repository) : base(repository)
        {

        }

        public override async Task<GetChartsOutput> GetCharts()
        {
            var now = DateTime.Now;
            var minute = now.Minute;
            var hourTime = DateTime.Now.Date.AddHours(now.Hour);
            var output = new GetChartsOutput(6);

            for (var i = 0; i < 6; i += 1)
            {
                if (i > minute)
                {
                    output.Access[i] = 0;
                    output.Generate[i] = 0;
                }
                else
                {
                    output.Access[i] = 0;
                    output.Generate[i] = await GetGenerateCountAsync(hourTime.AddMinutes(i * 10), hourTime.AddMinutes(i * 10 + 10));
                }

                output.Labels[i] = hourTime.AddMinutes(i * 10).ToString("HH:mm");
            }
            return output;
        }
    }
}
