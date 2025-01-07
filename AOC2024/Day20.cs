using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2024
{
    using Day20Classes;
    using System.Diagnostics;

    internal class Day20 : IAdventSolution
    {
        public (long, long) Execute(string[] lines)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var maze = ReadMaze(lines);
            var path = maze.GetPath();

            // var cheats = maze.GetCheats(path, 2);

            var cheats20 = maze.GetCheats(path, 20);

            /*foreach (var kvp in cheats20)
            {
                if (kvp.Value >= 100)
                {
                    Console.Write(maze.Debug((kvp.Key.Item1, kvp.Key.Item2), (kvp.Key.Item3, kvp.Key.Item4)));
                    break;
                }
                
            }*/

            var partB = maze.GetCheatsFast(path, 20);
            stopwatch.Stop();
            Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");

            return (0/*cheats.Where(pair => pair.Value >= 100).Count()*/, cheats20.Where(pair => pair.Value >= 100).Count());
            //return (0, partB);
        }

        private Maze ReadMaze(string[] lines)
        {
            return new Maze(lines.Select(x => x.ToString()).ToList());
        }
    }
}
