using Microsoft.AspNetCore.Http;

namespace MentalHealthcare.Application.Common;

public static class ConversionUtility
{
    public static string BitsToSizeString(string bitsString)
    {
        // Parse the input string to a long
        if (!long.TryParse(bitsString, out long bits))
        {
            throw new BadHttpRequestException("يجب أن يكون الإدخال عددًا صحيحًا طويلًا صالحًا.");
        }

        double megabytes = bits / 8.0 / 1024.0 / 1024.0; // Convert bits to megabytes

        if (megabytes >= 1024)
        {
            double gigabytes = megabytes / 1024.0; // Convert to gigabytes
            return $"{gigabytes:F2} GB"; // Format to 2 decimal places
        }
        else
        {
            return $"{megabytes:F2} MB"; // Format to 2 decimal places
        }
    }

    public static string ConvertSeconds(string second)
    {
        if (!int.TryParse(second, out int seconds))
        {
            throw new BadHttpRequestException("يجب أن يكون الإدخال عددًا صحيحًا طويلًا صالحًا.");
        }

        int minutes = seconds / 60;
        int remainingSeconds = seconds % 60;
        int hours = minutes / 60;
        minutes %= 60; // Remaining minutes after calculating hours

        if (hours > 0)
        {
            return $"{hours} hour{(hours > 1 ? "s" : "")} and {minutes} minute{(minutes != 1 ? "s" : "")}";
        }
        else
        {
            return
                $"{minutes} minute{(minutes != 1 ? "s" : "")} and {remainingSeconds} second{(remainingSeconds != 1 ? "s" : "")}";
        }
    }
}