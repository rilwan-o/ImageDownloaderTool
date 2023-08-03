namespace ImageDownloaderTool.Services
{
    public interface IImageDownloader
    {
        List<string> ReadUrlsFromFile(string filePath);
        List<string> ReadUrlsFromFiles(string[] files);
    }
}