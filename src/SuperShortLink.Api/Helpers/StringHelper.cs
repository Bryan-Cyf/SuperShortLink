using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SuperShortLink.Api.Helpers
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static T ToEnum<T>(this string value) where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return (T)Enum.Parse(typeof(T), value);
        }

        public static T ToEnum<T>(this string value, bool ignoreCase) where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        public static string ToMd5(this string str)
        {
            using MD5 mD = MD5.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] array = mD.ComputeHash(bytes);
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array2 = array;
            foreach (byte b in array2)
            {
                stringBuilder.Append(b.ToString("X2"));
            }

            return stringBuilder.ToString();
        }
    }
}
