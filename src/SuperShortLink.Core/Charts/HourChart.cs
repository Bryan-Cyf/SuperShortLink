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
                    output.Generate[i / 10] = await GetGenerateCountAsync(hourTime.AddMinutes(i), hourTime.AddMinutes(i + 10));
                }

                output.Labels[i / 10] = hourTime.AddMinutes(i).ToString("HH:mm");
            }
            return output;
        }
    }
}
