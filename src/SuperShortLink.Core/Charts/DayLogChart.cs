using SuperShortLink.Models;
using SuperShortLink.Repository;
using System;
using System.Threading.Tasks;

namespace SuperShortLink.Charts
{
    public class DayLogChart : IChart
    {
        public ChartTypeEnum ChartType => ChartTypeEnum.Day;
        private readonly IShortLinkRepository _repository;

        public DayLogChart(IShortLinkRepository repository)
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
                    var startTime = date.AddHours(i);
                    output.Access[i] = 1;
                    output.Generate[i] = await CountAsync(startTime, i);
                    //output.Labels[i] = dtNow.AddMinutes(-i).ToString("HH:mm:ss");
                }
            }
            return output;
        }


        private async Task<int> CountAsync(DateTime time, int i)
        {
            return await _repository.GetGenerateCountAsync(time.AddMinutes(-i), time.AddMinutes(-i + 10));
        }

        //private async Task<int> CountAsync<T>(string level, IRepository<T> repository, DateTime startTime, int i) where T : class, ILogModel
        //{
        //    return await repository.CountAsync(x => x.LongDate >= startTime && x.LongDate <= startTime.AddMinutes(59).AddSeconds(59) && x.Level == level);
        //}
    }
}
