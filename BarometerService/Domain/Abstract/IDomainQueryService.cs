using BarometerService.Domain.Contracts;

namespace BarometerService.Domain.Abstract
{
    internal interface IDomainQueryService
    {
        Task<MeasureResponse?> GetLatestMeasure();
        Task<IEnumerable<MeasureResponse>> GetMeasuresByDays();
    }
}
