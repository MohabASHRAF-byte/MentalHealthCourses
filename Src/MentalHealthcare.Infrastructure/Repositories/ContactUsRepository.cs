using MentalHealthcare.Application.Utitlites;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MentalHealthcare.Infrastructure.Repositories;

public class ContactUsRepository(
    MentalHealthDbContext dbContext
) : IContactUsRepository
{
    public async Task<int> CreateAsync(ContactUsForm contactUsForm)
    {
        await dbContext.AddAsync(contactUsForm);
        await dbContext.SaveChangesAsync();
        return contactUsForm.ContactUsFormId;
    }



    public async Task DeleteAsync(List<int> formsId)
    {
        var formsToDelete = await dbContext.ContactUses
            .Where(f => formsId.Contains(f.ContactUsFormId))
            .ToListAsync();

        dbContext.ContactUses.RemoveRange(formsToDelete);

        await dbContext.SaveChangesAsync();
    }

    public async Task<ContactUsForm> GetFromById(int formId)
    {
        var form = await dbContext.ContactUses.FindAsync(formId);
        if (form == null)
            throw new ResourceNotFound(
                "Contact Us Form",
                "نموذج اتصل بنا",
                formId.ToString()
            );
        return form;
    }

    public async Task<(int, List<ContactUsForm>)> GetAllFormsAsync(int pageNumber, int pageSize,int msgPreviewLength, string? senderName, string? senderEmail, string? senderPhone,
        bool? isRead)
    {
        var baseQuery = dbContext.ContactUses.AsQueryable();

        if (!string.IsNullOrWhiteSpace(senderName))
            baseQuery = baseQuery.Where(f => f.Name.ToLower().Contains(senderName.ToLower()));
        if(!string.IsNullOrWhiteSpace(senderEmail))
            baseQuery = baseQuery.Where(f => f.Email.ToLower().Contains(senderEmail.ToLower()));
        if(!string.IsNullOrWhiteSpace(senderPhone))
            baseQuery = baseQuery.Where(f => f.PhoneNumber.ToLower().Contains(senderPhone.ToLower()));
        if(isRead.HasValue)
            baseQuery = baseQuery.Where(f => f.IsRead == isRead.Value);

        var totalCount = await baseQuery.CountAsync();

        // Apply ordering and pagination, and limit the message to the first 50 characters
        var advertisements = await baseQuery
            .OrderBy(cf => cf.ContactUsFormId) // Order by ID
            .Skip(pageSize * (pageNumber - 1)) // Pagination: Skip
            .Take(pageSize) // Pagination: Take
            .Select(cf => new ContactUsForm
            {
                ContactUsFormId = cf.ContactUsFormId,
                Name = cf.Name,
                Email = cf.Email,
                PhoneNumber = cf.PhoneNumber,
                Message = cf.Message.Length > msgPreviewLength ? cf.Message.Substring(0, msgPreviewLength) +"...": cf.Message , // Truncate Message to 50 chars
                IsRead = cf.IsRead,
                CreatedDate = cf.CreatedDate
            })
            .ToListAsync();

        return (totalCount, advertisements);
    }

    public async Task ChangeReadState(int requestFormId, bool requestState)
    {
        var form = await dbContext.ContactUses.FindAsync(requestFormId);
        if (form == null)
            throw new ResourceNotFound(
                "Contact Us Form",
                "نموذج اتصل بنا",
                requestFormId.ToString()
            );
        form.IsRead = requestState;
        await dbContext.SaveChangesAsync();
        
    }
}