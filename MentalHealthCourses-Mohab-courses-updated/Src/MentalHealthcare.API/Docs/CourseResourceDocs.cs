namespace MentalHealthcare.API.Docs;

public static class CourseResourceDocs
{
  public const string UploadLessonResourceDescription = @"
### Upload Lesson Resource
Uploads a resource file for a specific lesson within a course section. The uploaded file must meet the size limit of 10MB. The client is encouraged to validate the file size on the client side to avoid unnecessary API calls.

#### Request:
- **Route Parameters**:
  - **courseId** (int): The ID of the course.
  - **sectionId** (int): The ID of the section within the course.
  - **lessonId** (int): The ID of the lesson within the section.

- **Form Data**:
  - **File** (IFormFile, Required): The resource file to upload. Supported file types and their corresponding content types:
    - Video: `.mp4`
    - Image: `.jpeg`
    - Audio: `.mp3`
    - PDF: `.pdf`
    - Text: `.txt`
    - Zip: `.zip`
  - **FileName** (string, Required): The name of the file to be uploaded.
  - **ContentType** (enum, Required): The type of the resource being uploaded. Valid values:
    - `Video = 0`
    - `Image = 1`
    - `Audio = 2`
    - `Pdf = 3`
    - `Text = 4`
    - `Zip = 5`

#### Response:
- Returns an integer representing the ID of the newly uploaded resource.

#### Validation:
- Ensure the file size does not exceed **10MB**.
- Validate the file size and content type on the client side to prevent invalid API calls.
- Use proper MIME type headers when submitting the file.

#### Notes:
- This endpoint requires authentication with a valid Bearer token if authorization is enabled.
- Invalid content types or oversized files will result in a `400 Bad Request` error.
"";
";
}