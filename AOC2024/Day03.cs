using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2024
{
    enum InstructionType
    {
        Enable,
        Disable,
        Multiply
    }

    internal class Instruction
    {
        public InstructionType type { get; }
        private long a;
        private long b;

        public long Result
        {
            get
            {
                return a * b;
            }
        }

        public Instruction(InstructionType type, long a, long b)
        {
            this.type = type;
            this.a = a;
            this.b = b;
        }

        public override string ToString()
        {
            return $"{type}({a}, {b})";
        }
    }

    internal class Day03 : IAdventSolution
    {

        public record DiffResult(int Index, int Result);

        public (long, long) Execute(string[] lines)
        {
            var instructions = ReadInstructions(lines);

            List<Instruction> part2 = new();

            bool enabled = true;
            foreach (var instruction in instructions)
            {
                if (enabled && instruction.type == InstructionType.Multiply)
                {
                    part2.Add(instruction);
                } 
                else if (instruction.type == InstructionType.Enable)
                {
                    enabled = true;
                }
                else
                {
                    enabled = false;
                }
            }


            return (instructions.Sum(mul => mul.Result), part2.Sum(mul => mul.Result));
        }

        private List<Instruction> ReadInstructions(string[] lines)
        {
            List<Instruction> result = new();

            foreach (var line in lines)
            {
                var matches = Regex.Matches(line, @"(mul\(|do\(|don't\()((\d+),(\d+))?\)");
                Instruction instruction = null;

                foreach (Match match in matches)
                {
                    switch (match.Groups[1].Value)
                    {
                        case "mul(":
                            instruction = new Instruction(InstructionType.Multiply, long.Parse(match.Groups[3].Value), long.Parse(match.Groups[4].Value));
                            break;

                        case "do(":
                            instruction = new Instruction(InstructionType.Enable, 1, 0);
                            break;

                        default:
                            instruction = new Instruction(InstructionType.Disable, 1, 0);
                            break;
                    }
                    

                    result.Add(instruction);
                }
            }

            return result;
        }
    }
}
