//using SuperShortLink.Cache;
//using SuperShortLink.Models;
//using SuperShortLink.Repository;
//using System;
//using System.Threading.Tasks;

//namespace SuperShortLink.Charts
//{
//    public class HourChart : ChartAbstract
//    {
//        public override ChartTypeEnum ChartType => ChartTypeEnum.Hour;
//        private readonly IMemoryCaching _memory;

//        public HourChart(IShortLinkRepository repository,
//            IMemoryCaching memory) : base(repository)
//        {
//            _memory = memory;
//        }

//        public override async Task<GetChartsOutput> GetCharts()
//        {
//            var now = DateTime.Now;
//            var minute = now.Minute;
//            var hourTime = DateTime.Now.Date.AddHours(now.Hour);
//            var output = new GetChartsOutput(6);
//            for (var i = 0; i < 6; i += 1)
//            {
//                output.Labels[i] = hourTime.AddMinutes(i * 10).ToString("HH:mm");

//                if (i * 10 > minute)
//                {
//                    output.Access[i] = 0;
//                    output.Generate[i] = 0;
//                    continue;
//                }

//                if (minute - i * 10 >= 10)
//                {
//                    await GetValueFromCache(hourTime, output, i);
//                }
//                else
//                {
//                    output.Generate[i] = await GetGenerateCountAsync(hourTime.AddMinutes(i * 10), hourTime.AddMinutes(i * 10 + 10));
//                }
//                output.Access[i] = 0;

//            }
//            return output;
//        }

//        private async Task GetValueFromCache(DateTime hourTime, GetChartsOutput output, int i)
//        {
//            var generateCount = _memory.Get<int>($"HortChart:{i * 10}");
//            if (generateCount.HasValue)
//            {
//                output.Generate[i] = generateCount.Value;
//            }
//            else
//            {
//                output.Generate[i] = await GetGenerateCountAsync(hourTime.AddMinutes(i * 10), hourTime.AddMinutes(i * 10 + 10));
//                _memory.Set($"HortChart:{i * 10}", output.Generate[i]);
//            }
//        }
//    }
//}
