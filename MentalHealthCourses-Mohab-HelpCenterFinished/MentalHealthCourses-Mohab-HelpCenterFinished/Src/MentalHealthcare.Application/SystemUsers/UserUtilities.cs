using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MentalHealthcare.Application.SystemUsers;

public static class UserUtilities
{
    public static bool IsValidEmail(this string input)
    {
        var emailChecker = new EmailAddressAttribute();
        return emailChecker.IsValid(input);
    }

    public static bool IsValidPhoneNumber(this string input)
    {
        var phoneNumberPattern = @"^\d{7,15}$";
        return Regex.IsMatch(input, phoneNumberPattern);
    }

}