using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.OrderProcessing;
using MentalHealthcare.Domain.Entities.OrderProcessing;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.OrderProcessing;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories.OrderProcessing;

public class InvoiceRepository(
    MentalHealthDbContext dbContext
) : IInvoiceRepository
{
    public async Task AddAsync(Invoice invoice)
    {
        dbContext.Add(invoice);
        await dbContext.SaveChangesAsync();
    }

    public async Task<InvoiceDto> GetInvoiceByIdAsync(int id, string? userId = null)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invoice ID must be greater than 0.", nameof(id));
        }

        // Query invoice and project directly into InvoiceDto
        var invoiceDto = await dbContext.Invoices
            .Where(i => i.InvoiceId == id)
            .Where(i => string.IsNullOrEmpty(userId) || i.UserId == userId)
            .Select(i => new InvoiceDto
            {
                InvoiceId = i.InvoiceId,
                UserId = i.UserId,
                FirstName = dbContext.SystemUsers
                    .Where(s => s.UserId == i.UserId)
                    .Select(s => s.FName)
                    .FirstOrDefault(),
                LastName = dbContext.SystemUsers
                    .Where(s => s.UserId == i.UserId)
                    .Select(s => s.LName)
                    .FirstOrDefault(),
                PhoneNumber = dbContext.Users
                    .Where(u => u.Id == i.UserId)
                    .Select(u => u.PhoneNumber)
                    .FirstOrDefault(),
                Email = dbContext.Users
                    .Where(u => u.Id == i.UserId)
                    .Select(u => u.Email)
                    .FirstOrDefault(),
                OrderStatus = OrderStatus.Pending,
                Courses = i.Courses,
                TotalPrice = i.TotalPrice,
                Notes = i.Notes,
                DiscountPercentage = i.DiscountPercentage,
                SubTotalPrice = i.SubTotalPrice,
                NumberOfItems = i.NumberOfItems,
                TaxesValue = i.TaxesValue,
                TaxesPercentage = i.TaxesPercentage,
                OrderDate = i.OrderDate,
                PromoCode = i.PromoCode,
                DiscountValue = i.DiscountValue
            })
            .FirstOrDefaultAsync();

        if (invoiceDto == null)
        {
            throw new ResourceNotFound(nameof(Invoice), id.ToString());
        }

        return invoiceDto;
    }

    public async Task<(int, List<InvoiceViewDto>)> GetInvoicesAsync(int pageNumber, int pageSize,
        string? invoiceId,
        OrderStatus? status, string? email,
        string? phoneNumber, DateTime? fromDate, DateTime? toDate,
        string? promoCode, string? userId = null)
    {
        // Start with all invoices
        var invoices = dbContext.Invoices.AsNoTracking();

        // Apply filters in order of expected elimination effectiveness
        if (!string.IsNullOrEmpty(userId))
        {
            invoices = invoices.Where(i => i.UserId == userId);
        }

        if (!string.IsNullOrEmpty(invoiceId))
        {
            var normalizedInvoiceId = invoiceId.ToString().ToLower();
            invoices = invoices.Where(i => i.InvoiceId.ToString() == normalizedInvoiceId);
        }

        if (!string.IsNullOrEmpty(email))
        {
            var normalizedEmail = email.ToLower().Trim();
            invoices = invoices
                .Where(i => i.User.Email != null &&
                            i.User.Email.ToLower().Contains(normalizedEmail)
                );
        }

        if (!string.IsNullOrEmpty(phoneNumber))
        {
            var normalizedPhone = phoneNumber.ToLower().Trim() ?? "";
            invoices = invoices
                .Where(i => i.User.PhoneNumber != null &&
                            i.User.PhoneNumber.ToLower().Contains(normalizedPhone)
                );
        }

        // Filter by date range
        if (fromDate.HasValue)
        {
            invoices = invoices.Where(i => i.OrderDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            invoices = invoices.Where(i => i.OrderDate <= toDate.Value);
        }

        // Filter by user ID


        // Filter by order status
        if (status.HasValue)
        {
            invoices = invoices.Where(i => i.OrderStatus == status);
        }

        // Filter by promo code
        if (!string.IsNullOrEmpty(promoCode))
        {
            invoices = invoices.Where(i => i.PromoCode == promoCode);
        }

        // Get total count before applying pagination
        var totalCount = await invoices.CountAsync();

        // Apply pagination and projection
        var paginatedInvoices = await invoices
            .OrderByDescending(i => i.OrderDate)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .Select(i => new InvoiceViewDto
            {
                InvoiceId = i.InvoiceId,
                NumberOfItems = i.NumberOfItems,
                TotalPrice = i.TotalPrice,
                OrderDate = i.OrderDate,
                OrderStatus = i.OrderStatus,
                AdminName = dbContext.Admins
                    .Where(a => a.AdminId == i.AdminId)
                    .Select(a => a.FName + " " + a.LName)
                    .FirstOrDefault() ?? "N/A", // Fetch Admin name
                Name = dbContext.SystemUsers
                    .Where(s => s.UserId == i.UserId)
                    .Select(s => s.FName + " " + s.LName)
                    .FirstOrDefault() ?? "Unknown", // Fetch User name
                Email = i.User.Email ?? "",
                Phone = i.User.PhoneNumber ?? "",
                PromoCode = i.PromoCode
            })
            .ToListAsync();

        return (totalCount, paginatedInvoices);
    }
    
    
    
}