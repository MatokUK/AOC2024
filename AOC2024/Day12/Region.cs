using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024.Day12Classes
{
    class Region
    {
        enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }

        public string Name { get; init; }
        private List<(int, int)> cells;

        public long Area
        {
            get
            {
                return cells.Count;
            }
        }

        public long Sides
        {
            get
            {
                if (cells.Count <= 2)
                {    // 1 or 2 cells: allways 4 sides
                    return 4;
                }

                var minRows = cells.Min(cell => cell.Item1);

                var corner = cells
                    .Where(cell => cell.Item1 == minRows) // Filter tuples with the minimum first item
                    .OrderBy(cell => cell.Item2)          // Order by the second item
                    .First();

                return SSS(Direction.RIGHT, corner);
            }

        }

        public Region(string name, List<(int, int)> cells)
        {
            this.Name = name;
            this.cells = cells;
        }

        public bool HasSameName(string name)
        {
            return this.Name.Equals(name);
        }

        public void AddCell((int, int) cell)
        {
            cells.Add(cell);
        }

        public bool HasCell((int, int) cell)
        {
            return cells.Contains(cell);
        }

        public List<(int, int)> GetCells()
        {
            return this.cells;
        }

        private long SSS(Direction direction, (int r, int c) start)
        {
            Direction prevDirection = Direction.UP;
            long directorinChanges = 0;
            int actRow = start.r;
            int actCol = start.c;

            List<(int, int)> path = new() { start };


            do
            {
                switch (direction)
                {
                    case Direction.RIGHT:
                        while (cells.Contains((actRow, actCol + 1)) && !cells.Contains((actRow - 1, actCol)))
                        {
                            actCol++;
                            path.Add((actRow, actCol));

                        }
                        if (cells.Contains((actRow - 1, actCol))) // shape:   #
                        {                                         //         ## 
                            actRow--;
                            path.Add((actRow, actCol));
                            direction = Direction.UP;
                        }
                        else
                        {
                            // direction = SwitchDirection(direction, (actRow, actCol));
                            direction = Direction.DOWN;
                        }
                        directorinChanges++;
                        break;

                    case Direction.DOWN:                        
                        while (cells.Contains((actRow + 1, actCol)) && !cells.Contains((actRow, actCol + 1)))
                        {
                            actRow++;
                            path.Add((actRow, actCol));
                        }

                        if (cells.Contains((actRow, actCol + 1))) // shape: L
                        {
                            actCol++;
                            path.Add((actRow, actCol));
                            direction = Direction.RIGHT;
                        }
                        else
                        {
                            // direction = SwitchDirection(direction, (actRow, actCol));
                            direction = Direction.LEFT;
                        }
                        directorinChanges++;
                        break;

                    case Direction.LEFT:
                        while (cells.Contains((actRow, actCol - 1)) && !cells.Contains((actRow + 1, actCol)))
                        {
                            actCol--;
                            path.Add((actRow, actCol));
                        }

                        prevDirection = direction;
                        if (cells.Contains((actRow + 1, actCol))) // shape: ## <---- left #####
                        {                                         //        #
                            actRow++;
                            path.Add((actRow, actCol));
                            direction = Direction.DOWN;
                        }
                        else
                        {
                            //direction = SwitchDirection(direction, (actRow, actCol));
                            direction = Direction.UP;
                        }
                        directorinChanges++;
                        break;

                    case Direction.UP:
                        while (cells.Contains((actRow - 1, actCol)) && !cells.Contains((actRow, actCol - 1)))
                        {
                            actRow--;
                            path.Add((actRow, actCol));
                        }

                    //    prevDirection = direction;
                        if (cells.Contains((actRow, actCol - 1))) // shape: -|
                        {
                            actCol--;
                            path.Add((actRow, actCol));
                            direction = Direction.LEFT;
                        }
                        else
                        {
                            //direction = SwitchDirection(direction, (actRow, actCol));
                            direction = Direction.RIGHT;
                        }
                        directorinChanges++;
                        break;
                }
            } while (directorinChanges < 4 || (actRow, actCol) != start);


            // fix direction changes when top is onlouy one cell
            if (!cells.Contains((start.r, start.c + 1)) && !cells.Contains((start.r, start.c -1)))
            {
                directorinChanges++;
            }

            // debug:
            Console.WriteLine("Region:" + Name + " [" + directorinChanges + "]");
            PrintDebugPerimeter(path);

            return directorinChanges;
        }

        private void PrintDebugPerimeter(List<(int, int)> path)
        {
            var rows = cells.Max(cell => cell.Item1);
            var cols = cells.Max(cell => cell.Item2);

            for (int r = 0; r <= rows; r++)
            {
                for (int c = 0; c <= cols; c++)
                {
                    if (path.Contains((r, c)))
                    {
                        Console.Write("X");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }

                Console.WriteLine("");
            }
        }

        public override string ToString()
        {
            return $"{Name} Area: {Area}";
        }
    }
}
