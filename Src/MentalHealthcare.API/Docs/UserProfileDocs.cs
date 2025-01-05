

namespace MentalHealthcare.API.Docs;

public static class UserProfileDocs
{
    public const string UpdateProfileDescription = """
        ### Update User Profile
        Updates the profile information of the authenticated user.

        #### Usage
        - **Endpoint**: `PUT /UserProfile`
        - **Authorization**: Requires a Bearer token.

        #### Request Body
        - `FirstName` (string): The user's updated first name.
        - `LastName` (string): The user's updated last name.
        - `PhoneNumber` (string): The user's updated phone number.
        - `BirthDate` (DateTime?): The user's updated birth date.

        #### Behavior
        - The user must be authenticated.
        - The profile information is updated with the provided data.

        #### Response
        - **204 No Content**: The profile was successfully updated.
        - **401 Unauthorized**: If the user is not authenticated.
        - **403 Forbidden**: If the user does not have permission to update the profile.
        - **400 Bad Request**: If the request body contains invalid data.

        #### Example
        ```http
        PUT /UserProfile
        Authorization: Bearer <token>
        Content-Type: application/json

        {
            "FirstName": "John",
            "LastName": "Doe",
            "PhoneNumber": "123456789",
            "BirthDate": "1990-01-01"
        }
        ```
        """;

    public const string GetProfileDescription = """
        ### Get User Profile
        Retrieves the profile information of the authenticated user.

        #### Usage
        - **Endpoint**: `GET /UserProfile`
        - **Authorization**: Requires a Bearer token.

        #### Behavior
        - The user must be authenticated.
        - Returns the user's profile data.

        #### Response
        - **200 OK**: Returns the user's profile data as a `UserProfileDto`.
        - **401 Unauthorized**: If the user is not authenticated.
        - **403 Forbidden**: If the user does not have permission to access the profile.

        #### Example
        ```http
        GET /UserProfile
        Authorization: Bearer <token>
        ```

        #### Response Example
        ```json
        {
            "FirstName": "John",
            "LastName": "Doe",
            "PhoneNumber": "123456789",
            "BirthDate": "1990-01-01"
        }
        ```
        """;
}
