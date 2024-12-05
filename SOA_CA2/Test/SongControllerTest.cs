using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOA_CA2.Controllers;
using SOA_CA2.Models;
using Xunit;

namespace SOA_CA2.Test
{
    public class SongControllerTest
    {
        private readonly SingerContext _context;
        private readonly SongItemsController _controller;

        public SongControllerTest()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<SingerContext>()
                .UseInMemoryDatabase(databaseName: "SingersList")
                .Options;

            _context = new SingerContext(options);
            _context.Database.EnsureCreated(); // Seed the data
            _controller = new SongItemsController(_context);
        }

        [Fact]
        public async Task GetSongsItem_ReturnsAllSongs()
        {
            // Act
            var result = await _controller.GetSongsItem();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var songs = Assert.IsType<List<SongDto>>(okResult.Value);

            Assert.NotNull(songs);
            Assert.Equal(6, songs.Count); // Verify seeded data
        }

        [Fact]
        public async Task GetSongItem_ReturnsSingleSong_WhenSongExists()
        {
            // Arrange
            var existingSongId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb5");

            // Act
            var result = await _controller.GetSongItem(existingSongId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var song = Assert.IsType<SongDto>(okResult.Value);

            Assert.NotNull(song);
            Assert.Equal("oath sign", song.SongName);
        }

        [Fact]
        public async Task GetSongItem_ReturnsNotFound_WhenSongDoesNotExist()
        {
            // Arrange
            var nonExistingSongId = Guid.NewGuid();

            // Act
            var result = await _controller.GetSongItem(nonExistingSongId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostSongItem_AddsNewSong_WhenUserIsAdmin()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.Items["UserRole"] = "Admin";
            var newSong = new SongItem
            {
                ID = Guid.NewGuid(),
                SongName = "New Song",
                SongDuration = 3.45,
                Lyricist = "Test Lyricist",
                Composer = "Test Composer",
                Arranger = "Test Arranger",
                SongURL = "https://example.com",
                AlbumID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afc6")
            };

            // Act
            var result = await _controller.PostSongItem(newSong);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var song = Assert.IsType<SongItem>(createdResult.Value);

            Assert.NotNull(song);
            Assert.Equal(newSong.SongName, song.SongName);
        }

        [Fact]
        public async Task PutSongItem_UpdatesExistingSong_WhenUserIsAdmin()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.Items["UserRole"] = "Admin";
            var existingSongId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb5");
            var updatedSong = new SongItem
            {
                ID = existingSongId,
                SongName = "Updated Song",
                SongDuration = 4.00,
                Lyricist = "Updated Lyricist",
                Composer = "Updated Composer",
                Arranger = "Updated Arranger",
                SongURL = "https://updatedexample.com",
                AlbumID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afc6")
            };

            // Act
            var result = await _controller.PutSongItem(existingSongId, updatedSong);

            // Assert
            Assert.IsType<NoContentResult>(result);

            var updatedEntity = await _context.SongsItem.FindAsync(existingSongId);
            Assert.NotNull(updatedEntity);
            Assert.Equal("Updated Song", updatedEntity.SongName);
        }

        [Fact]
        public async Task DeleteSongItem_DeletesSong_WhenUserIsAdmin()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.Items["UserRole"] = "Admin";
            var existingSongId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb5");

            // Act
            var result = await _controller.DeleteSongItem(existingSongId);

            // Assert
            Assert.IsType<NoContentResult>(result);

            var deletedEntity = await _context.SongsItem.FindAsync(existingSongId);
            Assert.Null(deletedEntity);
        }

        [Fact]
        public async Task PostSongItem_ReturnsForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.Items["UserRole"] = "User";
            var newSong = new SongItem
            {
                ID = Guid.NewGuid(),
                SongName = "Forbidden Song",
                SongDuration = 4.00,
                Lyricist = "Test",
                Composer = "Test",
                Arranger = "Test",
                SongURL = "https://example.com",
                AlbumID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afc6")
            };

            // Act
            var result = await _controller.PostSongItem(newSong);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }
    }
}
