using System.Threading.Tasks;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Infrastructure.Persistence;
using MentalHealthcare.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MentalHealthcare.Infrastructure.Tests.Repositories
{
    public class CourseRepositoryTests
    {
        private MentalHealthDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<MentalHealthDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            return new MentalHealthDbContext(options);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddCourseAndReturnId()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var logger = new Mock<ILogger<CourseRepository>>(); // Mock the ILogger
            var repository = new CourseRepository(dbContext, logger.Object);
            var course = new Course { Name = "Test Course" }; // No CourseId here, let it auto-generate

            // Act
            var result = await repository.CreateAsync(course);

            // Assert
            Assert.NotEqual(0, result); // Ensure that an ID is returned
            var createdCourse = await dbContext.Courses.FindAsync(result);
            Assert.NotNull(createdCourse); // The course should exist in the database
            Assert.Equal("Test Course", createdCourse.Name); // The course name should match
        }
       

    }
}
