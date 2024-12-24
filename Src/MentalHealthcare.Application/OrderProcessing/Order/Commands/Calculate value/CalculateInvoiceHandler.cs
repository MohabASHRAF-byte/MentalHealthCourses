using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Dtos.OrderProcessing;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.OrderProcessing.Order.Commands.Calculate_value;

public class CalculateInvoiceHandler(
    ILogger<CalculateInvoiceHandler> logger,
    IMapper mapper
) : IRequestHandler<CalculateInvoice, CalculateInvoiceResponse>
{
    public async Task<CalculateInvoiceResponse> Handle(CalculateInvoice request, CancellationToken cancellationToken)
    {
        // Check for duplicate course IDs
        if (request.Courses.GroupBy(course => course.CourseId).Any(g => g.Count() > 1))
        {
            logger.LogWarning("Duplicate course IDs found in the request.");
            throw new ArgumentException("All provided course IDs must be unique.");
        }

        // Map InvoiceCourse to CourseCartDto
        var coursesDto = request.Courses.Select(course => mapper.Map<CourseCartDto>(course)).ToList();

        // Use the PriceCalculator to calculate totals
        var calculatedCart = PriceCalculator.Calculate(coursesDto, request.DiscountPercentage, request.TaxPercentage);

        // Map CartDto to CalculateInvoiceResponse
        var response = mapper.Map<CalculateInvoiceResponse>(calculatedCart);

        // Populate Result field in the response
        response.Result = calculatedCart.Courses
            .Select(course => mapper.Map<InvoiceCourse>(course))
            .ToList();

        logger.LogInformation("Invoice calculated with {CourseCount} courses, Total: {TotalPrice}",
            response.Result.Count, response.TotalPrice);

        return response;
    }
}