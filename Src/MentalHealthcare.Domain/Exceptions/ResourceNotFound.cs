using System.Globalization;

namespace MentalHealthcare.Domain.Exceptions;

public class ResourceNotFound(string typeEn, string typeAr, string id, string language = "ar")
    : Exception(GetMessage(typeEn, typeAr, id, language))
{
    private static string GetMessage(string typeEn, string typeAr, string id, string language)
    {
        string translatedId = TranslateNumberToArabic(id);
        string type = language == "ar" ? typeAr : typeEn;
        return language == "ar" 
            ? $"لا يوجد {type} بالرقم : {translatedId}."
            : $"No {type} with Id: {id} exists.";
    }

    private static string TranslateNumberToArabic(string input)
    {
        if (long.TryParse(input, out _)) // Check if the string is a number
        {
            CultureInfo arabicCulture = new CultureInfo("ar-SA");
            return long.Parse(input).ToString("N0", arabicCulture).Replace(",", "");
        }
        return input;
    }
}