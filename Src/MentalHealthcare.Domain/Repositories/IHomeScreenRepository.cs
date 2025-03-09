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
        public Task<(int TotalCount, IEnumerable<Advertisement>)> GetAdvertisementsAsync(int pageNumber, int pageSize, int isActive);
        public Task<(int TotalCount, IEnumerable<Instructor>)> GetInstructorsAsync(string? search, int requestPageNumber, int requestPageSize);

        public Task<IEnumerable<CourseViewDto>> GetRecentlyAddedCoursesAsync(int count);







    }
}
