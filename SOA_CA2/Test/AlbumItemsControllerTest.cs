using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SOA_CA2.Controllers;
using SOA_CA2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SOA_CA2.Test
{
    public class AlbumItemsControllerTest
    {
        private readonly SingerContext _context;
        private readonly AlbumItemsController _controller;

        public AlbumItemsControllerTest()
        {
            // Setup in-memory database with data seeding from SingerContext
            var options = new DbContextOptionsBuilder<SingerContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new SingerContext(options); // Create the in-memory context
            _context.Database.EnsureCreated(); // Ensure the database is created and seeded
            _controller = new AlbumItemsController(_context); // Inject the context into the controller
        }

        // Helper method to mock the role in HttpContext
        private void SetUserRole(string role)
        {
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Items["UserRole"] = role;
        }

        // Test Get all albums
        [Fact]
        public async Task GetAlbumsItem_ReturnsOkResult_WhenAlbumsExist()
        {
            // Act: Call the GetAlbumsItem method
            var result = await _controller.GetAlbumsItem();

            // Assert: Check if the result is Ok and contains the expected data
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<AlbumDto>>(okResult.Value);

            // Ensure data from seed is correctly returned (assuming 3 albums are seeded)
            Assert.Equal(6, returnValue.Count);
        }

        // Test Get a specific album by ID
        [Fact]
        public async Task GetAlbumItem_ReturnsNotFound_WhenAlbumDoesNotExist()
        {
            // Act: Call GetAlbumItem with an invalid ID
            var result = await _controller.GetAlbumItem(Guid.NewGuid());

            // Assert: Verify that NotFound result is returned
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAlbumItem_ReturnsOkResult_WhenAlbumExists()
        {
            // Assuming the album with ID "3fa85f64-5717-4562-b3fc-2c963f66afc6" exists
            var albumId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afc6");

            // Act: Call GetAlbumItem with a valid ID
            var result = await _controller.GetAlbumItem(albumId);

            // Assert: Check that the result is OK and the album is returned
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var album = Assert.IsType<AlbumDto>(okResult.Value);
            Assert.Equal(albumId, album.ID);
        }

        // Test PUT: Update album (admin only)
        [Fact]
        public async Task PutAlbumItem_ReturnsForbid_WhenUserIsNotAdmin()
        {
            // Arrange: Set user role to a non-admin
            SetUserRole("User");

            var album = new AlbumItem
            {
                ID = Guid.NewGuid(),
                AlbumName = "Updated Album",
                ReleaseDate = DateTime.Now
            };

            // Act: Try to update the album
            var result = await _controller.PutAlbumItem(Guid.NewGuid(), album);

            // Assert: Ensure that the result is Forbid when user is not an admin
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task PutAlbumItem_ReturnsNoContent_WhenAdminUpdatesSuccessfully()
        {
            // Arrange: Set user role to Admin
            SetUserRole("Admin");

            var album = new AlbumItem
            {
                ID = Guid.NewGuid(),
                AlbumName = "Updated Album",
                ReleaseDate = DateTime.Now
            };

            // Act: Try to update the album
            var result = await _controller.PutAlbumItem(Guid.NewGuid(), album);

            // Assert: Ensure that the result is NoContent (successful update)
            Assert.IsType<NoContentResult>(result);
        }

        // Test POST: Create album (admin only)
        [Fact]
        public async Task PostAlbumItem_ReturnsForbid_WhenUserIsNotAdmin()
        {
            // Arrange: Set user role to non-admin
            SetUserRole("User");

            var albumItem = new AlbumItem
            {
                ID = Guid.NewGuid(),
                AlbumName = "New Album",
                ReleaseDate = DateTime.Now
            };

            // Act: Try to add an album
            var result = await _controller.PostAlbumItem(albumItem);

            // Assert: Ensure the result is Forbid when the user is not an admin
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task PostAlbumItem_ReturnsCreatedAtAction_WhenAdminAddsAlbum()
        {
            // Arrange: Set user role to admin
            SetUserRole("Admin");

            var albumItem = new AlbumItem
            {
                ID = Guid.NewGuid(),
                AlbumName = "New Album",
                ReleaseDate = DateTime.Now
            };

            // Act: Try to add the album
            var result = await _controller.PostAlbumItem(albumItem);

            // Assert: Ensure that the result is CreatedAtAction (successful creation)
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetAlbumItem", createdResult.ActionName);
        }

        // Test DELETE: Delete album (admin only)
        [Fact]
        public async Task DeleteAlbumItem_ReturnsForbid_WhenUserIsNotAdmin()
        {
            // Arrange: Set user role to non-admin
            SetUserRole("User");

            // Act: Try to delete an album
            var result = await _controller.DeleteAlbumItem(Guid.NewGuid());

            // Assert: Ensure the result is Forbid
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task DeleteAlbumItem_ReturnsNoContent_WhenAdminDeletesAlbum()
        {
            // Arrange: Set user role to admin
            SetUserRole("Admin");

            var albumId = Guid.NewGuid();
            var album = new AlbumItem
            {
                ID = albumId,
                AlbumName = "Album to Delete",
                ReleaseDate = DateTime.Now
            };

            // Act: Add album and then delete it
            await _controller.PostAlbumItem(album);
            var result = await _controller.DeleteAlbumItem(albumId);

            // Assert: Ensure that the result is NoContent (successful deletion)
            Assert.IsType<NoContentResult>(result);
        }
    }
}
