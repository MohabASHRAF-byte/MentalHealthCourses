using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using MentalHealthcare.Infrastructure.Repositories.Course;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Infrastructure.Repositories
{
    public class HomeScreenRepository(MentalHealthDbContext dbContext,
    ILogger<HomeScreenRepository> logger) : IHomeScreenRepository
    {
        public async Task<(int TotalCount, IEnumerable<Advertisement>)> GetAdvertisementsAsync(int pageNumber, int pageSize, int isActive)
        {
            // Base query
            var baseQuery = dbContext.Advertisements.AsQueryable();

            // Filter based on isActive
            if (isActive == 0)
            {
                baseQuery = baseQuery.Where(ad => !ad.IsActive);
            }
            else if (isActive == 1)
            {
                baseQuery = baseQuery.Where(ad => ad.IsActive);
            }
            // No filtering for other cases (get all advertisements)

            // Add Include after filtering
            baseQuery = baseQuery.Include(ad => ad.AdvertisementImageUrls);

            // Total count before pagination
            var totalCount = await baseQuery.CountAsync();

            // Apply ordering and pagination
            var advertisements = await baseQuery
                .OrderBy(ad => ad.AdvertisementId) // Order by ID
                .Skip(pageSize * (pageNumber - 1)) // Pagination: Skip
                .Take(pageSize) // Pagination: Take
                .Select(ad => new Advertisement
                {
                    AdvertisementId = ad.AdvertisementId,
                    AdvertisementName = ad.AdvertisementName,
                    AdvertisementDescription = ad.AdvertisementDescription,
                    IsActive = ad.IsActive,
                    AdvertisementImageUrls = ad.AdvertisementImageUrls
                        .Select(img => new AdvertisementImageUrl { ImageUrl = img.ImageUrl })
                        .ToList()
                })
                .ToListAsync();

            return (totalCount, advertisements);
        }

        public async Task<(int TotalCount, IEnumerable<Instructor>)> GetInstructorsAsync(string? search, int requestPageNumber, int requestPageSize)
        {
            // Base query
            var baseQuery = dbContext.Instructors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                baseQuery = baseQuery.Where(a => a.Name.Contains(search));
            }

            // Total count before pagination
            var totalCount = await baseQuery.CountAsync();

            // Apply ordering and pagination
            var Instructors = await baseQuery
                .OrderBy(ad => ad.InstructorId) // Order by ID
                .Skip(requestPageSize * (requestPageNumber - 1)) // Pagination: Skip
                .Take(requestPageSize) // Pagination: Take
                .Select(ad => new Instructor
                {
                    InstructorId = ad.InstructorId,
                    Name = ad.Name,
                    About = ad.About,
                    ImageUrl = ad.ImageUrl,
                    Courses = new List<Domain.Entities.Courses.Course>
                    {
                        new Domain.Entities.Courses.Course
                            { Name = ad.Name }
                    }
                })
                .ToListAsync();

            return (totalCount, Instructors);
        }

        public async Task<IEnumerable<CourseViewDto>> GetRecentlyAddedCoursesAsync(int count)
        {


         return await dbContext.Courses
        .OrderByDescending(c => c.CreatedAt) // Assuming CreatedAt exists
        .Take(count)
        .Select(c => new CourseViewDto
        {
            CourseId = c.CourseId,
            Name = c.Name,
            Price = c.Price,
            Rating = c.Rating,
            ThumbnailUrl = c.ThumbnailUrl
        })
        .ToListAsync();








        }
    }
}
