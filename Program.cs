var dataDownloader = new CachingDataDownloader(new SlowDataDownloader());

System.Console.WriteLine(dataDownloader.DownloadData("id1"));
System.Console.WriteLine(dataDownloader.DownloadData("id2"));
System.Console.WriteLine(dataDownloader.DownloadData("id3"));
System.Console.WriteLine(dataDownloader.DownloadData("id1"));
System.Console.WriteLine(dataDownloader.DownloadData("id3"));
System.Console.WriteLine(dataDownloader.DownloadData("id1"));
System.Console.WriteLine(dataDownloader.DownloadData("id2"));

Console.ReadKey();

public class Cache<TKey, TData>
{
  private readonly Dictionary<TKey, TData> _cachedData = new();

  public TData Get(TKey key, Func<TKey, TData> getForTheFirstTime) {
    if (!_cachedData.ContainsKey(key))
    {
      _cachedData[key] = getForTheFirstTime(key);
    }
      return _cachedData[key];
  }
}

public class SlowDataDownloader : IDataDownloader
{

  public string DownloadData(string resouceId)
  {
    // let's imagine this method downloads real data,
    // and it does it slowly
    Thread.Sleep(1000);
    return $"Some data for {resouceId}";
  }
}

public class CachingDataDownloader : IDataDownloader
{
  private readonly IDataDownloader _dataDownloader;
  private readonly Cache<string, string> _cache = new();

  public CachingDataDownloader(IDataDownloader dataDownloader)
  {
    _dataDownloader = dataDownloader;
  }

  public string DownloadData(string resouceId)
  {
    return _cache.Get(resouceId, _dataDownloader.DownloadData);
  }
}

public interface IDataDownloader
{
  string DownloadData(string resouceId);
}