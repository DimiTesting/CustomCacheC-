namespace CustomCache
{
    public class Programm
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Setting up the template");
            //Cache<string, string> customCache = new Cache<string, string>();
            IDataDownloader dataDownloader = new CachingDataDownloader(
                new PrintingDataDownloader(
                    new SlowDataDownloader()));
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
        public string DownloadData(string resourceId)
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

    public class CachingDataDownloader: IDataDownloader
    {
        private Cache<string, string> _cache = new();
        private IDataDownloader _dataDownloader;
        public CachingDataDownloader(IDataDownloader dataDownloader)
        {
            _dataDownloader = dataDownloader;
        }
        public string DownloadData(string resourceId)
        {
            return _cache.AddData(resourceId, _dataDownloader.DownloadData);
        }
    }

    public class PrintingDataDownloader: IDataDownloader
    {
        private IDataDownloader _dataDownloader;
        public PrintingDataDownloader(IDataDownloader dataDownloader)
        {
            _dataDownloader = dataDownloader;
        }
        public string DownloadData(string resourceId)
        {
            var data = _dataDownloader.DownloadData(resourceId);
            Console.WriteLine($"Data is ready");
            return data;
        }
    }
}