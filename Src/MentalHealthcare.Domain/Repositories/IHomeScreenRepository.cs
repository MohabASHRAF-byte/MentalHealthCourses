using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Dtos.User;
using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Repositories
{
    public interface IHomeScreenRepository
    {


        Task<HomeScreenDto> GetHomeScreenDataAsync(
            int instructorCount,
            int advertisementCount,
            int recentlyAddedCoursesCount ,
            int recentlyArticlesCount
);



    }
}
