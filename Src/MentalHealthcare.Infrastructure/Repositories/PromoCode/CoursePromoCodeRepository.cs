using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.PromoCode;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories.PromoCode;

public class CoursePromoCodeRepository(
    MentalHealthDbContext dbContext
) : ICoursePromoCodeRepository
{
    public async Task<int> AddCoursePromoCode(CoursePromoCode coursePromoCode)
    {
        var promoCodeExists = await dbContext.CoursePromoCodes
            .AnyAsync(cp => cp.Code == coursePromoCode.Code && cp.CourseId == coursePromoCode.CourseId);

        if (promoCodeExists)
        {
            throw new ArgumentException("Course promo code already exists for the specified course.");
        }

        try
        {
            dbContext.CoursePromoCodes.Add(coursePromoCode);
            await dbContext.SaveChangesAsync();
        }
        catch
        {
            throw new ArgumentException("Please try again.");
        }

        return coursePromoCode.CoursePromoCodeId;
    }

    public async Task<CoursePromoCode> GetCoursePromoCodeByIdAsync(int coursePromoCodeId)
    {
        var coursePromoCode = await dbContext.CoursePromoCodes.FindAsync(coursePromoCodeId);
        if (coursePromoCode == null)
            throw new ResourceNotFound(nameof(CoursePromoCode), coursePromoCodeId.ToString());
        return coursePromoCode;
    }
}

