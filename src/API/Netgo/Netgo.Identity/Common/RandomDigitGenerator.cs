using System.Security.Cryptography;

namespace Netgo.Identity.Common
{
    internal static class RandomDigitGenerator
    {
        public static string Generate(int length)
        {
            byte[] bytes = new byte[2];
            RandomNumberGenerator.Fill(bytes);
            int number = BitConverter.ToUInt16(bytes, 0) % (int)Math.Pow(10, length);
            return number.ToString($"D{length}");
        }
    }
}
