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

            List<string> imageUrls = ReadUrlsFromFile(textFilePath);
            if (imageUrls.Count == 0)
            {
                Console.WriteLine("No image URLs found in the file.");
                return;
            }

            string downloadFolderPath = "downloaded_images"; // Change this to your desired folder path

            // Create the download folder if it doesn't exist
            if (!Directory.Exists(downloadFolderPath))
            {
                Directory.CreateDirectory(downloadFolderPath);
            }

            await DownloadImagesAsync(imageUrls, downloadFolderPath);
        }

        static List<string> ReadUrlsFromFile(string filePath)
        {
            List<string> urls = new List<string>();
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();
                    if (!string.IsNullOrWhiteSpace(trimmedLine) && Uri.IsWellFormedUriString(trimmedLine, UriKind.Absolute))
                    {
                        urls.Add(trimmedLine);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading the file: " + ex.Message);
            }
            return urls;
        }

        static async Task DownloadImagesAsync(List<string> imageUrls, string downloadFolderPath)
        {
            using (var httpClient = new HttpClient())
            {
                foreach (string imageUrl in imageUrls)
                {
                    try
                    {
                        string fileName = Path.GetFileName(imageUrl);
                        string filePath = Path.Combine(downloadFolderPath, fileName);

                        using (var response = await httpClient.GetAsync(imageUrl))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                using (var content = await response.Content.ReadAsStreamAsync())
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await content.CopyToAsync(stream);
                                    Console.WriteLine("Downloaded: " + fileName);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Failed to download: " + fileName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error downloading image: " + ex.Message);
                    }
                }
            }
        }
    }

}