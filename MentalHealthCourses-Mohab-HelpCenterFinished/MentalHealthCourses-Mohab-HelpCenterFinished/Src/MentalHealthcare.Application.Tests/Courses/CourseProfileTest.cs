using AutoMapper;
using MentalHealthcare.Application.Courses;
using MentalHealthcare.Application.Courses.Commands.Create;
using MentalHealthcare.Domain.Entities;
using Xunit;

namespace MentalHealthcare.Application.Tests.Courses
{
    public class CourseProfileTests
    {
        private readonly IMapper _mapper;

        public CourseProfileTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CourseProfile>();
            });
            
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public void CreateCourseCommand_ShouldMapTo_Course()
        {
            // Arrange: Create a sample CreateCourseCommand
            var createCourseCommand = new CreateCourseCommand
            {
                Name = "Test Course",
                Price = 50,
                Description = "Test Description",
                IsFree = true,
                InstructorId = 1
            };

            // Act: Perform the mapping
            var mappedCourse = _mapper.Map<Course>(createCourseCommand);

            // Assert: Verify that the properties are correctly mapped
            Assert.Equal(createCourseCommand.Name, mappedCourse.Name);
            Assert.Equal(createCourseCommand.Price, mappedCourse.Price);
            Assert.Equal(createCourseCommand.Description, mappedCourse.Description);
            Assert.Equal(createCourseCommand.IsFree, mappedCourse.IsFree);
            Assert.Equal(createCourseCommand.InstructorId, mappedCourse.InstructorId);
        }

        [Fact]
        public void Course_ShouldMapTo_CreateCourseCommand()
        {
            // Arrange: Create a sample Course entity
            var course = new Course
            {
                CourseId = 1,
                Name = "Test Course",
                Price = 50,
                Description = "Test Description",
                IsFree = true,
                InstructorId = 1
            };

            // Act: Perform the mapping
            var mappedCommand = _mapper.Map<CreateCourseCommand>(course);

            Assert.Equal(course.Name, mappedCommand.Name);
            Assert.Equal(course.Price, mappedCommand.Price);
            Assert.Equal(course.Description, mappedCommand.Description);
            Assert.Equal(course.IsFree, mappedCommand.IsFree);
            Assert.Equal(course.InstructorId, mappedCommand.InstructorId);
        }
    }
}
