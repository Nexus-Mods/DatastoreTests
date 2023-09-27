using System.Text.Json;
using DataStoreTests.DTOs;
using FASTER.core;
using NexusMods.Paths;

namespace DataStoreTests.Sources;

public class FasterKV : IDataSource
{
    private readonly FasterKVSettings<Guid, Loadout> _settings;
    private readonly FasterKV<Guid,Loadout> _kv;
    private readonly ClientSession<Guid, string, string, string, Empty, IFunctions<Guid, string, string, string, Empty>> _session;

    class LoadoutSerializer : IVariableLengthStruct<Loadout>
    {
        private Stream _ms = Stream.Null;
        
        public void BeginSerialize(Stream stream)
        {
            _ms = stream;
        }

        public void Serialize(ref Loadout obj)
        {
            JsonSerializer.Serialize(_ms, obj);
        }

        public void EndSerialize()
        {
            _ms = Stream.Null;
        }

        public void BeginDeserialize(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(out Loadout obj)
        {
            throw new NotImplementedException();
        }

        public void EndDeserialize()
        {
            throw new NotImplementedException();
        }
    }

    public FasterKV(AbsolutePath folderPath)
    {
        _settings = new FasterKVSettings<Guid, string>(folderPath.ToString())
        {
            ValueSerializer = () => new LoadoutSerializer();
        };
        _kv = new FasterKV<Guid, string>(_settings);
        _session = _kv.NewSession(new SimpleFunctions<Guid, string>((a, b) => a));

    }
    public void Put(Loadout loadout)
    {
        _session.Upsert(loadout.Id, JsonSerializer.Serialize(loadout));
    }

    public Loadout Get(Guid id)
    {
        return JsonSerializer.Deserialize<Loadout>(_session.Read(id).output)!;
    }

    public IEnumerable<Loadout> GetAll()
    {
        throw new NotImplementedException();
    }
}