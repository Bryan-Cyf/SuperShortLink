using SuperShortLink.Models;
using SuperShortLink.Repository;
using System;
using System.Threading.Tasks;

namespace SuperShortLink.Charts
{
    public class WeekChart : ChartAbstract
    {
        public override ChartTypeEnum ChartType => ChartTypeEnum.Week;

        public WeekChart(IShortLinkRepository repository) : base(repository)
        {

        }

        public override async Task<GetChartsOutput> GetCharts()
        {
            var now = DateTime.Now.Date;

            var dayOfWeek = now.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)now.DayOfWeek;

            var output = new GetChartsOutput(7);
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
                    output.Generate[i] = await GetGenerateCountAsync(day, day.AddDays(1));
                    output.Access[i] = 0;
                }

                output.Labels[i] = $"{day.ToString("MM-dd")}|{((DayOfWeek)(i == 6 ? 0 : i + 1))}";
            }

            return output;
        }
    }
}
