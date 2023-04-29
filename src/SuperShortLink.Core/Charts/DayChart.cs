using SuperShortLink.Models;
using SuperShortLink.Repository;
using System;
using System.Threading.Tasks;

namespace SuperShortLink.Charts
{
    public class DayChart : ChartAbstract
    {
        public override ChartTypeEnum ChartType => ChartTypeEnum.Day;

        public DayChart(IShortLinkRepository linkRepository,
            ILogRepository logRepository) : base(linkRepository, logRepository)
        {

        }

        public override async Task<GetChartsOutput> GetCharts()
        {
            var now = DateTime.Now;
            var hour = now.Hour;
            var date = now.Date;

            var output = new GetChartsOutput(24);
            output.Title = now.ToString("yyyy-MM-dd");

            for (var i = 0; i < 24; i++)
            {
                if (i > hour)
                {
                    output.Access[i] = 0;
                    output.Generate[i] = 0;
                }
                else
                {
                    var start = date.AddHours(i);
                    var end = date.AddHours(i + 1);
                    output.Access[i] = await GetAccessCountAsync(start, end);
                    output.Generate[i] = await GetGenerateCountAsync(start, end);
                }

                output.Labels[i] = date.AddHours(i).ToString("HH:mm");
            }
            return output;
        }
    }
}
