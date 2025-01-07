using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024.Day16Classes
{
    class Maze
    {
        private int totalRows;
        private int totalCols;
        private long bestScore;
        private Dictionary<(int r, int c), bool> walls = new();
        private List<Reindeer> reindeers = new();
        private Dictionary<(int r, int c), int> visited = new();
        private Reindeer shortestPath;

        private (int r, int c) start;
        private (int r, int c) end;

        public long Score
        {
            get 
            {
                return shortestPath.Score;
            }
        }

        public long Tiles
        {
            get
            {
                var tmp = reindeers.Where(r => r.Score == bestScore && r.IsOnPosition(end)).ToList();
                 return tmp.SelectMany(r => r.AllPostions).ToList().Distinct().Count();
            }
        }

        public Maze(List<string> lines)
        {
            totalRows = lines.Count;
            totalCols = lines[0].Length;            

            for (int r = 0; r < lines.Count; r++)
            {
                for (int c = 0; c < lines[r].Length; c++)
                {
                    if (lines[r][c] == '#')
                    {
                        walls.Add((r, c), true);
                    }
                    else if (lines[r][c] == 'E')
                    {
                        end = (r, c);
                    }
                    else if (lines[r][c] == 'S')
                    {
                        start = (r, c);
                        reindeers.Add(new Reindeer(start));
                    }
                }
            }
        }

        public override string ToString()
        {
            string result = "";

            for (int r = 0; r < totalRows; r++)
            {
                for (int c = 0;c < totalCols; c++)
                {
                    if (walls.ContainsKey((r, c)))
                    {
                        result += "#";
                    }
                    else
                    {
                        if (visited.ContainsKey((r, c)))
                        {
                            result += "O";
                        }
                        else
                        {
                            result += ".";
                        }
                    }
                }
                result += "\n";
            }
            
            return result;
        }

        private void PrintMerging(Reindeer a, Reindeer b)
        {
            Console.WriteLine(ShowSinglePath(a.GetPath()));
            Console.WriteLine(" <--------");
            Console.WriteLine(ShowSinglePath(b.GetPath()));
        }

        private string ShowSinglePath(List<(int, int)> path)
        {
            string result = "";

            for (int r = 0; r < totalRows; r++)
            {
                for (int c = 0; c < totalCols; c++)
                {
                    if (walls.ContainsKey((r, c)))
                    {
                        result += "#";
                    }
                    else
                    {
                        if (path.Contains((r, c)))
                        {
                            result += "O";
                        }
                        else
                        {
                            result += ".";
                        }
                    }
                }
                result += "\n";
            }

            return result;
        }


        public void Go()
        {
            Reindeer reindder;

            int counter = 0;
            do
            {
                reindder = reindeers.Min();

                if (IsOnCrossing(reindder))
                {
                    reindeers.Remove(reindder);
                    var xx = SplitOnCrossing(reindder);

                    xx.ForEach(x => x.Forward());
                    reindeers.AddRange(xx);
                }
                else if (CanMove(reindder.ForwardCoords()))
                {
                    reindder.Forward();

                    /* this in needed and good egough for optimalisation */
                    if (!visited.ContainsKey(reindder.current))
                    {
                        visited[reindder.current] = reindder.Score;
                    }
                    else
                    {
                        reindeers.Remove(reindder);
                    }
                }
                else
                {
                    reindeers.Remove(reindder);
                }

               /* if (counter++ % 500 == 0)
                {
                    Console.WriteLine(this);
                }*/
            } while (!reindder.IsOnPosition(end));

            shortestPath = reindder;
        }

        public void GoAllPaths(long bestScore)
        {
            this.bestScore = bestScore;
            this.visited.Clear();
            this.reindeers.Clear();
            reindeers.Add(new Reindeer(start));

            Reindeer reindder;

            int counter = 0;
            do
            {
                reindder = reindeers.Min();

                if (IsOnCrossing(reindder))
                {
                    reindeers.Remove(reindder);
                    var xx = SplitOnCrossing(reindder);

                    xx.ForEach(x => x.Forward());
                    xx = xx.Where(x => !visited.ContainsKey(x.current)).ToList();
   //                 xx.ForEach(x => visited[x.current] = x.Score);
                    reindeers.AddRange(xx);
                }
                else if (CanMove(reindder.ForwardCoords()))
                {
                    reindder.Forward();
                    /*  if (reindder.Score > bestScore)
                      {
                          reindeers.Remove(reindder);
                      }
                      else
                      {*/
                    if (!visited.ContainsKey(reindder.current) || visited[reindder.current] > reindder.Score)
                    {
                        visited[reindder.current] = reindder.Score;
                    }
                    else
                    {
                         Console.WriteLine("big problem!!!");
                        //reindeers.Remove(reindder);
                    }
                  //  }


                    /* this in needed and good egough for optimalisation */
                    /*if (reindder.Score > visited[reindder.current])
                    {
                     //   reindeers.Remove(reindder);
                    }*/
                }
                else
                {
                    reindeers.Remove(reindder);
                }


                // merge
               /* List<int> skipIndexes = new();
                for (int j = 0; j < reindeers.Count; j++)
                {
                    for (int k = j+1; k < reindeers.Count; k++)
                    {
                        if (skipIndexes.Contains(k))
                        {
                            continue;
                        }

                        if (reindeers[j].MergeAltPath(reindeers[k]))
                        {
                           // PrintMerging(reindeers[j], reindeers[k]);
                            skipIndexes.Add(k);
                        }
                    }
                }*/

                // remove merged:
               /* for (int j = reindeers.Count - 1; j >= 0; j--)
                {
                    if (skipIndexes.Contains(j))
                    {
                        reindeers.RemoveAt(j);
                    }
                }*/
                // merge - end

                if (counter++ % 1 == 0)
                {
                    Console.WriteLine(this);
                    Console.WriteLine(this.reindeers.Count);
                    Console.WriteLine(this.reindeers.Min().Score);
                }
            } while (reindeers.Any(r => r.Score < bestScore));
        }

        public bool CanMove((int r, int c) coords)
        {
            return !walls.ContainsKey(coords);
        }

        private bool IsOnCrossing(Reindeer reindeer)  
        {
            var coords = reindeer.TurnCoords().Where(coords => !walls.ContainsKey(coords)).ToList();
            if (coords.Count == 0)
            {
                return false;
            }

            return coords.Count >= 2 || !coords.Contains(reindeer.ForwardCoords());
        }

        private List<Reindeer> SplitOnCrossing(Reindeer reindeer)
        {
            List<Reindeer> reindeers = new();

            if (CanMove(reindeer.ForwardCoords()))
            {
                reindeers.Add(new Reindeer(reindeer));
            }

            Reindeer reindderCW = new Reindeer(reindeer);
            reindderCW.RotateClockwise();
            if (CanMove(reindderCW.ForwardCoords()))
            {
                reindeers.Add(reindderCW);
            }

            Reindeer reindderCCW = new Reindeer(reindeer);
            reindderCCW.RotateCounterClockwise();
            if (CanMove(reindderCCW.ForwardCoords()))
            {
                reindeers.Add(reindderCCW);
            }

            return reindeers;
        }        
    }
}
