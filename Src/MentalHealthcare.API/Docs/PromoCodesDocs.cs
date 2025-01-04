namespace MentalHealthcare.API.Docs;


public class PromoCodesDocs
{
  public const string CreateDescription = @"
### Create a New General Promo Code
To create a new general promo code, provide the necessary details for the promo code in the request body.

#### Fields:
- **`Code`** (string): The unique code for the promo code. 
  - This should be a non-empty string.
  - The code must be unique; duplicate codes will result in an error.

- **`ExpireDate`** (string): The expiration date for the promo code in ISO 8601 format (e.g., ""2024-12-30T14:40:00Z"").
  - The expiration date must be a valid datetime in the UTC format.
  - Example: ""expireDate"": ""2024-12-30T14:40:00Z""

- **`Percentage`** (float): The discount percentage applied by the promo code.
  - The value should be a non-negative number (e.g., `0` to `100`).
  - The percentage must be below 100; providing a percentage of 100 or higher will result in an error.

- **`IsActive`** (bool): Indicates whether the promo code is active.
  - The value should be `true` if the promo code is active, or `false` if it is inactive.
  - If this field is not provided, it will be treated as `true` by default.

#### Example Usage:
To create a new promo code with the code `test1`, an expiration date of ""2024-12-30T14:40:00Z"", a discount of `20%`, and set it as active:
```json
{
  ""code"": ""test1"",
  ""expireDate"": ""2024-12-30T14:40:00Z"",
  ""percentage"": 20,
  ""isActive"": true
}";
  
  public const string GetGeneralPromoCodesDescription = @"
Retrieves a paginated list of general promo codes.

Parameters:
- PageNumber (Optional): The current page number. Defaults to 1 if not provided.
- PageSize (Optional): The number of promo codes to display per page. Defaults to 10 if not provided.
- SearchText (Optional): A case-insensitive filter to search promo codes by their 'Code' field. If omitted, all promo codes are considered.
- IsActive (Optional): Filter based on promo code activation status:
  - 0: Expired promo codes only.
  - 1: Active promo codes only.
  - Any other value: Include both expired and active promo codes.

Example Usage:
1. Get all general promo codes with default pagination:
   GET /promoCodes
2. Search general promo codes by a keyword:
   GET /promoCodes?SearchText=discount
3. Get only active promo codes, with custom pagination:
   GET /promoCodes?PageNumber=2&PageSize=5&IsActive=1
4. Retrieve all expired promo codes:
   GET /promoCodes?IsActive=0

Response Format:
The API returns a PageResult<GeneralPromoCodeDto> object containing:
1. TotalCount: The total number of promo codes matching the criteria.
2. Items: A collection of promo codes with the following fields:
   - GeneralPromoCodeId: The unique identifier for the promo code.
   - Code: The promo code string.
   - ExpireDate: The expiration date of the promo code.
   - Percentage: The discount percentage.
   - ExpiresInDays: The number of days remaining until the promo code expires (or 0 if already expired).
   - IsActive: The activation status of the promo code.

Validation Notes:
- Invalid or missing query parameters will revert to default values.
- Page numbers exceeding the total count of items will return an empty Items list.

Edge Cases:
- If no promo codes match the criteria, the Items list will be empty, but TotalCount will reflect the total matching count.
- To avoid unexpected results, ensure PageSize and PageNumber are reasonable values.
";
  public const string UpdateCoursePromoCodeDescription = """
                                                         ### Update a Course Promo Code
                                                         Allows an authorized admin to update an existing promo code for a specific course.

                                                         #### Authorization
                                                         - **Requires**: A valid Bearer token with admin privileges.

                                                         #### Endpoint
                                                         - **PUT** `/api/course-promo-code/{courseId}/promocode/{promoCodeId}`

                                                         #### Parameters
                                                         - **Path Parameters**:
                                                           - `courseId` (int): The ID of the course associated with the promo code.
                                                           - `promoCodeId` (int): The ID of the promo code to update.
                                                         - **Body**:
                                                           - `Percentage` (float, optional): The new discount percentage (e.g., 25.5). Send `null` if no update is required.
                                                           - `ExpireDate` (string, optional): The new expiration date in `YYYY-MM-DD` format. Must be a future date. Send `null` if no update is required.
                                                           - `IsActive` (bool, optional): Indicates whether the promo code should be active. Send `null` if no update is required.

                                                         #### Behavior
                                                         - If the promo code is not found, the endpoint will return a `404 Not Found` error.
                                                         - If `ExpireDate` is provided, it must be a valid future date.
                                                         - If the input data is invalid, a `400 Bad Request` response will be returned.

                                                         #### Response
                                                         - **204 No Content**: Promo code updated successfully.
                                                         - **400 Bad Request**: Invalid input data or `ExpireDate` is not a future date.
                                                         - **401 Unauthorized**: If the user is not authenticated.
                                                         - **403 Forbidden**: If the user does not have admin privileges.
                                                         - **404 Not Found**: If the promo code or course does not exist.

                                                         #### Example Request
                                                         ```http
                                                         PUT /api/course-promo-code/123/promocode/456 HTTP/1.1
                                                         Authorization: Bearer <token>
                                                         Content-Type: application/json

                                                         {
                                                           "Percentage": 25.5,
                                                           "ExpireDate": "2025-06-01",
                                                           "IsActive": null
                                                         }
                                                         ```
                                                         """;

}