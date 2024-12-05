using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories;

public interface IHelpCenterRepository
{
    public Task<int> AddAsync(HelpCenterItem term);
    public Task DeleteAsync(int id);
    public Task<List<HelpCenterItem>> GetAllAsync(Global.HelpCenterItems type);
    
    public Task Update(HelpCenterItem term);
}