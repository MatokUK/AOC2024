using System.Text.RegularExpressions;

namespace AOC2024
{
    internal class Day15: IAdventSolution
    {
        enum Move
        {
            UP = '^',
            DOWN = 'v',
            LEFT = '<',
            RIGHT = '>',
        }

        class Robot
        {
            private int row;
            private int col;            

            public Robot(int x, int y)
            {
                this.row = x;
                this.col = y;
            }

            public (int, int) GetPos()
            {
                return (row, col);
            }

            public (int x, int y) NextMove(Move move)
            {
                return move switch
                {
                    Move.UP => (row - 1, col),
                    Move.DOWN => (row + 1, col),
                    Move.RIGHT => (row, col + 1),
                    Move.LEFT => (row, col - 1),
                };
            }

            public static (int x, int y) NextMove2(Move move, int r, int c)
            {
                return move switch
                {
                    Move.UP => (r - 1, c),
                    Move.DOWN => (r + 1, c),
                    Move.RIGHT => (r, c + 1),
                    Move.LEFT => (r, c - 1),
                };
            }

            public void MakeMove(Move move)
            {
                var next = NextMove(move);
                this.row = next.x;
                this.col = next.y;
            }
        }

        class Warehouse
        {
            private List<(int, int)> obstacles = new();
            private List<(int, int)> goods = new();
            private Robot robot;
            private int totalRows;
            private int totalCols;

            public Warehouse(List<string> lines, bool scaleUp = false)
            {
                totalRows = lines.Count;
                totalCols = lines[0].Length;

                for (int r = 0; r < totalRows; r++)
                {
                    for (int c = 0; c < totalCols; c++)
                    {
                        int scaleUpC = c * 2;
                        if (lines[r][c] == '#') 
                        {                            
                            if (scaleUp)
                            {
                                obstacles.Add((r, scaleUpC));
                                obstacles.Add((r, scaleUpC+1));
                            }
                            else
                            {
                                obstacles.Add((r, c));
                            }
                        }
                        else if (lines[r][c] == 'O')
                        {
                            if (scaleUp)
                            {
                                goods.Add((r, scaleUpC));
                                goods.Add((r, scaleUpC + 1));
                            }
                            else
                            {
                                goods.Add((r, c));
                            }                            
                        }
                        else if (lines[r][c] == '@')
                        {
                            robot = new Robot(r, c);
                        }
                    }

                }

                if (scaleUp)
                {
                    totalCols *= 2;
                }
            }  
            
            public void MoveRobot(Move move)
            {
                var next = robot.NextMove(move);
                
                if (obstacles.Contains(next)) // do nothing
                {
                    return;
                }

                if (!goods.Contains(next))
                {
                    robot.MakeMove(move);
                    return;
                }

                List<(int, int)> goodsToMove = new();

                var pos = robot.GetPos();
                bool canMove = false;
                do
                {
                    var kkt = Robot.NextMove2(move, pos.Item1, pos.Item2);
                    if (!goods.Contains(kkt))
                    {
                        canMove = !obstacles.Contains(kkt);
                        break;
                    }
                    goodsToMove.Add(kkt);
                    pos = kkt;
                } while (true);

                if (canMove)
                {
                    goods.RemoveAll(x => goodsToMove.Contains(x));
                    goodsToMove =  goodsToMove.Select(good =>
                    {
                        return move switch
                        {
                            Move.UP => (good.Item1 - 1, good.Item2),
                            Move.DOWN => (good.Item1 + 1, good.Item2),
                            Move.RIGHT => (good.Item1, good.Item2 + 1),
                            Move.LEFT => (good.Item1, good.Item2 - 1)
                        };
                    }).ToList();
                    goods.AddRange(goodsToMove);
                    robot.MakeMove(move);
                }
            }

            public long GPS()
            {
                return goods.Select(pos => pos.Item1 * 100 + pos.Item2).Sum();
            }

            public void Visualize()
            {
                var tmp = robot.NextMove(Move.RIGHT);

                for (int r = 0; r < totalRows; r++)
                {
                    for (int c = 0; c < totalCols; c++)
                    {
                        if (obstacles.Contains((r, c)))
                        {
                            Console.Write('#');
                        }
                        else if (goods.Contains((r, c)))
                        {
                            Console.Write('O');
                        }
                        else if (tmp.x == r && tmp.y -1 == c)
                        {
                            Console.Write('R');
                        }
                        else
                        {
                            Console.Write(' ');
                        }
                    }
                    Console.WriteLine();

                }
            }
        }

        public (long, long) Execute(string[] lines)
        {
            Warehouse warehouse = ReadWarehouse(lines);
            List<Move> moves = ReadMoves(lines);

            foreach (Move move in moves)
            {
             //   Console.WriteLine("moving " + move);
                warehouse.MoveRobot(move);
            }

            return (warehouse.GPS(), 0);
        }

        private Warehouse ReadWarehouse(string[] lines)
        {
            List<string> warehouseLines = new();

            foreach (string line in lines)
            {
                if (line.Contains('#'))
                {
                    warehouseLines.Add(line);
                    continue;
                }

                break;                
            }

            return new Warehouse(warehouseLines);
        }

        private List<Move> ReadMoves(string[] lines)
        {
            var moves = new List<Move>();

            foreach (string line in lines)
            {
                if (line.Contains('^') || line.Contains('>') || line.Contains('<') || line.Contains('v'))
                {
                    moves.AddRange(
                        line.Select(x =>
                        {
                            return x switch
                            {
                                '^' => Move.UP,
                                'v' => Move.DOWN,
                                '>' => Move.RIGHT,
                                '<' => Move.LEFT,
                            };
                        }).ToList()
                    );                    
                }
            }

            return moves;
        }
    }
}
