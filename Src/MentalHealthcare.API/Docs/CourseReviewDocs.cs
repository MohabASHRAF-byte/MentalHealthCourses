namespace MentalHealthcare.API.Docs;

public class CourseReviewDocs
{
    public const string UpdateCourseReviewDescription = @"
### Update Course Review
Updates an existing review for a specific course. This endpoint allows users to update the content and/or rating of a review.

#### Request:
- **Route Parameters**:
  - **courseId** (int): The ID of the course associated with the review.
  - **reviewId** (int): The ID of the review to update.

- **Request Body**:
  - **Content** (string, Optional): The updated review content. If not provided, the content remains unchanged.
  - **Rating** (float, Optional): The updated review rating. If not provided, the rating remains unchanged.

#### Response:
- **204 No Content**: Indicates the review was successfully updated.
- **400 Bad Request**: If the request parameters are invalid.
- **401 Unauthorized**: If the user is not authenticated.
- **403 Forbidden**: If the user does not have permission to update the review.
- **404 Not Found**: If the review or course does not exist.

#### Notes:
- This endpoint requires authentication with a valid Bearer token (Only User).
- Partial updates are allowed; you can update either content, rating, or both.

#### Example:
```http
PUT /courses/123/review/456
Authorization: Bearer <token>
Content-Type: application/json

{
    ""Content"": null,
    ""Rating"": 4.5
}
```
";

    public const string DeleteCourseReviewDescription = @"
### Delete Course Review
Deletes an existing review for a specific course. This endpoint allows users to delete their own reviews or admins to delete any review.

#### Request:
- **Route Parameters**:
  - **courseId** (int): The ID of the course associated with the review.
  - **reviewId** (int): The ID of the review to delete.

#### Response:
- **204 No Content**: Indicates the review was successfully deleted.
- **400 Bad Request**: If the request parameters are invalid.
- **401 Unauthorized**: If the user is not authenticated.
- **403 Forbidden**: If the user does not have permission to delete the review.
- **404 Not Found**: If the review or course does not exist.

#### Notes:
- This endpoint requires authentication with a valid Bearer token.
- Users can delete only their own reviews.
- Admins can delete any review.

#### Example:
```http
DELETE /courses/123/review/456
Authorization: Bearer <token>
```
";
    public const string GetCourseReviewDescription = @"
### Get Course Review
Retrieves a specific review for a course based on the review ID and course ID.

#### Request:
- **Route Parameters**:
  - **courseId** (int): The ID of the course associated with the review.
  - **reviewId** (int): The ID of the review to retrieve.

#### Response:
- **200 OK**: Returns the review details in the `UserReviewDto` format.
- **400 Bad Request**: If the request parameters are invalid.
- **401 Unauthorized**: If the user is not authenticated.
- **403 Forbidden**: If the user does not have permission to retrieve the review.
- **404 Not Found**: If the review or course does not exist.

#### Notes:
- This endpoint requires authentication with a valid Bearer token.
- Users can only access reviews for courses they have permissions to view.

#### Example:
```http
GET /courses/123/reviews/456
Authorization: Bearer <token>
```
";
}
