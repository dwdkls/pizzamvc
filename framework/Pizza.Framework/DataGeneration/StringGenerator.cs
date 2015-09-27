using System;
using System.Text;

namespace Pizza.Framework.DataGeneration
{
    public static class StringGenerator
    {
        public static string GenerateRandomString(int length)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= length / 36; i++)
            {
                sb.Append(Guid.NewGuid());
            }

            return sb.ToString(0, length);
        }
    }
}