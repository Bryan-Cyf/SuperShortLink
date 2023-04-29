using SuperShortLink.Models;
using SuperShortLink.Repository;
using System;
using System.Threading.Tasks;

namespace SuperShortLink.Charts
{
    public class MonthChart : ChartAbstract
    {
        public override ChartTypeEnum ChartType => ChartTypeEnum.Month;

        public MonthChart(IShortLinkRepository linkRepository,
            ILogRepository logRepository) : base(linkRepository, logRepository)
        {

        }

        public override async Task<GetChartsOutput> GetCharts()
        {
            var now = DateTime.Now;
            var days = DateTime.DaysInMonth(now.Year, now.Month);
            var day = now.Day;
            var output = new GetChartsOutput(days);
            var date = new DateTime(now.Year, now.Month, 1);
            output.Title = now.ToString("yyyy-MM");

            for (var i = 0; i < days; i++)
            {
                if (i > day)
                {
                    output.Access[i] = 0;
                    output.Generate[i] = 0;
                }
                else
                {
                    var start = date.AddDays(i);
                    var end = date.AddDays(i + 1);
                    output.Access[i] = await GetAccessCountAsync(start, end);
                    output.Generate[i] = await GetGenerateCountAsync(start, end);
                }

                output.Labels[i] = date.AddDays(i).ToString("MM-dd");
            }
            return output;
        }
    }
}
