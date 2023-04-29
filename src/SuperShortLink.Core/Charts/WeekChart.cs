using SuperShortLink.Models;
using SuperShortLink.Repository;
using System;
using System.Threading.Tasks;

namespace SuperShortLink.Charts
{
    public class WeekChart : ChartAbstract
    {
        public override ChartTypeEnum ChartType => ChartTypeEnum.Week;

        public WeekChart(IShortLinkRepository linkRepository,
            ILogRepository logRepository) : base(linkRepository, logRepository)
        {

        }

        public override async Task<GetChartsOutput> GetCharts()
        {
            var now = DateTime.Now.Date;

            var dayOfWeek = now.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)now.DayOfWeek;

            var output = new GetChartsOutput(7);
            output.Title = $"{now.AddDays(-dayOfWeek + 1).ToString("yyyy-MM-dd")}~{now.AddDays(-dayOfWeek + 7).ToString("yyyy-MM-dd")}";

            for (var i = 0; i < 7; i++)
            {
                var day = now.AddDays(0 - (dayOfWeek - i) + 1);
                if (i >= dayOfWeek)
                {
                    output.Access[i] = 0;
                    output.Generate[i] = 0;
                }
                else
                {
                    var start = day;
                    var end = day.AddDays(1);
                    output.Access[i] = await GetAccessCountAsync(start, end);
                    output.Generate[i] = await GetGenerateCountAsync(start, end);
                }

                output.Labels[i] = $"{day.ToString("MM-dd")}|{((DayOfWeek)(i == 6 ? 0 : i + 1))}";
            }

            return output;
        }
    }
}
