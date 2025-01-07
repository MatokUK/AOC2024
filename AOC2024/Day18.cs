using System;
using System.Text.RegularExpressions;

namespace AOC2024
{
    using AOC2024.Day18Classes;

    internal class Day18: IAdventSolution
    {            
        public (long, long) Execute(string[] lines)
        {
            int bytes = lines.Length > 30 ? 1024 : 12;

            var corrupted = ReadMemory(lines);

            Memory memory = new Memory(corrupted.GetRange(0, bytes));

            Path shortestPath = memory.CalculatePath();

            // part II
            bool hasSolution = false;
            int nextCorrupted = lines.Length > 30 ? 1024 : 12;
            do
            {
                memory.AddCorruption(corrupted.ElementAt(nextCorrupted));
                Console.WriteLine($"[{nextCorrupted}] Added corrupted memory " + corrupted.ElementAt(nextCorrupted));                

               /* if (nextCorrupted > 2956)
                {
                    Console.WriteLine(memory);
                }*/

                if (!shortestPath.Contains(corrupted.ElementAt(nextCorrupted)))
                {
                    hasSolution = true;

                    /*  if (nextCorrupted > 2956)
                      {
                          Console.WriteLine("Shortest " + shortestPath.Length);
                          // wrong 26, 66

                          Console.WriteLine(memory);
                      }*/
                    ++nextCorrupted;

                    continue;
                }

                shortestPath = memory.CalculatePath();                

                hasSolution = shortestPath != null;

                if (hasSolution)
                {
                    Console.WriteLine("Shortest " + shortestPath.Length);
                    // wrong 26, 66

                   // Console.WriteLine(memory);
                }
                 ++nextCorrupted;

            } while (hasSolution);
            // 26, 50 -= wrong


            return (shortestPath.Length, 0);
        }

        private List<(int, int)> ReadMemory(string[] lines)
        {
            List<(int, int)> corrupted = new();

            foreach (string line in lines)
            {
                var splitted = line.Split(",");

                corrupted.Add((int.Parse(splitted[0]), int.Parse(splitted[1])));
            }

            return corrupted;
        }
    }
}
