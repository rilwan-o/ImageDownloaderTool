using ImageDownloaderTool.Exceptions;
using ImageDownloaderTool.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
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
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfiguration config = builder.Build();

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
                        loggingBuilder.AddNLog(config);
                    }).BuildServiceProvider();

                ImageDownloader imageDownloader = services.GetRequiredService<ImageDownloader>();
                List<string> imageUrls = imageDownloader.ReadUrlsFromFile(textFilePath);
                if (imageUrls.Count == 0)
                {
                    Console.WriteLine("No valid image URLs found in the file.");
                    return;
                }

                // Get the DownloadFolderPath from appsettings.json
                string downloadFolderPath = config.GetSection("AppSettings")["DownloadFolderPath"];// Change this to your desired folder path

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