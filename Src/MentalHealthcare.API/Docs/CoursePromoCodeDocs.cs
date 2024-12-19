namespace MentalHealthcare.API.Docs;

public static class CoursePromoCodeDocs
{
public const string GetPromoCodesWithCourseIdDescription = @"
Retrieves a paginated list of promo codes associated with a specific course.

Parameters:
- courseId (Required): The unique identifier of the course for which promo codes are being retrieved.
- PageNumber (Optional): The current page number. Defaults to 1 if not provided.
- PageSize (Optional): The number of promo codes to display per page. Defaults to 10 if not provided.
- SearchText (Optional): A case-insensitive filter to search promo codes by their 'Code' field. If omitted, all promo codes are considered.
- IsActive (Optional): Filter based on promo code expiration status:
  - 0: Expired promo codes only.
  - 1: Active promo codes only.
  - Any other value: Include both expired and active promo codes.

Example Usage:
1. Get all promo codes for a specific course with default pagination:
   GET /course/{courseId}
2. Search promo codes for a course by a keyword:
   GET /course/{courseId}?SearchText=discount
3. Get only active promo codes for a course, with custom pagination:
   GET /course/{courseId}?PageNumber=2&PageSize=5&IsActive=1
4. Retrieve all expired promo codes for a course:
   GET /course/{courseId}?IsActive=0

Response Format:
The API returns a PageResult<CoursePromoCodeDto> object containing:
1. TotalCount: The total number of promo codes matching the criteria.
2. Items: A collection of promo codes with the following fields:
   - CoursePromoCodeId: The unique identifier for the promo code.
   - Code: The promo code string.
   - CourseName: The name of the associated course.
   - CourseId: The ID of the associated course.
   - expiredate: The expiration date of the promo code.
   - percentage: The discount percentage.
   - expiresInDays: The number of days remaining until the promo code expires (or 0 if already expired).

Validation Notes:
- Ensure the provided courseId exists.
- Invalid or missing query parameters will revert to default values.
- Page numbers exceeding the total count of items will return an empty Items list.

Edge Cases:
- If no promo codes match the criteria, the Items list will be empty, but TotalCount will reflect the total matching count.
- To avoid unexpected results, ensure PageSize and PageNumber are reasonable values.
";

}