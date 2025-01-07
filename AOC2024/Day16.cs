using System.Text.RegularExpressions;

namespace AOC2024
{
    using AOC2024.Day16Classes;

    internal class Day16: IAdventSolution
    {
        public (long, long) Execute(string[] lines)
        {
            Maze maze = ReadMaze(lines);
            long partA = 0;

            maze.Go();
            partA = maze.Score;

            maze.GoAllPaths(partA);



            return (partA, maze.Tiles);
        }

        private Maze ReadMaze(string[] lines)
        {
            return new Maze(lines.Select(x => x).ToList());
        }
    }
}
