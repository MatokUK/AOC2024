using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024.Day20Classes
{
    class Path
    {
        private Dictionary<(int r, int c), bool> visited = new();
        private (int r, int c) _position;

        public (int r, int c) Position
        {
            get { return _position;  }
        }      

        public int Length
        {
            get { return visited.Count; }
        }        

        public bool IsVisited((int, int) pos)
        {
            return visited.ContainsKey(pos);
        }

        public Path((int, int) position)
        {
            visited[position] = true;
            _position = position;
        }

        public void MoveTo((int r, int c) next) 
        {
            visited[next] = true;
            _position = next;
        }

        public int TryCheat((int r, int c) throughWall)
        {
            int shortcut = 0;
            bool wall = false;
            foreach (var pos in visited.Keys) 
            { 
                if (wall)
                {
                    shortcut++;                    
                    if (shortcut > 2 && IsNextTo(pos, throughWall))
                    {
                        wall = false;
                        break;
                    }
                }

                if (IsNextTo(pos, throughWall))
                {
                    wall = true;
                }
            }

            return wall ? 0 : shortcut - 2;
        }

        public (int, int) CoordsAt(int index)
        {
            return visited.Keys.ToList().ElementAt(index);
        }

        public List<(int, int)> GetVisitedFromIndex(int index)
        {
            return visited.Keys.Skip(index).ToList();
        }

        private bool IsNextTo((int, int) a, (int, int) b)
        {

            return a == (b.Item1 + 1, b.Item2) || a == (b.Item1 - 1, b.Item2) || a == (b.Item1, b.Item2 + 1) || a == (b.Item1, b.Item2  - 1);
        }
    }
}
