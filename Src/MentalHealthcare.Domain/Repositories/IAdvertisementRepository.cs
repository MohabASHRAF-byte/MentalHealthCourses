using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories;

public interface IAdvertisementRepository
{
    public Task<int> CreateAdvertisementAsync(Advertisement advertisement);
    public Task<Advertisement> GetAdvertisementByIdAsync(int advertisementId);
    public Task UpdateAdvertisementAsync(Advertisement advertisement);
    public Task DeleteAdvertisementAsync(int advertisementId);
    public Task<(int TotalCount,IEnumerable<Advertisement>)> GetAdvertisementsAsync(int pageNumber, int pageSize , int isActive);
}