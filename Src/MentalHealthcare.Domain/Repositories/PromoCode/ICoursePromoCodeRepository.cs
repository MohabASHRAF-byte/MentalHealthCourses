using MentalHealthcare.Domain.Dtos.PromoCode;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories.PromoCode;

public interface ICoursePromoCodeRepository
{
    public Task<int> AddCoursePromoCodeAsync(CoursePromoCode coursePromoCode);

    public Task<CoursePromoCode> GetCoursePromoCodeByIdAsync(int coursePromoCodeId);

    public Task<(int TotalCount, IEnumerable<CoursePromoCodeDto>)> GetCoursePromoCodeByCourseIdAsync(
        int courseId, int pageNumber,
        int pageSize, string searchText, int isActive);

    public Task DeleteCoursePromoCodeByIdAsync(int coursePromoCodeId);

    public Task<CoursePromoCode?> CheckIfPromoCodeAppliedForCourseAsync(
        string promoCode, int courseId
    );

    public Task SaveChangesAsync();
}