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
                                                    - Name (Required): The name of the course.
                                                    - InstructorId (Required): The ID of the instructor associated with the course.
                                                    - Price (Required): The price of the course. If the course is free, set the price to 0.
                                                    - Description (Required): A brief description of the course (max 800 characters).
                                                    - Categories (Required): List of IDs for the categories the course belongs to. Send an empty list if none.

                                                  #### Behavior:
                                                  - After creating the course, it will be moved to the archived courses section. This allows administrators to freely add items and manipulate the course data before it is published.

                                                  #### Response:
                                                  - **201 Created**:
                                                    - Returns the URI of the created course and its ID in the Location header.
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
                                                       - `IconUrl`: URL of the course icon.
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
                                                   - The `IsOwned` field has relevance only if the user is not an admin.
                                                   - The `Price` field will retain its value unless explicitly set to `0`, and if the course is free with a non-zero price, it indicates the previous price.
                                                   """;

    public const string UpdateCourseDescription = """
                                                  ### Update Course

                                                  This endpoint allows administrators to update an existing course's details.

                                                  #### Request:
                                                  - **Authentication**: Requires a Bearer token.
                                                  - **Path Parameter**:
                                                    - `CourseId` (Required): The ID of the course to update.
                                                  - **Body Parameters**:
                                                    - `Name` (Optional): The new name of the course. If not updating, send as `null`.
                                                    - `InstructorId` (Optional): The ID of the instructor associated with the course. If not updating, send as `null`.
                                                    - `Price` (Optional): The new price of the course. If not updating, send as `null`.
                                                    - `Description` (Optional): A brief description of the course (max 800 characters). If not updating, send as `null`.
                                                    - `Categories` (Optional): A list of category IDs the course belongs to. 
                                                      - If an empty list is passed, the course will have no categories.
                                                      - If not updating, send as `null`.
                                                    - `IsFree` (Optional): Whether the course is free. If not updating, send as `null`.
                                                      > ⚠️ **Caution**: Setting `IsFree` to `true` will make the course free regardless of the price specified. Ensure this is intended.
                                                    - `IsFeatured` (Optional): Whether the course is featured. If not updating, send as `null`.
                                                    - `IsArchived` (Optional): Whether the course is archived. If not updating, send as `null`.

                                                  #### Response:
                                                  - **204 No Content**:
                                                    - The course was successfully updated.
                                                  - **401 Unauthorized**:
                                                    - If the user is not authenticated.
                                                  - **403 Forbidden**:
                                                    - If the user does not have permission to update the course.
                                                  - **400 Bad Request**:
                                                    - If required fields are missing or invalid.

                                                  #### Edge Cases:
                                                  - Categories:
                                                    - Passing an empty list means the course will have no categories.
                                                    - Passing `null` means categories will not be updated.

                                                  #### Example Usage:
                                                  To update a course's name, price, and categories:
                                                  ```http
                                                  PUT /api/courses/{courseId} HTTP/1.1
                                                  Authorization: Bearer <token>
                                                  Content-Type: application/json

                                                  {
                                                    "name": "Updated Course Name",
                                                    "price": 49.99,
                                                    "categories": [1, 2],
                                                    "isFree": true
                                                  }
                                                  ```
                                                  """;

    public const string GetCourseByIdDescription = """
                                                   ### Get Course By ID

                                                   This endpoint retrieves the details of a specific course by its ID.

                                                   #### Access:
                                                   - **Authentication**: Requires a Bearer token.
                                                   - **Roles**: Accessible to Admins and Users.

                                                   #### Behavior:
                                                   - If the course is archived, it will not be accessible to regular users (403 Forbidden).
                                                   - Retrieves detailed information about the course, including:
                                                     - Instructor details
                                                     - Course sections, lessons, and associated resources
                                                     - Categories
                                                     - Ratings and reviews

                                                   #### Request:
                                                   - **Path Parameter**:
                                                     - `courseId` (Required): The unique identifier of the course.

                                                   #### Response:
                                                   - **200 OK**:
                                                     - Returns the course details as a `CourseDto` object.
                                                   - **401 Unauthorized**:
                                                     - If the user is not authenticated.
                                                   - **403 Forbidden**:
                                                     - If the course is archived and the user is not an Admin.
                                                   - **404 Not Found**:
                                                     - If the course does not exist.

                                                   #### Notes:
                                                   - The `IsFavourite` and `IsOwned` fields are included only for Users and are set to `null` for Admins.
                                                   - Reviews are paginated, returning the top 5 reviews with a maximum length of 50 characters each.
                                                   """;
    
    public const string AddOrUpdateThumbnailDescription = """
                                              ### Add or Update Course Thumbnail

                                              This endpoint allows administrators to add or update the thumbnail for a specific course.

                                              #### Request:
                                              - **Authentication**: Requires a Bearer token.
                                              - **Path Parameter**:
                                                - `CourseId` (Required): The ID of the course for which the thumbnail is being added or updated.
                                              - **Body Parameters**:
                                                - `File` (Required): The thumbnail file to upload.
                                                  - The file size must not exceed **1 MB**.

                                              #### Response:
                                              - **200 OK**:
                                                - Returns a success message with the URL of the uploaded thumbnail.
                                              - **401 Unauthorized**:
                                                - If the user is not authenticated.
                                              - **403 Forbidden**:
                                                - If the user does not have permission to update the course thumbnail.
                                              - **400 Bad Request**:
                                                - If the file size exceeds the allowed limit or the file format is invalid.
                                              - **404 Not Found**:
                                                - If the course does not exist.

                                              #### Example Usage:
                                              ```http
                                              POST /api/courses/{courseId}/thumbnail HTTP/1.1
                                              Authorization: Bearer <token>
                                              Content-Type: multipart/form-data

                                              --boundary
                                              Content-Disposition: form-data; name="file"; filename="thumbnail.jpg"
                                              Content-Type: image/jpeg

                                              [binary data]
                                              --boundary--
                                              ```
                                              """;
    
    public const string AddCourseIconDescription = """
                                              ### Add Course Icon

                                              This endpoint allows administrators to add an icon for a specific course.

                                              #### Request:
                                              - **Authentication**: Requires a Bearer token.
                                              - **Path Parameter**:
                                                - `CourseId` (Required): The ID of the course for which the icon is being added.
                                              - **Body Parameters**:
                                                - `File` (Required): The icon file to upload.
                                                  - The file size must not exceed **100 KB**.

                                              #### Response:
                                              - **204 No Content**:
                                                - The icon was successfully added.
                                              - **401 Unauthorized**:
                                                - If the user is not authenticated.
                                              - **403 Forbidden**:
                                                - If the user does not have permission to add the course icon.
                                              - **400 Bad Request**:
                                                - If the file size exceeds the allowed limit or the file format is invalid.
                                              - **404 Not Found**:
                                                - If the course does not exist.

                                              #### Example Usage:
                                              ```http
                                              POST /api/courses/{courseId}/icon HTTP/1.1
                                              Authorization: Bearer <token>
                                              Content-Type: multipart/form-data

                                              --boundary
                                              Content-Disposition: form-data; name="file"; filename="icon.jpg"
                                              Content-Type: image/jpeg

                                              [binary data]
                                              --boundary--
                                              ```
                                              """;

}