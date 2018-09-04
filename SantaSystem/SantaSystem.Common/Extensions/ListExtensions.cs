using System;
using System.Collections.Generic;

namespace SantaSystem.Common.Extensions
{
    public static class ListExtensions
    {
        private static Random rand = new Random();

        public static List<T> Shuffle<T>(List<T> list)
        {
            var elements = new List<T>(list);

            for (int i = 0; i < elements.Count; i++)
            {
                int newIndex = rand.Next(0, elements.Count);

                T temp = elements[i];
                elements[i] = elements[newIndex];
                elements[newIndex] = temp;
            }

            return elements;
        }
    }
}
