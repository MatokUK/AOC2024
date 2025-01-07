using System.Text.RegularExpressions;

namespace AOC2024
{
    internal class Day19: IAdventSolution
    {
        private List<string> blocks = new();
        public (long, long) Execute(string[] lines)
        {
            blocks = lines[0].Split(", ").ToList();
            List<string> remove = new List<string>();

            foreach (string b in blocks)
            {
                if (IsPossibleToMake2(b, blocks.Where(x => x.Length < b.Length).ToList()))
                {
                    Console.WriteLine(b + " can be skipped");
                    remove.Add(b);
                }
            }

            blocks.RemoveAll( x => remove.Contains(x));

            List<string> designs = ReadDesigns(lines);

            long possibleDesigns = 0;

            foreach (string design in designs) 
            { 
                Console.WriteLine(design);
                if (IsPossibleToMake2(design, this.blocks))
                {
                    possibleDesigns++;
                }
            }


            return (possibleDesigns, 1);
        }

        private bool IsPossibleToMake2(string design, List<string> blocks, int nested = 0) 
        {
            if (design.Length == 0)
            {
                return true;
            }

            foreach (string block in blocks)
            {
                if (design.StartsWith(block) && IsPossibleToMake2(design.Substring(block.Length), blocks, nested + 1))
                {
                    return true;
                }
            }

            return false;
        }

        private List<string> ReadDesigns(string[] lines)
        {
            List<string> result = new();

            foreach (string line in lines)
            {
                if (line.Contains(',') || line.Length < 1)
                {
                    continue;
                }

                result.Add(line);
            }

            return result;
        }
    }
}
