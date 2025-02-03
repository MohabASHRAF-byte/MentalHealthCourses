namespace MentalHealthcare.API.Docs
{
    public class ArticleControllerDocs
    {

        #region Update
        public const string UpdateDescription = @"
To update an article, provide the updated values for the fields you wish to change.  
- If a field is provided with a new value, it will be updated accordingly.  
- If a field is sent as `null`, the old value will be retained for that field.  

### Special Notes for Updating Article Images:
1. **`ArticleImageUrls` Field**:  
   - If `ArticleImageUrls` is provided as an empty list (`[]`), it will be considered as keeping all existing images. This ensures that articles always have at least one image.
   - If `ArticleImageUrls` contains one or more values, only these specified images will be retained, and any unspecified images will be deleted.

2. **`Images` Field**:  
   - New images can be added by attaching them to the `Images` field.
   - At least one image (either existing or new) must be present for the article to remain valid.

### Example Usage:
1. To update the article title and content while keeping the rest unchanged:
   - Set `Title` and `Content` to the new values.
   - Set `ArticleImageUrls` to `null` if you want to retain the current images.

2. To replace all current images with new ones:
   - Provide new images in the `Images` field and set `ArticleImageUrls` to an empty list (`[]`).

3. To retain specific images and add new ones:
   - Specify the images to keep in the `ArticleImageUrls` field.
   - Attach new images, if any, in the `Images` field.

### Edge Case for Replacing All Images:
If you want to replace all existing images with new ones, perform the following steps in separate requests:
1. Retrieve and save the URLs of the old images.
2. Submit a request to upload all new images.
3. Fetch the article data again to obtain the updated URLs (containing both old and new images).
4. Delete the old images by their URLs.
5. Submit another update request specifying only the URLs of the new images in the `ArticleImageUrls` field.

### Validation Notes:
- An article must have at least one image after the update. Sending an empty `ArticleImageUrls` or omitting it entirely will be treated as keeping all current images unless new images are provided in the `Images` field.
- Ensure all necessary validations are met when providing updated values.
";

        #endregion


        #region Get by ID
        public const string GetByIdDescription = @"
### Get Article by ID
Retrieves the details of an article based on the provided article ID.

#### Request:
- **ArticleId**: The ID of the article to retrieve. This should be passed as a path parameter in the URL.

#### Response:
- Returns an `ArticleDto` containing the article's details, such as title, content, author name, creation date, and associated images.

#### Example Usage:
To get an article by its ID, pass the article ID as part of the URL path:
```json
GET /api/articles/{ArticleId}
```";

        #endregion





        #region Get All
        public const string GetAllDescription = @"
### Get All Articles
Retrieves a paginated list of articles with optional filtering by search text.

#### Request:
- **PageNumber**: The page number for pagination (integer, default is `1`).
- **PageSize**: The number of results per page (integer, default is `10`).
- **SearchText** (optional): Text to filter articles by their title or content.

#### Response:
- Returns a paginated result containing a list of `ArticleDto` objects, along with the total count of articles, the current page, and the page size.

#### Example Usage:
To retrieve all articles with pagination and optional search filtering:
```json
{
""PageNumber"": 1,
""PageSize"": 10,
""SearchText"": ""mental health""
}
```";
        #endregion










    }
}