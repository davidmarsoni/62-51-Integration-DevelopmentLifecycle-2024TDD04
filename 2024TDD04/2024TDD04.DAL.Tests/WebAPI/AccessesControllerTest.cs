using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Controllers;
using Xunit;

namespace TTD04_2024.DAL.Tests.WebAPI
{
    public class AccessesControllerTest
    {
        private readonly AccessesController _accessesController;
        private readonly RoomAccessContext _testDbContext;

        public AccessesControllerTest()
        {
            _testDbContext = InMemoryRoomContext.CreateInMemoryContext();
            _accessesController = new AccessesController(_testDbContext);
        }

        [Fact]
        public async Task HasAccessUserAsync_WhenTryingToKnowIfOneUserHasAccess_ShouldReturnTrue(){
            // Arrange

            // Act
            var result = await _accessesController.HasAccessAsync(1, 1);

            // Assert
            Assert.True(result.Value);
        }
    }
}
