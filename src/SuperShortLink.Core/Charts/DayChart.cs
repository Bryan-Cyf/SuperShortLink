using SuperShortLink.Models;
using SuperShortLink.Repository;
using System;
using System.Threading.Tasks;

namespace SuperShortLink.Charts
{
    public class DayChart : ChartAbstract
    {
        public override ChartTypeEnum ChartType => ChartTypeEnum.Day;

        public DayChart(IShortLinkRepository repository) : base(repository)
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
                    output.Access[i] = 1;
                    output.Generate[i] = await GetGenerateCountAsync(date.AddHours(i), date.AddHours(i + 1));
                }

                output.Labels[i] = date.AddHours(i).ToString("HH:mm");
            }
            return output;
        }
    }
}
