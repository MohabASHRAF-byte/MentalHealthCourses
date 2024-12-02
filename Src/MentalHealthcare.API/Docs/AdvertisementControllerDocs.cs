namespace MentalHealthcare.API.Docs;

public static class AdvertisementControllerDocs
{
    #region update

    public const string UpdateDescription = @"
To update an advertisement, provide the updated values for the fields you wish to change.  
- If a field is provided with a new value, it will be updated accordingly.  
- If a field is sent as `null`, the old value will be retained for that field.  

### Special Notes for Updating Advertisement Images:
1. **`ImagesUrls` Field**:  
   - If `ImagesUrls` is provided as an empty list (`[]`), it will be considered as keeping all existing images. This ensures that advertisements always have at least one image.
   - If `ImagesUrls` contains one or more values, only these specified images will be retained, and any unspecified images will be deleted.

2. **`Images` Field**:  
   - New images can be added by attaching them to the `Images` field.
   - At least one image (either existing or new) must be present for the advertisement to remain valid.

### Example Usage:
1. To update the advertisement name and description while keeping the rest unchanged:
   - Set `AdvertisementName` and `AdvertisementDescription` to the new values.
   - Set `IsActive` and `ImagesUrls` to `null` if you want to retain their current values.

2. To update the active status and replace all current images with new ones:
   - Set `IsActive` to the new status (`true` or `false`).
   - Provide new images in the `Images` field and set `ImagesUrls` to an empty list (`[]`).

3. To update the active status and retain specific images:
   - Set `IsActive` to the new status.
   - Specify the images to keep in the `ImagesUrls` field.
   - Attach new images, if any, in the `Images` field.

### Edge Case for Replacing All Images:
If you want to replace all existing images with new ones, perform the following steps in separate requests:
1. Retrieve and save the URLs of the old images.
2. Submit a request to upload all new images.
3. Fetch the advertisement data again to obtain the updated URLs (containing both old and new images).
4. Delete the old images by their URLs.
5. Submit another update request specifying only the URLs of the new images in the `ImagesUrls` field.

### Validation Notes:
- An advertisement must have at least one image after the update. Sending an empty `ImagesUrls` or omitting it entirely will be treated as keeping all current images unless new images are provided in the `Images` field.
- Ensure all necessary validations are met when providing updated values.
";

    #endregion

    #region Get by id

    public const string GetByIdDescription = @"
### Get Advertisement by ID
Retrieves the details of an advertisement based on the provided advertisement ID.

#### Request:
- **AdvertisementId**: The ID of the advertisement to retrieve. This should be passed as a path parameter in the URL.

#### Response:
- Returns an `AdvertisementDto` containing the advertisement's details, such as name, description, status, and associated images.

#### Example Usage:
To get an advertisement by its ID, pass the advertisement ID as part of the URL path:
```json
GET /api/advertisements/{AdvertisementId}
```";

    #endregion

    #region GetAll

    public const string GetAllDescription = @"
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
}