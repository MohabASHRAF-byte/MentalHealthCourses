using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.BunnyServices.VideoContent.Collection.Add;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Commands.Create;
  /// <summary>
    /// Processes the creation of a new course based on the given command.
    /// </summary>
    /// <param name="request">The command containing the details required to create a new course.</param>
    /// <param name="cancellationToken">Token to observe for cancellation requests.</param>
    /// <returns>A task that represents the result of the operation, containing the ID of the created course.</returns>
    /// 
    /// <remarks>
    /// The following describes the logic flow of the method:
    /// <list type="number">
    /// <item>
    /// <description>Log the start of the course creation process with the provided course name.</description>
    /// </item>
    /// <item>
    /// <description>Create a new collection:
    /// <list type="bullet">
    /// <item>Initialize an <c>AddCollectionCommand</c> with the collection name from the request.</item>
    /// <item>Send the collection command using the mediator to create the collection asynchronously.</item>
    /// </list>
    /// </description>
    /// </item>
    /// <item>
    /// <description>Map the request to a new <see cref="Course"/> object:
    /// <list type="bullet">
    /// <item>Use <c>mapper.Map&lt;Course&gt;(request)</c> to convert the command to a course entity.</item>
    /// <item>Set the <c>CollectionId</c> of the course to the ID returned from the collection creation.</item>
    /// </list>
    /// </description>
    /// </item>
    /// <item>
    /// <description>Insert the course into the database:
    /// <list type="bullet">
    /// <item>Call <c>courseRepository.CreateAsync(course)</c> to save the course asynchronously.</item>
    /// </list>
    /// </description>
    /// </item>
    /// <item>
    /// <description>Prepare the response:
    /// <list type="bullet">
    /// <item>Create a new <see cref="CreateCourseCommandResponse"/> with the ID of the newly created course.</item>
    /// </list>
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <exception cref="Exception">
    /// Thrown if there is an error during the creation of the course or collection.
    /// </exception>
public class CreateCourseCommandHandler(
    ILogger<CreateCourseCommandHandler> logger,
    IMapper mapper,
    ICourseRepository courseRepository,
    IConfiguration configuration
    ): IRequestHandler<CreateCourseCommand, CreateCourseCommandResponse>
{
  
    public async Task<CreateCourseCommandResponse> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        // Log the start of the course creation process
        logger.LogInformation("Creating new course: {CourseName}", request.Name);
        //Todo: add auth 
        var bunny = new BunnyClient(configuration);
        var thumbnailResponse = await bunny.UploadFileAsync(request.Thumbnail, $"{request.Name}.jpeg", $"CoursesThumbnail");
        if (!thumbnailResponse.IsSuccessful)
        {
            logger.LogWarning("Failed to upload thumbnail image: {ErrorMessage}", thumbnailResponse.Message);
            throw new TryAgain();
        }
        logger.LogInformation("Creating collection for course: {CourseName}", request.Name);
        var collectionId = await bunny.CreateVideoFolderAsync(request.Name);
        if (collectionId == null)
        {
            logger.LogWarning($"Cannot create folder {request.Name}");
            throw new TryAgain();
        }
        logger.LogInformation("Collection created with ID: {CollectionId}", collectionId);
    
        var course = mapper.Map<Domain.Entities.Course>(request);
        course.CollectionId = collectionId;
        course.ThumbnailUrl = thumbnailResponse.Url;
        course.ThumbnailName = $"{request.Name}.jpeg";
        logger.LogInformation("Inserting  {CourseName} into dataBase :", request.Name);
        var id = await courseRepository.CreateAsync(course);
    
        logger.LogInformation("Course created successfully with ID: {CourseId}", id);
    
        var ret = new CreateCourseCommandResponse
        {
            CourseId = id
        };
        return ret;
    }

}
