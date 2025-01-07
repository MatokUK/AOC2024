
using AOC2024;
using System.Reflection.Metadata.Ecma335;

public class Day06: IAdventSolution
{
    private enum Direction {
        UP,
        DOWN,
        LEFT,
        RIGTH
    };

    private List<(int, int)> visited = new();
    private List<string> maze;

    private (int, int) initPos;

    private (int, int) pos;

    private (int, int) obstacle;
    private Direction direction;

    public int PathLength {
        get {
            return this.visited.Count;
        }
    }

    public (long, long) Execute(string[] lines)
    {
        this.maze = lines.Select(line => line).ToList();
        this.pos = FindStart();
        this.initPos = pos;
        this.direction = Direction.UP;
        this.visited.Add(this.pos);
        this.obstacle = (-5, -5);

        while (!IsOnEdge()) {
            Move();
        }

        return (this.visited.Count, Cycles(10000));
    }

    private (int, int) FindStart()
    {
        int row = 0;
        foreach (var line in maze) {
            if (line.Contains('^')) {
                return (row, line.IndexOf('^'));
            }
            row++;
        }

        return (0, 0);
    }

    public bool Move()
    {
        if (FacingObstacle()) {
            TurnRight();
            if (FacingObstacle()) // corner
            {
                TurnRight();
                if (FacingObstacle())
                {
                    throw new Exception("Problem");
                }
            }
        }

        return DoMove();
    }

    private bool DoMove()
    {
        this.pos = NextPos(this.pos);

        if (!this.visited.Contains(this.pos))
        {
            this.visited.Add(this.pos);

            return false;
        }

        return true;
    }

    public long Cycles(int max)
    {
        int cycles = 0;

        for (int r = 0; r < maze.Count; ++r) 
        {
            for (int c = 0; c < maze.ElementAt(r).Length; ++c) 
            {
                if (maze.ElementAt(r).ElementAt(c) == '#') {
                    continue;
                }

                this.obstacle = (r, c);
                this.visited.Clear();
                int steps = 0;
                this.pos = initPos;
                this.direction = Direction.UP;
                this.visited.Add(this.pos);
                int sequence = 0;

                while(!IsOnEdge()) {
                    ++steps;
                    if (Move())
                    {
                        sequence++;
                    }
                    else
                    {
                        sequence = 0;
                    }

                    if (steps >= max) {
                        ++cycles;
                        break;
                    }

                    /*if (sequence > 50)
                    {
                        ++cycles;
                        break;
                    }*/
                }
            }
        }

        return cycles;
    }

    private bool IsOnEdge()
    {
        switch (this.direction){
            case Direction.UP:
                return pos.Item1 == 0;

            case Direction.DOWN:
                return pos.Item1 == this.maze.Count - 1;

            case Direction.LEFT:
                return pos.Item2 == 0;
    
            case Direction.RIGTH:
                return pos.Item2 == this.maze.ElementAt(0).Length - 1;
        }

        return false;
    }


    private bool FacingObstacle()
    {
        var nextPos = NextPos(this.pos);

        return this.maze.ElementAt(nextPos.Item1).ElementAt(nextPos.Item2) == '#' || (nextPos.Item1 == obstacle.Item1 && nextPos.Item2 == obstacle.Item2);
    }

    private void TurnRight()
    {
         switch (this.direction){
            case Direction.UP:
                this.direction = Direction.RIGTH;
                break;

            case Direction.DOWN:
                this.direction = Direction.LEFT;
                break;

            case Direction.LEFT:
                this.direction = Direction.UP;
                break;
    
            case Direction.RIGTH:
                this.direction = Direction.DOWN;
                break;
        }
    }

    private (int, int) NextPos((int, int) currentPos)
    {
        switch (this.direction){
            case Direction.UP:
                return (currentPos.Item1 - 1, currentPos.Item2);

            case Direction.DOWN:
                return (currentPos.Item1 + 1, currentPos.Item2);

            case Direction.LEFT:
                return (currentPos.Item1, currentPos.Item2 - 1);
    
            case Direction.RIGTH:
            default:
                return (currentPos.Item1, currentPos.Item2 + 1);
        }
    }

    public String toString()
    {
        String o = "";

        for (int r = 0; r < maze.Count; ++r) 
        {
            for (int c = 0; c < maze.ElementAt(r).Length; ++c) 
            {
                if (visited.Contains((r, c))) {
                    o += "X";
                } else {
                    o += maze.ElementAt(r).ElementAt(c);
                }
            }
            o += "\n";
        }


        return o;
    }
}