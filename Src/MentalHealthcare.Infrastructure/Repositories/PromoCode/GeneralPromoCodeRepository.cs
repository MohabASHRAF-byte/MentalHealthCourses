using MentalHealthcare.Domain.Dtos.PromoCode;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.PromoCode;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories.PromoCode;

public class GeneralPromoCodeRepository(
    MentalHealthDbContext dbContext
) : IGeneralPromoCodeRepository
{
    public async Task<int> AddGeneralPromoCodeAsync(GeneralPromoCode generalPromoCode)
    {
        dbContext.GeneralPromoCodes.Add(generalPromoCode);
        await dbContext.SaveChangesAsync();
        return generalPromoCode.GeneralPromoCodeId;
    }

    public async Task<GeneralPromoCode> GetGeneralPromoCodeByIdAsync(int generalPromoCodeId)
    {
        var generalPromoCode = await dbContext.GeneralPromoCodes.FindAsync(generalPromoCodeId);
        if (generalPromoCode == null)
            throw new ResourceNotFound(nameof(GeneralPromoCode), generalPromoCodeId.ToString());
        return generalPromoCode;
    }

    public async Task<(int TotalCount, IEnumerable<GeneralPromoCodeDto>)> GetGeneralPromoCodeAsync(
        int pageNumber, int pageSize,
        string searchText, int isActive)
    {
        var baseQuery = dbContext.GeneralPromoCodes.AsQueryable();


        if (!string.IsNullOrWhiteSpace(searchText))
            baseQuery = baseQuery.Where(gpc => gpc.Code.ToLower().Contains(searchText.ToLower()));

        baseQuery = isActive switch
        {
            0 => baseQuery.Where(gpc => gpc.expiredate <= DateTime.Now || gpc.isActive == false),
            1 => baseQuery.Where(gpc => gpc.expiredate > DateTime.Now || gpc.isActive == true),
            _ => baseQuery
        };

        var totalCount = await baseQuery.CountAsync();

        var promoCodes = await baseQuery
            .OrderByDescending(gpc => gpc.expiredate)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .Select(gpc => new GeneralPromoCodeDto()
            {
                GeneralPromoCodeId = gpc.GeneralPromoCodeId,
                Code = gpc.Code,
                isActive = gpc.isActive,
                expiredate = gpc.expiredate,
                percentage = gpc.percentage,
                expiresInDays = (gpc.expiredate - DateTime.Now).TotalDays <= 0
                    ? 0
                    : (int)Math.Floor((gpc.expiredate - DateTime.Now).TotalDays)
            })
            .ToListAsync();
        return (totalCount, promoCodes);
    }

    public async Task DeleteGeneralPromoCodeByIdAsync(int generalPromoCodeId)
    {
        var generalPromoCode = await dbContext.GeneralPromoCodes.FindAsync(generalPromoCodeId);
        if (generalPromoCode == null)
            throw new ResourceNotFound(nameof(generalPromoCodeId), generalPromoCodeId.ToString());
        dbContext.GeneralPromoCodes.Remove(generalPromoCode);
        await dbContext.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}