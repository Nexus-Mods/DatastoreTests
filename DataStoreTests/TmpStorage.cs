using NexusMods.Paths;

namespace DataStoreTests;

public class TmpStorage
{
    private readonly AbsolutePath _root;

    public TmpStorage(AbsolutePath path)
    {
        _root = path;
    }
    
    public AbsolutePath MakeTempPath()
    {
        return _root.Combine(Guid.NewGuid().ToString());
    }
    
}