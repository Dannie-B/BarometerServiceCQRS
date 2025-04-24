using BarometerService.Domain.Contracts;
using BarometerService.Domain.Model;

namespace BarometerService.Domain.Abstract
{
    internal interface IDomainCommandService
    {
        Task<MeasureResponse> PostMeasure(Measure measure);
    }
}
