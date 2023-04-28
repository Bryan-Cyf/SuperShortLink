using SuperShortLink.Models;
using SuperShortLink.Repository;
using System;
using System.Threading.Tasks;

namespace SuperShortLink.Charts
{
    public class DayChart : IChart
    {
        public ChartTypeEnum ChartType => ChartTypeEnum.Day;
        private readonly IShortLinkRepository _repository;

        public DayChart(IShortLinkRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetChartsOutput> GetCharts()
        {
            var now = DateTime.Now;
            var hour = now.Hour;

            var output = new GetChartsOutput(24);

            var date = now.Date;
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
                    output.Generate[i] = await CountAsync(date, i);
                }
                output.Labels[i] = date.AddHours(i).ToString("HH:mm");
            }
            return output;
        }


        private async Task<int> CountAsync(DateTime time, int i)
        {
            return await _repository.GetGenerateCountAsync(time.AddHours(i), time.AddHours(i + 1));
        }
    }
}
