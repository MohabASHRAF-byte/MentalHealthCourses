namespace MentalHealthcare.API.Docs;

public static class ContactUsDocs
{
    #region submit

    public const string SubmitContactUsDescription = @"
### Submit Contact Us Form
Submits a contact request form with either email or phone, but at least one of them must be provided.

#### Request:
- **Email** (Optional): The email address of the person submitting the request.
- **Phone** (Optional): The phone number of the person submitting the request.
- **Name**: The name of the person submitting the request.
- **Message**: The content of the message submitted through the contact form.

#### Response:
- Returns an integer representing the ID of the newly created contact request form.

#### Example Usage:
To submit a contact form, send the following data in the request body:
```json
POST /api/contactus
{
  ""email"": ""user@example.com"",
  ""phone"": ""+123456789"",
  ""name"": ""John Doe"",
  ""message"": ""I have a question about your services.""
}";

    #endregion

    #region Get all

    public const string GetAllFormsDescription = @"
### Get All Contact Forms
Retrieves a paginated list of contact forms, with optional filtering based on sender details, read status, and the ability to limit the preview length of the message.

#### Request:
You can provide any combination of the following query parameters for filtering and customization. If a parameter is omitted or set to `null`, it will not affect the filtering logic.

- **PageNumber**: The page number to retrieve (starting from 1).
- **PageSize**: The number of items to include per page.
- **ViewMsgLengthLimiter**: Limits the length of the returned message. If the message exceeds this limit, it is truncated and suffixed with `...`.
- **SenderName**: Filter forms by the sender's name (case-insensitive substring match).
- **SenderEmail**: Filter forms by the sender's email address (case-insensitive substring match).
- **SenderPhone**: Filter forms by the sender's phone number (case-insensitive substring match).
- **IsRead**: Filter forms by their read status (`true` for read, `false` for unread).

**Note**: Filters are combined using logical AND, meaning all provided filters must match for a form to be included in the result.

#### Response:
Returns a paginated `PageResult<ContactUsForm>` object containing:
- The total count of matching forms.
- The current page of contact forms, with messages limited to the specified length.

Each `ContactUsForm` includes:
- **ContactUsFormId**: The unique identifier of the form.
- **Name**: The sender's name.
- **Email**: The sender's email address.
- **PhoneNumber**: The sender's phone number.
- **Message**: The sender's message, limited to the specified `ViewMsgLengthLimiter` and suffixed with `...` if truncated.
- **IsRead**: Whether the form has been marked as read.
- **CreatedDate**: The date and time the form was created.

#### Example Usage:
To retrieve forms with a 30-character message preview on page 1 with a page size of 10, filtering by sender name and read status:
```json
GET /api/contact-forms?pageNumber=1&pageSize=10&viewMsgLengthLimiter=30&senderName=John&isRead=true";

    #endregion
}