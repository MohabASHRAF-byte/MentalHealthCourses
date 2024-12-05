namespace MentalHealthcare.API.Docs;

public static class HelpCenterControllerDocs
{
    #region patch update

    public const string PatchSummery = "Update an existing HelpCenter items ";

    public const string HelpCenterUpdateDescription = @"
### Update Help Center Item
Updates an existing Help Center item (e.g., Terms and Conditions, Privacy Policy, FAQ) with new details.

#### Request:
- The request body must include the following fields as part of the `Item` object:
  - **Type** *(required)*: The type of the Help Center item to update. Must be one of the following values:
    - `1`: Terms and Conditions
    - `2`: Privacy Policy
    - `3`: FAQ
  - **Name** *(required)*: A string containing the updated name of the Help Center item.
  - **Description** *(required)*: A string containing the updated description of the Help Center item.

#### Response:
- **204 No Content**: Indicates that the update was successful.
- **400 Bad Request**: Indicates that the request was invalid (e.g., missing required fields, invalid `Type` value).

#### Example Usage:
```json
PUT /api/helpcenter
{
  ""item"": {
    ""Type"": 1,
    ""Name"": ""Updated Terms and Conditions"",
    ""Description"": ""The updated description for Terms and Conditions.""
  }
}";

    #endregion

    #region get all

    public const string GetAllSummery = "Get All Terms and Conditions";

    public const string HelpCenterGetDescription = @"
### Get Help Center Items
Retrieves a list of Help Center items based on the specified type (e.g., Terms and Conditions, Privacy Policy, FAQ).

#### Request:
- **itemType** *(required)*: The type of Help Center items to retrieve. Must be one of the following values:
  - `1`: Terms and Conditions
  - `2`: Privacy Policy
  - `3`: FAQ

#### Response:
- Returns a list of Help Center items, each containing details such as ID, name, and description.

#### Example Usage:
```json
GET /api/helpcenter?itemType=1
";

    #endregion

    #region post Help Center

    public const string HelpCenterPostDescription = @"
### Create Help Center Item
Creates a new Help Center item (e.g., Terms and Conditions, Privacy Policy, FAQ).

#### Request:
- **HelpCenterItemType** *(required)*: The type of the Help Center item. Must be one of the following values:
  - `1`: Terms and Conditions
  - `2`: Privacy Policy
  - `3`: FAQ
- **Name** *(required)*: The name of the Help Center item. Maximum length: 100 characters.
- **Description** *(optional)*: A description for the item. Maximum length: 500 characters.

#### Response:
- Returns the ID of the newly created Help Center item.

#### Example Usage:
```json
POST /api/helpcenter
{
  ""HelpCenterItemType"": 1,
  ""Name"": ""Website Terms of Service"",
  ""Description"": ""These are the terms of service for using the website.""
}";

    #endregion
}