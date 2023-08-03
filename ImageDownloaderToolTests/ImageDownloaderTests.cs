using ImageDownloaderTool.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace ImageDownloaderToolTests
{
    public class ImageDownloaderTests
    {
        [Fact]
        public void ReadUrlsFromFile_ValidFile_ReturnsValidUrls()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ImageDownloader>>();
            var imageDownloader = new ImageDownloader(mockLogger.Object);
            string filePath = "TestFiles/ValidUrls.txt"; // Replace with the path to your test file with invalid URLs
            // Act
            List<string> urls = imageDownloader.ReadUrlsFromFile(filePath);

            // Assert
            Assert.NotNull(urls);
            Assert.NotEmpty(urls);
            Assert.Contains("https://example.com/image1.jpg", urls);
            Assert.Contains("https://example.com/image2.jpg", urls);
        }

        [Fact]
        public void ReadUrlsFromFile_InvalidFile_ReturnsEmptyList()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ImageDownloader>>();
            var imageDownloader = new ImageDownloader(mockLogger.Object);
            string filePath = "TestFiles/InvalidUrls.txt"; // Replace with the path to your test file with invalid URLs

            // Act
            List<string> urls = imageDownloader.ReadUrlsFromFile(filePath);

            // Assert
            Assert.NotNull(urls);
            Assert.Empty(urls);
        }

        [Fact]
        public void ReadUrlsFromFile_EmptyFile_ReturnsEmptyList()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ImageDownloader>>();
            var imageDownloader = new ImageDownloader(mockLogger.Object); 
            string filePath = "TestFiles/EmptyFile.txt"; // Replace with the path to your test file with an empty content

            // Act
            List<string> urls = imageDownloader.ReadUrlsFromFile(filePath);

            // Assert
            Assert.NotNull(urls);
            Assert.Empty(urls);
        }

        [Fact]
        public void ReadUrlsFromFile_FileNotFound_ReturnsEmptyList()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ImageDownloader>>();
            var imageDownloader = new ImageDownloader(mockLogger.Object);
            string filePath = "NonExistentFile.txt"; // File does not exist

            // Act
            List<string> urls = imageDownloader.ReadUrlsFromFile(filePath);

            // Assert
            Assert.NotNull(urls);
            Assert.Empty(urls);
        }

        [Fact]
        public void ReadUrlsFromFile_UrlsWithWhitespace_ReturnsValidUrlsWithoutWhitespace()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ImageDownloader>>();
            var imageDownloader = new ImageDownloader(mockLogger.Object);
            string filePath = "TestFiles/MoreScenariosUrls.txt"; // Replace with the path to your test file

            // Act
            List<string> urls = imageDownloader.ReadUrlsFromFile(filePath);

            // Assert
            Assert.NotNull(urls);
            Assert.NotEmpty(urls);
            Assert.Contains("https://example.com/image3.jpg", urls);
            Assert.Contains("https://example.com/image4.jpg", urls);
            Assert.Contains("https://example.com/image5.jpg", urls);
            // Check that URLs with whitespace are trimmed
            Assert.DoesNotContain("   https://example.com/image6.jpg  ", urls);
        }

        [Fact]
        public void ReadUrlsFromFile_MalformedUrls_ReturnsValidUrlsOnly()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ImageDownloader>>();
            var imageDownloader = new ImageDownloader(mockLogger.Object);
            string filePath = "TestFiles/MoreScenariosUrls.txt"; // Replace with the path to your test file

            // Act
            List<string> urls = imageDownloader.ReadUrlsFromFile(filePath);

            // Assert
            Assert.NotNull(urls);
            Assert.NotEmpty(urls);
            Assert.Contains("https://example.com/image7.jpg", urls);
            Assert.DoesNotContain("malformed.url", urls); // Invalid URL
        }

        [Fact]
        public void ReadUrlsFromFile_DuplicateUrls_ReturnsDistinctUrls()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ImageDownloader>>();
            var imageDownloader = new ImageDownloader(mockLogger.Object);
            string filePath = "TestFiles/MoreScenariosUrls.txt"; // Replace with the path to your test file

            // Act
            List<string> urls = imageDownloader.ReadUrlsFromFile(filePath);

            // Assert
            Assert.NotNull(urls);
            Assert.NotEmpty(urls);
            Assert.Equal(urls.Count, new HashSet<string>(urls).Count); // All URLs are distinct
        }

        [Fact]
        public void ReadUrlsFromFilesFolder_DuplicateUrls_ReturnsDistinctUrls()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ImageDownloader>>();
            var imageDownloader = new ImageDownloader(mockLogger.Object);
            string folder = "TestFiles"; // Replace with the path to your test file
            string[] files = Directory.GetFiles(folder);
            // Act
            List<string> urls = imageDownloader.ReadUrlsFromFiles(files);

            // Assert
            Assert.NotNull(urls);
            Assert.NotEmpty(urls);
            Assert.Equal(urls.Count, new HashSet<string>(urls).Count); // All URLs are distinct
        }

        [Fact]
        public void ReadUrlsFromFile_UrlsWithHttpAndHttpsSchemes_ReturnsValidUrls()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ImageDownloader>>();
            var imageDownloader = new ImageDownloader(mockLogger.Object);
            string filePath = "TestFiles/MoreScenariosUrls.txt"; // Replace with the path to your test file

            // Act
            List<string> urls = imageDownloader.ReadUrlsFromFile(filePath);

            // Assert
            Assert.NotNull(urls);
            Assert.NotEmpty(urls);
            Assert.Contains("http://example.com/image8.jpg", urls);
            Assert.Contains("https://example.com/image9.jpg", urls);
        }

        [Fact]
        public void ReadUrlsFromFile_UrlsWithQueryParameters_ReturnsValidUrls()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ImageDownloader>>();
            var imageDownloader = new ImageDownloader(mockLogger.Object);
            string filePath = "TestFiles/MoreScenariosUrls.txt"; // Replace with the path to your test file

            // Act
            List<string> urls = imageDownloader.ReadUrlsFromFile(filePath);

            // Assert
            Assert.NotNull(urls);
            Assert.NotEmpty(urls);
            Assert.Contains("https://example.com/image10.jpg?size=medium", urls);
            Assert.Contains("https://example.com/image11.jpg?width=800&height=600", urls);
        }

        [Fact]
        public void ReadUrlsFromFile_UrlsWithSpecialCharacters_ReturnsValidUrls()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ImageDownloader>>();
            var imageDownloader = new ImageDownloader(mockLogger.Object);
            string filePath = "TestFiles/MoreScenariosUrls.txt"; // Replace with the path to your test file

            // Act
            List<string> urls = imageDownloader.ReadUrlsFromFile(filePath);

            // Assert
            Assert.NotNull(urls);
            Assert.NotEmpty(urls);
            Assert.Contains("https://example.com/image12.jpg#section1", urls);
            Assert.Contains("https://example.com/image13.jpg?size=small#section2", urls);
        }

    }

}
