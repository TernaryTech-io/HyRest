using System.Security.Cryptography;
using System.Text;

namespace HyRest.Utilities;

public class PasswordGenerator
{
    private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
    private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string NumericChars = "0123456789";
    private const string SpecialChars = "!@#$%^&*()-_=+[]{}|;:,.<>?";

    public static string GenerateRandomPassword(int length = 14,
                                                bool useLowercase = true,
                                                bool useUppercase = true,
                                                bool useNumeric = true,
                                                bool useSpecial = true)
    {
        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Password length must be greater than zero.");
        }

        var charPool = new StringBuilder();
        if (useLowercase) charPool.Append(LowercaseChars);
        if (useUppercase) charPool.Append(UppercaseChars);
        if (useNumeric) charPool.Append(NumericChars);
        if (useSpecial) charPool.Append(SpecialChars);

        if (charPool.Length == 0)
        {
            throw new InvalidOperationException("At least one character type must be used for password generation.");
        }

        var password = new char[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] randomBytes = new byte[length];
            rng.GetBytes(randomBytes); // Fill the array with cryptographically strong random bytes

            for (int i = 0; i < length; i++)
            {
                password[i] = charPool[randomBytes[i] % charPool.Length];
            }
        }

        return new string(password);
    }
}
