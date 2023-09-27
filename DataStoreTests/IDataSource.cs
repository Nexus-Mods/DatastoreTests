using DataStoreTests.DTOs;

namespace DataStoreTests
{
    public interface IDataSource
    {
        public void Put(Loadout loadout);
        public Loadout Get(Guid id);
        public IEnumerable<Loadout> GetAll(); 
    }
}