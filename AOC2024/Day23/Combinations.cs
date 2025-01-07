using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024.Day23Classes
{
    internal class Combinations
    {
        static List<List<string>> Generate(List<string> list, int length)
        {
            if (length == 0)
            {
                return new List<List<string>> { new List<string>() };
            }

            if (list.Count == 0)
            {
                return new List<List<string>>();
            }

            string firstElement = list[0];
            var remainingList = list.Skip(1).ToList();

            // Include the first element in combinations
            var combinationsWithFirst = Generate(remainingList, length - 1)
                .Select(combination => new List<string> { firstElement }.Concat(combination).ToList())
                .ToList();

            // Exclude the first element from combinations
            var combinationsWithoutFirst = Generate(remainingList, length);

            // Combine both results
            return combinationsWithFirst.Concat(combinationsWithoutFirst).ToList();
        }
    }
}
