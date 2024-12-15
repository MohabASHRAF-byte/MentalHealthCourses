using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Courses.Commands.Create;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Contrib.ExpressionBuilders.Logging; // Add this for logging support
using Xunit;

namespace MentalHealthcare.Application.Tests.Courses.Create;

public class CreateCourseCommandHandlerTests
{
    private readonly Mock<ILogger<CreateCourseCommandHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly CreateCourseCommandHandler _handler;
    private readonly IMediator _mediator;
    //
    // public CreateCourseCommandHandlerTests()
    // {
    //     _loggerMock = new Mock<ILogger<CreateCourseCommandHandler>>();
    //     _mapperMock = new Mock<IMapper>();
    //     _courseRepositoryMock = new Mock<ICourseRepository>();
    //     _mediator = new Mock<IMediator>();
    //
    //     _handler = new CreateCourseCommandHandler(
    //         _loggerMock.Object,
    //         _mapperMock.Object,
    //         _courseRepositoryMock.Object
    //     );
    // }
    public static IEnumerable<object[]> Handle_ShouldCreateCourse_ReturnsCorrectResponseTests()
    {
        yield return ["Test Course 1", 1, 15, true, 1];
        yield return ["Test Course 2", 2, 30, false, 2];
        yield return ["Advanced Course", 3, 50, false, 3];
    }  
    public static IEnumerable<object[]> Handle_ShouldCreateCourse_ReturnsErrorNoInstructorFoundTests()
    {
        yield return ["Test Course 1", 1, 15, true, 1];
    }

    [Theory]
    [MemberData(nameof(Handle_ShouldCreateCourse_ReturnsCorrectResponseTests))]
    public async Task Handle_ShouldCreateCourse_ReturnsCorrectResponse(
        string courseName, int instructorId, decimal price, bool isFree, int expectedCourseId)
    {
        // Arrange
        var command = new CreateCourseCommand
        {
            Name = courseName,
            InstructorId = instructorId,
            Price = price,
            IsFree = isFree,
        };

        var course = new Course
        {
            Name = courseName,
            InstructorId = instructorId,
            Price = price,
            IsFree = isFree,
        };

        _mapperMock.Setup(m => m.Map<Course>(command)).Returns(course);
        _courseRepositoryMock.Setup(r => r.CreateAsync(course)).ReturnsAsync(expectedCourseId); // Return the expected course ID

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(expectedCourseId, result.CourseId);
        _mapperMock.Verify(m => m.Map<Course>(command), Times.Once);
        _courseRepositoryMock.Verify(r => r.CreateAsync(course), Times.Once);
    }
    
    
    
    [MemberData(nameof(Handle_ShouldCreateCourse_ReturnsErrorNoInstructorFoundTests))]
    
    public async Task Handle_ShouldCreateCourse_ReturnsErrorNoInstructorFound(
        string courseName, int instructorId, decimal price, bool isFree, int expectedCourseId)
    {
        // Arrange
        var command = new CreateCourseCommand
        {
            Name = courseName,
            InstructorId = instructorId,
            Price = price,
            IsFree = isFree,
        };

        var course = new Course
        {
            Name = courseName,
            InstructorId = instructorId,
            Price = price,
            IsFree = isFree,
        };

        _mapperMock.Setup(m => m.Map<Course>(command)).Returns(course);
        _courseRepositoryMock.Setup(r => r.CreateAsync(course)).ReturnsAsync(null); 

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(expectedCourseId, result.CourseId);
        _mapperMock.Verify(m => m.Map<Course>(command), Times.Once);
        _courseRepositoryMock.Verify(r => r.CreateAsync(course), Times.Once);
    }

}
