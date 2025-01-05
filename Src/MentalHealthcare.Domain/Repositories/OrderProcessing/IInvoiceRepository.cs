using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Dtos.OrderProcessing;
using MentalHealthcare.Domain.Entities.OrderProcessing;

namespace MentalHealthcare.Domain.Repositories.OrderProcessing;

public interface IInvoiceRepository
{
    public Task AddAsync(Invoice invoice);
    public Task<InvoiceDto> GetInvoiceByIdAsync(int id, int? userId = null);

    public Task<(int, List<InvoiceViewDto>)> GetInvoicesAsync(
        int pageNumber,
        int pageSize,
        string? invoiceId,
        OrderStatus? status, string? email,
        string? phoneNumber, DateTime? fromDate, DateTime? toDate,
        string? promoCode,
        int? systemUserId = null
    );

    public Task AcceptInvoice(
        int invoiceId,
        List<MiniCourse> courses,
        decimal discount,
        int adminId
    );

    public Task ChangeInvoiceState(
        int invoiceId,
        OrderStatus status
    );
}