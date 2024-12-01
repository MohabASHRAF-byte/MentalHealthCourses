using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories;

public interface ITermsRepository
{
    public Task<int> AddAsync(TermsAndConditions term);
    public Task DeleteAsync(int id);
    public Task<List<TermsAndConditions>> GetAllAsync();
    
    public Task Update(TermsAndConditions term);
}