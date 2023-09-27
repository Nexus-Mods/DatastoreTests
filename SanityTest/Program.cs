// See https://aka.ms/new-console-template for more information


using System.Diagnostics;
using DataStoreTests;
using DataStoreTests.DTOs;
using NexusMods.Paths;

var dataSet = Enumerable.Range(0, 10000)
    .Select(l => new Loadout()
    {
        Id = Guid.NewGuid(),
        Name = "Loadout : " + l,
        ModIds = Enumerable.Range(0, 1000).Select(_ => Guid.NewGuid()).ToList()
    }).ToList();

void BasicSanityTest(IDataSource source)
{
    var sw = Stopwatch.StartNew();
    Console.WriteLine("Starting Basic Sanity Test for " + source.GetType().Name);
    
    Console.WriteLine("Putting " + dataSet.Count + " items into " + source.GetType().Name);
    sw.Restart();
    foreach (var itm in dataSet)
        source.Put(itm);

    Console.WriteLine("Completed Putting " + dataSet.Count + " items into " + source.GetType().Name + " in " + sw.ElapsedMilliseconds + "ms");
    sw.Restart();
    foreach (var itm in dataSet)
        source.Get(itm.Id);
    Console.WriteLine("Completed Getting " + dataSet.Count + " items from " + source.GetType().Name + " in " + sw.ElapsedMilliseconds + "ms");
    
}


var tmp = new TmpStorage(FileSystem.Shared.FromUnsanitizedFullPath("C:\\tmp"));

var path = tmp.MakeTempPath();
path.CreateDirectory();
var ldb = new DataStoreTests.Sources.FasterKV(path);

BasicSanityTest(ldb);

Console.WriteLine("End Size of " + path + " is " + path.FolderSize());