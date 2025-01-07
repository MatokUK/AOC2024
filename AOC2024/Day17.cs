using System.Text.RegularExpressions;

namespace AOC2024
{
    using AOC2024.Day17Classes;
    using System.Collections.Generic;

    internal class Day17: IAdventSolution
    {
        public (long, long) Execute(string[] lines)
        {
            // part I:
            var instructions = ReadInstructions(lines);

            var computerA = new Computer(59397658, 0, 0);
            var output = computerA.Execute(instructions);

            Console.WriteLine(computerA);
            Console.WriteLine(string.Join(",", output));


            // part II:

            // After experitenting "here and there":
            // - it works with numbers in octal base
            // - wanted output has 16 numbers, it means initial number in register A must also have 16 digits in octal (!), smallest: 1000000000000000
            // - my number must begin 56..............


            var wantetOutput = new List<int> { 2, 4, 1, 1, 7, 5, 4, 6, 1, 4, 0, 3, 5, 5, 3, 0 };

            List<string> feasibleNumbers = new() { "5600000000000000" };
            for (int n = 4; n <= 16; n += 2)
            {
                int insertPosition = n - 1;
                List <string> nextIteration = new();
                foreach (var feasibleNumber in feasibleNumbers)
                {
                    var partialResult = PossibleValues(feasibleNumber, insertPosition, wantetOutput.Skip(16 - n).ToList(), instructions);
                    nextIteration.AddRange(partialResult);
                }
                feasibleNumbers = nextIteration.ToList();
            }
        
            return (0, feasibleNumbers.Select(x => Convert.ToInt64(x, 8)).Min());
        }

        private List<string> PossibleValues(string initNumberOct, int insertPosition, List<int> wanted, List<int> instructions)
        {
            List<string> pairs = new();
            var oct = 0;
            do
            {                
                var val = initNumberOct.Substring(0, insertPosition-1) + Convert.ToString(oct, 8).PadLeft(2, '0') + initNumberOct.Substring(1 + insertPosition);
                var computer = new Computer(Convert.ToInt64(val, 8));
                var output = computer.Execute(instructions);
                if (output.Skip(16 - wanted.Count).SequenceEqual(wanted))
                {
                    pairs.Add(val);
                }

            } while (Convert.ToString(oct++, 8) != "77");

            return pairs;
        }

        private List<int> ReadInstructions(string[] lines)
        {
            foreach (var line in lines)
            {
                if (line.Contains("Program"))
                {
                    return line.Substring(9).Split(",").Select(x => int.Parse(x)).ToList();
                }
            }

            return new List<int> { };
        }
    }
}
