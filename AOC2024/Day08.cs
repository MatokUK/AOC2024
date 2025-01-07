using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AOC2024
{
    internal class Day08: IAdventSolution
    {
        record Coord (int x, int y)
        {
            public bool isWithimArea(int rows,  int cols)
            {
                return x >= 0 && x < rows && y >= 0 && y < cols;
            }
        }

        private int maxRows;
        private int maxCols;


        public (long, long) Execute(string[] lines)
        {
            this.maxRows = lines.Length;
            this.maxCols = lines[0].Length;

            var antennas = ReadAntenas(lines);
            Dictionary<char, List<Coord>> antinodes = new();
            Dictionary<char, List<Coord>> antinodes2 = new();

            foreach (var antena in antennas)
            {
                var antinodesOfAntena = MakeAntinodes(antena.Key, antena.Value);
                antinodes.Add(antena.Key, antinodesOfAntena);

                var antinodesOfAntena2 = MakeAntinodes(antena.Key, antena.Value, true);
                antinodes2.Add(antena.Key, antinodesOfAntena2);
            }

     
            long totalAntinodes = antinodes.Values
                .SelectMany(list => list)
                .Distinct()
                .ToList()
              //  .Where(e => e.x >= 0 && e.x < lines.Length && e.y >= 0 && e.y < lines.ElementAt(0).Length)
                .Count();

            long totalAntinodes2 = antinodes2.Values
                .SelectMany(list => list)
                .Distinct()
                .ToList()
                //.Where(e => e.x >= 0 && e.x < lines.Length && e.y >= 0 && e.y < lines.ElementAt(0).Length)
                .Count();

            return (totalAntinodes, totalAntinodes2); // That's not the right answer; your answer is too low - 853
        }

        private List<Coord> MakeAntinodes(char antena, List<Coord> coords, bool repeat = false)
        {
            List<Coord> antinode = new();

            for (int k = 0; k < coords.Count; k++)
            {
                for (int l = k + 1; l < coords.Count; l++)
                {
                 //   Console.WriteLine(antena +" antinode " + coords[k] + " | " + coords[l]);

                    if (repeat)
                    {
                        antinode.AddRange(MakeTwoAntinodes(coords[k], coords[l], 200));
                    }
                    else
                    {
                        antinode.AddRange(MakeTwoAntinodes(coords[k], coords[l]));
                    }                    
                }
            }

            return antinode;
        }

        private List<Coord> MakeTwoAntinodes(Coord a, Coord b, int repeat = 1)
        {
            List<Coord> result = new();

            var xDiff = a.x - b.x;
            var yDiff = a.y - b.y;

            for (int i = 1; i <= repeat; ++i)
            {
                var coordA = new Coord(a.x + xDiff*i, a.y + yDiff*i);
                var coordB = new Coord(b.x - xDiff*i, b.y - yDiff*i);

                if (coordA.isWithimArea(this.maxRows, this.maxCols))
                {
                    result.Add(coordA);
                }
                if (coordB.isWithimArea(this.maxRows, this.maxCols))
                {
                    result.Add(coordB);
                }
            }

            if (repeat > 1)
            {
                result.Add(a);
                result.Add(b);
            }

            return result;
        }

        private Dictionary<char, List<Coord>> ReadAntenas(string[] map)
        {
            Dictionary<char, List<Coord>> result = new();

            for (int r = 0; r < map.Length; ++r)
            {
                for (int c = 0; c < map.ElementAt(r).Length; ++c)
                {
                    char ch = map[r][c];

                    if (ch == '.')
                    {
                        continue;
                    }

                    if (result.ContainsKey(ch))
                    {
                        result[ch].Add(new Coord(r, c));
                    }
                    else
                    {
                        var list = new List<Coord>
                        {
                            new Coord(r, c)
                        };
                        result.Add(ch, list);
                    }
                }
            }

            return result;
        }

        private bool HasAntinode(int x, int y, Dictionary<char, List<Coord>> antinodes)
        {
            foreach (var coord in antinodes.Values)
            {
                if (coord.Contains(new Coord(x, y)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
