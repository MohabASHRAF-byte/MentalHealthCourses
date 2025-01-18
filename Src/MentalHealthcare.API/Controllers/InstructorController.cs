using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Advertisement.Commands.Create;
using MentalHealthcare.Application.Advertisement.Commands.Update;
using MentalHealthcare.Application.Advertisement.Queries.GetAll;
using MentalHealthcare.Application.Advertisement.Queries.GetById;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Instructors.Commands.AddPhoto;
using MentalHealthcare.Application.Instructors.Commands.Create;
using MentalHealthcare.Application.Instructors.Commands.Update;
using MentalHealthcare.Application.Instructors.Queries.GetAll;
using MentalHealthcare.Application.Instructors.Queries.GetById;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers
{
    [AllowAnonymous]

    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiExplorerSettings(GroupName = Global.DashboardVersion)]

    //[Route("api/[controller]")]
    //[ApiController]
    public class InstructorController(
    IMediator mediator
) : ControllerBase
    {




        [HttpPost]
        [SwaggerOperation(
           Summary = "Creates new Instructor",
           Description = "Creates new Instructor with it's details"
       )
   ]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateInstructor([FromForm] CreateInstructorCommand command)
        {
            var instructorId = await mediator.Send(command);
            return CreatedAtAction(nameof(GetInstructorById), new { InstructorId = instructorId }, null);
        }




        [SwaggerOperation(Summary = "Get the Instructor detailed with it's id")]
        [ProducesResponseType(typeof(InstructorDto), 200)]

        [HttpGet("{instructorId}")]
        public async Task<IActionResult> GetInstructorById([FromRoute] int instructorId)
        {
            var query = new GetInstructorByIdQuery()
            {
                instructorid = instructorId
            };
            var instructor = await mediator.Send(query);
            return Ok(instructor);
        }





        [HttpPost("{InstructorId}/photo")]
        [Authorize(AuthenticationSchemes = "Bearer")]

       [SwaggerOperation(
          Summary = "add photo to Instructor"
      )
  ]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddPhoto([FromRoute] int InstructorId, [FromForm] AddPhotoCommand command)
        {
            command.InstructorsId = InstructorId;   
            var result = await mediator.Send(command);
            var operationResult = OperationResult<string>.SuccessResult(result);
            return Ok(operationResult);
        }



        [SwaggerOperation(Summary = "Updated Existing Instructor")]
        [HttpPut("{instructorId}")]
        public async Task<IActionResult> UpdateInstructor([FromRoute] int instructorId,
       [FromForm] UpdateInstructorCommand command)
        {
            command.instructorid = instructorId;
            var Instructor = await mediator.Send(command);
            return CreatedAtAction(nameof(GetInstructorById), new { instructorId = Instructor }, null);
        }







        [SwaggerOperation(Summary = "Get all Instructors")]
        [ProducesResponseType(typeof(PageResult<InstructorDto>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetAllInstructors([FromQuery] GetAllInstructorsQuery query)
        {
            var Instructors = await mediator.Send(query);
            return Ok(Instructors);
        }






    }
}
