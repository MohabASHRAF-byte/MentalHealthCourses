using MentalHealthcare.Application.PromoCode.Course;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories.PromoCode;

public interface ICoursePromoCodeRepository
{
    public Task<int> AddCoursePromoCode(CoursePromoCode coursePromoCode);

    public Task<CoursePromoCode> GetCoursePromoCodeByIdAsync(int coursePromoCodeId);

    public Task<(int TotalCount, IEnumerable<CoursePromoCodeDto>)> GetCoursePromoCodeByCourseIdAsync(
        int courseId, int pageNumber,
        int pageSize, string searchText, int isActive);
}