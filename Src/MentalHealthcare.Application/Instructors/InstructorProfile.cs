using AutoMapper;
using MentalHealthcare.Application.Instructors.Commands.Create;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Instructors
{
    public class InstructorProfile  : Profile
    {public InstructorProfile()
        {CreateMap<CreateInstructorCommand, Instructor>()
            .ForMember(dest => dest.ImageUrl, 
         opt => opt.Ignore()) // Ignore fields set elsewhere
           .ForMember(dest => dest.Courses, 
           opt => opt.Ignore()); // Ignore related collections










        }






    }
}