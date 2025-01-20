namespace MentalHealthcare.Application.Resources.Localization.Resources;

public interface ILocalizationService
{
    string GetMessage(string key,  string defaultMessage = "",string lang = "ar");
    public string TranslateNumber(decimal number,string lang="ar");
}