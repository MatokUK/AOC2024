namespace AOC2024.Day12Classes
{
    class Area
    {
        private string[] lines;
        private Dictionary<(int, int), bool> visited = new();

        private int rows;
        private int cols;

        public Area(string[] lines)
        {
            this.lines = lines;
            this.rows = lines.Length;
            this.cols = lines[0].Length;
        }

        public long Perimeter(Region region)
        {
            long perimeter = 0;

            foreach (var cell in region.GetCells())
            {
                perimeter += PerimeterOfCell(region.Name, cell);
            }

            return perimeter;
        }

        private long PerimeterOfCell(string name, (int r, int c) cell)
        {
            List<bool> boundaries = new()
            {
                HasBoundary(name, cell, (cell.r, cell.c + 1)),
                HasBoundary(name, cell, (cell.r, cell.c - 1)),
                HasBoundary(name, cell, (cell.r + 1, cell.c)),
                HasBoundary(name, cell, (cell.r - 1, cell.c)),
            };

            return boundaries.Where(b => b).Count();
        }

        private bool HasBoundary(string name, (int r, int c) cellInRegion, (int r, int c) cellInMap)
        {
            if (cellInMap.r < 0 || cellInMap.c < 0)
            {
                return true;
            }

            if (cellInMap.r >= rows || cellInMap.c >= cols)
            {
                return true;
            }

            return !name.Equals(lines[cellInMap.r][cellInMap.c].ToString());
        }

        public List<Region> GetRegions()
        {
            List<Region> result = new();

            for (int r = 0; r < lines.Length; r++)
            {
                for (int c = 0; c < lines[r].Length; c++)
                {
                    if (visited.ContainsKey((r, c)))
                    {
                        continue;
                    }


                    var region = CutRegion((r, c));
                    result.Add(region);

                    foreach (var cell in region.GetCells())
                    {
                        visited.Add(cell, true);
                    }
                }
            }

            return result;
        }

        private Region CutRegion((int r, int c) pos)
        {
            Region region = new Region(lines[pos.r][pos.c].ToString(), new List<(int, int)>() { pos });

            SpreadRegion((pos.r + 1, pos.c), region);
            SpreadRegion((pos.r, pos.c + 1), region);
            SpreadRegion((pos.r - 1, pos.c), region);
            SpreadRegion((pos.r, pos.c - 1), region);

            return region;
        }

        private void SpreadRegion((int r, int c) pos, Region region)
        {
            if (region.HasCell(pos))
            {
                return;
            }

            if (pos.r < 0 || pos.c < 0 || pos.r > lines.Length - 1 || pos.c > lines[0].Length - 1)
            {
                return;
            }

            string name = lines[pos.r][pos.c].ToString();

            if (!region.HasSameName(name))
            {
                return;
            }

            region.AddCell(pos);

            SpreadRegion((pos.r + 1, pos.c), region);
            SpreadRegion((pos.r, pos.c + 1), region);
            SpreadRegion((pos.r - 1, pos.c), region);
            SpreadRegion((pos.r, pos.c - 1), region);
        }
    }
}
