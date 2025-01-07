using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024.Day16Classes
{
    class Reindeer: IComparable<Reindeer>
    {
        enum Direction
        {
            UP = '^',
            DOWN = 'v',
            LEFT = '<',
            RIGHT = '>',
        }

        private Direction direction;
        public (int r, int c) current;
        private int turns = 0;
        private List<(int, int)> prev = new ();
        private List<(int, int)> altPath = new();

        public int Score
        {
            get
            { 
                return 1000 * turns + prev.Count;
            }
        }

        public List<(int, int)> AllPostions
        {
            get
            {
                var positions = prev.ToList();
                positions.Add(current);

                return positions;
            }
        }

        public List<(int, int)> GetPath()
        {
            var positions = prev.ToList();
            positions.Add(current);

            return positions;
        }
        
        public int CompareTo(Reindeer other)
        {
            if (other == null)
            {
                return 1;
            }

            // remove - same previos must return 0;
            if (this.Score == other.Score && this.prev.SequenceEqual(other.prev) && this.current == other.current)
            {
                return 0;
            }

            return this.Score > other.Score ? 1 : -1; 
        }

        public Reindeer((int r, int c) current)
        {
            this.direction = Direction.RIGHT;
            this.current = current;
        }

        public Reindeer(Reindeer other)
        {
            this.direction = other.direction;
            this.current = other.current;
            this.turns = other.turns;
            this.prev = other.prev.ToList();
            this.altPath = other.altPath.ToList();
        }

        public bool IsOnPosition((int, int) pos)
        {
            return current == pos;
        }

        public (int r, int c) ForwardCoords()
        {
            return direction switch
            {
                Direction.UP => (current.r - 1, current.c),
                Direction.RIGHT => (current.r, current.c + 1),
                Direction.DOWN => (current.r + 1, current.c),
                _ => (current.r, current.c - 1)
            };
        }

        public void Forward()
        {
            prev.Add(current);
            current = ForwardCoords();
        }

        public List<(int r, int c)> TurnCoords()
        {
            return direction switch
            {
                Direction.UP => new List<(int r, int c)>() { (current.r - 1, current.c), (current.r, current.c + 1), (current.r, current.c - 1) },
                Direction.RIGHT => new List<(int r, int c)>() { (current.r, current.c + 1), (current.r + 1, current.c), (current.r - 1, current.c) },
                Direction.DOWN => new List<(int r, int c)>() { (current.r + 1, current.c), (current.r, current.c + 1), (current.r, current.c - 1) },
                _ => new List<(int r, int c)>() { (current.r, current.c - 1), (current.r + 1, current.c), (current.r - 1, current.c) },
            };
        }

        public bool MergeAltPath(Reindeer other)
        {
            if (other.Score != this.Score || other.current != this.current)
            {
                return false;
            }

            var alt = other.prev.Where(coord => !this.prev.Contains(coord)).ToList();

            return true;
        }

        public void RotateClockwise()
        {
            direction = direction switch
            {
                Direction.UP => Direction.RIGHT,
                Direction.RIGHT => Direction.DOWN,
                Direction.DOWN => Direction.LEFT,
                _ => Direction.UP
            };

            turns++;
        }

        public void RotateCounterClockwise()
        {
            direction = direction switch
            {
                Direction.UP => Direction.LEFT,
                Direction.LEFT => Direction.DOWN,
                Direction.DOWN => Direction.RIGHT,
                _ => Direction.UP
            };

            turns++;
        }

        public override string ToString()
        {
            string result = "";

            result += current.ToString() + "[score: " + this.Score + "]" + string.Join("", prev);

            return result;
        }
    }   
}
