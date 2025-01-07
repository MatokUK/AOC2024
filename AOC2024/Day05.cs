using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2024
{
    internal class Day05 : IAdventSolution
    {
        private Dictionary<(int, int), bool> pageOrdering;

        public (long, long) Execute(string[] lines)
        {
            pageOrdering = ReadPageOrdering(lines)
                .ToDictionary(
                 x => (x.Item1, x.Item2),
                 x => true
            );

            var updates = ReadUpdates(lines);

            long partA = 0;
            long partB = 0;
            foreach (var update in updates) 
            {
                if (IsOrdered(update))
                {
                    partA += update.ElementAt(update.Count / 2);
                }
                else
                {
                    var orderedUpdate = DoOrdering(update);
                    partB += orderedUpdate.ElementAt(update.Count / 2);
                }
            }

            return (partA ,partB);
        }

        private bool IsOrdered(List<int> sequence)
        {
            if (sequence.Count == 1)
            {
                return true;
            }

            var key = (sequence.ElementAt(0), sequence.ElementAt(1));

            if (!pageOrdering.ContainsKey(key))
            {
                return false;
            }

            return IsOrdered(sequence.Skip(1).ToList());
        }

        private List<int> DoOrdering(List<int> sequence)
        {
            List<int> orderedSequence = new(sequence);

            var sub = pageOrdering.Where(x => sequence.Contains(x.Key.Item1) && sequence.Contains(x.Key.Item2)).ToList();

            while (!IsOrdered(orderedSequence))
            {
                for (int i = 0; i < orderedSequence.Count - 1; i++)
                {
                    if (!IsOrdered(new List<int>() { orderedSequence[i], orderedSequence[i + 1] }))
                    {
                        var temp = orderedSequence[i];
                        orderedSequence[i] = orderedSequence[i + 1];
                        orderedSequence[i + 1] = temp;
                    }
                }
            }

            return orderedSequence;
        }

        private List<(int, int)> ReadPageOrdering(string[] lines)
        {
            List<(int, int)> result = new();

            foreach (var line in lines)
            {
                if (line.Contains("|"))
                {
                    var pages = line.Split('|');
                    result.Add((int.Parse(pages[0]), int.Parse(pages[1])));
                }               
            }

            return result;
        }

        private List<List<int>> ReadUpdates(string[] lines)
        {
            List<List<int>> result = new();

            foreach (var line in lines)
            {
                if (line.Contains(","))
                {
                    var update = line.Split(',')
                        .Select(int.Parse)
                        .ToList();
                    result.Add(update);
                }
            }

            return result;
        }
    }
}
