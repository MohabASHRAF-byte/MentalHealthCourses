using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories;

public class AdvertisementRepository(
    MentalHealthDbContext dbContext
) : IAdvertisementRepository
{
    public async Task<int> CreateAdvertisementAsync(Advertisement advertisement)
    {
        await dbContext.Advertisements.AddAsync(advertisement);
        await dbContext.SaveChangesAsync();
        return advertisement.AdvertisementId;
    }


    public async Task<Advertisement> GetAdvertisementByIdAsync(int advertisementId)
    {
        var advertisement = await dbContext.Advertisements
            .Where(a => a.AdvertisementId == advertisementId)
            .Select(a => new Advertisement
            {
                AdvertisementId = a.AdvertisementId,
                AdvertisementName = a.AdvertisementName,
                AdvertisementDescription = a.AdvertisementDescription,
                IsActive = a.IsActive,
                LastUploadImgCnt = a.LastUploadImgCnt,
                AdvertisementImageUrls = a.AdvertisementImageUrls
                    .Select(img => new AdvertisementImageUrl { ImageUrl = img.ImageUrl })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (advertisement == null)
            throw new ResourceNotFound(nameof(Advertisement), advertisementId.ToString());

        return advertisement;
    }


    public async Task UpdateAdvertisementAsync(Advertisement advertisement)
    {
        dbContext.Advertisements.Update(advertisement);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAdvertisementAsync(int advertisementId)
    {
        var newAdvertisement = await dbContext.Advertisements.FindAsync(advertisementId);
        if (newAdvertisement == null)
            throw new ResourceNotFound(nameof(Advertisement), advertisementId.ToString());
        dbContext.Advertisements.Remove(newAdvertisement);
        await dbContext.SaveChangesAsync();
    }

    public async Task<(int, IEnumerable<Advertisement>)> GetAdvertisementsAsync(
        int pageNumber, int pageSize, int isActive)
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

    public async Task DeleteAdvertisementPhotosUrlsAsync(int advertisementId)
    {
        var adimgs = dbContext.AdvertisementImageUrls.Where(
            img => img.AdvertisementId == advertisementId
        );
         dbContext.AdvertisementImageUrls.RemoveRange(adimgs);
         await dbContext.SaveChangesAsync();
       
    }
}