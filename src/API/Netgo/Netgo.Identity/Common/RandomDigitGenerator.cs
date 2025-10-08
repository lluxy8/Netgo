using System.Security.Cryptography;

namespace Netgo.Identity.Common
{
    internal static class RandomDigitGenerator
    {
        public static string Generate(int length)
        {
            if (length <= 0)
                throw new ArgumentException("Length must be greater than 0.", nameof(length));

            char[] digits = new char[length];
            byte[] buffer = new byte[1];

            for (int i = 0; i < length; i++)
            {
                do
                {
                    RandomNumberGenerator.Fill(buffer);
                    int digit = buffer[0] % 10; // 0-9
                    if (digit != 0)
                    {
                        digits[i] = (char)('0' + digit);
                        break;
                    }
                } while (true);
            }

            return new string(digits);
        }
    }
}
