namespace Nats.Provider;

using System.Buffers;
using System.Text.Json;

using NATS.Client.Core;

public class CommandEventSerializer<T> : INatsSerializer<T>
{
  public INatsSerializer<T> CombineWith(INatsSerializer<T> next)
  {
    throw new NotImplementedException();
  }

  public T? Deserialize(in ReadOnlySequence<byte> buffer)
  {
    byte[] buf = buffer.ToArray();
    T? action = JsonSerializer.Deserialize<T>(buf);
    return action;
  }

  public void Serialize(IBufferWriter<byte> bufferWriter, T value)
  {
    byte[] buf = JsonSerializer.SerializeToUtf8Bytes(value);
    bufferWriter.Write(buf);
  }
}
