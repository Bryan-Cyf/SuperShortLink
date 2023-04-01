using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperShortLink.Helpers
{
    internal class EnumerableHelper
    {
    }
    public static class ListExtension
    {
        private static readonly Random random = new Random();

        public static List<T> Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }

}
