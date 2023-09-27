using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;
using DataStoreTests.DTOs;
using LightningDB;
using NexusMods.Paths;

/// Has strange errors, probably ignore it for now
namespace DataStoreTests.Sources
{
    public class LightningDB : IDataSource
    {
        private readonly LightningEnvironment _env;
        private readonly DatabaseConfiguration _config;

        public LightningDB(AbsolutePath folderPath)
        {
            _env = new LightningEnvironment(folderPath.ToString());
            _env.MapSize = 1024 * 1024 * 512;
            _env.Open();
            _config = new DatabaseConfiguration()
            {
                Flags = DatabaseOpenFlags.Create
                
            };

        }

        public void Put(Loadout loadout)
        {
            using var tx = _env.BeginTransaction();
            using var db = tx.OpenDatabase();
            Span<byte> idSpan = stackalloc byte[16];
            loadout.ToId(idSpan);
            var buff = loadout.ToJson();
            var result = tx.Put(db , idSpan, buff.AsSpan());
            if (result != MDBResultCode.Success)
                throw new Exception("Failed to put item " + result);
            tx.Commit();
        }

        public Loadout? Get(Guid id)
        {
            using var tx = _env.BeginTransaction(TransactionBeginFlags.ReadOnly);
            using var db = tx.OpenDatabase();
            Span<byte> idSpan = stackalloc byte[16];
            id.TryWriteBytes(idSpan);
            var buff = tx.Get(db, idSpan);
            if (buff.resultCode != MDBResultCode.Success)
                throw new Exception("Failed to get item");
            
            return JsonSerializer.Deserialize<Loadout>(buff.value.AsSpan());
        }

        public IEnumerable<Loadout> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}