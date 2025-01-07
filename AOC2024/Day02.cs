using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2024
{
    internal class Day02 : IAdventSolution
    {
        record DiffResult(int Index, int Result);

        public (long, long) Execute(string[] lines)
        {
            var reports = ReadReports(lines);
            long partA = 0;
            long partB = 0;

            foreach (var report in reports)
            {               
                if (ReportAnalyse(report)) {
                    partA++;
                    partB++;
                    continue;
                }

                if (ProblemDumper(report))
                {
                    partB++;
                } 
            }


            return (partA, partB);
        }

        private bool ReportAnalyse(List<int> report)
        {
            IEnumerable<DiffResult> diff = report.Zip(report.Skip(1), (a, b) => a - b)
                    .Select((result, index) => new DiffResult(1 + index, result));

            var groups = diff.GroupBy(n =>
            {
                return n.Result switch
                {
                    0 => "Zero",
                    _ when n.Result > 0 && n.Result <= 3 => "Positive",
                    _ when n.Result < 0 && n.Result >= -3 => "Negative",
                    _ => "Offlimit",
                };
            });

            return groups.Count() == 1 && groups.Any(g => g.Key == "Positive" || g.Key == "Negative");
        }

        private bool ProblemDumper(List<int> report)
        {
            for (int idx =  0; idx < report.Count; ++idx)
            {
                List<int> possibleFixedReport = report.ToList();
                possibleFixedReport.RemoveAt(idx);

                if (ReportAnalyse(possibleFixedReport))
                {
                    return true;
                }
            }

            return false;
        }

        private List<List<int>> ReadReports(string[] lines)
        {
            List<List<int>> result = new();

            foreach (var line in lines)
            {
                result.Add(line.Split(' ').Select(int.Parse).ToList());
            }

            return result;
        }
    }
}
