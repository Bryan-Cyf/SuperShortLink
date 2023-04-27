using System;
using System.Collections.Generic;
using System.Text;
using SuperShortLink.Models;

namespace SuperShortLink.Handle
{
    public class LogChartFactory
    {
        private static readonly Dictionary<ChartTypeEnum, Type> LogChartDict;

        static LogChartFactory()
        {
            LogChartDict = new Dictionary<ChartTypeEnum, Type>
            {
                { ChartTypeEnum.Hour,typeof(HourChart)},
                { ChartTypeEnum.Week,typeof(WeekLogChart)},
                { ChartTypeEnum.Day,typeof(DayLogChart)},
                { ChartTypeEnum.Month,typeof(MonthLogChart)}
            };
        }

        public static ILogChart GetLogChart(ChartDataType type)
        {
            return (ILogChart)Activator.CreateInstance(LogChartDict[type]);
        }
    }
}
