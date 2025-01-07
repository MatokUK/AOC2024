using System.Text.RegularExpressions;

namespace AOC2024
{
    internal class Day01: IAdventSolution
    {
        public (long, long) Execute(string[] lines)
        {
            List<long> locationsA = ReadList(0, lines);
            List<long> locationsB = ReadList(1, lines);

            // part 2:
            List<long> similarityScore = locationsA.Select(x => x * locationsB.FindAll(y => y == x).Count()).ToList();

            // part 1:
            locationsA.Sort();
            locationsB.Sort();

            long sum = locationsA.Zip(locationsB, (a, b) => a > b ? a - b : b - a).Sum(); 

            return (sum, similarityScore.Sum());
        }

        private List<long> ReadList(int index, string[] lines)
        {
            List<long> locations = new();

            foreach (string line in lines)
            {
                var splitted = Regex.Split(line, @"\s+");

                locations.Add(long.Parse(splitted[index]));
            }

            return locations;
        }
    }
}
