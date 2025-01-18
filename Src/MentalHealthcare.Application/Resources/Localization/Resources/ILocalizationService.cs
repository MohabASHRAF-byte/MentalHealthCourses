namespace MentalHealthcare.Application.Resources.Localization.Resources;

public interface ILocalizationService
{
    string GetMessage(string key, string lang = "ar", string defaultMessage = "");
}