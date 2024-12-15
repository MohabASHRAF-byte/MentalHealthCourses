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
}