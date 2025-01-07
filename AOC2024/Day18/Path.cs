using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024.Day18Classes
{
    internal class Path : IComparable<Path>
    {
        private List<(int, int)> values;

        public (int, int) last
        {
            get
            {
                return values.Last();
            }
        }

        public int Length
        {
            get { return values.Count - 1; }
        }

        private Path(List<(int, int)> values)
        {
            this.values = values;
        }

        public static Path FromXY(int x, int y)
        {
            return new Path(new List<(int, int)> { (x, y) });
        }

        public Path FromNextMove(int x, int y)
        {
            var newPathValues = this.values.ToList();
            newPathValues.Add((x, y));

            return new Path(newPathValues);
        }

        public int CompareTo(Path other)
        {
            if (other == null)
            {
                return 1;
            }

            if (this.values.Count == other.values.Count)
            {
                return this.values.SequenceEqual(other.values) ? 0 : 1;
            }

            return this.values.Count < other.values.Count ? 1 : -1;
        }

        public bool Contains((int, int) coord)
        {
            return this.values.Contains(coord);
        }

        /*     public override bool Equals(object? obj)
             {
                 return base.Equals(obj);
             }

             public override int GetHashCode()
             {
                 // Generate a hash code based on the elements in the list
                 return this.values.Aggregate(0, (hash, value) => hash ^ value.GetHashCode());
             }*/

        public override string ToString()
        {
            string result = "";

            foreach (var item in this.values)
            {
                result += item.ToString();
            }

            return result;
        }
    }

}
