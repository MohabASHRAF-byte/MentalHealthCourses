namespace MentalHealthcare.API.Docs;

public static class CourseFavouriteDocs
{
    public const string ToggleFavouriteCourseDescription = """
### Toggle Favourite Course
Allows the user to mark a course as favourite or remove it from their favourites list.

#### Usage
- **Endpoint**: `POST /Api/Favourite/{courseId}`
- **Authorization**: Requires a Bearer token.
- **Parameters**:
  - `courseId` (int): The ID of the course to toggle.

#### Behavior
- If the course is not already marked as a favourite, it will be added to the favourites list.
- If the course is already marked as a favourite, it will be removed from the favourites list.

#### Response
- **204 No Content**: Successfully toggled the favourite status of the course.
- **401 Unauthorized**: If the user is not authenticated.

#### Example
```http
POST /Api/Favourite/123
Authorization: Bearer <token>
```
""";

public const string GetUsersWhoFavouriteCourseDescription = """
### Get Users Who Favourited a Course
Retrieves a list of users who have marked a specific course as their favourite.

#### Usage
- **Endpoint**: `GET /Api/Favourite/{courseId}`
- **Authorization**: Requires a Bearer token.
- **Parameters**:
  - `courseId` (int): The ID of the course.

#### Response
- **200 OK**: Returns a paginated list of users who favourited the course.
  - `count` (int): Total number of users.
  - `users` (List<SystemUser>): List of user details.
- **401 Unauthorized**: If the user is not authenticated.

#### Example
```http
GET /Api/Favourite/123
Authorization: Bearer <token>
```
""";

public const string GetFavouriteCoursesDescription = """
### Get Favourite Courses
Retrieves the list of favourite courses for the authenticated user.

#### Usage
- **Endpoint**: `GET /Api/Favourite`
- **Authorization**: Requires a Bearer token.
- **Query Parameters**:
  - `pageNumber` (int): Page number for pagination (default: 1).
  - `pageSize` (int): Number of courses per page (default: 10).
  - `searchTerm` (string, optional): Filter by course name (case insensitive).

#### Response
- **200 OK**: Returns a paginated list of favourite courses.
  - `count` (int): Total number of favourite courses.
  - `courses` (List<CourseViewDto>): List of course details.
- **401 Unauthorized**: If the user is not authenticated.

#### Example
```http
GET /Api/Favourite?pageNumber=1&pageSize=10&searchTerm=math
Authorization: Bearer <token>
```
""";

}