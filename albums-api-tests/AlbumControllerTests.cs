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
        public void GetById_ReturnsOkResult_WithCorrectAlbum()
        {
            // Act
            var result = _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var album = Assert.IsType<Album>(okResult.Value);

            Assert.Equal(1, album.Id);
            Assert.Equal("You, Me and an App Id", album.Title);
            Assert.Equal("Daprize", album.Artist);
        }

        [Fact]
        public void GetById_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var result = _controller.Get(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
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

        // Search by Year Tests
        [Fact]
        public void SearchByYear_WithValidYear_ReturnsMatchingAlbums()
        {
            // Act
            var result = _controller.SearchByYear(2020);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var albums = Assert.IsType<List<Album>>(okResult.Value);

            Assert.Single(albums);
            Assert.Equal(2020, albums[0].Year);
            Assert.Equal("You, Me and an App Id", albums[0].Title);
        }

        [Fact]
        public void SearchByYear_WithYearHavingNoAlbums_ReturnsNotFound()
        {
            // Act
            var result = _controller.SearchByYear(2025);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public void SearchByYear_WithMultipleMatches_ReturnsAllMatches()
        {
            // First add another album with year 2020
            var newAlbum = new Album(0, "Test Album", "Test Artist", 2020, 9.99, "http://test.com");
            _controller.Create(newAlbum);

            // Act
            var result = _controller.SearchByYear(2020);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var albums = Assert.IsType<List<Album>>(okResult.Value);

            Assert.True(albums.Count >= 2);
            Assert.All(albums, a => Assert.Equal(2020, a.Year));
        }

        // Create Album Tests
        [Fact]
        public void Create_WithValidAlbum_ReturnsCreatedResult()
        {
            // Arrange
            var newAlbum = new Album(0, "New Album", "New Artist", 2024, 15.99, "http://image.com");

            // Act
            var result = _controller.Create(newAlbum);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var album = Assert.IsType<Album>(createdResult.Value);

            Assert.Equal("New Album", album.Title);
            Assert.Equal("New Artist", album.Artist);
            Assert.True(album.Id > 0);
        }

        [Fact]
        public void Create_WithNullAlbum_ReturnsBadRequest()
        {
            // Act
            var result = _controller.Create(null!);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public void Create_WithEmptyTitle_ReturnsBadRequest()
        {
            // Arrange
            var invalidAlbum = new Album(0, "", "Artist", 2024, 10.99, "http://image.com");

            // Act
            var result = _controller.Create(invalidAlbum);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Title and Artist are required", badRequestResult.Value?.ToString());
        }

        [Fact]
        public void Create_WithNegativePrice_ReturnsBadRequest()
        {
            // Arrange
            var invalidAlbum = new Album(0, "Album", "Artist", 2024, -5.99, "http://image.com");

            // Act
            var result = _controller.Create(invalidAlbum);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Price cannot be negative", badRequestResult.Value?.ToString());
        }

        [Fact]
        public void Create_AssignsNewId_Automatically()
        {
            // Arrange
            var album1 = new Album(0, "Album 1", "Artist 1", 2024, 10.99, "http://image.com");
            var album2 = new Album(0, "Album 2", "Artist 2", 2024, 12.99, "http://image.com");

            // Act
            var result1 = _controller.Create(album1);
            var result2 = _controller.Create(album2);

            // Assert
            var created1 = Assert.IsType<CreatedAtActionResult>(result1);
            var created2 = Assert.IsType<CreatedAtActionResult>(result2);

            var savedAlbum1 = Assert.IsType<Album>(created1.Value);
            var savedAlbum2 = Assert.IsType<Album>(created2.Value);

            Assert.NotEqual(savedAlbum1.Id, savedAlbum2.Id);
        }

        // Update Album Tests
        [Fact]
        public void Update_WithValidAlbum_ReturnsUpdatedAlbum()
        {
            // Arrange
            var updatedAlbum = new Album(0, "Updated Title", "Updated Artist", 2023, 19.99, "http://updated.com");

            // Act
            var result = _controller.Update(1, updatedAlbum);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var album = Assert.IsType<Album>(okResult.Value);

            Assert.Equal(1, album.Id);
            Assert.Equal("Updated Title", album.Title);
            Assert.Equal("Updated Artist", album.Artist);
        }

        [Fact]
        public void Update_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var updatedAlbum = new Album(0, "Updated Title", "Updated Artist", 2023, 19.99, "http://updated.com");

            // Act
            var result = _controller.Update(999, updatedAlbum);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public void Update_WithNullAlbum_ReturnsBadRequest()
        {
            // Act
            var result = _controller.Update(1, null!);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public void Update_WithEmptyArtist_ReturnsBadRequest()
        {
            // Arrange
            var invalidAlbum = new Album(0, "Title", "", 2024, 10.99, "http://image.com");

            // Act
            var result = _controller.Update(1, invalidAlbum);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Title and Artist are required", badRequestResult.Value?.ToString());
        }

        [Fact]
        public void Update_PreservesId_EvenIfDifferentInPayload()
        {
            // Arrange
            var updatedAlbum = new Album(999, "Updated Title", "Updated Artist", 2023, 19.99, "http://updated.com");

            // Act
            var result = _controller.Update(2, updatedAlbum);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var album = Assert.IsType<Album>(okResult.Value);

            Assert.Equal(2, album.Id); // Should use the ID from the route, not the payload
        }

        // Delete Album Tests
        [Fact]
        public void Delete_WithValidId_ReturnsNoContent()
        {
            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var result = _controller.Delete(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public void Delete_RemovesAlbum_FromCollection()
        {
            // Arrange - Get initial count
            var initialResult = _controller.Get();
            var initialOkResult = Assert.IsType<OkObjectResult>(initialResult);
            var initialAlbums = Assert.IsType<List<Album>>(initialOkResult.Value);
            var initialCount = initialAlbums.Count;

            // Act - Delete an album
            _controller.Delete(3);

            // Assert - Verify count decreased
            var afterResult = _controller.Get();
            var afterOkResult = Assert.IsType<OkObjectResult>(afterResult);
            var afterAlbums = Assert.IsType<List<Album>>(afterOkResult.Value);

            Assert.Equal(initialCount - 1, afterAlbums.Count);
        }

        [Fact]
        public void Delete_CannotGetDeletedAlbum_ById()
        {
            // Arrange & Act
            _controller.Delete(4);
            var result = _controller.Get(4);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
