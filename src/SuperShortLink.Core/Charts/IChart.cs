using SuperShortLink.Models;
using System.Threading.Tasks;

namespace SuperShortLink.Charts
{
    public interface IChart
    {
        ChartTypeEnum ChartType { get; }

        Task<GetChartsOutput> GetCharts();
    }
}
