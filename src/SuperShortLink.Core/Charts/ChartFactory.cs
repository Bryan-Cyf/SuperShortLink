using System;
using System.Collections.Generic;
using System.Text;
using SuperShortLink.Models;
using System.Linq;

namespace SuperShortLink.Charts
{
    public class ChartFactory
    {
        private readonly IEnumerable<IChart> _charts;

        public ChartFactory(IEnumerable<IChart> charts)
        {
            _charts = charts;
        }

        public IChart GetChart(ChartTypeEnum type)
        {
            return _charts.First(x => x.ChartType == type);
        }
    }
}
