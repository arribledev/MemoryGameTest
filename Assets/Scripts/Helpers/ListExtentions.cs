using System;
using System.Collections.Generic;
using System.Linq;

namespace MemoryGame.Helpers
{
    public static partial class ListExtentions
    {
        public static List<T> RandomOrder<T>(this IReadOnlyList<T> list, Random random)
        {
            var indexes = RandomNonoverlappedIndices(list.Count, list.Count, random);
            var result = new List<T>(list.Count);
            for (int i = 0; i < list.Count; i++) { result.Add(default(T)); }
            for (var i = 0; i < indexes.Length; i++)
            {
                var index = indexes[i];
                result[i] = list[index];
            }
            return result;
        }

        public static List<T> RandomElements<T>(this List<T> list, Random random, int count)
        {
            return RandomNonoverlappedIndices(list.Count, count, random).Select(i => list[i]).ToList();
        }

        // max is exclusive index max, count is number of random indexes returned
        public static int[] RandomNonoverlappedIndices(int max, int count, Random random)
        {
            if (max < count)
            {
                int[] r = new int[max];
                for (int i = 0; i < max; i++)
                {
                    r[i] = i;
                }
                return r;
            }
            int[] result = new int[count];
            var range = Enumerable.Range(0, max).ToList();
            for (int i = 0; i < count; ++i)
            {
                int randIndex = random.Next(0, max - i);
                int rand = range[randIndex];
                result[i] = rand;
                range[randIndex] = range[max - i - 1];
            }

            return result;
        }
    }
}
