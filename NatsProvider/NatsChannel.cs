namespace Nats.Provider;

using System.Threading.Channels;

using Microsoft.Extensions.Logging;

public class NatsChannel<T> 
  : Channel<NatsWriter<T>, NatsReader<T>> where T : class
{
  private readonly ILogger<NatsChannel<T>> logger;
  public new NatsReader<T> Reader { get; }
  public new NatsWriter<T> Writer { get; }

  public NatsChannel(NatsReader<T> reader, NatsWriter<T> writer, NatsServiceConfig config, ILoggerFactory factory)
  {
    Reader = reader;
    Writer = writer;
    logger = factory.CreateLogger<NatsChannel<T>>();
  }
}

