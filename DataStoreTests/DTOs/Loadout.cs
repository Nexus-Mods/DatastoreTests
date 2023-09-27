using System.Text.Json;

namespace DataStoreTests.DTOs
{
    public class Loadout
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public List<Guid> ModIds { get; set; } = new();

        public byte[] ToJson()
        {
            return JsonSerializer.SerializeToUtf8Bytes(this);
        }

        public static Loadout FromJson(Stream stream)
        {
            return JsonSerializer.Deserialize<Loadout>(stream)!;
        }
        
        public void ToId(Span<byte> bytes)
        {
            Id.TryWriteBytes(bytes);
        }
    }
}