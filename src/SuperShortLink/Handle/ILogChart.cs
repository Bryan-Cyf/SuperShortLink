using System.Threading.Tasks;

namespace LogDashboard.Handle
{
    public interface ILogChart
    {
        Task<GetLogChartsOutput> GetCharts<T>(IRepository<T> repository) where T : class, ILogModel;
    }
}
