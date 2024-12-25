using MentalHealthcare.Application.OrderProcessing;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Dtos.OrderProcessing;
using MentalHealthcare.Domain.Entities.Courses;
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
            var normalizedInvoiceId = invoiceId.ToLower();
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

    public async Task AcceptInvoice(
        int invoiceId,
        List<MiniCourse> courses,
        decimal discount,
        string adminId
    )
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            // Fetch the invoice
            var invoice = await dbContext.Invoices
                .Include(i => i.Courses)
                .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);

            if (invoice == null)
            {
                throw new ArgumentException("Invoice not found.", nameof(invoiceId));
            }

            if (invoice.OrderStatus != OrderStatus.Pending)
            {
                throw new ArgumentException($"Invoice is not in Pending State", nameof(invoiceId));
            }

            // Check and update courses
            var courseIdsInInvoice = invoice.Courses.Select(c => c.CourseId).ToList();
            var passedCourseIds = courses.Select(c => c.CourseId).ToList();

            // Validate that all passed course IDs exist in the invoice
            var invalidCourseIds = passedCourseIds.Except(courseIdsInInvoice).ToList();
            if (invalidCourseIds.Any())
            {
                throw new ArgumentException($"Invalid course IDs provided: {string.Join(", ", invalidCourseIds)}");
            }

            // Remove courses not in the passed list
            var coursesToRemove = invoice.Courses
                .Where(c => !passedCourseIds.Contains(c.CourseId)).ToList();
            dbContext.RemoveRange(coursesToRemove);

            // Update invoice discount and promo code if necessary
            if (invoice.DiscountPercentage != discount)
            {
                invoice.PromoCode = "Provided By Admin";
                invoice.DiscountPercentage = discount;

                // Recalculate prices using the PriceCalculator
                var courseCartDtos = invoice.Courses.Select(c => new CourseCartDto
                    { CourseId = c.CourseId, Price = c.Price });
                var recalculatedCart = PriceCalculator.Calculate(courseCartDtos, discount, invoice.TaxesPercentage);

                invoice.SubTotalPrice = recalculatedCart.SubTotalPrice;
                invoice.DiscountValue = recalculatedCart.DiscountValue;
                invoice.TaxesValue = recalculatedCart.TaxesValue;
                invoice.TotalPrice = recalculatedCart.TotalPrice;
            }

            // Assign admin and update status
            invoice.Admin = await dbContext.Admins
                .Where(a => a.UserId == adminId)
                .FirstOrDefaultAsync();
            invoice.OrderStatus = OrderStatus.Done;
            invoice.ProcessedDate = DateTime.UtcNow;

            // Add course progress records
            foreach (var course in courses)
            {
                // check if the course is exist and increase the enrollment count 
                var courseEntity = await dbContext.Courses
                    .FirstOrDefaultAsync(c => c.CourseId == course.CourseId);
                if (courseEntity != null)
                {
                    courseEntity.EnrollmentsCount += 1;
                }

                // Check if user already has the course
                var existingCourseProgress = await dbContext
                    .CourseProgresses
                    .FirstOrDefaultAsync(
                        cp => cp.CourseId == course.CourseId &&
                              cp.UserId == invoice.UserId
                    );
                if (existingCourseProgress != null)
                {
                    throw new ArgumentException($"User already has course with ID: {course.CourseId}");
                }

                var courseProgress = new CourseProgress
                {
                    CourseId = course.CourseId,
                    UserId = invoice.UserId,
                    LastLessonIdx = 0,
                    LastChange = DateTime.UtcNow
                };

                dbContext.CourseProgresses.Add(courseProgress);
            }

            // Save changes
            await dbContext.SaveChangesAsync();

            // Commit transaction
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task ChangeInvoiceState(int invoiceId, OrderStatus status)
    {
        var invoice = await dbContext.Invoices.FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);

        if (invoice == null)
        {
            throw new ArgumentException("Invoice not found.", nameof(invoiceId));
        }

        // Check if the provided status is a valid enum value
        if (!Enum.IsDefined(typeof(OrderStatus), status))
        {
            throw new ArgumentException("Invalid order status provided.", nameof(status));
        }

        // Handle state transitions
        switch (status)
        {
            case OrderStatus.Expired:
                if (invoice.OrderStatus == OrderStatus.Pending)
                {
                    invoice.OrderStatus = OrderStatus.Expired;
                }
                else
                {
                    throw new InvalidOperationException(
                        "Can only change to 'Expired' if the current status is 'Pending'.");
                }

                break;

            case OrderStatus.Pending:
                if (invoice.OrderStatus == OrderStatus.Expired || invoice.OrderStatus == OrderStatus.Cancelled)
                {
                    invoice.OrderStatus = OrderStatus.Pending;
                }
                else
                {
                    throw new InvalidOperationException(
                        "Can only change to 'Pending' if the current status is 'Expired' or 'Cancelled'.");
                }

                break;

            case OrderStatus.Cancelled:
                if (invoice.OrderStatus == OrderStatus.Pending)
                {
                    invoice.OrderStatus = OrderStatus.Cancelled;
                }
                else
                {
                    throw new InvalidOperationException(
                        "Can only change to 'Cancelled' if the current status is 'Pending'.");
                }

                break;

            default:
                throw new InvalidOperationException($"Changing to status '{status}' is not allowed.");
        }

        await dbContext.SaveChangesAsync();
    }
}