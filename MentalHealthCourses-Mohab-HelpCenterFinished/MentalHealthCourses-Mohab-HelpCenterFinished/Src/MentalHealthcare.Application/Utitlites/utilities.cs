namespace MentalHealthcare.Application.Utitlites;

public static class Utilities
{
    public static bool IsSubsequence(string subsequence, string target)
    {
        target = target.ToLower();
        subsequence = subsequence.ToLower();
        int j = 0;
        for (int i = 0; i < target.Length && j < subsequence.Length; i++)
        {
            if (target[i] == subsequence[j])
            {
                j++;
            }
        }
        return j == subsequence.Length;
    }
}