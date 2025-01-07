using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AOC2024
{
    internal class Day07: IAdventSolution
    {
        class OperatorGenerator
        {
            private char[] values;

            public OperatorGenerator(char[] values)
            {
                this.values = values;
            }

            public IEnumerable<List<char>> GenerateCombinations(int length)
            {
                if (length == 0)
                {
                    yield return new List<char>();
                    yield break;
                }

                foreach (var value in this.values)
                {
                    foreach (var combination in GenerateCombinations(length - 1))
                    {
                        combination.Insert(0, value);
                        yield return combination;
                    }
                }
            }
        }

        class TestCase
        {
            public long result;
            private List<long> operators;

            public TestCase(long result, List<long> operators)
            {
                this.result = result;
                this.operators = operators;
            }
                         
            public bool HasSolution(bool concatOperator = false)
            {
                OperatorGenerator generator;
               
                  if (!concatOperator)
                  {
                      generator = new OperatorGenerator(new[] { '+', '*' });
                  }
                  else
                  {
                      generator = new OperatorGenerator(new[] { '+', '*', '|'});
                  }

                foreach (var op in generator.GenerateCombinations(this.operators.Count))
                {
                    long currentResult = this.operators.ElementAt(0);
                    for (int i = 1; i < op.Count; i++)
                    {
                        if (op.ElementAt(i) == '+')
                        {
                            currentResult += operators.ElementAt(i);
                        }
                        else if(op.ElementAt(i) == '*')
                        {
                            currentResult *= operators.ElementAt(i);
                        }
                        else
                        {
                            currentResult = long.Parse(currentResult.ToString() + operators.ElementAt(i).ToString());
                        }
                    }

                    if (currentResult == result)
                    {
                        return true;
                    }
                }

                return false;
            }

            public override string ToString()
            {
                return $"{result}, [{string.Join(',', operators)}]";
            }
        }


        public (long, long) Execute(string[] lines)
        {
            var testCases = ReadList(lines);
            long sumA = 0;
            long sumB = 0;

            foreach (var testCase in testCases)
            {
                Console.WriteLine(testCase.ToString());
                if (testCase.HasSolution())
                {
                    sumA += testCase.result;
                }
                else if (testCase.HasSolution(true))
                {
                    sumB += testCase.result;
                }
            }

            return (sumA, sumA + sumB);
        }

        private List<TestCase> ReadList(string[] lines)
        {
            List<TestCase> result = new();

            foreach (string line in lines)
            {
                var splitted = Regex.Split(line, @":\s+");


                result.Add(new TestCase(long.Parse(splitted[0]), splitted[1].Split(' ').Select(long.Parse).ToList()));
            }

            return result;
    }
    }
}
