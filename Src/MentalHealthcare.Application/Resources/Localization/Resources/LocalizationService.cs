using System.Globalization;
using System.Text;
using Microsoft.Extensions.Localization;

namespace MentalHealthcare.Application.Resources.Localization.Resources;

public class LocalizationService : ILocalizationService
{
    private readonly IStringLocalizer _stringLocalizer;

    public LocalizationService(IStringLocalizerFactory factory)
    {
        var type = typeof(Messages);
        _stringLocalizer = factory.Create(type);
    }

    public string GetMessage(string key, string lang = "ar", string defaultMessage = "")
    {
        // Set the current culture to Arabic (ar)
        CultureInfo.CurrentUICulture = new CultureInfo(lang);
        var localizedString = _stringLocalizer[key];
        return localizedString.ResourceNotFound ? defaultMessage : localizedString.Value;
    }

    public string TranslateNumber(decimal input)
    {
        string[] arabicDigits = { "٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩" };
        var result = new StringBuilder();

        foreach (char digit in input.ToString())
        {
            if (char.IsDigit(digit))
            {
                result.Append(arabicDigits[digit - '0']);
            }
            else
            {
                result.Append(digit);
            }
        }

        return result.ToString();
    }
}