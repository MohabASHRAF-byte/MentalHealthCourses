using MentalHealthcare.Application.OrderProcessing;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Dtos.OrderProcessing;
using MentalHealthcare.Domain.Entities.Courses;
using MentalHealthcare.Domain.Entities.OrderProcessing;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.OrderProcessing;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories.OrderProcessing;

public class InvoiceRepository(
    MentalHealthDbContext dbContext,
    ILocalizationService localizationService
) : IInvoiceRepository
{
    public async Task AddAsync(Invoice invoice)
    {
        dbContext.Add(invoice);
        await dbContext.SaveChangesAsync();
    }

    public async Task<InvoiceDto> GetInvoiceByIdAsync(int id, int? userId = null)
    {
        if (id <= 0)
        {
            throw new BadHttpRequestException(
                localizationService.GetMessage("InvoiceIdMustBeGreaterThanZero")
            );
        }

        // Query invoice and project directly into InvoiceDto
        var invoiceDto = await dbContext.Invoices
            .Where(i => i.InvoiceId == id)
            .Where(i => (userId != null && i.SystemUserId == userId) || userId == null)
            .Include(i => i.SystemUser)
            .Select(i => new InvoiceDto
            {
                InvoiceId = i.InvoiceId,
                UserId = i.SystemUserId,
                FirstName = i.SystemUser.FName,
                LastName = i.SystemUser.LName,
                PhoneNumber = dbContext.Users
                    .Where(u => u.Id == i.SystemUser.UserId)
                    .Select(u => u.PhoneNumber)
                    .FirstOrDefault(),
                Email = dbContext.Users
                    .Where(u => u.Id == i.SystemUser.UserId)
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
            throw new ResourceNotFound(
                "Invoice",
                "فاتورة",
                id.ToString()
            );
        }

        return invoiceDto;
    }

    public async Task<(int, List<InvoiceViewDto>)> GetInvoicesAsync(int pageNumber, int pageSize,
        string? invoiceId,
        OrderStatus? status, string? email,
        string? phoneNumber, DateTime? fromDate, DateTime? toDate,
        string? promoCode,
        int? systemUserId = null
    )
    {
        // Start with all invoices
        var invoices = dbContext.Invoices.AsNoTracking();

        // Apply filters in order of expected elimination effectiveness
        if (systemUserId != null)
        {
            invoices = invoices.Where(i => i.SystemUserId == systemUserId);
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
            .Include(p => p.SystemUser)
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
                Name = i.SystemUser.FName,
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
        int adminId
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
                throw new BadHttpRequestException(
                    localizationService.GetMessage("InvoiceNotFound")
                );
            }

            if (invoice.OrderStatus != OrderStatus.Pending)
            {
                throw new BadHttpRequestException(
                    localizationService.GetMessage("InvoiceNotPending")
                );
            }

            // Check and update courses
            var courseIdsInInvoice = invoice.Courses.Select(c => c.CourseId).ToList();
            var passedCourseIds = courses.Select(c => c.CourseId).ToList();

            // Validate that all passed course IDs exist in the invoice
            var invalidCourseIds = passedCourseIds.Except(courseIdsInInvoice).ToList();
            if (invalidCourseIds.Any())
            {
                throw new BadHttpRequestException(
                    string.Format(
                        localizationService.GetMessage("InvalidCourseIdsProvided"),
                        string.Join(", ", invalidCourseIds)
                    )
                );
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
            invoice.AdminId = adminId;
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

                var existingCourseProgress = await dbContext.CourseProgresses
                    .Where(p => p.CourseId == course.CourseId &&
                                p.SystemUserId == invoice.SystemUserId
                    ).FirstOrDefaultAsync();
                if (existingCourseProgress != null)
                {
                    throw new BadHttpRequestException(
                        string.Format(
                            localizationService.GetMessage("UserAlreadyHasCourse"),
                            course.CourseId
                        )
                    );
                }

                var courseProgress = new CourseProgress
                {
                    CourseId = course.CourseId,
                    SystemUserId = invoice.SystemUserId,
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
            throw new BadHttpRequestException(
                localizationService.GetMessage("InvoiceNotFound")
            );
        }

        // Check if the provided status is a valid enum value
        if (!Enum.IsDefined(typeof(OrderStatus), status))
        {
            throw new BadHttpRequestException(
                localizationService.GetMessage("InvalidOrderStatus")
            );
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
                    throw new BadHttpRequestException(
                        localizationService.GetMessage("CanOnlyChangeToExpired", "Can only change to 'Expired' if the current status is 'Pending'.")
                    );
                }

                break;

            case OrderStatus.Pending:
                if (invoice.OrderStatus == OrderStatus.Expired || invoice.OrderStatus == OrderStatus.Cancelled)
                {
                    invoice.OrderStatus = OrderStatus.Pending;
                }
                else
                {
                    throw new BadHttpRequestException(
                        localizationService.GetMessage(
                            "CanOnlyChangeToPending",
                            "Can only change to 'Pending' if the current status is 'Expired' or 'Cancelled'."
                        )
                    );
                }

                break;

            case OrderStatus.Cancelled:
                if (invoice.OrderStatus == OrderStatus.Pending)
                {
                    invoice.OrderStatus = OrderStatus.Cancelled;
                }
                else
                {
                    throw new BadHttpRequestException(
                        localizationService.GetMessage(
                            "CanOnlyChangeToCancelled",
                            "Can only change to 'Cancelled' if the current status is 'Pending'."
                        )
                    );
                }

                break;

            default:
                throw new BadHttpRequestException(
                    string.Format(
                        localizationService.GetMessage("StatusChangeNotAllowed", "Changing to status '{0}' is not allowed."),
                        status
                    )
                );
        }

        await dbContext.SaveChangesAsync();
    }
}