public static class Day6_2024
{
    private static Dictionary<char, CardinalDirection> cardinalDirections = new Dictionary<char, CardinalDirection>();
    
    public static void Solution()
    {
        int time = DateTime.Now.Millisecond;
        string rawInput = Utils.GetInput("day6_2024.txt").Trim();

        string[] lines = rawInput.Split('\n', StringSplitOptions.TrimEntries);
        
        var PointMap = new Point[lines.Length][];

        cardinalDirections = new Dictionary<char, CardinalDirection>()
        {
            {'^', CardinalDirection.Up},
            {'>', CardinalDirection.Right},
            {'v', CardinalDirection.Down},
            {'<', CardinalDirection.Left},
        };

        (int x, int y) start = default;

        for (int y = 0; y < lines.Length; y++)
        {
            char[] points = lines[y].ToCharArray();
            PointMap[y] = new Point[points.Length];
            
            for (int x = 0; x < points.Length; x++)
            {
                var character = points[x];
                bool isStart = !(character is '.' or '#');
                if (isStart) start = (x, y);
                PointMap[y][x] = new Point(x, y, character == '#', !isStart ? CardinalDirection.None : cardinalDirections[character]);
            }
        }

        var guard = new Guard(start, PointMap);
        Point.StartCoordinates = start;
        
        PointMap[start.y][start.x].FaceDirection(CardinalDirection.Up);
        
        List<(int x, int y)> pointsVisited = new List<(int x, int y)>();
        while (!(guard.HasLooped || guard.HasLeft))
        {
            guard.TakeTurn();
            if (guard.CurrentPoint.Coordinates != start && !pointsVisited.Contains(guard.CurrentPoint.Coordinates))
                pointsVisited.Add(guard.CurrentPoint.Coordinates);
        }

        int pointsSeen = guard.PointsSeen;
        int loops = 0;

        foreach (var point in pointsVisited)
        {
            guard.Reset();
            guard.OverrideObstacle = point;
            
            while (!(guard.HasLooped || guard.HasLeft))
            {
                guard.TakeTurn();
                if (guard.HasLooped)
                    loops++;
            }
        }

        Console.WriteLine($"Part 1: {pointsSeen} | Part 2: {loops} | Time taken: {DateTime.Now.Millisecond - time}");
    }
}


public class Point
{
    public (int x, int y) Coordinates;
    public static (int x, int y) StartCoordinates;

    public bool IsObstacle;

    public bool HasBeenSeen = false;

    public void Reset()
    {
        HasBeenSeen = false;
        HasFacedDirection = GetNewDirectionDict();
    }
    
    public Dictionary<CardinalDirection, bool> HasFacedDirection = new Dictionary<CardinalDirection, bool>()
    {
        { CardinalDirection.Up , false},
        { CardinalDirection.Right , false},
        { CardinalDirection.Down , false},
        { CardinalDirection.Left , false},
    };

    private Dictionary<CardinalDirection, bool> GetNewDirectionDict()
    {
        return new Dictionary<CardinalDirection, bool>()
        {
            { CardinalDirection.Up , false},
            { CardinalDirection.Right , false},
            { CardinalDirection.Down , false},
            { CardinalDirection.Left , false},
        };
    }

    public Point(int x, int y, bool isObstacle, CardinalDirection dir)
    {
        Coordinates = new (x,y);
        IsObstacle = isObstacle;
    }

    public bool FaceDirection(CardinalDirection dir)
    {
        if (!HasFacedDirection[dir])
        {
            HasFacedDirection[dir] = true;
            return false;
        }

        return true;
    }
}

public class Guard
{
    public Point CurrentPoint;
    public int PointsSeen = 0;
    public bool HasLooped = false;
    public bool HasLeft = false;

    public (int x, int y) OverrideObstacle = (-1, -1);
    
    public Point[][] Points;
    
    public CardinalDirection CurrentDirection;

    public void Reset()
    {
        PointsSeen = 0;
        CurrentPoint = Points[Point.StartCoordinates.y][Point.StartCoordinates.x];
        HasLooped = false;
        HasLeft = false;
        CurrentDirection = CardinalDirection.Up;
        
        for (int y = 0; y < Points.Length; y++)
        {
            for (int x = 0; x < Points[y].Length; x++)
            {
                Points[y][x].Reset();
            }
        }
    }

    public Dictionary<CardinalDirection, (int x, int y)> CardinalToCoordinate =
        new Dictionary<CardinalDirection, (int x, int y)>()
        {
            { CardinalDirection.Up, (0, -1) },
            { CardinalDirection.Right, (1, 0) },
            { CardinalDirection.Down, (0, 1) },
            { CardinalDirection.Left, (-1, 0) },
        };

    public Dictionary<CardinalDirection, CardinalDirection> RightTurn =
        new Dictionary<CardinalDirection, CardinalDirection>()
        {
            { CardinalDirection.Up, CardinalDirection.Right },
            { CardinalDirection.Right, CardinalDirection.Down },
            { CardinalDirection.Down, CardinalDirection.Left },
            { CardinalDirection.Left, CardinalDirection.Up },
        };

    public Guard((int x, int y) coordinates, Point[][] points)
    {
        Points = points;
        CurrentPoint = Points[coordinates.y][coordinates.x];
        CurrentDirection = CardinalDirection.Up;
        PointsSeen = 1;
    }

    public void TakeTurn()
    {
        (int x, int y) forwardCoords = (CurrentPoint.Coordinates.x + CardinalToCoordinate[CurrentDirection].x, CurrentPoint.Coordinates.y + CardinalToCoordinate[CurrentDirection].y);
        bool pointExists = forwardCoords.y < Points.Length && forwardCoords.x < Points[0].Length && forwardCoords.x >= 0 && forwardCoords.y >= 0;
        if (!pointExists)
        {
            HasLeft = true;
            return;
        }
        Point forwardPoint = Points[forwardCoords.y][forwardCoords.x];

        if (!forwardPoint.IsObstacle && forwardPoint.Coordinates != OverrideObstacle)
        {
            CurrentPoint = forwardPoint;
            if (!CurrentPoint.HasBeenSeen)
            {
                PointsSeen++;
                CurrentPoint.HasBeenSeen = true;
            }
            if (CurrentPoint.FaceDirection(CurrentDirection))
            {
                HasLooped = true;
                return;
            }
        }
        else
        {
            CurrentDirection = RightTurn[CurrentDirection];
            CurrentPoint.FaceDirection(CurrentDirection);
        }
    }
}

[Flags]
public enum CardinalDirection
{
    None = 0,
    Up = 1<<1,
    Right = 1<<2,
    Down = 1<<3,
    Left = 1<<4,
}