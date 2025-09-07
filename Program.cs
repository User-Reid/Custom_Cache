var dataDownloader = new CachingDataDownloader(new PrintingDataDownloader(new SlowDataDownloader()));

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
  private readonly Cache<string, string> _cache = new();
  private readonly IDataDownloader _dataDownloader;
  public CachingDataDownloader(IDataDownloader dataDownloader)
  {
    _dataDownloader = dataDownloader;
  }

  public string DownloadData(string resouceId)
  {
    return _cache.Get(resouceId, _dataDownloader.DownloadData);
  }
}

public class PrintingDataDownloader : IDataDownloader
{
  private readonly IDataDownloader _dataDownloader;
  public PrintingDataDownloader(IDataDownloader dataDownloader)
  {
    _dataDownloader = dataDownloader;
  }

  public string DownloadData(string resouceId)
  {
    var data = _dataDownloader.DownloadData(resouceId);
    System.Console.WriteLine("Data is ready!");
    return data;
  }
}

public interface IDataDownloader
{
  string DownloadData(string resouceId);
}