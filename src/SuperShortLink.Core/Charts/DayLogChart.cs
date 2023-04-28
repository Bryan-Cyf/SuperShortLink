//using System;
//using System.Threading.Tasks;

//namespace SuperShortLink.Charts
//{
//    public class DayLogChart : ILogChart
//    {
//        public async Task<GetChartsOutput> GetCharts<T>(IRepository<T> repository) where T : class, ILogModel
//        {
//            var now = DateTime.Now;
//            var hour = now.Hour;

//            var output = new GetChartsOutput(24);

//            var date = now.Date;
//            for (var i = 0; i < 24; i++)
//            {
//                if (i > hour)
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
//                    var startTime = date.AddHours(i);
//                    output.All[i] = await repository.CountAsync(x => x.LongDate >= startTime && x.LongDate <= startTime.AddMinutes(59).AddSeconds(59));
//                    output.Error[i] = await CountAsync(LogLevelConst.Error, repository, startTime, i);
//                    output.Info[i] = await CountAsync(LogLevelConst.Info, repository, startTime, i);
//                    output.Debug[i] = await CountAsync(LogLevelConst.Debug, repository, startTime, i);
//                    output.Fatal[i] = await CountAsync(LogLevelConst.Fatal, repository, startTime, i);
//                    output.Trace[i] = await CountAsync(LogLevelConst.Trace, repository, startTime, i);
//                    output.Warn[i] = await CountAsync(LogLevelConst.Warn, repository, startTime, i);
//                }
//            }
//            return output;
//        }

//        private async Task<int> CountAsync<T>(string level, IRepository<T> repository, DateTime startTime, int i) where T : class, ILogModel
//        {
//            return await repository.CountAsync(x => x.LongDate >= startTime && x.LongDate <= startTime.AddMinutes(59).AddSeconds(59) && x.Level == level);
//        }
//    }
//}
