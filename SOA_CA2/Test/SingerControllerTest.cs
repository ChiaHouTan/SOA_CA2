using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore;
using SOA_CA2.Controllers;
using SOA_CA2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SOA_CA2.Test
{
    public class SingerItemsControllerTest
    {
        private readonly SingerContext _context;
        private readonly SingerItemsController _controller;

        public SingerItemsControllerTest()
        {
            // Setup in-memory database with data seeding from SingerContext
            var options = new DbContextOptionsBuilder<SingerContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new SingerContext(options); // Create the in-memory context
            _context.Database.EnsureCreated(); // Ensure the database is created
            _controller = new SingerItemsController(_context); // Inject the context into the controller
        }

        // Helper method to mock the role in HttpContext
        private void SetUserRole(string role)
        {
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Items["UserRole"] = role;
        }

        // Test Get all singers
        [Fact]
        public async Task GetSingers_ReturnsOkResult_WhenSingersExist()
        {
            SetUserRole("Admin");
            // Arrange: Seed data
            var singer1 = new SingerItem
            {
                ID = Guid.NewGuid(),
                SingerName = "Singer 1",
                SingerAge = 25,
                SingerGender = SingerItem.gender.male,
                YearOfDebut = new DateTime(2010, 1, 1)
            };
            var singer2 = new SingerItem
            {
                ID = Guid.NewGuid(),
                SingerName = "Singer 2",
                SingerAge = 30,
                SingerGender = SingerItem.gender.female,
                YearOfDebut = new DateTime(2010, 1, 1)
            };
            _context.SingersItem.Add(singer1);
            _context.SingersItem.Add(singer2);
            await _context.SaveChangesAsync();

            // Act: Call the GetSingers method
            var result = await _controller.GetSingersItem();

            // Assert: Check if the result is Ok and contains the expected data
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<SingerDto>>(okResult.Value);

            // Ensure the seed data is returned correctly
            Assert.Equal(2, returnValue.Count);
        }

        // Test Get a specific singer by ID
        [Fact]
        public async Task GetSinger_ReturnsNotFound_WhenSingerDoesNotExist()
        {
            // Act: Call GetSinger with an invalid ID
            var result = await _controller.GetSingerItem(Guid.NewGuid());

            // Assert: Verify that NotFound result is returned
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetSinger_ReturnsOkResult_WhenSingerExists()
        {
            // Arrange: Seed data
            var singer = new SingerItem
            {
                ID = Guid.NewGuid(),
                SingerName = "Singer 3",
                SingerAge = 28,
                SingerGender = SingerItem.gender.unknown,
                YearOfDebut = new DateTime(2010, 1, 1)
            };
            _context.SingersItem.Add(singer);
            await _context.SaveChangesAsync();

            // Act: Call GetSinger with the valid ID of the seeded singer
            var result = await _controller.GetSingerItem(singer.ID);

            // Assert: Check that the result is OK and the singer is returned
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var singerDto = Assert.IsType<SingerDto>(okResult.Value);
            Assert.Equal(singer.ID, singerDto.ID);
        }

        // Test PUT: Update singer (admin only)
        [Fact]
        public async Task PutSinger_ReturnsForbid_WhenUserIsNotAdmin()
        {
            // Arrange: Set user role to a non-admin
            SetUserRole("User");

            var singer = new SingerItem
            {
                ID = Guid.NewGuid(),
                SingerName = "Updated Singer",
                SingerAge = 35,
                SingerGender = SingerItem.gender.male,
                YearOfDebut = DateTime.Parse("2010-05-01")
            };

            // Act: Try to update the singer
            var result = await _controller.PutSingerItem(Guid.NewGuid(), singer);

            // Assert: Ensure that the result is Forbid when user is not an admin
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task PutSinger_ReturnsNoContent_WhenAdminUpdatesSuccessfully()
        {
            // Arrange: Set user role to Admin
            SetUserRole("Admin");

            var singer = new SingerItem
            {
                ID = Guid.NewGuid(),
                SingerName = "Updated Singer",
                SingerAge = 35,
                SingerGender = SingerItem.gender.male,
                YearOfDebut = DateTime.Parse("2010-05-01")
            };

            // Act: Add the singer and then update it
            await _controller.PostSingerItem(singer);
            var result = await _controller.PutSingerItem(singer.ID, singer);

            // Assert: Ensure that the result is NoContent (successful update)
            Assert.IsType<NoContentResult>(result);
        }

        // Test POST: Create singer (admin only)
        [Fact]
        public async Task PostSinger_ReturnsForbid_WhenUserIsNotAdmin()
        {
            // Arrange: Set user role to non-admin
            SetUserRole("User");

            var singer = new SingerItem
            {
                ID = Guid.NewGuid(),
                SingerName = "New Singer",
                SingerAge = 25,
                SingerGender = SingerItem.gender.male,
                YearOfDebut = new DateTime(2010, 1, 1)
            };

            // Act: Try to add a singer
            var result = await _controller.PostSingerItem(singer);

            // Assert: Ensure the result is Forbid when the user is not an admin
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task PostSinger_ReturnsCreatedAtAction_WhenAdminAddsSinger()
        {
            // Arrange: Set user role to admin
            SetUserRole("Admin");

            var singer = new SingerItem
            {
                ID = Guid.NewGuid(),
                SingerName = "New Singer",
                SingerAge = 25,
                SingerGender = SingerItem.gender.male,
                YearOfDebut = DateTime.Parse("2020-01-01")
            };

            // Act: Try to add the singer
            var result = await _controller.PostSingerItem(singer);

            // Assert: Ensure that the result is CreatedAtAction (successful creation)
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetSingerItem", createdResult.ActionName);
        }

        // Test DELETE: Delete singer (admin only)
        [Fact]
        public async Task DeleteSinger_ReturnsForbid_WhenUserIsNotAdmin()
        {
            // Arrange: Set user role to non-admin
            SetUserRole("User");

            // Act: Try to delete a singer
            var result = await _controller.DeleteSingerItem(Guid.NewGuid());

            // Assert: Ensure the result is Forbid
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task DeleteSinger_ReturnsNoContent_WhenAdminDeletesSinger()
        {
            // Arrange: Set user role to admin
            SetUserRole("Admin");

            var singer = new SingerItem
            {
                ID = Guid.NewGuid(),
                SingerName = "Singer to Delete",
                SingerAge = 28,
                SingerGender = SingerItem.gender.female,
                YearOfDebut = DateTime.Parse("2015-08-01")
            };

            // Act: Add singer and then delete it
            await _controller.PostSingerItem(singer);
            var result = await _controller.DeleteSingerItem(singer.ID);

            // Assert: Ensure that the result is NoContent (successful deletion)
            Assert.IsType<NoContentResult>(result);
        }
    }
}