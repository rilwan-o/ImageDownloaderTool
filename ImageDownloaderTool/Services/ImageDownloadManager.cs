using ImageDownloaderTool.Exceptions;
using Microsoft.Extensions.Logging;

namespace ImageDownloaderTool.Services
{
    public class ImageDownloadManager : IImageDownloadManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ImageDownloadManager> _logger;

        public ImageDownloadManager(HttpClient httpClient, ILogger<ImageDownloadManager> logger)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task DownloadImagesAsync(List<string> imageUrls, string downloadFolderPath)
        {
            var downloadTasks = new List<Task>();

            foreach (string imageUrl in imageUrls)
            {
                downloadTasks.Add(DownloadImageAsync(imageUrl, downloadFolderPath));
            }

            try
            {
                await Task.WhenAll(downloadTasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading images");
                throw new ImageDownloaderException("Error downloading images");
            }
        }

        private async Task DownloadImageAsync(string imageUrl, string downloadFolderPath)
        {
            try
            {
                string fileName = Path.GetFileName(imageUrl);
                string filePath = Path.Combine(downloadFolderPath, fileName);

                using (var response = await _httpClient.GetAsync(imageUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (var contentStream = await response.Content.ReadAsStreamAsync())
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await contentStream.CopyToAsync(fileStream);
                        }
                        _logger.LogInformation("Downloaded: {FileName}", fileName);
                    }
                    else
                    {
                        _logger.LogError("Failed to download: {FileName} and STATUS CODE : {Code}", fileName, response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading image: {ImageUrl}", imageUrl);
                throw new ImageDownloaderException("Error downloading image");
            }
        }
    }
}
