using CQRSContracts;

namespace BarometerService.Domain.Abstract
{
    internal interface ICommandService
    {
        Task<bool> AddMeasureCommand(AddMeasureCommand command);
    }
}
