using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AOC2024
{
    internal class Day14: IAdventSolution
    {
        private const int X = 101;
        private const int Y = 103;
        class Robot
        {
            public static int maxX = 0;
            public static int maxY = 0;

            public (int, int) posititon;
            private (int, int) velocity;

            public Robot((int, int) posititon, (int, int) velocity)
            {
                this.posititon = posititon;
                this.velocity = velocity;
            }

            public void ElapseTime(int seconds)
            {
                posititon.Item1 = (posititon.Item1 + velocity.Item1 * seconds) % maxX;
                posititon.Item2 = (posititon.Item2 + velocity.Item2 * seconds) % maxY;

                if (posititon.Item1 < 0)
                {
                    posititon.Item1 += maxX;
                }

                if (posititon.Item2 < 0)
                {
                    posititon.Item2 += maxY;
                }
            }

            public bool InQuadrant((int x, int y) from, (int x, int y) to)
            {
                return posititon.Item1 >= from.x && posititon.Item2 >= from.y && posititon.Item1 <= to.x && posititon.Item2 <= to.y;
            }

            public bool IsOnPosition(int x, int y)
            {
                return posititon == (x, y);
            }
        }

        public (long, long) Execute(string[] lines)
        {
            Robot.maxX = X;
            Robot.maxY = Y;
            int halfX = X / 2;
            int halfY = Y / 2;
            var robots = ReadGame(lines);

            foreach (var robot in robots)
            {
                robot.ElapseTime(100);                
            }

            // part 2
          /*  for (int s = 0; s < 10000; ++s)
            {
                foreach (var robot in robots)
                {
                    robot.ElapseTime(1);
                }

                var duplicates = robots
                    .GroupBy(x => new { x.posititon })
                    .Where(g => g.Count() > 1)
                    .ToList();

                Console.WriteLine("Seconds " + (s + 1));

                if (duplicates.Count() == 0)
                {
                    debug(robots, X, Y);
                    break;
                }
            }*/

            long q1 = robots.Where(robot => robot.InQuadrant((0, 0), (halfX - 1, halfY - 1))).Count();
            long q2 = robots.Where(robot => robot.InQuadrant((halfX + 1, 0), (X, halfY - 1))).Count();
            long q3 = robots.Where(robot => robot.InQuadrant((0, halfY + 1), (halfX -1, Y))).Count();
            long q4 = robots.Where(robot => robot.InQuadrant((halfX + 1, halfY + 1), (X, Y))).Count();

            return (q1 * q2 * q3 * q4, 7338);
        }


        private void debug(List<Robot> robots, int maxX, int maxY)
        {
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    var count = robots.Where(robot => robot.IsOnPosition(x, y)).Count();
                    if (count > 0) {
                        Console.Write(count);
                    } else
                    {
                        Console.Write(' ');
                    }
                    
                }
                Console.WriteLine();
            }
        }

        private List<Robot> ReadGame(string[] lines)
        {
            var robots = new List<Robot>();
         

            foreach (string line in lines)
            {
                var match = Regex.Match(line, @"(\d+),(\d+).+\=(-?\d+),(-?\d+)");
                int a = int.Parse(match.Groups[1].Value);
                int b = int.Parse(match.Groups[2].Value);
                int c = int.Parse(match.Groups[3].Value);
                int d = int.Parse(match.Groups[4].Value);

                robots.Add(new Robot((a, b), (c, d)));
            }

            return robots;        
        }
    }
}
