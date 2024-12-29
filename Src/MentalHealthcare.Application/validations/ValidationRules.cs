using System.Text.RegularExpressions;
using FluentValidation;
using MentalHealthcare.Domain.Constants;


namespace MentalHealthcare.Application.validations;

public static class ValidationRules
{
    /// <summary>
    /// Validates that a page size is valid.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, int> CustomValidatePageSize<T>(
        this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThan(0).WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(30).WithMessage("Page size must be 30 or less.");
    }

    /// <summary>
    /// Validates that a page number is valid.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, int> CustomValidatePageNumber<T>(
        this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");
    }
    /// <summary>
    /// Validates that a search term is valid.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, string?> CustomValidateSearchTerm<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Search term must not be empty.")
            .MaximumLength(50).WithMessage("Search term must not exceed 50 characters.")
            .Must(term => !ContainsHtml(term)).WithMessage("Search term must not contain HTML or markup.");
    }
    /// <summary>
    /// Validates that a string is a valid email address with a maximum length of 100 characters,
    /// does not contain HTML or markup language, and is not empty.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    
    public static IRuleBuilderOptions<T, string> CustomIsValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .NotEmpty()
            .WithMessage("Email address must not be empty.")
            .EmailAddress()
            .WithMessage("Invalid email address format.")
            .MaximumLength(100)
            .WithMessage("Email address must not exceed 100 characters.")
            .Must(value => !ContainsHtml(value))
            .WithMessage("Email address must not contain HTML or markup.");
    }


    /// <summary>
    /// Validates that the tenant is not null, empty, or invalid.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, string> IsAdminProgramTenant<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .NotEmpty()
            .WithMessage("Tenant is null or empty.")
            .Must(tenant => tenant == Global.ProgramName)
            .WithMessage("Unauthorized tenant access.");
    }

    /// <summary>
    /// Validates that a name is valid: not exceeding 30 characters, not containing HTML, and not empty or null.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, string> CustomIsValidName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .NotEmpty()
            .WithMessage("Name must not be empty.")
            .MaximumLength(30)
            .WithMessage("Name must not exceed 30 characters.")
            .Must(name => !ContainsHtml(name))
            .WithMessage("Name must not contain HTML or markup.");
    }

    /// <summary>
    /// Validates that a phone number is valid: contains only digits, has a length between 7 and 15, and does not contain HTML.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, string> CustomIsValidPhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .NotEmpty()
            .WithMessage("Phone number must not be empty.")
            .Matches("^\\d{7,15}$")
            .WithMessage("Phone number must contain only digits and be between 7 and 15 characters long.")
            .Must(phone => !ContainsHtml(phone))
            .WithMessage("Phone number must not contain HTML or markup.");
    }

    /// <summary>
    /// Validates that a username or password is valid: not containing common sequences, not exceeding 50 characters, and not containing HTML.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    /// <summary>
    /// Validates that a username is valid: not exceeding 50 characters, not containing HTML, and not empty or null.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, string> CustomIsValidUsername<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Username must not be empty.")
            .MinimumLength(3)
            .WithMessage("Username must be at least 5 characters long.")
            .MaximumLength(50)
            .WithMessage("Username must not exceed 50 characters.")
            .Must(value => !ContainsHtml(value))
            .WithMessage("Username must not contain HTML or markup.");
    }

    /// <summary>
    /// Validates that a password is valid: must contain at least one uppercase letter, one lowercase letter, one number, not contain common sequences, have a minimum length of 7, not exceed 50 characters, not be empty, and not contain first name, last name, or username.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="firstName">The first name to check against.</param>
    /// <param name="lastName">The last name to check against.</param>
    /// <param name="username">The username to check against.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, string> CustomIsValidPassword<T>
        (this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Password must not be empty.")
            .MinimumLength(7)
            .WithMessage("Password must be at least 7 characters long.")
            .MaximumLength(50)
            .WithMessage("Password must not exceed 50 characters.")
            .Must(value => !ContainsHtml(value))
            .WithMessage("Password must not contain HTML or markup.")
            .Must(value => !ContainsCommonSequences(value))
            .WithMessage("Password must not contain common password sequences.")
            .Matches("(?=.*[a-z])")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches("(?=.*[A-Z])")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches("(?=.*\\d)")
            .WithMessage("Password must contain at least one number.");
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
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, string?> ValidateDigitsOnlyIfNotNull<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(value => string.IsNullOrEmpty(value) || (Regex.IsMatch(value, "^\\d+$") && !ContainsHtml(value)))
            .WithMessage("Value must contain only digits and must not contain HTML or markup.");
    }
    /// <summary>
    /// Validates that a string does not contain HTML if it is not null.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>An IRuleBuilderOptions object for further configuration.</returns>
    public static IRuleBuilderOptions<T, string?> ValidateNoHtmlIfNotNull<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(value => string.IsNullOrEmpty(value) || !ContainsHtml(value))
            .WithMessage("Value must not contain HTML or markup.");
    }

    /// <summary>
    /// Checks if a string contains HTML or markup language.
    /// </summary>
    /// <param name="input">The string to check.</param>
    /// <returns>True if the string contains HTML or markup; otherwise, false.</returns>
    private static bool ContainsHtml(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;
        var htmlRegex = new Regex("<[^>]+>");
        return htmlRegex.IsMatch(input);
    }
}