using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories;

public interface IContactUsRepository
{
    public Task<int> CreateAsync(ContactUsForm contactUsForm);
    public Task DeleteAsync(List<int> formsId);
    public Task<ContactUsForm> GetFromById(int formId);

    public Task<(int, List<ContactUsForm>)> GetAllFormsAsync(
        int pageNumber,
        int pageSize,
        int msgPreviewLen,
        string? senderName,
        string? senderEmail,
        string? senderPhone,
        bool? isRead 
    );

    public Task ChangeReadState(int requestFormId, bool requestState);
}