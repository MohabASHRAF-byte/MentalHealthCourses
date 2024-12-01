using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories;

public class TermsRepository(
    MentalHealthDbContext dbContext
) : ITermsRepository
{
    public async Task<int> AddAsync(TermsAndConditions term)
    {
        await dbContext.TermsAndConditions.AddAsync(term);
        await dbContext.SaveChangesAsync();
        return term.TermsAndConditionsId;
    }

    public async Task DeleteAsync(int id)
    {
        var term = dbContext.TermsAndConditions.FirstOrDefault(t => t.TermsAndConditionsId == id);
        if (term == null)
            throw new ResourceNotFound("TermsAndConditions", id.ToString());
        dbContext.TermsAndConditions.Remove(term);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<TermsAndConditions>> GetAllAsync()
    {
        var terms = await dbContext.TermsAndConditions
            .OrderBy(x => x.TermsAndConditionsId)
            .ToListAsync();
        return terms;
    }

    public async Task Update(TermsAndConditions term)
    {
        var newTerm =
            dbContext.TermsAndConditions.FirstOrDefault(t => t.TermsAndConditionsId == term.TermsAndConditionsId);
        if (newTerm == null)
            throw new ResourceNotFound("TermsAndConditions", term.TermsAndConditionsId.ToString());
        newTerm.Description = term.Description;
        newTerm.Name = term.Name;
        await dbContext.SaveChangesAsync();
    }
}