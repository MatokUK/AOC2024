using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AOC2024
{
    internal class Day13: IAdventSolution
    {
        class ClawGame
        {
            private (int, int) a;
            private (int, int) b;
            private (int, int) price;

            public ClawGame((int, int) a, (int, int) b, (int, int) price)
            {
                this.a = a;
                this.b = b;
                this.price = price;
            }

            public void Solve()
            {
                int x, y;


                /* bool solutionExists = DiophantineSolver.SolveDiophantine(this.a.Item1, this.b.Item1, this.price.Item1, out x, out y);

                 if (solutionExists)
                 {
                     Console.WriteLine($"Solution found: x = {x}, y = {y}");
                 }
                 else
                 {
                     Console.WriteLine("No solution exists.");
                 }*/

                Console.WriteLine($"GCD is {DiophantineSolver.gcd(this.a.Item1, this.b.Item1)}");
            }

            public override string ToString()
            {
                return $"A: {a.ToString()} b: {b.ToString()} === {price.ToString()}";
            }
        }

        class DiophantineSolver
        {
            // Method to implement the Extended Euclidean Algorithm
            // It returns the GCD of a and b, and also sets x and y such that
            // a*x + b*y = gcd(a, b)
            public static int ExtendedGCD(int a, int b, out int x, out int y)
            {
                if (b == 0)
                {
                    x = 1;
                    y = 0;
                    return a;
                }

                int gcd = ExtendedGCD(b, a % b, out int x1, out int y1);
                x = y1;
                y = x1 - (a / b) * y1;
                return gcd;
            }

            public static int gcd(int a, int b)
            {
                if (b == 0)
                {
                    return a;
                }

                if (a > b)
                {
                    return gcd(b, a % b);
                }

                return gcd(a, b % a);
            }

            // Method to solve the Diophantine equation Ax + By = C
            public static bool SolveDiophantine(int A, int B, int C, out int x, out int y)
            {
                // Step 1: Compute GCD of A and B using Extended Euclidean Algorithm
                int gcd = ExtendedGCD(A, B, out x, out y);

                // Step 2: Check if a solution exists
                if (C % gcd != 0)
                {
                    x = 0;
                    y = 0;
                    return false; // No solution exists
                }

                // Step 3: Scale the solution to match the desired C
                int scale = C / gcd;
                x *= scale;
                y *= scale;

                return true; // A solution exists
            }
        }

        public (long, long) Execute(string[] lines)
        {
            var games = ReadGames(lines);

            foreach (var game in games)
            {
                Console.WriteLine(game);
                game.Solve();
            }

            return (0, 0); // That's not the right answer; your answer is too low - 853
        }

        private List<ClawGame> ReadGames(string[] lines)
        {
            var games = new List<ClawGame>();
            (int, int) a = (0, 0);
            (int, int) b = (0, 0);
            (int, int) price;

            foreach (string line in lines)
            {
                var match = Regex.Match(line, @"X[+=](\d+),\s+Y[+=](\d+)");

                if (line.Contains("Button A"))
                {
                    a = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
                }
                else if (line.Contains("Button B"))
                {
                    b = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
                }
                else if (line.Contains("Prize: "))
                {
                    price = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
                    games.Add(new ClawGame(a, b, price));
                }
            }

            return games;        
        }

    }
}
