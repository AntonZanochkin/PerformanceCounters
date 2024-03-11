using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace PerformanceCounters.Transmitter.Helpers
{
  public static class DeepCloneHelper
  {
    public static T DeepClone<T>(T obj)
    {
      using (var memoryStream = new MemoryStream())
      {
        // Serialize the object to a memory stream
        IFormatter formatter = new BinaryFormatter();
        formatter.Serialize(memoryStream, obj);

        // Reset the stream position to the beginning
        memoryStream.Seek(0, SeekOrigin.Begin);

        // Deserialize the object from the memory stream
        return (T)formatter.Deserialize(memoryStream);
      }
    }
  }
}
