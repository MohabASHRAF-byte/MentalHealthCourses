using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories;

public class HelpCenterRepository(
    MentalHealthDbContext dbContext
) : IHelpCenterRepository
{
    public async Task<int> AddAsync(HelpCenterItem helpCenterItem)
    {
        await dbContext.HelpCenterItems.AddAsync(helpCenterItem);
        await dbContext.SaveChangesAsync();
        return helpCenterItem.HelpCenterItemId;
    }

    public async Task DeleteAsync(int id)
    {
        var term = dbContext.HelpCenterItems.FirstOrDefault(t => t.HelpCenterItemId == id);
        if (term == null)
            throw new ResourceNotFound("HelpCenterItems", id.ToString());
        dbContext.HelpCenterItems.Remove(term);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<HelpCenterItem>> GetAllAsync(Global.HelpCenterItems type)
    {
        var terms = await dbContext.HelpCenterItems
            .Where(hc => hc.Type == type)
            .OrderBy(x => x.HelpCenterItemId)
            .ToListAsync();
        return terms;
    }

    public async Task Update(HelpCenterItem term)
    {
        var newTerm =
            dbContext.HelpCenterItems.FirstOrDefault(
                t => t.HelpCenterItemId == term.HelpCenterItemId
                     && t.Type == term.Type
            );
        if (newTerm == null)
            throw new ResourceNotFound("HelpCenterItems", term.HelpCenterItemId.ToString());
        newTerm.Description = term.Description;
        newTerm.Name = term.Name;
        await dbContext.SaveChangesAsync();
    }
}