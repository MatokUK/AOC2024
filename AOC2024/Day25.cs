using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2024
{
    internal class Day25 : IAdventSolution
    {
        public (long, long) Execute(string[] lines)
        {
            long partA = 0;

            var locks = ReadLocks(lines);
            var keys = ReadKeys(lines);

            foreach (var a in locks)
            {
                foreach (var b in keys)
                {
                    if (a.Zip(b, (x, y) => x + y).All(x => x < 6))
                    {
                        partA++;
                    }
                }
            }

            return (partA, 0);
        }

        private List<List<int>> ReadLocks(string[] lines)
        {
            return ReadOnly(lines, "#####");
        }

        private List<List<int>> ReadKeys(string[] lines)
        {
            return ReadOnly(lines, ".....");
        }

        private List<List<int>> ReadOnly(string[] lines, string pattern)
        {
            var input = lines.Where(x => x.Length > 0)              // skip blank lines in input
                .Select((item, index) => new { item, index })       // add indexes
                .GroupBy(x => x.index / 7)                          // create groups to keep "lock"/"key" together
                .Select(group => group.Select(x => x.item));        // remove indexes

            var locks = input.Where(group => group.First().Equals(pattern))  // filter only "keys" or "locks"
                .Select(group => group.Skip(1).SkipLast(1))
                .ToList();

            List<List<int>> result = new();
            foreach (var item in locks)
            {
                var n = Enumerable.Range(0, 5)
                    .Select(columnIndex => item.Count(x => x[columnIndex] == '#'))
                    .ToList();
                result.Add(n);
            }

            return result;
        }
    }
}
