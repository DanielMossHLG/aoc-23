public static class Day6_2024
{
    private static List<List<Point>> points = new List<List<Point>>();

    private static Dictionary<char, CardinalDirection> cardinalDirections = new Dictionary<char, CardinalDirection>();
    
    public static void Solution()
    {
        string rawInput = Utils.GetInput("day6_2024.txt").Trim();

        string[] lines = rawInput.Split('\n', StringSplitOptions.TrimEntries);

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
            
            for (int x = 0; x < points.Length; x++)
            {
                var character = points[x];
                bool isStart = !(character is '.' or '#');
                if (isStart) start = (x, y);
                _ = new Point(x, y, character == '#', !isStart ? CardinalDirection.None : cardinalDirections[character]);
            }
        }

        Guard guard = new Guard(start);

        while (!guard.HasLooped && !guard.HasLeft)
        {
            guard.TakeTurn();
            Console.WriteLine($"Current point: {guard.CurrentPoint} | Current direction: {guard.CurrentDirection}");
        }
        
        Console.WriteLine(guard.PointsSeen);
    }
}

public static class PointLookup
{
    public static Dictionary<(int x, int y), Point> Points = new Dictionary<(int x, int y), Point>();
}

public class Point
{
    public (int x, int y) Coordinates;

    public bool IsObstacle;

    public bool HasBeenSeen => HasFacedDirection[CardinalDirection.Up] || HasFacedDirection[CardinalDirection.Down] || HasFacedDirection[CardinalDirection.Left] || HasFacedDirection[CardinalDirection.Right];

    public Dictionary<CardinalDirection, bool> HasFacedDirection = new Dictionary<CardinalDirection, bool>()
    {
        { CardinalDirection.Up , false},
        { CardinalDirection.Right , false},
        { CardinalDirection.Down , false},
        { CardinalDirection.Left , false},
    };

    public Point(int x, int y, bool isObstacle, CardinalDirection dir)
    {
        Coordinates = new (x,y);
        IsObstacle = isObstacle;
        if (dir != CardinalDirection.None)
            HasFacedDirection[dir] = true;
        
        PointLookup.Points.Add((Coordinates.x,Coordinates.y), this);
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
    
    public CardinalDirection CurrentDirection;

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

    public Guard((int x, int y) coordinates)
    {
        CurrentPoint = PointLookup.Points[coordinates];
        CurrentDirection = CardinalDirection.Up;
        PointsSeen = 1;
    }

    public void TakeTurn()
    {
        var forwardCoords = (CurrentPoint.Coordinates.x + CardinalToCoordinate[CurrentDirection].x, CurrentPoint.Coordinates.y + CardinalToCoordinate[CurrentDirection].y);
        if (!PointLookup.Points.TryGetValue(forwardCoords, out var forwardPoint))
        {
            HasLeft = true;
            return;
        }
        if (!forwardPoint.IsObstacle)
        {
            CurrentPoint = forwardPoint;
            if (!CurrentPoint.HasBeenSeen)
                PointsSeen++;
            if (forwardPoint.FaceDirection(CurrentDirection))
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