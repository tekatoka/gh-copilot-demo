using Microsoft.AspNetCore.Mvc;
using Xunit;
using albums_api.Controllers;
using albums_api.Models;

namespace albums_api.Tests
{
    public class AlbumControllerTests
    {
        private readonly AlbumController _controller;

        public AlbumControllerTests()
        {
            _controller = new AlbumController();
        }

        [Fact]
        public void Get_ReturnsOkResult_WithAllAlbums()
        {
            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            var albums = Assert.IsType<List<Album>>(okResult.Value);
            Assert.Equal(6, albums.Count);
        }

        [Fact]
        public void Get_ReturnsAlbumsWithCorrectProperties()
        {
            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var albums = Assert.IsType<List<Album>>(okResult.Value);

            var firstAlbum = albums.First();
            Assert.Equal(1, firstAlbum.Id);
            Assert.Equal("You, Me and an App Id", firstAlbum.Title);
            Assert.Equal("Daprize", firstAlbum.Artist);
            Assert.Equal(2020, firstAlbum.Year);
            Assert.Equal(10.99, firstAlbum.Price);
        }

        [Fact]
        public void Get_ReturnsStatusCode200()
        {
            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void GetById_ReturnsOkResult()
        {
            // Act
            var result = _controller.Get(1);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void GetSorted_WithTitleParameter_ReturnsSortedByTitle()
        {
            // Act
            var result = _controller.GetSorted("title");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var albums = Assert.IsType<List<Album>>(okResult.Value);

            // Verify albums are sorted by title
            for (int i = 0; i < albums.Count - 1; i++)
            {
                Assert.True(string.Compare(albums[i].Title, albums[i + 1].Title, StringComparison.Ordinal) <= 0);
            }
        }

        [Fact]
        public void GetSorted_WithArtistParameter_ReturnsSortedByArtist()
        {
            // Act
            var result = _controller.GetSorted("artist");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var albums = Assert.IsType<List<Album>>(okResult.Value);

            // Verify albums are sorted by artist
            for (int i = 0; i < albums.Count - 1; i++)
            {
                Assert.True(string.Compare(albums[i].Artist, albums[i + 1].Artist, StringComparison.Ordinal) <= 0);
            }
        }

        [Fact]
        public void GetSorted_WithPriceParameter_ReturnsSortedByPrice()
        {
            // Act
            var result = _controller.GetSorted("price");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var albums = Assert.IsType<List<Album>>(okResult.Value);

            // Verify albums are sorted by price
            for (int i = 0; i < albums.Count - 1; i++)
            {
                Assert.True(albums[i].Price <= albums[i + 1].Price);
            }
        }

        [Fact]
        public void GetSorted_WithTitleParameterCaseInsensitive_ReturnsSortedByTitle()
        {
            // Act
            var result = _controller.GetSorted("TITLE");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var albums = Assert.IsType<List<Album>>(okResult.Value);

            // Verify albums are sorted (case insensitive)
            for (int i = 0; i < albums.Count - 1; i++)
            {
                Assert.True(string.Compare(albums[i].Title, albums[i + 1].Title, StringComparison.Ordinal) <= 0);
            }
        }

        [Fact]
        public void GetSorted_WithInvalidParameter_ReturnsBadRequest()
        {
            // Act
            var result = _controller.GetSorted("invalid");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Contains("Invalid sort parameter", badRequestResult.Value?.ToString());
        }

        [Fact]
        public void GetSorted_WithInvalidParameter_ReturnsHelpfulMessage()
        {
            // Act
            var result = _controller.GetSorted("unknown");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var message = badRequestResult.Value?.ToString();

            Assert.NotNull(message);
            Assert.Contains("title", message);
            Assert.Contains("artist", message);
            Assert.Contains("price", message);
        }

        [Fact]
        public void GetSorted_ReturnsAllAlbums_RegardlessOfSortOrder()
        {
            // Act
            var result = _controller.GetSorted("title");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var albums = Assert.IsType<List<Album>>(okResult.Value);

            // Should still return all 6 albums
            Assert.Equal(6, albums.Count);
        }

        [Theory]
        [InlineData("title")]
        [InlineData("artist")]
        [InlineData("price")]
        public void GetSorted_WithValidParameters_ReturnsOkStatus(string sortBy)
        {
            // Act
            var result = _controller.GetSorted(sortBy);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Get_ReturnsConsistentData_OnMultipleCalls()
        {
            // Act
            var result1 = _controller.Get();
            var result2 = _controller.Get();

            // Assert
            var okResult1 = Assert.IsType<OkObjectResult>(result1);
            var okResult2 = Assert.IsType<OkObjectResult>(result2);

            var albums1 = Assert.IsType<List<Album>>(okResult1.Value);
            var albums2 = Assert.IsType<List<Album>>(okResult2.Value);

            Assert.Equal(albums1.Count, albums2.Count);
            Assert.Equal(albums1[0].Id, albums2[0].Id);
        }
    }
}
