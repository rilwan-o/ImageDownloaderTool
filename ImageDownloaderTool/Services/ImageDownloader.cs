namespace ImageDownloaderTool.Services
{
    class ImageDownloader
    {
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
                        Console.WriteLine("Invalid URL: " + url);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading the file: " + ex.Message);
            }
            return urls;
        }
    }
   
}
