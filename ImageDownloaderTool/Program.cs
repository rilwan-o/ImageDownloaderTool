using ImageDownloaderTool.Exceptions;
using ImageDownloaderTool.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System.Text;

namespace ImageDownloaderTool
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
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

                // Setup Dependency Injection
                using var services = new ServiceCollection()
                    .AddHttpClient()
                    .AddSingleton<ImageDownloader>()
                    .AddSingleton<ImageDownloadManager>()
                    .AddLogging(loggingBuilder =>
                    {
                        // configure Logging with NLog
                        loggingBuilder.ClearProviders();
                        loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                        loggingBuilder.AddNLog(new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .Build());
                    }).BuildServiceProvider();

                ImageDownloader imageDownloader = services.GetRequiredService<ImageDownloader>();
                List<string> imageUrls = imageDownloader.ReadUrlsFromFile(textFilePath);
                if (imageUrls.Count == 0)
                {
                    Console.WriteLine("No valid image URLs found in the file.");
                    return;
                }

                string downloadFolderPath = "C:\\Temp\\downloaded_images"; // Change this to your desired folder path

                // Create the download folder if it doesn't exist
                Directory.CreateDirectory(downloadFolderPath);

                ImageDownloadManager downloadManager = services.GetRequiredService<ImageDownloadManager>();

                await downloadManager.DownloadImagesAsync(imageUrls, downloadFolderPath);
            }
            catch (ImageDownloaderException ex)
            {
                logger.Error(ex.Message);
            }
        }
    }

}