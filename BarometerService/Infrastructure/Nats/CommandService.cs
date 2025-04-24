using BarometerService.Domain.Abstract;
using CQRSContracts;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;

namespace BarometerService.Infrastructure.Nats
{
    public class CommandService(
        INatsJSContext context,
        ILogger<CommandService> logger) 
        : ICommandService
    {
        private readonly INatsJSStream stream = null;
        public Task<bool> AddMeasureCommand(AddMeasureCommand command)
        {
            // Create a stream if it does not exist
            stream ??= await context.CreateStreamAsync(new StreamConfig(streamName, [subjectName]));

            PubAckResponse ack = await context.PublishAsync(subjectName, message);
            ack.EnsureSuccess();
        }
    }
}
