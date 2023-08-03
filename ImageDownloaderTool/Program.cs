using ImageDownloaderTool.Exceptions;
using ImageDownloaderTool.Services;

namespace ImageDownloaderTool
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: dotnet run <path_to_text_file>");
                return;
            }

            string textFilePath = args[0];
            if (!File.Exists(textFilePath))
            {
                Console.WriteLine("File not found.");
                return;
            }

            ImageDownloader imageDownloader = new ImageDownloader();
            List<string> imageUrls = imageDownloader.ReadUrlsFromFile(textFilePath);
            if (imageUrls.Count == 0)
            {
                Console.WriteLine("No valid image URLs found in the file.");
                return;
            }

            string downloadFolderPath = "downloaded_images"; // Change this to your desired folder path

            // Create the download folder if it doesn't exist
            Directory.CreateDirectory(downloadFolderPath);

            ImageDownloadManager downloadManager = new ImageDownloadManager();
            try
            {
                await downloadManager.DownloadImagesAsync(imageUrls, downloadFolderPath);
            }
            catch (ImageDownloaderException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}