using System.Text.RegularExpressions;

namespace AOC2024
{
    internal class Day11: IAdventSolution
    {
        public (long, long) Execute(string[] lines)
        {
            long partA = 0;
            var stones = lines[0].Split(' ').Select(long.Parse).ToList();

            for (int blinks = 1; blinks <= 25; blinks++)
            {
                stones = stones.SelectMany(n =>
                {
                    if (n == 0)
                    {
                        return new List<long> { 1 };
                    }

                    if (n.ToString().Length % 2 == 0)
                    {
                        var len = n.ToString().Length;
                        return new List<long> { long.Parse(n.ToString().Substring(0, len / 2)), long.Parse(n.ToString().Substring(len / 2)) };
                    }

                    return new List<long> { n * 2024 };
                }).ToList();
            }

            partA = stones.Count;


            var stoneD = stones
                .GroupBy(stone => stone)
                .ToDictionary(
                    group => group.Key,        
                    group => group.LongCount() 
                );

            // part 2 make next 50 iterations:
            for (int blinks = 1; blinks <= 50; blinks++)
            {
                var nextStones = new Dictionary<long, long> { { 1, 0 } };

                foreach (var (stone, count) in stoneD)
                {
                    if (stone == 0)
                    {
                        nextStones[1] = count;
                        continue;
                    }

                    var len = stone.ToString().Length;

                    if (len % 2 == 0)
                    {
                        var left = long.Parse(stone.ToString().Substring(0, len / 2));
                        var right = long.Parse(stone.ToString().Substring(len / 2));

                        if (!nextStones.ContainsKey(left))
                        {
                            nextStones[left] = 0;
                        }

                        if (!nextStones.ContainsKey(right))
                        {
                            nextStones[right] = 0;
                        }

                        nextStones[left] += count;
                        nextStones[right] += count;
                        continue;
                    }

                    if (!nextStones.ContainsKey(stone * 2024))
                    {
                        nextStones[stone * 2024] = 0;
                    }
                    nextStones[stone * 2024] += count;

                }

                stoneD = nextStones;
            }


            return (partA, stoneD.Values.ToList().Sum());
        }      
    }
}
