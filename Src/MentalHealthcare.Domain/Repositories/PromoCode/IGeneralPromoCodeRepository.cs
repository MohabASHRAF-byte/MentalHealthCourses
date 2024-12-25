using MentalHealthcare.Domain.Dtos.PromoCode;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories.PromoCode;

public interface IGeneralPromoCodeRepository
{
    public Task<int> AddGeneralPromoCodeAsync(GeneralPromoCode generalPromoCode);

    public Task<GeneralPromoCode> GetGeneralPromoCodeByIdAsync(int generalPromoCodeId);

    public Task<(int TotalCount, IEnumerable<GeneralPromoCodeDto>)> GetGeneralPromoCodeAsync(
        int pageNumber,
        int pageSize, string searchText, int isActive);
    
    public Task DeleteGeneralPromoCodeByIdAsync(int generalPromoCodeId);
    public Task<GeneralPromoCode?> GetGeneralPromoCodeByPromoCodeNameAsync(string promoCodeName);
    
    public Task SaveChangesAsync();
}