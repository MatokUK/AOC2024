using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024.Day17Classes
{
    class Computer
    {
        private long regA;
        private long regB;
        private long regC;
        private int ip;
        private List<int> output = new();
        public Computer(long regA = 0, long regB = 0, long regC = 0) 
        {
            this.regA = regA;
            this.regB = regB;
            this.regC = regC;
            ip = 0;
        }

        public List<int> Execute(List<int> instructions)
        {
            do
            {
                RunOpCode(instructions.ElementAt(ip), instructions.ElementAt(ip + 1));
                ip += 2;                
            } while (ip < instructions.Count);

            return output;
        }

        private void RunOpCode(int opcode, int operand)
        {
            var value = GetOperandFromRegister(operand);

            switch (opcode)
            {
                case 0:
                    regA = adv(value);
                    break;
                case 1:
                    regB = regB ^ operand;
                    break;
                case 2:
                    regB = value % 8;
                    break;
                case 3: // jnz
                    if (regA != 0)
                    {
                        ip = (int)operand - 2;
                    }
                    break;
                case 4: // bitwise XOR
                    regB = regB ^ regC;
                    break;
                case 5:
                    output.Add((int)(value % 8));
                    break;
                case 6: // NEVER USED IN MY INPUT
                    regB = adv(value);
                    break;
                case 7:
                    regC = adv(value);
                    break;
                default:
                    throw new Exception("Not implemented");
            }
        }

        private long adv(long value)
        {
            return (long)(regA / Math.Pow(2, value));
        }

        private long GetOperandFromRegister(int operand)
        {
            return operand switch
            {
                4 => regA,
                5 => regB,
                6 => regC,
                7 => throw new Exception("Ivalid operand"),
                _ => operand,
            };
        }

        public override string ToString()
        {
            return $"[{string.Join(",", output)}]({output.Count})";
        }
    }
}
