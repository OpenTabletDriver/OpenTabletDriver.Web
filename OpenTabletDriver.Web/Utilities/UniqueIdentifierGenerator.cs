using System;
using System.Text;

namespace OpenTabletDriver.Web.Utilities
{
    public static class UniqueIdentifierGenerator
    {
        private const string CHARS = "abcdefghijklmnopqrstuvwxyz";
        private static readonly Random Random = new Random();

        public static string RandomString(int length)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                var charIndex = Random.Next(0, CHARS.Length);
                sb.Append(CHARS[charIndex]);
            }

            return sb.ToString();
        }
    }
}