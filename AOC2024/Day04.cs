using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2024
{
    internal class Day04 : IAdventSolution
    {
        public (long, long) Execute(string[] lines)
        {

            List<string> original = lines.ToList();
            List<string> rotated = Rotate90Degrees(original);
            List<string> diagonalPrimary = GetDiagonalStrings(original);
            List<string> diagonalSecondary = GetSecondaryDiagonals(original);

            return (OccurencesIn(original) + OccurencesIn(rotated) + OccurencesIn(diagonalPrimary) + OccurencesIn(diagonalSecondary), CrossMasOccurences(lines));
        }

        private int OccurencesIn(List<string> lines)
        {
            int total = 0;
            foreach (string line in lines)
            {
                total += Occurences(line, "XMAS");
                total += Occurences(line, "SAMX");
            }

            return total;
        }

        private int CrossMasOccurences(string[] lines)
        {
            int occurences = 0;
            for (int i = 0; i < lines.Length -2; ++i)
            {
                // MatchCollection matches = Regex.Matches(lines[i], @"[M|S].[M|S]");
                var matches = OverlappingMatches(lines[i], @"[M|S].[M|S]");
                Console.WriteLine(lines[i]);
                foreach (var match in matches)
                {
                    Console.WriteLine($"Match: '{match.Value[0]} / {match.Value[2]}' at Index: {match.Index}");
                    Console.WriteLine($"Next: " + HasCrossMasSecondLine(match.Index, lines[i+1]));
                    Console.WriteLine($"Next: " + HasCrossMasThirdLine(match.Index, match.Value, lines[i + 2]));

                    if (HasCrossMasSecondLine(match.Index, lines[i+1]) && HasCrossMasThirdLine(match.Index, match.Value, lines[i + 2]))
                    {
                        occurences++;
                    }
                }
            }
            return occurences;
        }

        private List<(int Index, string Value)> OverlappingMatches(string input, string regexpr)
        {
            List<(int Index, string Value)> result = new();
            int indexOffest = 0;

            Match match;
            do
            {
                match = Regex.Match(input, regexpr);
                if (match.Success)
                {
                    result.Add((match.Index + indexOffest, match.Value));
                    input = input.Substring(match.Index + 1);
                    indexOffest += match.Index + 1;
                }
            } while (match.Success);

            return result;
        }

        private bool HasCrossMasSecondLine(int index, string line)
        {
            return line[index + 1] == 'A';
        }

        private bool HasCrossMasThirdLine(int index, string matched, string line)
        {
            return line[index] == (matched[2] == 'M' ? 'S' : 'M') && line[index+2] == (matched[0] == 'M' ? 'S' : 'M');
        }

        private int Occurences(string input, string find)
        {
            return Regex.Matches(input, Regex.Escape(find)).Count;
        }

        private List<string> Rotate90Degrees(List<string> input)
        {
            int numRows = input.Count;
            int numCols = input[0].Length;
        
            List<string> rotated = new();

            for (int col = 0; col < numCols; col++)
            {
                char[] newRow = new char[numRows];
                for (int row = 0; row < numRows; row++)
                {
                    newRow[numRows - row - 1] = input[row][col];
                }
                rotated.Add(new string(newRow));
            }

            return rotated;
        }

        private List<string> GetDiagonalStrings(List<string> input)
        {
            int n = input.Count;
            var result = new List<string>();

            // Traverse the columns (diagonals) of the input strings
            for (int i = 0; i < n; i++)
            {
                var diagonal = new StringBuilder();

                // For each diagonal, pick the elements from the strings
                for (int j = 0; j <= i; j++)
                {
                    if (i - j < input[j].Length)
                        diagonal.Append(input[j][i - j]);
                }

                result.Add(diagonal.ToString());
            }

            // Now we process the lower part of the matrix (below the main diagonal)
            for (int i = 1; i < n; i++)
            {
                var diagonal = new StringBuilder();

                // For each diagonal, pick the elements from the strings
                for (int j = i; j < n; j++)
                {
                    if (j - i < input[j].Length)
                        diagonal.Append(input[j][j - i]);
                }

                result.Add(diagonal.ToString());
            }

            return result;
        }

        private List<string> GetSecondaryDiagonals(List<string> input)
        {
            int n = input.Count;
            var result = new List<string>();

            // Traverse the diagonals starting from the top-right corner and going to the bottom-left corner
            for (int i = 0; i < n; i++)
            {
                var diagonal = new StringBuilder();

                // Collect elements from each diagonal starting from the top row (rightmost column)
                for (int j = 0; j <= i; j++)
                {
                    if (i - j < input[j].Length)
                        diagonal.Append(input[j][n - 1 - (i - j)]); // Adjust the column index for the secondary diagonal
                }

                result.Add(diagonal.ToString());
            }

            // Now process the lower part of the matrix (below the main diagonal)
            for (int i = 1; i < n; i++)
            {
                var diagonal = new StringBuilder();

                // Collect elements from each diagonal starting from the last column of the matrix
                for (int j = i; j < n; j++)
                {
                    if (j - i < input[j].Length)
                        diagonal.Append(input[j][n - 1 - (j - i)]); // Adjust the column index for the secondary diagonal
                }

                result.Add(diagonal.ToString());
            }

            return result;
        }
    }
}
