using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Instructors.Commands.Update
{
    public class UpdateInstructorCommand : IRequest<int>
    {   public int? instructorid { get; set; }
        public string? Name { get; set; }
        public string? About { get; set; }
        public IFormFile? File { get; set; } = default!;
        public int? CollectionId { get; set; } = default!;} }