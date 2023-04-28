//using System;
//using System.Threading.Tasks;

//namespace SuperShortLink.Charts
//{
//    public class MonthLogChart : ILogChart
//    {
//        public async Task<GetChartsOutput> GetCharts<T>(IRepository<T> repository) where T : class, ILogModel
//        {
//            var now = DateTime.Now;
//            var days = DateTime.DaysInMonth(now.Year, now.Month);
//            var day = now.Day;
//            var output = new GetChartsOutput(days);
//            var date = new DateTime(now.Year, now.Month, 1);
//            for (var i = 0; i < days; i++)
//            {
//                if (i > day)
//                {
//                    output.All[i] = 0;
//                    output.Error[i] = 0;
//                    output.Info[i] = 0;
//                    output.Debug[i] = 0;
//                    output.Fatal[i] = 0;
//                    output.Trace[i] = 0;
//                    output.Warn[i] = 0;
//                }
//                else
//                {
//                    var dayTime = date.AddDays(i);
//                    output.All[i] = await repository.CountAsync(x => x.LongDate >= dayTime && x.LongDate <= dayTime.AddHours(23).AddMinutes(59).AddSeconds(59));
//                    output.Error[i] = await CountAsync(LogLevelConst.Error, repository, dayTime);
//                    output.Info[i] = await CountAsync(LogLevelConst.Info, repository, dayTime);
//                    output.Debug[i] = await CountAsync(LogLevelConst.Debug, repository, dayTime);
//                    output.Fatal[i] = await CountAsync(LogLevelConst.Fatal, repository, dayTime);
//                    output.Trace[i] = await CountAsync(LogLevelConst.Trace, repository, dayTime);
//                    output.Warn[i] = await CountAsync(LogLevelConst.Warn, repository, dayTime);
//                }
//            }
//            return output;
//        }

//        private async Task<int> CountAsync<T>(string level, IRepository<T> repository, DateTime dayTime) where T : class, ILogModel
//        {
//            return await repository.CountAsync(x => x.LongDate >= dayTime && x.LongDate <= dayTime.AddHours(23).AddMinutes(59).AddSeconds(59) && x.Level == level);
//        }

//    }
//}
