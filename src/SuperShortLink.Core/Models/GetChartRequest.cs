using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShortLink.Models
{
    public class GetChartRequest
    {
        public ChartTypeEnum ChartDataType { get; set; }
    }

    public enum ChartTypeEnum
    {
        Hour = 0,
        Day = 1,
        Week = 2,
        Month = 3
    }
}
