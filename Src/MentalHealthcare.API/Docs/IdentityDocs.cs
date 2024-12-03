namespace MentalHealthcare.API.Docs;

public static class IdentityDocs
{
    #region Login

    public const string LoginDescription = @"
Handles the login process for users.

This endpoint is used to authenticate users into the system. It validates the user's credentials and handles scenarios where two-factor authentication (2FA) is required.

### Usage Instructions:
1. Send the user's identifier (email, phone, or username) and password to this endpoint.
2. If the credentials are valid and the user's account does not require 2FA, the API will return a JWT token and refresh token.
3. If the credentials are valid but 2FA is enabled for the user, a 2FA code will be sent to the user's email, and the response will indicate that 2FA is required (`requireOtp: true`).
4. If 2FA is required, send the identifier, password, and the 2FA OTP in a follow-up request to this endpoint. Upon successful verification, the API will return the JWT token and refresh token.

### Request Parameters:
- `userIdentifier` (string, required): The user's unique identifier. It can be an email, phone number, or username.
- `password` (string, required): The user's password.
- `otp` (string, optional): The one-time password (OTP) for two-factor authentication.
- `tenant` (string, optional): This can be ignored (don't send this field).

### Response:
**Success (200):**
- If 2FA is not required:
  ```json
  {
    ""name"": ""string"",
    ""token"": ""string"",
    ""refreshToken"": ""string"",
    ""requireOtp"": false
  }";

    #endregion
    
    #region reset password

    public const string ResetPasswordDescription = @"
Handles the password reset process for users.

This endpoint allows users to reset their password after verifying a reset code sent to their registered email.

### Usage Instructions:
1. Ensure the user has previously used the **ForgetPassword** endpoint to request a password reset. This will send a reset code to their registered email.
2. Use this endpoint to reset the password by providing:
   - The registered email address.
   - The reset code received via email.
   - The new desired password.

### Request Parameters:
- `email` (string, required): The email address of the user requesting the password reset.
- `newPassword` (string, required): The new password that the user wants to set.
- `resetCode` (string, required): The reset code received via the **ForgetPassword** process.
- `tenant` (string, optional): The tenant to which the user belongs. This can be ignored if multi-tenancy is not applicable.

### Response:
**Success (200):**
- If the reset code is valid and the password reset is successful:
  ```json
  {
    ""stateCode"": ""Success"",
    ""message"": ""Password reset successful"",
    ""result"": null
  }

**Failure:**
- If the reset code is invalid or expired:
  ```json
  {
    ""stateCode"": ""BadRequest"",
    ""message"": ""Invalid or expired OTP"",
    ""result"": null
  }";

    #endregion

    public const string ChangePasswordDescription = @"
Handles the password change process for authenticated users.

This endpoint allows logged-in users to update their password by providing their current password and a new password.

### Usage Instructions:
1. The user must be authenticated (logged in) and include a valid Bearer token in the authorization header of the request.
2. Submit the old password and the desired new password to this endpoint.
3. If the old password matches the user's current password and the new password meets security requirements, the password will be updated.

### Request Parameters:
- `oldPassword` (string, required): The user's current password.
- `newPassword` (string, required): The new password the user wants to set.

### Response:
**Success (200):**
- If the password is successfully changed:
  ```json
  {
    ""stateCode"": ""Success"",
    ""message"": ""Password changed successfully"",
    ""result"": null
  }";

    public const string AddRolesDescription = @"
Handles the assignment of roles to users.

This endpoint allows an authorized admin to add roles to a specific user. The roles determine the permissions and access level for the user in the system.

### Authorization:
- **Admin access required**: The user making this request must be authenticated and have an admin role.
- Include a valid Bearer token in the authorization header.

### Usage Instructions:
1. Ensure the requesting user has admin privileges.
2. Send the username of the target user and a list of integers representing the roles to be assigned.

### Request Parameters:
- `userName` (string, required): The username of the user to whom roles will be assigned.
- `roles` (array of integers, required): A list of integers representing the roles to assign. Each role corresponds to a specific permission level:
  - `0`: Admin
  - `1`: User

### Response:
**Success (200):**
- If the roles are successfully assigned:
  ```json
  {
    ""stateCode"": ""Success"",
    ""message"": ""Success"",
    ""result"": null
  }";

    public const string RemoveRolesDescription = @"
Handles the removal of roles from users.

This endpoint allows an authorized admin to remove specific roles from a user. Roles define the user's permissions and access levels in the system.

### Authorization:
- **Admin access required**: The user making this request must be authenticated and have an admin role.
- Include a valid Bearer token in the authorization header.

### Usage Instructions:
1. Ensure the requesting user has admin privileges.
2. Send the username of the target user and a list of integers representing the roles to be removed.

### Request Parameters:
- `userName` (string, required): The username of the user from whom roles will be removed.
- `roles` (array of integers, required): A list of integers representing the roles to remove. Each role corresponds to a specific permission level:
  - `0`: Admin
  - `1`: User

### Response:
**Success (200):**
- If the roles are successfully removed:
  ```json
  {
    ""stateCode"": ""Success"",
    ""message"": ""Roles removed successfully"",
    ""result"": null
  }";
}