# ImageDownloaderTool
A tool for downloading a list of images from a plain text file
## How to run
#To use this tool, you can run it from the command line by following these steps:

1. Make sure you have .NET Core SDK installed on your system. If you don't have it, you can download and install it from the official .NET website: https://dotnet.microsoft.com/download

2. Open a terminal or command prompt and navigate to the folder containing the Program.cs file.

3. Compile the code using the dotnet build command. This will generate the necessary build artifacts. :
## dotnet build
4. Then run with this command:
## dotnet run path_to_text_file.txt
NOTE: The program will read the image URLs from the file, download the images, and store them in the "downloaded_images" folder (created automatically if it doesn't exist). If there are any invalid URLs or errors during the download process, appropriate messages will be displayed on the console.

Please ensure that your text file contains image URLs separated by whitespace (spaces, tabs, newlines, or carriage returns). Additionally, make sure that you have proper internet connectivity to download the images.

## The tool will read the URLs from the file, download the images, and save them in the "downloaded_images" folder (you can change this folder path in the code to your desired location). If the download is successful, it will display a message indicating that the image has been downloaded. If there's an issue with the URL or download, it will display an error message.
### For future purpose incase a batch download is needed,the DownloadImagesAsync method creates a list of Task objects for each image download and then waits for all tasks to complete using Task.WhenAll. This allows multiple images to be downloaded concurrently, which can significantly speed up the process when there are multiple URLs to download.
### Functions/methods :

ImageDownloader: The class responsible for reading URLs from the file and validating them.

ImageDownloaderException: A custom exception class for handling specific errors related to image downloading.

ImageDownloadManager: The class responsible for managing the download tasks of multiple image URLs concurrently.
