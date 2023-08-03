using Microsoft.Extensions.Logging;

namespace ImageDownloaderTool.Services
{
    public class ImageDownloader : IImageDownloader
    {
        private readonly ILogger<ImageDownloader> _logger;
        public ImageDownloader(ILogger<ImageDownloader> logger)
        {
            _logger = logger;
        }

        public List<string> ReadUrlsFromFiles(string[] files)
        {
            List<string> urls = new List<string>();
            foreach (string filePath in files)
            {
                try
                {
                    var urlList = ReadUrlsFromFile(filePath);
                    Parallel.ForEach(urlList, url => { 
                        urls.Add(url); 
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error reading the file: {FilePath}", filePath);
                }
            }
            return urls;
        }

        public List<string> ReadUrlsFromFile(string filePath)
        {
            List<string> urls = new List<string>();
            try
            {
                string content = File.ReadAllText(filePath);
                string[] urlArray = content.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string url in urlArray)
                {
                    if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                    {
                        urls.Add(uri.AbsoluteUri);
                    }
                    else
                    {
                        _logger.LogError("Invalid URL: {InvalidUrl}", url);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading the file: {FilePath}", filePath);
            }
            return urls;
        }
    }
}
