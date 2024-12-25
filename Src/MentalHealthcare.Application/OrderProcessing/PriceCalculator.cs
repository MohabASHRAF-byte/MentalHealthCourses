using MentalHealthcare.Domain.Dtos.OrderProcessing;

namespace MentalHealthcare.Application.OrderProcessing;

public static class PriceCalculator
{
    public static CartDto Calculate(IEnumerable<CourseCartDto> courses, decimal discountPercentage,
        decimal taxesPercentage)
    {
        // Initialize a new CartDto
        var cart = new CartDto();

        // Calculate SubTotalPrice
        courses = courses.ToList();
        cart.SubTotalPrice = courses.Sum(course => course.Price);

        // Calculate DiscountValue (rounded up to the nearest 0.5)
        cart.DiscountValue = Math.Ceiling((cart.SubTotalPrice * (discountPercentage / 100)) * 2) / 2;
        cart.DiscountPercentage = discountPercentage;

        // Calculate TaxesValue (rounded up to the nearest 0.5)
        cart.TaxesValue = Math.Ceiling(((cart.SubTotalPrice - cart.DiscountValue) * (taxesPercentage / 100)) * 2) / 2;
        cart.TaxesPercentage = taxesPercentage;

        // Calculate TotalPrice (rounded up to the nearest 0.5)
        cart.TotalPrice = Math.Ceiling((cart.SubTotalPrice - cart.DiscountValue + cart.TaxesValue) * 2) / 2;

        // Populate the CartDto with Courses and NumberOfItems
        cart.Courses = courses;
        cart.NumberOfItems = courses.Count();

        return cart;
    }
}