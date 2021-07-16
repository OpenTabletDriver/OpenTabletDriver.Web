using System;
using System.Linq;
using System.Text;

namespace OpenTabletDriver.Web.Utilities
{
    public static class UniqueIdentifierGenerator
    {
        private const string chars = "abcdefghijklmnopqrstuvwxyz";
        private static readonly Random _random = new Random();

        public static string RandomString(int length)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                var charIndex = _random.Next(0, chars.Length);
                sb.Append(chars[charIndex]);
            }

            return sb.ToString();
        }
    }
}