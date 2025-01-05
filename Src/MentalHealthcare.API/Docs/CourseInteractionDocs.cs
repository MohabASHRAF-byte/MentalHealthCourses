namespace MentalHealthcare.API.Docs;

public static class CourseInteractionDocs
{
    public const string EnrollCourseDescription = """
                                                  ### Enroll Course
                                                  Enrolls the authenticated user into a specified free course.

                                                  #### Usage
                                                  - **Endpoint**: `POST /Enroll/{courseId}`
                                                  - **Authorization**: Requires a Bearer token.
                                                  - **Parameters**:
                                                    - `courseId` (int): The unique identifier of the course to enroll in.

                                                  #### Behavior
                                                  - The course must be marked as free.
                                                  - The user cannot already own the course.

                                                  #### Response
                                                  - **204 No Content**: User successfully enrolled in the course.
                                                  - **400 Bad Request**: The course is not free or is already owned by the user.
                                                  - **401 Unauthorized**: If the user is not authenticated.
                                                  - **500 Internal Server Error**: If an error occurs during enrollment.

                                                  #### Example
                                                  ```http
                                                  POST /Enroll/123
                                                  Authorization: Bearer <token>
                                                  ```
                                                  """;

    public const string CompleteLessonDescription = """
                                                  ### Complete Lesson
                                                  Marks a specific lesson as completed for the authenticated user.

                                                  #### Usage
                                                  - **Endpoint**: `POST /{courseId}/complete/{lessonId}`
                                                  - **Authorization**: Requires a Bearer token.
                                                  - **Parameters**:
                                                    - `courseId` (int): The unique identifier of the course containing the lesson.
                                                    - `lessonId` (int): The unique identifier of the lesson to complete.

                                                  #### Behavior
                                                  - The user must be enrolled in the course.
                                                  - The lesson must exist in the course.
                                                  - The previous lessons must be completed in sequential order.

                                                  #### Response
                                                  - **204 No Content**: Lesson successfully marked as completed.
                                                  - **400 Bad Request**: The lesson is invalid, not sequential, or does not belong to the course.
                                                  - **401 Unauthorized**: If the user is not authenticated.
                                                  - **500 Internal Server Error**: If an error occurs while completing the lesson.

                                                  #### Example
                                                  ```http
                                                  POST /123/complete/456
                                                  Authorization: Bearer <token>
                                                  ```
                                                  """;

    public const string GetCourseLessonDescription = """
                                                  ### Get Course Lesson
                                                  Retrieve a specific lesson for the authenticated user to watch.

                                                  #### Usage
                                                  - **Endpoint**: `GET /{courseId}/watch/{lessonId}`
                                                  - **Authorization**: Requires a Bearer token.
                                                  - **Parameters**:
                                                    - `courseId` (int): The unique identifier of the course containing the lesson.
                                                    - `lessonId` (int): The unique identifier of the lesson to retrieve.

                                                  #### Behavior
                                                  - The user must be enrolled in the course.
                                                  - The lesson must exist in the course.

                                                  #### Response
                                                  - **200 OK**: Returns the details of the lesson as a `CourseLessonDto`.
                                                  - **401 Unauthorized**: If the user is not authenticated.
                                                  - **403 Forbidden**: If the user does not have access to the lesson.
                                                  - **404 Not Found**: If the lesson or course does not exist.

                                                  #### Example
                                                  ```http
                                                  GET /123/watch/456
                                                  Authorization: Bearer <token>
                                                  ```
                                                  """;
}
