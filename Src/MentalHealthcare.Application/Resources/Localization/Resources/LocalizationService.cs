using System.Globalization;
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
}