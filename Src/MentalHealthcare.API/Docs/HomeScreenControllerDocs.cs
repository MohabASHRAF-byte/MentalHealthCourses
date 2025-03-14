using AngleSharp.Dom.Events;

namespace MentalHealthcare.API.Docs
{
    public class HomeScreenControllerDocs
    {
        #region GetAllAdvertisements

        public const string GetAllAdvertisements = @"
### Get All Advertisements
Retrieves a paginated list of advertisements with optional filtering by active status.

#### Request:
- **PageNumber**: The page number for pagination (integer).
- **PageSize**: The number of results per page (integer).
- **IsActive**: 
- `0` to get only inactive advertisements.
- `1` to get only active advertisements.
- Any other value (or not provided) to get all advertisements, regardless of their status.

#### Response:
- Returns a paginated result containing a list of `AdvertisementDto` objects, along with the total count of advertisements, the current page, and the page size.

#### Example Usage:
To retrieve all advertisements with pagination and filter by status:
```json
{
""PageNumber"": 1,
""PageSize"": 10,
""IsActive"": 1
}
";

        #endregion


        #region GetAllInstructors

        public const string GetAllInstructorDescription = @"
### Get All Instructors
Retrieves a paginated list of Instructors with optional filtering by active status.

#### Request:
- **PageNumber**: The page number for pagination (integer).
- **PageSize**: The number of results per page (integer).
- **SearchText** (optional): Text to filter articles by their title or content.

- Any other value (or not provided) to get all Instructors, regardless of their status.

#### Response:
- Returns a paginated result containing a list of `InstructorDto` objects, along with the total count of Instructors, the current page, and the page size.

#### Example Usage:
To retrieve all advertisements with pagination and filter by status:
```json
{
""PageNumber"": 1,
""PageSize"": 10,
""SearchText"": ABZ
}
";

        #endregion





        #region MostRecentCoursesSection:
        public const string MostRecentCoursesSection = @"
#### Request:
- **Count**: (integer) The number of recent courses to retrieve.  
  - Example: `3` will return the latest 3 courses.

#### Response:
- Returns a list of `CourseViewDto` objects with details of each course.

---

### CourseViewDto Structure:
- `CourseId` (int): Unique ID of the course.
- `Name` (string): Title of the course.
- `Price` (decimal): Cost of the course.
- `Rating` (double): Average rating of the course.
- `ThumbnailUrl` (string): Thumbnail image URL for the course.

---`";
        #endregion




    }
}
