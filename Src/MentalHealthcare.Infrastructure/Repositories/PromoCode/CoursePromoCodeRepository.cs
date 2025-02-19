using MentalHealthcare.Application.PromoCode.Course;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Domain.Dtos.PromoCode;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.PromoCode;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories.PromoCode;

public class CoursePromoCodeRepository(
    MentalHealthDbContext dbContext,
    ILocalizationService localizationService
) : ICoursePromoCodeRepository
{
    public async Task<int> AddCoursePromoCodeAsync(CoursePromoCode coursePromoCode)
    {
        var promoCodeExists = await dbContext.CoursePromoCodes
            .AnyAsync(cp => cp.Code == coursePromoCode.Code);

        if (promoCodeExists)
        {
            throw new BadHttpRequestException(
                localizationService.GetMessage("CoursePromoCodeAlreadyExists")
            );
        }

        try
        {
            dbContext.CoursePromoCodes.Add(coursePromoCode);
            await dbContext.SaveChangesAsync();
        }
        catch
        {
            throw new BadHttpRequestException(
                localizationService.GetMessage("TryAgain", "Please try again.")
            );
        }

        return coursePromoCode.CoursePromoCodeId;
    }

    public async Task<CoursePromoCode> GetCoursePromoCodeByIdAsync(int coursePromoCodeId)
    {
        var coursePromoCode = await dbContext.CoursePromoCodes.FindAsync(coursePromoCodeId);
        if (coursePromoCode == null)
            throw new ResourceNotFound(
                "Course Promo Code",
                "كود الخصم للدورة",
                coursePromoCodeId.ToString()
            );
        return coursePromoCode;
    }

    public async Task<(int TotalCount, IEnumerable<CoursePromoCodeDto>)> GetCoursePromoCodeByCourseIdAsync(
        int courseId, int pageNumber, int pageSize, string searchText, int isActive)
    {
        var baseQuery = dbContext.CoursePromoCodes.AsQueryable();

        baseQuery = baseQuery.Where(cp => cp.CourseId == courseId);

        if (!string.IsNullOrWhiteSpace(searchText))
            baseQuery = baseQuery.Where(cpc => cpc.Code.ToLower().Contains(searchText.ToLower()));

        baseQuery = isActive switch
        {
            0 => baseQuery.Where(cpc => cpc.expiredate <= DateTime.Now),
            1 => baseQuery.Where(cpc => cpc.expiredate > DateTime.Now),
            _ => baseQuery
        };

        var totalCount = await baseQuery.CountAsync();

        var promoCodes = await baseQuery
            .OrderByDescending(cpc => cpc.expiredate)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .Select(cf => new CoursePromoCodeDto()
            {
                CoursePromoCodeId = cf.CoursePromoCodeId,
                Code = cf.Code,
                CourseName = cf.Course.Name,
                CourseId = cf.CourseId,
                expiredate = cf.expiredate,
                percentage = cf.percentage,
                expiresInDays = (cf.expiredate - DateTime.Now).TotalDays <= 0
                    ? 0
                    : (int)Math.Floor((cf.expiredate - DateTime.Now).TotalDays)
            })
            .ToListAsync();

        return (totalCount, promoCodes);
    }

    public async Task DeleteCoursePromoCodeByIdAsync(int coursePromoCodeId)
    {
        var coursePromoCode = await dbContext.CoursePromoCodes.FindAsync(coursePromoCodeId);
        if (coursePromoCode == null)
            throw new ResourceNotFound(
                "Course Promo Code",
                "كود الخصم للدورة",
                coursePromoCodeId.ToString()
            );
        dbContext.CoursePromoCodes.Remove(coursePromoCode);
        await dbContext.SaveChangesAsync();
    }

    public async Task<CoursePromoCode?> CheckIfPromoCodeAppliedForCourseAsync(string promoCode, int courseId)
    {
       var promocode = dbContext.CoursePromoCodes
           .FirstOrDefault(cp => cp.Code == promoCode && cp.CourseId == courseId);
       
       return promocode;
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}