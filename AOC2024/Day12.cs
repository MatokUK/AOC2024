using System.Text.RegularExpressions;

namespace AOC2024
{
    using Day12Classes;

    internal class Day12: IAdventSolution
    { 
        public (long, long) Execute(string[] lines)
        {
            long partI = 0;
            long partII = 0;

            Area area = new Area(lines);
            var regions = area.GetRegions();

            foreach (var region in regions)
            {
                Console.WriteLine(region);
                var perimeter = area.Perimeter(region);
                Console.WriteLine(perimeter);
                partI += region.Area * perimeter;
                partII += region.Area * region.Sides;
            }           

            return (partI, partII);
        }      


    }
}
