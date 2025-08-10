//using MentalHealthcare.Domain.Entities;
//using MentalHealthcare.Domain.Exceptions;
//using MentalHealthcare.Domain.Repositories;
//using MentalHealthcare.Infrastructure.Persistence;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MentalHealthcare.Infrastructure.Repositories
//{
//    public class PodCasterRepository(MentalHealthDbContext dbContext) : IPodCasterRepository
//    {
//        public async Task<int> CreatePodCasterAsync(PodCaster podCaster)
//        {
//            await dbContext.PodCasters.AddAsync(podCaster);
//            await dbContext.SaveChangesAsync();
//            return podCaster.PodCasterId;
//        }

//        public async Task DeletePodCasterAsync(int podCasterId)
//        {
//            var podCaster = await dbContext.PodCasters.FindAsync(podCasterId);
//            if (podCaster == null)
//                throw new ResourceNotFound(
//                    "PodCaster",
//                    "السلسله",
//                    podCasterId.ToString()
//                );

//            dbContext.PodCasters.Remove(podCaster);
//            await dbContext.SaveChangesAsync();
//        }

//        public async Task<PodCaster> GetPodCasterByIdAsync(int podCasterId)
//        {
//            var podCaster = await dbContext.PodCasters
//                           .Where(p => p.PodCasterId == podCasterId)
//                           .Select(p => new PodCaster
//                           {
//                               PodCasterId = p.PodCasterId,
//                               Name = p.Name,
//                               About = p.About,
//                               TitleOfTopic=p.TitleOfTopic,
//                               ImageUrl = p.ImageUrl,
//                               Podcasts = p.Podcasts.Select(pc => new Podcast
//                               {PodcastId = pc.PodcastId,
//                                   Title = pc.Title
//                               }).ToList()
//                           })
//                           .FirstOrDefaultAsync();


//            if (podCaster == null)
//                throw new ResourceNotFound(
//                    "PodCaster",
//                    "السلسله",
//                    podCasterId.ToString()
//                );

//            return podCaster;





//        }

//        public async Task<(int TotalCount, IEnumerable<PodCaster>)> GetPodCastersAsync(string? search, int requestPageNumber, int requestPageSize)
//        {

//            var baseQuery = dbContext.PodCasters.AsQueryable();

//            if (!string.IsNullOrWhiteSpace(search))
//            {
//                baseQuery = baseQuery.Where(p => p.Name.Contains(search));
//            }

//            var totalCount = await baseQuery.CountAsync();

//            var podCasters = await baseQuery
//                .OrderBy(p => p.PodCasterId)
//                .Skip(requestPageSize * (requestPageNumber - 1))
//                .Take(requestPageSize)
//                .Select(p => new PodCaster
//                {
//                    PodCasterId = p.PodCasterId,
//                    Name = p.Name,
//                    TitleOfTopic = p.TitleOfTopic,
//                    ImageUrl = p.ImageUrl,
//                    Podcasts = p.Podcasts.Select(pc => new Podcast
//                    {PodcastId = pc.PodcastId,
//                        Title = pc.Title
//                    }).ToList()
//                })
//                .ToListAsync();

//            return (totalCount, podCasters);






//        }

//        public async Task<(int TotalCount, IEnumerable<PodCaster>)> GetRecentlyAddedPodCastersAsync(int count)
//        {
//            var totalCount = await dbContext.PodCasters.CountAsync();


//            var podCasters = await dbContext.PodCasters
//             .OrderByDescending(p => p.PodCasterId)
//             .Take(count)
//             .Select(p => new PodCaster
//             {
//                 PodCasterId = p.PodCasterId,
//                 Name = p.Name,
//                 About = p.About,
//                 TitleOfTopic = p.TitleOfTopic,
//                 ImageUrl = p.ImageUrl,
//                 Podcasts = p.Podcasts.Select(pc => new Podcast
//                 {PodcastId = pc.PodcastId,
//                     Title = pc.Title

//                 }).ToList()
//             })
//             .ToListAsync();

//            return (totalCount, podCasters);







//        }

//        public async Task SaveChangesAsync()
//        {
//            await dbContext.SaveChangesAsync();
//         }

//        public async Task UpdatePodCasterAsync(PodCaster podCaster)
//        {
//            dbContext.PodCasters.Update(podCaster);
//            await dbContext.SaveChangesAsync();
//        }
//    }
//}
