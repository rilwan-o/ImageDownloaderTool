using ImageDownloaderTool.Exceptions;

namespace ImageDownloaderTool.Services
{
    public class ImageDownloadManager
    {
        public async Task DownloadImagesAsync(List<string> imageUrls, string downloadFolderPath)
        {
            using (var httpClient = new HttpClient())
            {
                var downloadTasks = new List<Task>();

                foreach (string imageUrl in imageUrls)
                {
                    downloadTasks.Add(DownloadImageAsync(httpClient, imageUrl, downloadFolderPath));
                }

                try
                {
                    await Task.WhenAll(downloadTasks);
                }
                catch (Exception ex)
                {
                    throw new ImageDownloaderException("Error downloading images: " + ex.Message);
                }
            }
        }

        private async Task DownloadImageAsync(HttpClient httpClient, string imageUrl, string downloadFolderPath)
        {
            try
            {
                string fileName = Path.GetFileName(imageUrl);
                string filePath = Path.Combine(downloadFolderPath, fileName);

                using (var response = await httpClient.GetAsync(imageUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (var contentStream = await response.Content.ReadAsStreamAsync())
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await contentStream.CopyToAsync(fileStream);
                        }
                        Console.WriteLine("Downloaded: " + fileName);
                    }
                    else
                    {
                        Console.WriteLine("Failed to download: " + fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ImageDownloaderException("Error downloading image: " + ex.Message);
            }
        }
    }
}
