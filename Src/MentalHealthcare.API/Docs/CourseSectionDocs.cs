namespace MentalHealthcare.API.Docs;

public class CourseSectionDocs
{
    public const string RemoveSectionDescription = """
                                                   ### Remove Section

                                                   This endpoint deletes a specified section from a course.

                                                   #### Request:
                                                   - **Authentication**: Requires a Bearer token.
                                                   - **Path Parameters**:
                                                     - `CourseId` (Required): The ID of the course containing the section to be deleted.
                                                     - `SectionId` (Required): The ID of the section to delete.

                                                   #### Response:
                                                   - **204 No Content**:
                                                     - The section was successfully deleted.
                                                   - **401 Unauthorized**:
                                                     - The user is not authenticated.
                                                   - **403 Forbidden**:
                                                     - The user does not have permission to delete the section.
                                                   - **400 Bad Request**:
                                                     - The section is not empty.
                                                   - **404 Not Found**:
                                                     - The course or section does not exist.

                                                   #### Edge Cases:
                                                   - Sections can only be deleted if they are empty (i.e., contain no lessons).
                                                   - Only administrators are authorized to delete sections.

                                                   #### Example Usage:
                                                   To delete a section from a course:
                                                   ```http
                                                   DELETE /api/courses/123/sections/456 HTTP/1.1
                                                   Authorization: Bearer <token>
                                                   ```
                                                   """;   
    public const string UpdateSectionDescription = """
                                                   ### Update Section

                                                   This endpoint updates the name of a specified section in a course.

                                                   #### Request:
                                                   - **Authentication**: Requires a Bearer token.
                                                   - **Path Parameters**:
                                                     - `SectionId` (Required): The ID of the section to update.
                                                   - **Body Parameters**:
                                                     - `SectionName` (Required): The new name of the section.

                                                   #### Response:
                                                   - **204 NoContent**:
                                                   - **401 Unauthorized**:
                                                     - The user is not authenticated.
                                                   - **403 Forbidden**:
                                                     - The user does not have permission to update the section.
                                                   - **404 Not Found**:
                                                     - The section does not exist.

                                                   #### Edge Cases:
                                                   - Only administrators are authorized to update sections.

                                                   #### Example Usage:
                                                   To update a section's name:
                                                   ```http
                                                   PUT /api/courses/sections/456 HTTP/1.1
                                                   Authorization: Bearer <token>
                                                   Content-Type: application/json

                                                   {
                                                     "sectionName": "Updated Section Name"
                                                   }
                                                   ```
                                                   """;

}