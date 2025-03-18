using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Infrastructure.Persistence;
using MentalHealthcare.Infrastructure.Repositories.Course;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentalHealthcare.Infrastructure.Repositories
{
    public class HomeScreenRepository(
        MentalHealthDbContext dbContext,
        ILogger<HomeScreenRepository> logger,
        IInstructorRepository insRepo,
        IAdvertisementRepository AdvRepo,
        ICourseRepository CouRepo) : IHomeScreenRepository
    {
        public async Task<HomeScreenDto> GetHomeScreenDataAsync(
            int instructorCount,
            int advertisementCount,
            int recentlyAddedCoursesCount)
        {


        // Fetch instructors (limited to instructorCount)
    var (instructorsTotalCount, instructorEntities) = await insRepo.GetRecentlyAddedInstructorsAsync(instructorCount);

 // Map instructors
    var instructors = instructorEntities
        .Select(i => new InstructorDto
        {
            InstructorId = i.InstructorId,
            Name = i.Name,
            About = i.About,
            ImageUrl = i.ImageUrl,
            RecentlyAddedCourses = i.Courses.Select(c => new CourseViewDto
            {
                CourseId = c.CourseId,
                Name = c.Name,
                Price = c.Price,
                Rating = c.Rating,
                ThumbnailUrl = c.ThumbnailUrl
            }).ToList()
        })
        .ToList();




    // Fetch advertisements (limited to advertisementCount)
    var (advertisementsTotalCount, advertisementEntities) = await AdvRepo.GetRecentlyAddedAdvertisementsAsync(advertisementCount);
  // Map advertisements
    var advertisements = advertisementEntities
        .Select(a => new AdvertisementDto
        {
            AdvertisementId = a.AdvertisementId,
            AdvertisementName = a.AdvertisementName,
            AdvertisementImageUrls = a.AdvertisementImageUrls?.Select(img => img.ImageUrl).ToList() ?? new List<string>(),
            AdvertisementDescription = a.AdvertisementDescription,
            IsActive = a.IsActive
        })
        .ToList();



    // Fetch recently added courses (limited to recentlyAddedCoursesCount)
    var recentlyAddedCourses = await CouRepo.GetRecentlyAddedCoursesAsync(recentlyAddedCoursesCount);

  
    // Map courses
    var recentlyAddedCoursesList = recentlyAddedCourses
        .Select(c => new CourseViewDto
        {
            CourseId = c.CourseId,
            Name = c.Name,
            Price = c.Price,
            Rating = c.Rating,
            ThumbnailUrl = c.ThumbnailUrl
        })
        .ToList();

   

    // Return DTO
    return new HomeScreenDto
    {
        Advertisements = advertisements,
        AdvertisementsTotalCount = advertisementsTotalCount,
        Instructors = instructors,
        InstructorsTotalCount = instructorsTotalCount,
        RecentlyAddedCourses = recentlyAddedCoursesList
    };
        }
    }
} // // Fetch recently added courses (limited to recentlyAddedCoursesCount)
            // var recentlyAddedCourses = await CouRepo.GetRecentlyAddedCoursesAsync(recentlyAddedCoursesCount);

            // var (advertisementsTotalCount, advertisementEntities) = await AdvRepo.GetRecentlyAddedAdvertisementsAsync(advertisementCount);
            //// var (instructorTotalCount, instructorEntities) = await AdvRepo.GetRecentlyAddedAdvertisementsAsync(advertisementCount);

            // var advertisements = advertisementEntities
            //     .Select(a => new AdvertisementDto
            //     {
            //         AdvertisementId = a.AdvertisementId,
            //         AdvertisementName = a.AdvertisementName,
            //         AdvertisementImageUrls = a.AdvertisementImageUrls?.Select(img => img.ImageUrl).ToList() ?? new List<string>(),
            //         AdvertisementDescription = a.AdvertisementDescription,
            //         IsActive = a.IsActive
            //     })
            //     .ToList();


            // Map instructors
            //var instructors = instructorEntities
            //    .Select(i => new InstructorDto
            //    {
            //        InstructorId = i.InstructorId,
            //        Name = i.Name,
            //        About = i.About,
            //        ImageUrl = i.ImageUrl,
            //        RecentlyAddedCourses = i.Courses.Select(c => new CourseViewDto
            //        {
            //            Name = c.Name
            //        }).ToList()
            //    })
            //    .ToList();

            // Map courses
            //var recentlyAddedCoursesList = recentlyAddedCourses
            //    .Select(c => new CourseViewDto
            //    {
            //        Name = c.Name
            //    })
            //    .ToList();
