namespace MentalHealthcare.API.Docs;

public static class ClientIdentityDocs
{
    #region Register

    public const string RegisterDescription = @"
Handles user registration.

This endpoint allows new users to create an account in the system.

### Usage Instructions:
1. Provide the required registration details, such as email, password, and other necessary user information.
2. Submit the request to create a new account.

### Request Parameters:
- `email` (string, required): The email address for the new user.
- `password` (string, required): The password for the account.
- `userDetails` (object, required): Additional information about the user (e.g., name, phone).

### Response:
**Success (201):**
- If the registration is successful:
  ```json
  {
    ""stateCode"": ""Created"",
    ""message"": ""User registered successfully"",
    ""result"": null
  }

**Failure:**
- If registration fails due to validation errors or duplicate accounts:
  ```json
  {
    ""stateCode"": ""BadRequest"",
    ""message"": ""Error details"",
    ""result"": null
  }";

    #endregion

    #region Confirm Email

    public const string ConfirmEmailDescription = @"
Confirms the user's email address.

This endpoint verifies the user's email by validating the confirmation token sent to their email address.

### Usage Instructions:
1. Submit the user's email and the confirmation token received via email.
2. If the token is valid, the email will be confirmed.

### Request Parameters:
- `email` (string, required): The user's email address.
- `confirmationToken` (string, required): The token sent to the user's email for verification.

### Response:
**Success (200):**
- If the email is successfully confirmed:
  ```json
  {
    ""stateCode"": ""Success"",
    ""message"": ""Email confirmed successfully"",
    ""result"": null
  }

**Failure:**
- If the token is invalid or expired:
  ```json
  {
    ""stateCode"": ""BadRequest"",
    ""message"": ""Invalid or expired token"",
    ""result"": null
  }";

    #endregion

    #region Forget Password

    public const string ForgetPasswordDescription = @"
Initiates the password reset process.

This endpoint sends a one-time password (OTP) to the user's registered email address to reset their password.

### Usage Instructions:
1. Provide the registered email address of the user.
2. The system will send an OTP to the email if the account exists.

### Request Parameters:
- `email` (string, required): The user's registered email address.

### Response:
**Success (200):**
- If the OTP is sent successfully:
  ```json
  {
    ""stateCode"": ""Success"",
    ""message"": ""OTP sent successfully"",
    ""result"": null
  }

**Failure:**
- If the email does not exist in the system:
  ```json
  {
    ""stateCode"": ""NotFound"",
    ""message"": ""Email not found"",
    ""result"": null
  }";

    #endregion

    #region Login

    public const string LoginDescription = @"
Handles user authentication.

This endpoint authenticates the user and provides access and refresh tokens.

### Usage Instructions:
1. Submit the user's email and password to log in.
2. If 2FA is enabled, provide the OTP in a follow-up request.

### Request Parameters:
- `email` (string, required): The user's email address.
- `password` (string, required): The user's password.
- `otp` (string, optional): The one-time password (OTP) for two-factor authentication.

### Response:
**Success (200):**
- If login is successful:
  ```json
  {
    ""stateCode"": ""Success"",
    ""message"": ""Login successful"",
    ""result"": {
        ""token"": ""string"",
        ""refreshToken"": ""string"",
        ""requireOtp"": false
    }
  }

**Failure:**
- If the credentials are invalid:
  ```json
  {
    ""stateCode"": ""Unauthorized"",
    ""message"": ""Invalid credentials"",
    ""result"": null
  }";

    #endregion

    #region Reset Password

    public const string ResetPasswordDescription = @"
Resets the user's password.

This endpoint allows users to reset their password using a reset code.

### Usage Instructions:
1. Ensure the user has used the **ForgetPassword** endpoint to request an OTP.
2. Provide the user's email, the OTP, and the new password to reset the password.

### Request Parameters:
- `email` (string, required): The user's email address.
- `resetCode` (string, required): The OTP received via email.
- `newPassword` (string, required): The new password for the account.

### Response:
**Success (200):**
- If the password is successfully reset:
  ```json
  {
    ""stateCode"": ""Success"",
    ""message"": ""Password reset successful"",
    ""result"": null
  }

**Failure:**
- If the OTP is invalid or expired:
  ```json
  {
    ""stateCode"": ""BadRequest"",
    ""message"": ""Invalid or expired OTP"",
    ""result"": null
  }";

    #endregion

    #region Change Password

    public const string ChangePasswordDescription = @"
Updates the user's password.

This endpoint allows logged-in users to change their password.

### Usage Instructions:
1. The user must be authenticated (logged in) and provide a valid Bearer token.
2. Submit the current password and the new password.

### Request Parameters:
- `oldPassword` (string, required): The user's current password.
- `newPassword` (string, required): The new password.

### Response:
**Success (200):**
- If the password is successfully changed:
  ```json
  {
    ""stateCode"": ""Success"",
    ""message"": ""Password changed successfully"",
    ""result"": null
  }";

    #endregion
}
