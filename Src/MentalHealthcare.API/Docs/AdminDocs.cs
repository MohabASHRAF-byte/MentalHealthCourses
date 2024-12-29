namespace MentalHealthcare.API.Docs;

public static class AdminDocs
{

    public const string AddPendingAdminDescription = @"
### Add a New Pending Admin
This endpoint allows an authorized Admin user to add an email to the pending admin list. The added email can later be used by the corresponding user to register as an admin.

#### Authorization
- The user must be an authorized admin with a valid Bearer token provided in the `Authorization` header.

#### Request
- **Header**: Include an `Authorization` header with a valid Bearer token.
- **Body**: Provide the email to be added to the pending admin list.

#### Example Request:
```http
POST /api/admin/PendingAdmin HTTP/1.1
Host: example.com
Authorization: Bearer YOUR_ACCESS_TOKEN
Content-Type: application/json

{
  ""email"": ""newadmin@example.com""
}";


    public const string UpdatePendingAdmin = @"
### Update a Pending Admin's Email
This endpoint allows an authorized Admin user to update the email address of a pending admin in the system.

#### Authorization
- The user must be an authorized admin with a valid Bearer token provided in the `Authorization` header.

#### Request
- **Header**: Include an `Authorization` header with a valid Bearer token.
- **Body**: Provide the current email of the pending admin (`OldEmail`) and the new email (`NewEmail`).

#### Example Request:
```http
PUT /api/admin/UpdatePendingAdminEmail HTTP/1.1
Host: example.com
Authorization: Bearer YOUR_ACCESS_TOKEN
Content-Type: application/json

{
  ""OldEmail"": ""oldemail@example.com"",
  ""NewEmail"": ""newemail@example.com""
}";
    public const string RegisterAdminDescription = """
                                                       ### Register a New Admin User
                                                       Creates a new admin user and registers them in the system.
                                                   
                                                       #### Usage
                                                       - **Endpoint**: `POST /api/admin/register`
                                                       - **Authorization**: No authorization required.
                                                       - **Parameters**:
                                                         - `RegisterAdminCommand` (body): The command object containing the details of the admin user to register.
                                                   
                                                       #### Behavior
                                                       - Ensures the email exists in the pending admin list.
                                                       - Registers the admin user and sends a confirmation email.
                                                   
                                                       #### Response
                                                       - **201 Created**: Successfully registered the admin user.
                                                       - **400 Bad Request**: If the request data is invalid.
                                                       - **403 Forbidden**: If the email is not in the pending admin list.
                                                   
                                                       #### Example
                                                       ```http
                                                       POST /api/admin/register
                                                       Content-Type: application/json
                                                   
                                                       {
                                                           "UserName": "adminuser",
                                                           "Email": "admin@example.com",
                                                           "FirstName": "Admin",
                                                           "LastName": "User",
                                                           "Password": "SecurePassword123!"
                                                       }
                                                       ```
                                                   """;

    
}