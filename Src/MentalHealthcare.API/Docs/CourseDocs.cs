namespace MentalHealthcare.API.Docs;

public static class CourseDocs
{
    public const string UpdateSectionOrderDescription = @"
### Update Section Order

This endpoint updates the order of sections for a specified course. You need to provide the course ID in the path, along with a list of sections and their new order.

**Rules:**

* **Course ID:**
    - Must be provided in the URL path.
    - Corresponds to an existing course in the system.
* **Section Ordering:**
    - The `orders` array in the body must contain each section's `sectionId` and its new `order`.
    - Order values must be:
        - Unique (no duplicates)
        - Sequential, starting from 1 and increasing by 1 for each section (no gaps)
    - Each `sectionId` provided must correspond to an existing section in the course. Any non-existent IDs will result in a 404 Not Found error.
* **Request Status Codes:**
    - 204 No Content: Successful update of section orders.
    - 400 Bad Request:
        - Order values are not unique or sequential.
        - Missing required fields (e.g., `courseId` in path or sections in body).
        - Invalid data format in the request body.
    - 404 Not Found:
        - The provided `courseId` does not match an existing course.
        - An `orders` array element contains a `sectionId` that doesn't exist in the course.

**Request Details:**

* **Path Parameter:**
    - `courseId`: The ID of the course whose sections are being reordered.
* **Body:**
    - A list of section objects with `sectionId` and `order` values:
        ```json
        {
          ""orders"": [
            { ""sectionId"": 22, ""order"": 1 },
            { ""sectionId"": 21, ""order"": 2 },
            // ... more section objects
          ]
        }
        ```

**Example Request:**

```http
PATCH /api/courses/{courseId}/sections/order HTTP/1.1
Host: example.com
Content-Type: application/json

{
  ""orders"": [
    { ""sectionId"": 22, ""order"": 1 },
    { ""sectionId"": 21, ""order"": 2 }
  ]
}
";

    public const string CreateCourseDescription = """
                                                  ### Create Course

                                                  This endpoint allows an authorized user to create a new course.

                                                  #### Request:
                                                  - **Authentication**: Requires a Bearer token.
                                                  - **Body Parameters**:
                                                    - `Name` (Required): The name of the course.
                                                    - `InstructorId` (Required): The ID of the instructor associated with the course.
                                                    - `Price` (Required): The price of the course. If the course is free, set the price to `0`.
                                                    - `Description` (Required): A brief description of the course (max 800 characters).
                                                    - `Categories` (Required): list of ids for the categories course belong to send empty if none.

                                                  #### Response:
                                                  - **201 Created**:
                                                    - Returns the URI of the created course and its ID in the `Location` header.
                                                  - **401 Unauthorized**:
                                                    - If the user is not authenticated.
                                                  - **403 Forbidden**:
                                                    - If the user does not have permission to create a course.
                                                  - **400 Bad Request**:
                                                    - If required fields are missing or invalid.

                                                  #### Example Usage:
                                                  To create a new course:
                                                  ```json
                                                  POST /api/courses
                                                  Authorization: Bearer <token>
                                                  Content-Type: application/json

                                                  {
                                                    "name": "Introduction to Mental Health",
                                                    "instructorId": 12,
                                                    "price": 0,
                                                    "description": "A comprehensive course on mental health basics.",
                                                    "categoryId": [1,2]
                                                  }
                                                  ```
                                                  """;

    public const string GetAllCoursesDescription = """
                                                   ### Get All Courses

                                                   This endpoint retrieves a paginated list of courses based on the provided search criteria and pagination details.

                                                   #### Request:
                                                   - **Authentication**: Requires a Bearer token.
                                                   - **Query Parameters**:
                                                     - `SearchText` (Optional): A string to filter courses by name.
                                                     - `PageNumber` (Required): The page number to retrieve.
                                                     - `PageSize` (Required): The number of courses per page.

                                                   #### Response:
                                                   - **200 OK**:
                                                     - Returns a paginated list of courses with the following details:
                                                       - `Name`: The name of the course.
                                                       - `ThumbnailUrl`: URL of the course thumbnail.
                                                       - `Price`: The price of the course. If the course is free and a non-zero price is provided, it represents the previous price.
                                                       - `Rating`: The average rating of the course.
                                                       - `EnrollmentsCount`: The number of users enrolled in the course.
                                                       - `ReviewsCount`: The number of reviews for the course.
                                                       - `IsOwned`: Indicates if the course is owned by the user. This field is meaningful only if the user is not an admin.
                                                       - `IsFree`: Indicates if the course is now free or not. This field is meaningful only if the user is not an admin.

                                                   #### Edge Cases:
                                                   - If the user is not authenticated, an error is returned.
                                                   - If the query parameters are missing or invalid, a `400 Bad Request` is returned.

                                                   #### Example Usage:
                                                   To retrieve the first page of courses with 10 items per page:
                                                   ```http
                                                   GET /api/courses?SearchText=mental&PageNumber=1&PageSize=10
                                                   Authorization: Bearer <token>
                                                   ```

                                                   #### Notes:
                                                   - The  `IsOwned` field has relevance only if the user is not an admin.
                                                   - The `Price` field will retain its value unless explicitly set to `0`, and if the course is free with a non-zero price, it indicates the previous price.
                                                   """;
}