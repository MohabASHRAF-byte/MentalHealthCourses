namespace MentalHealthcare.API.Docs;

public static class TermsAndConditionsControllerDocs
{
    #region patch update

    public const string PatchSummery = "Update an existing term or condition";

    public const string PatchDescription = "To update a term, send an object with the following structure:\n" +
                                           @"```json
                        {
                          ""id"": 1,
                          ""name"": ""Updated Term Name OR Existing Term Name"",
                          ""description"": ""Updated description of the term. OR existing term doesn't exist.""
                        }```";


    #endregion

    #region get all 
    
    public const string GetAllSummery = "Get All Terms and Conditions";
    public const string GetAllDescription = "No Need to send any information it will return the result\n";

    #endregion
}