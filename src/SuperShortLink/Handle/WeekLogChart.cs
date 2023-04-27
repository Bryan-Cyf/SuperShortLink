using System;
using System.Threading.Tasks;

namespace SuperShortLink.Handle
{
    public class WeekLogChart : ILogChart
    {
        public async Task<GetLogChartsOutput> GetCharts<T>(IRepository<T> repository) where T : class, ILogModel
        {
            var now = DateTime.Now;
            var dayOfWeek = (int)now.DayOfWeek;
            dayOfWeek = dayOfWeek == 0 ? 6 : dayOfWeek == 6 ? 0 : dayOfWeek;
            var output = new GetLogChartsOutput(7);
            for (var i = 0; i < 7; i++)
            {
                if (i > dayOfWeek)
                {
                    output.All[i] = 0;
                    output.Error[i] = 0;
                    output.Info[i] = 0;
                    output.Debug[i] = 0;
                    output.Fatal[i] = 0;
                    output.Trace[i] = 0;
                    output.Warn[i] = 0;
                }
                else
                {

                    var day = now.AddDays(0 - (dayOfWeek - i));
                    var weeHours = now.AddDays(i - dayOfWeek).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                    output.All[i] = await repository.CountAsync(x => x.LongDate >= day.Date && x.LongDate <= weeHours);
                    output.Error[i] = await CountAsync(LogLevelConst.Error, repository, day, weeHours);
                    output.Info[i] = await CountAsync(LogLevelConst.Info, repository, day, weeHours);
                    output.Debug[i] = await CountAsync(LogLevelConst.Debug, repository, day, weeHours);
                    output.Fatal[i] = await CountAsync(LogLevelConst.Fatal, repository, day, weeHours);
                    output.Trace[i] = await CountAsync(LogLevelConst.Trace, repository, day, weeHours);
                    output.Warn[i] = await CountAsync(LogLevelConst.Warn, repository, day, weeHours);
                }
            }

            return output;
        }

        private async Task<int> CountAsync<T>(string level, IRepository<T> repository, DateTime day, DateTime weeHours) where T : class, ILogModel
        {
            return await repository.CountAsync(x => x.LongDate >= day.Date && x.LongDate <= weeHours && x.Level == level);
        }
    }
}
