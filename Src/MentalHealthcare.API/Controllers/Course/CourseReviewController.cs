using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Courses.Reviews.Commands.AddCourseReview;
using MentalHealthcare.Application.Courses.Reviews.Commands.DeleteCourseReview;
using MentalHealthcare.Application.Courses.Reviews.Commands.UpdateCourseReview;
using MentalHealthcare.Application.Courses.Reviews.Queries.GetAllCourseReviews;
using MentalHealthcare.Application.Courses.Reviews.Queries.GetCourseReview;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers.Course;

[ApiController]
[Route("api/Course")]
[ApiExplorerSettings(GroupName = Global.DevelopmentVersion)]
public class CourseReviewController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost("{courseId}/reviews")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [SwaggerOperation(Description = CourseReviewDocs.PostCourseReviewDescription)]
    public async Task<IActionResult> PostReview([FromRoute] int courseId, AddCourseReviewCommand command)
    {
        command.CourseId = courseId;
        var res = await mediator.Send(command);
        return Ok(res);
    }

    [HttpGet("{courseId}/reviews")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(PageResult<UserReviewDto>), 200)]
    [SwaggerOperation(Description = CourseReviewDocs.GetCourseReviewsDescription)]
    public async Task<IActionResult> GetReviews(
        [FromRoute] int courseId,
        [FromQuery] GetAllCourseReviewsQuery query
    )
    {
        query.CourseId = courseId;
        var res = await mediator.Send(query);
        return Ok(res);
    }

    [HttpGet("{courseId}/reviews/{reviewId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(UserReviewDto), 200)]
    [SwaggerOperation(Description = CourseReviewDocs.GetCourseReviewDescription)]
    public async Task<IActionResult> GetReview(
        [FromRoute] int courseId,
        [FromRoute] int reviewId
    )
    {
        var query = new GetCourseReviewQuery
        {
            CourseId = courseId,
            ReviewId = reviewId
        };
        var res = await mediator.Send(query);
        return Ok(res);
    }

    [HttpPut("{courseId}/reviews/{reviewId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(204)]
    [SwaggerOperation(Description = CourseReviewDocs.UpdateCourseReviewDescription)]
    public async Task<IActionResult> UpdateReview(
        [FromRoute] int courseId,
        [FromRoute] int reviewId,
        [FromBody] UpdateCourseReviewCommand command
    )
    {
        command.CourseId = courseId;
        command.ReviewId = reviewId;
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{courseId}/reviews/{reviewId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(204)]
    [SwaggerOperation(Description = CourseReviewDocs.DeleteCourseReviewDescription)]
    public async Task<IActionResult> DeleteReview(
        [FromRoute] int courseId,
        [FromRoute] int reviewId
    )
    {
        var command = new DeleteCourseReviewCommand
        {
            ReviewId = reviewId,
            CourseId = courseId
        };
        await mediator.Send(command);
        return NoContent();
    }
}
