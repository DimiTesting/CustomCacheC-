namespace CustomCache
{
    public class Programm
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Setting up the template");
            Cache<string, string> customCache = new Cache<string, string>();
            IDataDownloader dataDownloader = new SlowDataDownloader(customCache);
            Console.WriteLine(dataDownloader.DownloadData("id1"));
            Console.WriteLine(dataDownloader.DownloadData("id2"));
            Console.WriteLine(dataDownloader.DownloadData("id3"));
            Console.WriteLine(dataDownloader.DownloadData("id1"));
            Console.WriteLine(dataDownloader.DownloadData("id2"));
            Console.WriteLine(dataDownloader.DownloadData("id3"));
        }
    }
    public interface IDataDownloader
    {
        public string DownloadData(string resourceId);
    }

    public class SlowDataDownloader : IDataDownloader
    {
        private Cache<string, string> _customCache;

        public SlowDataDownloader(Cache<string, string> customCache)
        {
            _customCache = customCache;
        }

        public string DownloadData(string resourceId)
        {
            return _customCache.AddData(resourceId, DownloadDataWithoutCaching);
        }
        public string DownloadDataWithoutCaching(string resourceId)
        {
            Thread.Sleep(1000);
            return resourceId;
        }
    }

    public class Cache<Tkey, TValue>
    {
        private Dictionary<Tkey, TValue> _cache = new ();

        public TValue AddData(Tkey data, Func<Tkey, TValue> DataLoader)
        {
            if(!_cache.ContainsKey(data))
            {
                _cache[data] = DataLoader(data);
            } 
            return _cache[data];
        }
    }
}