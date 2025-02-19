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
public class CourseReviewController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost("{courseId}/reviews")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [SwaggerOperation(Description = CourseReviewDocs.PostCourseReviewDescription)]
    [ApiExplorerSettings(GroupName = Global.MobileVersion)]

    public async Task<IActionResult> PostReview([FromRoute] int courseId, AddCourseReviewCommand command)
    {
        command.CourseId = courseId;
        var res = await mediator.Send(command);
        var op = OperationResult<object>
            .SuccessResult(new
            {
                courseReviewId = res,
            });
        return Ok(op);
    }

    [HttpGet("{courseId}/reviews")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(PageResult<UserReviewDto>), 200)]
    [SwaggerOperation(Description = CourseReviewDocs.GetCourseReviewsDescription)]
    [ApiExplorerSettings(GroupName = Global.SharedVersion)]

    public async Task<IActionResult> GetReviews(
        [FromRoute] int courseId,
        [FromQuery] GetAllCourseReviewsQuery query
    )
    {
        query.CourseId = courseId;
        var res = await mediator.Send(query);
        var op = OperationResult<PageResult<UserReviewDto>>
            .SuccessResult(res);
        return Ok(op);
    }

    [HttpGet("{courseId}/reviews/{reviewId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(UserReviewDto), 200)]
    [SwaggerOperation(Description = CourseReviewDocs.GetCourseReviewDescription)]
    [ApiExplorerSettings(GroupName = Global.SharedVersion)]

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
        var op = OperationResult<UserReviewDto>
                 .SuccessResult(res);
        return Ok(op);
    }

    [HttpPut("{courseId}/reviews/{reviewId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(204)]
    [SwaggerOperation(Description = CourseReviewDocs.UpdateCourseReviewDescription)]
    [ApiExplorerSettings(GroupName = Global.MobileVersion)]

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
    [ApiExplorerSettings(GroupName = Global.SharedVersion)]

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
