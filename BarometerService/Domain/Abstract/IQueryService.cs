using BarometerService.Domain.Model;

namespace BarometerService.Domain.Abstract
{
    internal interface IQueryService
    {
        Task<Measure?> GetLatestPersistedMeasure();
        Task<IEnumerable<Measure>> GetPersistedMeasuresByDays();
    }
}
