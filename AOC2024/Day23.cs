using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2024
{
    using AOC2024.Day23Classes;

    internal class Day23 : IAdventSolution
    {
        private List<(string, string)> connectedNodes;
        private List<string> singleNodes;
        class NodeListComparer : IEqualityComparer<List<string>>
        {
            public bool Equals(List<string> x, List<string> y)
            {
                if (x == null && y == null)
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                return x.OrderBy(s => s).SequenceEqual(y.OrderBy(s => s));
            }

            public int GetHashCode(List<string> obj)
            {
                // Generate a hash code based on the sorted list
                return string.Join(",", obj.OrderBy(s => s)).GetHashCode();
            }
        }

        public (long, long) Execute(string[] lines)
        {
            var connections = ReadConnections(lines);
            connectedNodes = ReadConnections(lines);
            //List<string> nodes = connections.SelectMany(x => new[] { x.Item1, x.Item2 }).ToList();
            singleNodes = connectedNodes.SelectMany(x => new[] { x.Item1, x.Item2 }).ToList();
            HashSet<List<string>> connectionOf3 = new(new NodeListComparer());
            long partA = 0;
            long partB = 0;


            Console.WriteLine("Computing....");
            foreach (var item in connections)
            {
                var thirdNodes = GetTrirdNode(item);                 
                foreach (var trirdNode in thirdNodes)
                {
                    var pod = new List<string> { item.Item1, item.Item2, trirdNode };
                    bool wasAdded = connectionOf3.Add(pod);
                    if (wasAdded && pod.Any(x => x.Contains("t")))
                    {
                        partA++;
                    }
                    //  Console.WriteLine(item + "   * " + trirdNode);
                }
            }

            foreach (var pod in connectionOf3)
            {
                Console.WriteLine(string.Join("-", pod));
            }

            Console.WriteLine("Total: " + connectionOf3.Count);

           // return (connectionOf3.Select(nodes => string.Join("", nodes)).Where(x => x.Contains('t')).Count(), partB);
           return (partA, partB);
        }

        private List<string> GetTrirdNode((string, string) pair)
        {
            List<string> result = new();
          //  List<string> nodes = connections.SelectMany(x => new[] { x.Item1, x.Item2 }).ToList();
            foreach (var node in singleNodes)
            {
                if (node == pair.Item1 || node == pair.Item2)
                {
                    continue;
                }
                var xx = connectedNodes.Where(x => x.Item1 == node || x.Item2 == node).Select(x => x.Item1 == node ? x.Item2 : x.Item1).ToList();
                var yy = xx.Where(x => x == pair.Item1 || x == pair.Item2).ToList();
                if (yy.Count == 2)
                {
                    result.Add(node);
                }
            //    Console.WriteLine(yy.Count);
            }


            // your answer is too high - 2660
            return result;
        }

        private List<(string, string)> ReadConnections(string[] lines)
        {
            return lines
                .Select(line => line.Split('-'))                
                .Select(parts =>
                {
                    return (parts[0], parts[1]);
                })
                .ToList();
        }
    }
}
