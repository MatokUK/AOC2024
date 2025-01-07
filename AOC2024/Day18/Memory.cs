using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024.Day18Classes
{
    class Memory
    {
        private List<(int, int)> corrupted;
        private Dictionary<(int, int), int> visited = new();
        private SortedSet<Path> paths = new();
        private int maxX;
        private int maxY;

        public Memory(List<(int x, int y)> corrupted)
        {
            this.maxX = corrupted.Select(a => a.x).Max();
            this.maxY = corrupted.Select(a => a.y).Max();
            this.corrupted = corrupted;
        }

        public void AddCorruption((int, int) corruptedSector)
        {
            visited.Clear();
            paths.Clear();

            corrupted.Add(corruptedSector);
        }

        public Path CalculatePath()
        {
            visited.Add((0, 0), 0);
            paths.Add(Path.FromXY(0, 0));

            int c = 0;
            do
            {
                var p = paths.Max();
                var isThere = paths.Remove(p);
                if (!isThere)
                {
                    Console.WriteLine("Cannot remove element!");
                }

                var nextMoves = PossibleNextMoves(p.last);
                foreach (var nextMove in nextMoves)
                {
                    if (this.corrupted.Contains(nextMove))
                    {
                        continue;
                    }
                    var nextPath = p.FromNextMove(nextMove.x, nextMove.y);

                    if (NotVisited(nextPath))
                    {
                        visited.Add(nextPath.last, nextPath.Length);
                        paths.Add(nextPath);

                        if (c % 100 == 0)
                        {
                         //   Console.WriteLine(this);
                        }
                    }
                }
                c++;


            } while (GetFinishedPath() == null);

            return GetFinishedPath();
        }

        private List<(int x, int y)> PossibleNextMoves((int x, int y) currentPos)
        {
            List<(int x, int y)> next = new() {
                    (currentPos.x + 1, currentPos.y),
                    (currentPos.x - 1, currentPos.y),
                    (currentPos.x, currentPos.y + 1),
                    (currentPos.x, currentPos.y - 1)
                };

            return next.Where(p => p.x >= 0 && p.y >= 0 && p.x <= maxX && p.y <= maxY).ToList();
        }

        private bool NotVisited(Path path)
        {
            var score = visited.GetValueOrDefault(path.last, int.MaxValue);

            return score > path.Length;
        }

        private Path GetFinishedPath()
        {
            foreach (var path in paths)
            {
                if (path.last == (maxX, maxY))
                {
                    return path;
                }
            }

            return null;
        }

        public override string ToString()
        {
            string output = "";

            for (int y = 0; y <= maxY; y++)
            {
                for (int x = 0; x <= maxX; x++)
                {
                    if (visited.ContainsKey((x, y)))
                    {
                        output += 'x';//visited.GetValueOrDefault((x, y), 0) % 10;
                    }
                    else if (corrupted.Contains((x, y)))
                    {
                        output += "#";
                    }
                    else
                    {
                        output += ".";
                    }

                }
                output += "\n";
            }

            return output;
        }
    }
}
