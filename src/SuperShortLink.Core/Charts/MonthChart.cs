using SuperShortLink.Models;
using SuperShortLink.Repository;
using System;
using System.Threading.Tasks;

namespace SuperShortLink.Charts
{
    public class MonthChart : ChartAbstract
    {
        public override ChartTypeEnum ChartType => ChartTypeEnum.Month;

        public MonthChart(IShortLinkRepository repository) : base(repository)
        {

        }

        public override async Task<GetChartsOutput> GetCharts()
        {
            var now = DateTime.Now;
            var days = DateTime.DaysInMonth(now.Year, now.Month);
            var day = now.Day;
            var output = new GetChartsOutput(days);
            var date = new DateTime(now.Year, now.Month, 1);

            for (var i = 0; i < days; i++)
            {
                if (i > day)
                {
                    output.Access[i] = 0;
                    output.Generate[i] = 0;
                }
                else
                {
                    var dayTime = date.AddDays(i);
                    output.Generate[i] = await GetGenerateCountAsync(dayTime, dayTime.AddDays(1));
                    output.Access[i] = 0;
                }

                output.Labels[i] = date.AddDays(i).ToString("MM-dd");
            }
            return output;
        }
    }
}
