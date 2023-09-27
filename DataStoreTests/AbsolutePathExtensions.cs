using NexusMods.Paths;

namespace DataStoreTests;

public static class AbsolutePathExtensions
{
    public static Size FolderSize(this AbsolutePath path)
    {
        return path.EnumerateFiles().Aggregate(Size.Zero, (current, file) => current + file.FileInfo.Size);
    }
    
}