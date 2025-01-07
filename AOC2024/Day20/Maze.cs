using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024.Day20Classes
{
    class Maze
    {
        private List<(int r, int c)> walls = new();
        private (int r, int c) start;
        private (int r, int c) end;
        private int maxRow;
        private int maxCol;

        public Maze(List<string> lines)
        {
            maxRow = lines.Count;
            maxCol = lines[0].Length;

            for (int r = 0; r < lines.Count; r++)
            {
                for (int c = 0; c < lines[r].Length; c++)
                {
                    if (lines[r][c] == '#')
                    {
                        walls.Add((r, c));
                    }
                    else if (lines[r][c] == 'S')
                    {
                        this.start = (r, c);
                    }
                    else if (lines[r][c] == 'E')
                    {
                        this.end = (r, c);
                    }
                }
            }
        }

        public Path GetPath()
        {
            Path path = new Path(start);

            while (path.Position != end)
            {
                path.MoveTo(NextMove(path));
            }

            return path;
        }

        public Dictionary<(int, int, int, int), int> GetCheats(Path path, int length)
        {
            Dictionary<(int, int, int, int), int> cheats = new();

            for (int i = 0; i < path.Length; i++)
            {
                var pos = path.CoordsAt(i);
                var currentCheats = path.GetVisitedFromIndex(i + 1)
                    .Select((x, idx) => new 
                        { 
                            manhantanDistance = Math.Abs(x.Item1 - pos.Item1) + Math.Abs(x.Item2 - pos.Item2), 
                            distance = idx + 1, 
                            to = x 
                        }
                    )
                    .Where(x => x.manhantanDistance < x.distance && x.manhantanDistance <= length)
                    .ToDictionary(x => (pos.Item1, pos.Item2, x.to.Item1, x.to.Item2), x => x.distance - x.manhantanDistance)
                    .ToList();

                foreach (var kvp in currentCheats)
                {
                    cheats[kvp.Key] = kvp.Value;
                }
            }            
            
            return cheats;
        }

        public int GetCheatsFast(Path path, int length)
        {
            int cheats = 0;

            for (int i = 0; i < path.Length; i++)
            {
                var pos = path.CoordsAt(i);
                cheats += path.GetVisitedFromIndex(i + 1)
                    .Select((x, idx) => new
                    {
                        manhantanDistance = Math.Abs(x.Item1 - pos.Item1) + Math.Abs(x.Item2 - pos.Item2),
                        distance = idx + 1,
                        to = x
                    }
                    )
                    .Where(x => x.distance - x.manhantanDistance >= 100 && x.manhantanDistance <= length)
                    .Count();                
            }

            return cheats;
        }

        private (int, int) NextMove(Path path)
        {
            var actualPosition = path.Position;
            var allPos = new List<(int, int)>()
            {
                (actualPosition.r + 1, actualPosition.c),
                (actualPosition.r - 1, actualPosition.c),
                (actualPosition.r, actualPosition.c + 1),
                (actualPosition.r, actualPosition.c - 1),
            };

            allPos = allPos.Where(x => !walls.Contains(x) && !path.IsVisited(x)).ToList();

            if (allPos.Count != 1)
            {
                throw new Exception("possible next move error");
            }

            return allPos.First();
        }

        private List<(int, int)> Neighbours((int r, int c) position)
        {
            return new List<(int, int)>()
            {
                (position.r + 1, position.c),
                (position.r - 1, position.c),
                (position.r, position.c + 1),
                (position.r, position.c - 1),
            };
        }


        private List<(int, int)> GetNearestWalls(Path path)
        {
            List<(int, int)> result = new();
            foreach (var wall in walls)
            {
                if (wall.r == 0 || wall.c == 0 || wall.r == maxRow - 1 || wall.c == maxCol - 1)
                {
                    continue;
                }

                var cells = Neighbours(wall);
                if (cells.Any(cell => path.IsVisited(cell)))
                {
                    result.Add(wall);
                }

            }
            
            return result;
        }

        public string Debug((int, int) a, (int, int) b)
        {
            string output = "";

            for (int y = 0; y <= maxRow; y++)
            {
                for (int x = 0; x <= maxCol; x++)
                {
                    if (walls.Contains((x, y)))
                    {
                        output += '#';//visited.GetValueOrDefault((x, y), 0) % 10;
                    }
                    else if (a == (x, y))
                    {
                        output += "1";
                    }
                    else if (b == (x, y))
                    {
                        output += "2";
                    }
                    else if (start == (x, y))
                    {
                        output += "S";
                    }
                    else
                    {
                        output += " ";
                    }

                }
                output += "\n";
            }

            return output;
        }
    }
}
