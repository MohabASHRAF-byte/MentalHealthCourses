using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories.PromoCode;

public interface ICoursePromoCodeRepository
{
    public Task<int> AddCoursePromoCode( CoursePromoCode coursePromoCode );
    
    public Task<CoursePromoCode> GetCoursePromoCodeByIdAsync(int coursePromoCodeId);
    
}