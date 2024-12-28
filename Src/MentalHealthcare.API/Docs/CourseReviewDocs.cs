namespace MentalHealthcare.API.Docs;

public static class CourseReviewDocs
{
    public const string PostCourseReviewDescription = """
                                                      ### Post Course Review
                                                      Allows an authenticated user to add a review for a specific course.

                                                      #### Usage
                                                      - **Endpoint**: `POST /courses/{courseId}/reviews`
                                                      - **Authorization**: Requires a Bearer token.
                                                      - **Parameters**:
                                                        - `courseId` (int): The unique identifier of the course.

                                                      #### Request Body
                                                      - `Rating` (float): The rating given by the user, rounded to one decimal place.
                                                      - `Content` (string): The textual content of the review.

                                                      #### Behavior
                                                      - The user must be authenticated and have the `User` role.
                                                      - The course must exist.
                                                      - The user must have completed a certain percentage of the course .
                                                      - Users can add reviews only up to a predefined limit .
                                                      - Updates the course rating and review count atomically.

                                                      #### Response
                                                      - **200 OK**: Returns the unique identifier of the added review (`UserReviewId`).
                                                      - **400 Bad Request**: If the course does not exist, the review limit is reached, or the user has not completed enough of the course.
                                                      - **401 Unauthorized**: If the user is not authenticated.
                                                      - **403 Forbidden**: If the user does not have the required permissions.
                                                      - **500 Internal Server Error**: If an unexpected error occurs during the process.

                                                      #### Example
                                                      ```http
                                                      POST /courses/123/reviews
                                                      Authorization: Bearer <token>
                                                      Content-Type: application/json

                                                      {
                                                          "Rating": 4.5,
                                                          "Content": "Great course! Very informative."
                                                      }
                                                      ```
                                                      """;

    public const string GetCourseReviewsDescription = """
                                                      ### Get Course Reviews
                                                      Retrieves all reviews for a specific course with pagination and content trimming.

                                                      #### Usage
                                                      - **Endpoint**: `GET /courses/{courseId}/reviews`
                                                      - **Authorization**: Requires a Bearer token.
                                                      - **Parameters**:
                                                        - `courseId` (int): The unique identifier of the course.
                                                        - Query Parameters:
                                                          - `PageNumber` (int, optional): The page number for pagination. Default is 1.
                                                          - `PageSize` (int, optional): The number of reviews per page. Default is 10.
                                                          - `ContentLimit` (int, optional): The maximum number of characters for each review's content. Default is 100.

                                                      #### Behavior
                                                      - Reviews are paginated and sorted by the most recent.
                                                      - If a review's content exceeds `ContentLimit`, it will be trimmed and appended with `...`.
                                                      - The `IsFullContent` field indicates whether the content is fully displayed or trimmed.
                                                      - The `SecondsSinceCreated` field provides the age of the review in seconds since it was created.
                                                      - The `SecondsSinceLastEdited` field provides the age of the review in seconds since it was last edited.

                                                      #### Response
                                                      - **200 OK**: Returns a paginated list of reviews in a `PageResult<UserReviewDto>` format.
                                                      - **400 Bad Request**: If the course does not exist.
                                                      - **401 Unauthorized**: If the user is not authenticated.
                                                      - **403 Forbidden**: If the user does not have the required permissions.
                                                      - **500 Internal Server Error**: If an unexpected error occurs during the process.

                                                      #### Example
                                                      ```http
                                                      GET /courses/123/reviews?pageNumber=1&pageSize=5&contentLimit=50
                                                      Authorization: Bearer <token>
                                                      ```
                                                      """;

    public const string UpdateCourseReviewDescription = """
                                                        ### Update Course Review
                                                        Updates an existing review for a specific course.

                                                        #### Usage
                                                        - **Endpoint**: `PUT /courses/{courseId}/reviews/{reviewId}`
                                                        - **Authorization**: Requires a Bearer token.
                                                        - **Parameters**:
                                                          - `courseId` (int): The unique identifier of the course.
                                                          - `reviewId` (int): The unique identifier of the review to update.

                                                        #### Request Body
                                                        - `Content` (string, optional): The updated review content. If not provided, the content remains unchanged.
                                                        - `Rating` (float, optional): The updated review rating. If not provided, the rating remains unchanged.

                                                        #### Response
                                                        - **204 No Content**: Indicates the review was successfully updated.
                                                        - **400 Bad Request**: If the request parameters are invalid.
                                                        - **401 Unauthorized**: If the user is not authenticated.
                                                        - **403 Forbidden**: If the user does not have permission to update the review.
                                                        - **404 Not Found**: If the review or course does not exist.

                                                        #### Example
                                                        ```http
                                                        PUT /courses/123/reviews/456
                                                        Authorization: Bearer <token>
                                                        Content-Type: application/json

                                                        {
                                                            "Content": "Updated review content.",
                                                            "Rating": 4.0
                                                        }
                                                        ```
                                                        """;

    public const string DeleteCourseReviewDescription = """
                                                        ### Delete Course Review
                                                        Deletes an existing review for a specific course.

                                                        #### Usage
                                                        - **Endpoint**: `DELETE /courses/{courseId}/reviews/{reviewId}`
                                                        - **Authorization**: Requires a Bearer token.
                                                        - **Parameters**:
                                                          - `courseId` (int): The unique identifier of the course.
                                                          - `reviewId` (int): The unique identifier of the review to delete.

                                                        #### Response
                                                        - **204 No Content**: Indicates the review was successfully deleted.
                                                        - **400 Bad Request**: If the request parameters are invalid.
                                                        - **401 Unauthorized**: If the user is not authenticated.
                                                        - **403 Forbidden**: If the user does not have permission to delete the review.
                                                        - **404 Not Found**: If the review or course does not exist.

                                                        #### Example
                                                        ```http
                                                        DELETE /courses/123/reviews/456
                                                        Authorization: Bearer <token>
                                                        ```
                                                        """;

    public const string GetCourseReviewDescription = """
                                                     ### Get Course Review
                                                     Retrieves a specific review for a course based on the review ID and course ID.

                                                     #### Usage
                                                     - **Endpoint**: `GET /courses/{courseId}/reviews/{reviewId}`
                                                     - **Authorization**: Requires a Bearer token.
                                                     - **Parameters**:
                                                       - `courseId` (int): The unique identifier of the course.
                                                       - `reviewId` (int): The unique identifier of the review to retrieve.

                                                     #### Response
                                                     - **200 OK**: Returns the review details in the `UserReviewDto` format.
                                                     - **400 Bad Request**: If the request parameters are invalid.
                                                     - **401 Unauthorized**: If the user is not authenticated.
                                                     - **403 Forbidden**: If the user does not have permission to retrieve the review.
                                                     - **404 Not Found**: If the review or course does not exist.

                                                     #### Example
                                                     ```http
                                                     GET /courses/123/reviews/456
                                                     Authorization: Bearer <token>
                                                     ```
                                                     """;
}