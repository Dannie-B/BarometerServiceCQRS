using BarometerService.Domain.Abstract;
using BarometerService.Domain.Contracts;
using BarometerService.Domain.Model;

namespace BarometerService.Domain.Services
{
    internal sealed class DomainQueryService(
        IQueryService service,
        ILogger<DomainQueryService> logger)
        : IDomainQueryService
    {
        public async Task<MeasureResponse?> GetLatestMeasure()
        {
            Measure? measure = await service.GetLatestPersistedMeasure();

            if (measure is null)
            {
                logger.LogWarning("Failed fetching measure, no data found");
                return null;
            }

            return measure.ToMeasureResponse();
        }

        public async Task<IEnumerable<MeasureResponse>> GetMeasuresByDays()
        {
            IEnumerable<Measure> measures = await service.GetPersistedMeasuresByDays();

            if (!measures.Any())
            {
                logger.LogWarning($"Failed fetching measures, no data found");
            }

            return measures
                .Select(m => m.ToMeasureResponse());
        }
    }
}
