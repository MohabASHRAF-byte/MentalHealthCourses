using System.Text.RegularExpressions;
using FluentValidation;
using Ganss.Xss;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Domain.Constants;
using Microsoft.AspNetCore.Http;


namespace MentalHealthcare.Application.validations;

public static class ValidationRules
{
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
    private const long MaxThumbnailFileSize = 1 * 1024 * 1024; // 1 MB
    private const long MaxIconFileSize = 10 * 1024; // 100 Kb

    public static IRuleBuilderOptions<T, IFormFile> CustomIsValidThumbnail<T>(
        this IRuleBuilder<T, IFormFile> ruleBuilder, ILocalizationService localizationService)
    {
        return ruleBuilder
            .Must(files => !string.IsNullOrEmpty(files?.FileName))
            .WithMessage(localizationService.GetMessage("ThumbnailRequired"))
            .Must(file =>
            {
                var extension = Path.GetExtension(file?.FileName);
                return !string.IsNullOrEmpty(extension);
            })
            .WithMessage(
                localizationService.GetMessage("ThumbnailInvalidExtension")
            )
            .Must(file =>
            {
                var extension = Path.GetExtension(file?.FileName)?.ToLower();
                return AllowedExtensions.Contains(extension);
            })
            .WithMessage(
                string.Format(
                    localizationService.GetMessage("ThumbnailInvalidType"),
                    string.Join(", ", AllowedExtensions)
                ))
            .Must(file => file?.Length > 0 && file?.Length <= MaxThumbnailFileSize)
            .WithMessage(
                string.Format(
                    localizationService.GetMessage("ThumbnailInvalidSize"),
                    localizationService.TranslateNumber(MaxThumbnailFileSize / (1024 * 1024))
                ));
    }


    public static IRuleBuilderOptions<T, IFormFile> CustomIsValidIcon<T>(
        this IRuleBuilder<T, IFormFile> ruleBuilder, ILocalizationService localizationService)
    {
        return ruleBuilder
            .NotNull()
            .WithMessage(localizationService.GetMessage("IconRequired"))
            .Must(file =>
            {
                var extension = Path.GetExtension(file?.FileName)?.ToLower();
                return AllowedExtensions.Contains(extension);
            })
            .WithMessage(
                string.Format(
                    localizationService.GetMessage("IconInvalidType"),
                    string.Join(", ", AllowedExtensions)
                ))
            .Must(file => file?.Length > 0 && file?.Length <= MaxIconFileSize)
            .WithMessage(
                string.Format(
                    localizationService.GetMessage("IconInvalidSize"),
                    localizationService.TranslateNumber(MaxIconFileSize / 1024)
                ));
    }


    /// <summary>
    /// Validates that a page size is valid based on dynamic limits.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="localizationService">The localization service for messages.</param>
    /// <param name="minLimit">The minimum allowed page size.</param>
    /// <param name="maxLimit">The maximum allowed page size.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, int> CustomValidatePageSize<T>(
        this IRuleBuilder<T, int> ruleBuilder,
        ILocalizationService localizationService
    )
    {
        return ruleBuilder
            .GreaterThan(0)
            .WithMessage(
                string.Format(
                    localizationService.GetMessage("PageSizeGreaterThan", "Page size must be greater than {0}."),
                    localizationService.TranslateNumber(0)
                ))
            .LessThanOrEqualTo(30)
            .WithMessage(
                string.Format(
                    localizationService.GetMessage("PageSizeLessThanOrEqualTo", "Page size must be {0} or less."),
                    localizationService.TranslateNumber(30)
                ));
    }


    /// <summary>
    /// Validates that a page number is valid based on dynamic limits.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="localizationService">The localization service for messages.</param>
    /// <param name="minPageNumber">The minimum allowed page number.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, int> CustomValidatePageNumber<T>(
        this IRuleBuilder<T, int> ruleBuilder,
        ILocalizationService localizationService,
        int minPageNumber = 1)
    {
        return ruleBuilder
            .GreaterThanOrEqualTo(minPageNumber)
            .WithMessage(
                string.Format(
                    localizationService.GetMessage("PageNumberGreaterThanOrEqualTo",
                        "Page number must be {0} or greater."),
                    localizationService.TranslateNumber(minPageNumber)
                ));
    }


    /// <summary>
    /// Validates that a search term is valid if it is not null.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="localizationService">The localization service for messages.</param>
    /// <param name="maxLength">The maximum allowed length for the search term.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, string?> CustomValidateSearchTerm<T>(
        this IRuleBuilder<T, string?> ruleBuilder,
        ILocalizationService localizationService,
        int maxLength = 50)
    {
        return ruleBuilder
            .Must(term => term == null || !string.IsNullOrWhiteSpace(term))
            .WithMessage(localizationService.GetMessage("SearchTermNotEmpty",
                "Search term must not be empty if provided."))
            .Must(term => term == null || term.Length <= maxLength)
            .WithMessage(
                string.Format(
                    localizationService.GetMessage("SearchTermMaxLength",
                        "Search term must not exceed {0} characters."),
                    localizationService.TranslateNumber(maxLength)
                ))
            .Must(term => term == null || !ContainsHtml(term))
            .WithMessage(localizationService.GetMessage("SearchTermNoHtml",
                "Search term must not contain HTML or markup."));
    }

    /// <summary>
    /// Validates that a string is a valid email address with a configurable maximum length,
    /// does not contain HTML or markup language, and is not empty.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="localizationService">The localization service for messages.</param>
    /// <param name="maxLength">The maximum allowed length for the email address.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, string> CustomIsValidEmail<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        ILocalizationService localizationService,
        int maxLength = 100)
    {
        return ruleBuilder
            .NotNull()
            .WithMessage(localizationService.GetMessage("EmailRequired", "Email address must not be empty."))
            .NotEmpty()
            .WithMessage(localizationService.GetMessage("EmailRequired", "Email address must not be empty."))
            .EmailAddress()
            .WithMessage(localizationService.GetMessage("EmailInvalidFormat", "Invalid email address format."))
            .MaximumLength(maxLength)
            .WithMessage(
                string.Format(
                    localizationService.GetMessage("EmailMaxLength", "Email address must not exceed {0} characters."),
                    localizationService.TranslateNumber(maxLength)
                ))
            .Must(value => !ContainsHtml(value))
            .WithMessage(
                localizationService.GetMessage("EmailNoHtml", "Email address must not contain HTML or markup."));
    }

    /// <summary>
    /// Validates that a name is valid: not exceeding a configurable maximum length, not containing HTML, and not empty or null.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="localizationService">The localization service for messages.</param>
    /// <param name="maxLength">The maximum allowed length for the name.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, string> CustomIsValidName<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        ILocalizationService localizationService,
        int maxLength = 30)
    {
        return ruleBuilder
            .NotNull()
            .WithMessage(localizationService.GetMessage("NameRequired", "Name must not be empty."))
            .NotEmpty()
            .WithMessage(localizationService.GetMessage("NameRequired", "Name must not be empty."))
            .MaximumLength(maxLength)
            .WithMessage(
                string.Format(
                    localizationService.GetMessage("NameMaxLength", "Name must not exceed {0} characters."),
                    localizationService.TranslateNumber(maxLength)
                ))
            .Must(name => !ContainsHtml(name))
            .WithMessage(localizationService.GetMessage("NameNoHtml", "Name must not contain HTML or markup."));
    }

    /// <summary>
    /// Validates that a name is either null or a valid value: not exceeding a configurable maximum length, not containing HTML, and not empty if provided.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="localizationService">The localization service for messages.</param>
    /// <param name="maxLength">The maximum allowed length for the name.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, string?> CustomIsValidNullableName<T>(
        this IRuleBuilder<T, string?> ruleBuilder,
        ILocalizationService localizationService,
        int maxLength = 30)
    {
        return ruleBuilder
            .Must(name => name == null || !string.IsNullOrWhiteSpace(name))
            .WithMessage(
                localizationService.GetMessage("NameRequiredIfProvided", "Name must not be empty if provided."))
            .Must(name => name == null || name.Length <= maxLength)
            .WithMessage(
                string.Format(
                    localizationService.GetMessage("NameMaxLength", "Name must not exceed {0} characters."),
                    localizationService.TranslateNumber(maxLength)
                ))
            .Must(name => name == null || !ContainsHtml(name))
            .WithMessage(localizationService.GetMessage("NameNoHtml", "Name must not contain HTML or markup."));
    }

    /// Validates that a phone number is valid: contains only digits, has a length between configurable minimum and maximum values, and does not contain HTML.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="localizationService">The localization service for messages.</param>
    /// <param name="minLength">The minimum allowed length for the phone number.</param>
    /// <param name="maxLength">The maximum allowed length for the phone number.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, string> CustomIsValidPhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        ILocalizationService localizationService,
        int minLength = 7,
        int maxLength = 15)
    {
        return ruleBuilder
            .NotNull()
            .WithMessage(localizationService.GetMessage("PhoneRequired", "Phone number must not be empty."))
            .NotEmpty()
            .WithMessage(localizationService.GetMessage("PhoneRequired", "Phone number must not be empty."))
            .Matches($"^\\d{{{minLength},{maxLength}}}$")
            .WithMessage(
                string.Format(
                    localizationService.GetMessage("PhoneInvalidFormat",
                        "Phone number must contain only digits and be between {0} and {1} characters long."),
                    localizationService.TranslateNumber(minLength),
                    localizationService.TranslateNumber(maxLength)
                ))
            .Must(phone => !ContainsHtml(phone))
            .WithMessage(localizationService.GetMessage("PhoneNoHtml",
                "Phone number must not contain HTML or markup."));
    }

    /// <summary>
    /// Validates that a phone number is valid if provided: contains only digits, has a length between configurable minimum and maximum values, and does not contain HTML.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="localizationService">The localization service for messages.</param>
    /// <param name="minLength">The minimum allowed length for the phone number.</param>
    /// <param name="maxLength">The maximum allowed length for the phone number.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, string?> CustomIsValidPhoneNumberIfNotNull<T>(
        this IRuleBuilder<T, string?> ruleBuilder,
        ILocalizationService localizationService,
        int minLength = 7,
        int maxLength = 15)
    {
        return ruleBuilder
            .Must(phone => phone == null || !string.IsNullOrWhiteSpace(phone))
            .WithMessage(localizationService.GetMessage("PhoneRequiredIfProvided",
                "Phone number must not be empty if provided."))
            .Must(phone => phone == null || Regex.IsMatch(phone, $"^\\d{{{minLength},{maxLength}}}$"))
            .WithMessage(
                string.Format(
                    localizationService.GetMessage("PhoneInvalidFormat",
                        "Phone number must contain only digits and be between {0} and {1} characters long."),
                    localizationService.TranslateNumber(minLength),
                    localizationService.TranslateNumber(maxLength)
                ))
            .Must(phone => phone == null || !ContainsHtml(phone))
            .WithMessage(localizationService.GetMessage("PhoneNoHtml",
                "Phone number must not contain HTML or markup."));
    }


    /// <summary>
    /// Validates that a username is valid: not exceeding a configurable maximum length, not containing HTML, and not empty or null.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="localizationService">The localization service for messages.</param>
    /// <param name="minLength">The minimum allowed length for the username.</param>
    /// <param name="maxLength">The maximum allowed length for the username.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, string> CustomIsValidUsername<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        ILocalizationService localizationService,
        int minLength = 3,
        int maxLength = 50)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage(localizationService.GetMessage("UsernameRequired", "Username must not be empty."))
            .MinimumLength(minLength)
            .WithMessage(
                string.Format(
                    localizationService.GetMessage("UsernameMinLength",
                        "Username must be at least {0} characters long."),
                    localizationService.TranslateNumber(minLength)
                ))
            .MaximumLength(maxLength)
            .WithMessage(
                string.Format(
                    localizationService.GetMessage("UsernameMaxLength", "Username must not exceed {0} characters."),
                    localizationService.TranslateNumber(maxLength)
                ))
            .Must(value => !ContainsHtml(value))
            .WithMessage(localizationService.GetMessage("UsernameNoHtml", "Username must not contain HTML or markup."));
    }


    /// <summary>
    /// Validates that a password is valid: must contain at least one uppercase letter, one lowercase letter, one number, 
    /// not contain common sequences, have a configurable minimum and maximum length, not be empty, and not contain HTML.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="localizationService">The localization service for messages.</param>
    /// <param name="minLength">The minimum allowed length for the password.</param>
    /// <param name="maxLength">The maximum allowed length for the password.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, string> CustomIsValidPassword<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        ILocalizationService localizationService,
        int minLength = 7,
        int maxLength = 50)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage(localizationService.GetMessage("PasswordRequired", "Password must not be empty."))
            .MinimumLength(minLength)
            .WithMessage(
                string.Format(
                    localizationService.GetMessage("PasswordMinLength",
                        "Password must be at least {0} characters long."),
                    localizationService.TranslateNumber(minLength)
                ))
            .MaximumLength(maxLength)
            .WithMessage(
                string.Format(
                    localizationService.GetMessage("PasswordMaxLength", "Password must not exceed {0} characters."),
                    localizationService.TranslateNumber(maxLength)
                ))
            .Must(value => !ContainsHtml(value))
            .WithMessage(localizationService.GetMessage("PasswordNoHtml", "Password must not contain HTML or markup."))
            .Must(value => !ContainsCommonSequences(value))
            .WithMessage(localizationService.GetMessage("PasswordNoCommonSequences",
                "Password must not contain common password sequences."))
            .Matches("(?=.*[a-z])")
            .WithMessage(localizationService.GetMessage("PasswordLowercase",
                "Password must contain at least one lowercase letter."))
            .Matches("(?=.*[A-Z])")
            .WithMessage(localizationService.GetMessage("PasswordUppercase",
                "Password must contain at least one uppercase letter."))
            .Matches("(?=.*\\d)")
            .WithMessage(localizationService.GetMessage("PasswordNumber",
                "Password must contain at least one number."));
    }


    /// <summary>
    /// Checks if a string contains common password sequences.
    /// </summary>
    /// <param name="input">The string to check.</param>
    /// <returns>True if the string contains common sequences; otherwise, false.</returns>
    private static bool ContainsCommonSequences(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;

        var commonSequences = new[] { "123", "password123", "qwerty51", "abcdefghi", "admin123", "testpass123" };
        return commonSequences.Any(seq => input.ToLower().Contains(seq));
    }

    /// <summary>
    /// Checks if a string contains any personal information such as first name, last name, or username.
    /// </summary>
    /// <param name="input">The string to check.</param>
    /// <param name="firstName">The first name to check against.</param>
    /// <param name="lastName">The last name to check against.</param>
    /// <param name="username">The username to check against.</param>
    /// <returns>True if the string contains personal information; otherwise, false.</returns>
    public static bool ContainsPersonalInformation(string input, string firstName, string lastName, string username)
    {
        if (string.IsNullOrEmpty(input)) return false;
        var lowerInput = input.ToLower();
        return !string.IsNullOrEmpty(firstName) && lowerInput.Contains(firstName.ToLower()) ||
               !string.IsNullOrEmpty(lastName) && lowerInput.Contains(lastName.ToLower()) ||
               !string.IsNullOrEmpty(username) && lowerInput.Contains(username.ToLower());
    }

/// <summary>
/// Validates that a string contains only digits and does not contain HTML if it is not null.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <param name="ruleBuilder">The rule builder.</param>
/// <param name="localizationService">The localization service for messages.</param>
/// <returns>An IRuleBuilderOptions object for further configuration.</returns>
public static IRuleBuilderOptions<T, string?> ValidateDigitsOnlyIfNotNull<T>(
    this IRuleBuilder<T, string?> ruleBuilder, 
    ILocalizationService localizationService)
{
    return ruleBuilder
        .Must(value => string.IsNullOrEmpty(value) || (Regex.IsMatch(value, "^\\d+$") && !ContainsHtml(value)))
        .WithMessage(localizationService.GetMessage("ValueDigitsOnlyNoHtml", "Value must contain only digits and must not contain HTML or markup."));
}

/// <summary>
/// Validates that a string does not contain HTML if it is not null.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <param name="ruleBuilder">The rule builder.</param>
/// <param name="localizationService">The localization service for messages.</param>
/// <returns>An IRuleBuilderOptions object for further configuration.</returns>
public static IRuleBuilderOptions<T, string?> ValidateNoHtmlIfNotNull<T>(
    this IRuleBuilder<T, string?> ruleBuilder, 
    ILocalizationService localizationService)
{
    return ruleBuilder
        .Must(value => string.IsNullOrEmpty(value) || !ContainsHtml(value))
        .WithMessage(localizationService.GetMessage("ValueNoHtml", "Value must not contain HTML or markup."));
}

/// <summary>
/// Validates that a string does not contain HTML and is not empty or null.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <param name="ruleBuilder">The rule builder.</param>
/// <param name="localizationService">The localization service for messages.</param>
/// <returns>An IRuleBuilderOptions object for further configuration.</returns>
public static IRuleBuilderOptions<T, string> ValidateNoHtmlNotNull<T>(
    this IRuleBuilder<T, string> ruleBuilder, 
    ILocalizationService localizationService)
{
    return ruleBuilder
        .Must(value => !string.IsNullOrEmpty(value) && !ContainsHtml(value))
        .WithMessage(localizationService.GetMessage("ValueRequiredNoHtml", "Value must not be empty and must not contain HTML or markup."));
}




/// <summary>
/// Validates that a provided birth date is valid: not null, not in the future, and within a reasonable range (e.g., person is not older than the specified maximum age).
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <param name="ruleBuilder">The rule builder.</param>
/// <param name="localizationService">The localization service for messages.</param>
/// <param name="maxAge">The maximum allowed age in years (default is 150).</param>
/// <returns>An IRuleBuilderOptions object for further configuration.</returns>
public static IRuleBuilderOptions<T, DateOnly> CustomIsValidBirthDate<T>(
    this IRuleBuilder<T, DateOnly> ruleBuilder,
    ILocalizationService localizationService,
    int maxAge = 150)
{
    return ruleBuilder
        .Must(date => date <= DateOnly.FromDateTime(DateTime.UtcNow))
        .WithMessage(localizationService.GetMessage("BirthDateFuture", "Birth date must not be in the future."))
        .Must(date => date >= DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-maxAge)))
        .WithMessage(
            string.Format(
                localizationService.GetMessage("BirthDateMaxAge", "Birth date must not indicate an age older than {0} years."),
                localizationService.TranslateNumber(maxAge)
            ));
}

/// <summary>
/// Validates that a provided birth date is valid if not null: not in the future, and within a reasonable range (e.g., person is not older than the specified maximum age).
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <param name="ruleBuilder">The rule builder.</param>
/// <param name="localizationService">The localization service for messages.</param>
/// <param name="maxAge">The maximum allowed age in years (default is 150).</param>
/// <returns>An IRuleBuilderOptions object for further configuration.</returns>
public static IRuleBuilderOptions<T, DateOnly?> CustomIsValidBirthDateIfNotNull<T>(
    this IRuleBuilder<T, DateOnly?> ruleBuilder,
    ILocalizationService localizationService,
    int maxAge = 150)
{
    return ruleBuilder
        .Must(date => date == null || date <= DateOnly.FromDateTime(DateTime.UtcNow))
        .WithMessage(localizationService.GetMessage("BirthDateFuture", "Birth date must not be in the future."))
        .Must(date => date == null || date >= DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-maxAge)))
        .WithMessage(
            string.Format(
                localizationService.GetMessage("BirthDateMaxAge", "Birth date must not indicate an age older than {0} years."),
                localizationService.TranslateNumber(maxAge)
            ));
}


/// <summary>
/// Validates that a price is valid: must be a non-negative value.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <param name="ruleBuilder">The rule builder.</param>
/// <param name="localizationService">The localization service for messages.</param>
/// <returns>An IRuleBuilderOptions object for further configuration.</returns>
public static IRuleBuilderOptions<T, decimal> CustomValidatePrice<T>(
    this IRuleBuilder<T, decimal> ruleBuilder,
    ILocalizationService localizationService)
{
    return ruleBuilder
        .Must(price => price >= 0)
        .WithMessage(localizationService.GetMessage("PriceNonNegative", "Price must be a non-negative value."));
}

/// <summary>
/// Validates that a price is valid: must be null or a non-negative value.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <param name="ruleBuilder">The rule builder.</param>
/// <param name="localizationService">The localization service for messages.</param>
/// <returns>An IRuleBuilderOptions object for further configuration.</returns>
public static IRuleBuilderOptions<T, decimal?> CustomValidateNullablePrice<T>(
    this IRuleBuilder<T, decimal?> ruleBuilder,
    ILocalizationService localizationService)
{
    return ruleBuilder
        .Must(price => price == null || price >= 0)
        .WithMessage(localizationService.GetMessage("PriceNullableNonNegative", "Price must be null or a non-negative value."));
}

/// <summary>
/// Validates that an ID is valid: must be a positive integer.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <param name="ruleBuilder">The rule builder.</param>
/// <param name="localizationService">The localization service for messages.</param>
/// <returns>An IRuleBuilderOptions object for further configuration.</returns>
public static IRuleBuilderOptions<T, int> CustomValidateId<T>(
    this IRuleBuilder<T, int> ruleBuilder,
    ILocalizationService localizationService)
{
    return ruleBuilder
        .GreaterThan(0)
        .WithMessage(localizationService.GetMessage("IdPositive", "ID must be a positive integer."));
}

/// <summary>
/// Validates that an ID is valid if not null: must be a positive integer.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <param name="ruleBuilder">The rule builder.</param>
/// <param name="localizationService">The localization service for messages.</param>
/// <returns>An IRuleBuilderOptions object for further configuration.</returns>
public static IRuleBuilderOptions<T, int?> CustomValidateNullableId<T>(
    this IRuleBuilder<T, int?> ruleBuilder,
    ILocalizationService localizationService)
{
    return ruleBuilder
        .Must(id => id == null || id > 0)
        .WithMessage(localizationService.GetMessage("IdNullablePositive", "ID must be null or a positive integer."));
}
    /// <summary>
    /// Checks if a string contains HTML or markup language.
    /// </summary>
    /// <param name="input">The string to check.</param>
    /// <returns>True if the string contains HTML or markup; otherwise, false.</returns>
    private static bool ContainsHtml(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;

        var sanitizer = new HtmlSanitizer();
        var sanitizedOutput = sanitizer.Sanitize(input);

        return !string.Equals(input, sanitizedOutput, StringComparison.Ordinal);
    }
}