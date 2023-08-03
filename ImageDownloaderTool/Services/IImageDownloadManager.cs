namespace ImageDownloaderTool.Services
{
    public interface IImageDownloadManager
    {
        Task DownloadImagesAsync(List<string> imageUrls, string downloadFolderPath);
    }
}