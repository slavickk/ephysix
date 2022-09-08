using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary
{
    public static class LinqExtender
    {
        public static IEnumerable<int> IndexesWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            int index = 0;
            foreach (T element in source)
            {
                if (predicate(element))
                {
                    yield return index;
                }
                index++;

            }

        }
        public static int IndexWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            int index = 0;
            foreach (T element in source)
            {
                if (predicate(element))
                {
                    return index;
                }
                index++;

            }
            return -1;

        }
    }
}
