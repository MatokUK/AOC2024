namespace AOC2024
{
    internal class Day10 : IAdventSolution
    {
        private List<string> map;

        public (long, long) Execute(string[] lines)
        {           
            long partA = 0;
            long partB = 0;

            map = ReadMap(lines);
            var zeroes = GetZeroes(map);

            foreach (var entry in zeroes)
            {
                var trailhed = Trailhead(entry);
                partA += trailhed.Distinct().Count();
                partB += trailhed.Count();
            }

            return (partA, partB);
        }

        private List<(int, int)> Trailhead((int, int) position)
        {
            return Tailhead(new List<(int, int)>() { position }, 1);
        }

        private List<(int, int)> Tailhead(List<(int r, int c)> positios, int score)
        {
            if (positios.Count == 0)
            {
                return new List<(int, int)>();
            }

            List<(int r, int c)> nextValidPositions = new();

            foreach (var position in positios)
            {
                List<(int r, int c)> neighbours = new() 
                { 
                    (position.r, position.c + 1),
                    (position.r, position.c - 1),
                    (position.r + 1, position.c),
                    (position.r - 1, position.c),
                };

                neighbours = neighbours.Where(x => x.r >= 0 && x.c >= 0 && x.r < map.Count && x.c < map[0].Length).ToList();

                foreach (var p in  neighbours)
                {
                    if (map[p.r][p.c] - '0' == score)
                    {
                        nextValidPositions.Add(p);
                    }
                }
            }

            if (score == 9)
            {
                return nextValidPositions;
            }

            return Tailhead(nextValidPositions, score + 1);
        }

        private List<(int r, int c)> GetZeroes(List<string> map)
        {
            List<(int r, int c)> zeroes = new();

            for (int r = 0; r < map.Count; r++)
            {
                for (int c = 0; c < map[r].Length; c++)
                {
                    if (map[r][c] == '0')
                    {
                        zeroes.Add((r, c));
                    }
                }
            }

            return zeroes;
        }

        private List<string> ReadMap(string[] lines)
        {
           return lines.Select(x => x.Trim()).ToList();
        }     
    }
}
