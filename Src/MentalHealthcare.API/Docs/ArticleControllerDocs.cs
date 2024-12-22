namespace MentalHealthcare.API.Docs
{
    public class ArticleControllerDocs
    {
        #region Update

        public const string UpdateDescription = @"
### Update an Existing Article
Updates the details of an existing article. You can selectively update one or more fields.

#### Request:
- **ArticleId**: The ID of the article to update (required, integer).
- **Title**: The new title for the article (optional, string).
- **Content**: The updated content of the article (optional, string).
- **AuthorName**: The updated author name (optional, string).
- **Tags**: A new list of tags for the article (optional, array of strings).
- **Image**: A new image file to replace the current thumbnail (optional, file).

#### Notes:
- Any field sent as `null` will retain its existing value.
- Tags will completely replace the old tags if provided.

#### Response:
- Returns the updated `ArticleDto` with the new values.

#### Example Usage:
To update the title and tags of an article:
```json
{
  ""ArticleId"": 123,
  ""Title"": ""Mental Health Awareness"",
  ""Tags"": [""Awareness"", ""Health""]
}
```";

        #endregion

        #region Get by ID

        public const string GetByIdDescription = @"
### Get Article by ID
Retrieves the details of an article based on the provided ID.

#### Request:
- **ArticleId**: The ID of the article to retrieve (required, integer).

#### Response:
- Returns an `ArticleDto` containing the article details, such as title, content, author, tags, and image.

#### Example Usage:
To get an article by its ID:
```json
GET /api/articles/{ArticleId}
```";

        #endregion

        #region GetAll

        public const string GetAllDescription = @"
### Get All Articles
Retrieves a paginated list of all articles, with optional filtering by tags or author.

#### Request:
- **PageNumber**: The page number for pagination (integer).
- **PageSize**: The number of results per page (integer).
- **Tags**: A list of tags to filter articles by (optional, array of strings).
- **AuthorName**: The name of the author to filter by (optional, string).

#### Response:
- Returns a paginated result containing:
  - A list of `ArticleDto` objects.
  - Total count of articles.
  - Current page and page size.

#### Example Usage:
To retrieve articles filtered by tags and author:
```json
{
  ""PageNumber"": 1,
  ""PageSize"": 10,
  ""Tags"": [""Mental Health"", ""Well-being""],
  ""AuthorName"": ""John Doe""
}
```";

        #endregion
    }
}
