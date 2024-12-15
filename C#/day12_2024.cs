public static class Day12_2024
{
    public static Dictionary<Direction, (int x, int y)> DirectionLookup = new Dictionary<Direction, (int x, int y)>()
    {
        [Direction.Up] = (0, 1),
        [Direction.Down] = (0, -1),
        [Direction.Left] = (-1, 0),
        [Direction.Right] = (1, 0)
    };

    public static List<List<Point>> points;
    
    public static void Solution()
    {
        var time = DateTime.Now;
        string rawInput = Utils.GetInput("day12_2024.txt").Trim();

        var lines = rawInput.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        
        points = new List<List<Point>>();
        List<Plot> plots = new List<Plot>();

        for (int y = 0; y < lines.Length; y++)
        {
            points.Add(new List<Point>());
            for (int x = 0; x < lines[y].Length; x++)
            {
                var point = new Point(x, y, lines[y][x]);
                points[y].Add(point);
            }
        }

        for (int y = 0; y < points.Count; y++)
        {
            for (int x = 0; x < points[y].Count; x++)
            {
                var point = points[y][x];
                if (!point.HasBeenPlaced)
                {
                    Plot plot = new Plot(point.Type);
                    plot.AddToPlot(point);
                    plots.Add(plot);
                }
            }
        }

        long result = 0;
        long result2 = 0;
        
        foreach (var plot in plots)
            result += plot.Points.Count * plot.GetPerimeter();
        

        foreach (var plot in plots)
            result2 += plot.Points.Count * plot.GetNumberOfSides();
            
        Console.WriteLine($"Part 1: {result} | Part 2: {result2} | Time: {DateTime.Now - time}");
    }

    public class Point(int x, int y, char c)
    {
        public (int x, int y) Coordinates = (x, y);
        public char Type = c;
        public bool HasBeenPlaced = false;

        public List<Direction> Edges = new List<Direction>();
    }

    public class Plot(char c)
    {
        public char Type = c;
        public HashSet<Point> Points = new HashSet<Point>();
        public HashSet<(int x, int y)> Coordinates = new HashSet<(int x, int y)>();

        public long Area => Points.Count;

        public void AddToPlot(Point point, Direction checkedDirection = Direction.None)
        {
            Points.Add(point);
            Coordinates.Add(point.Coordinates);
            point.HasBeenPlaced = true;
            
            if (checkedDirection != Direction.Down)
                FindNeighbours(point, Direction.Up);
            if (checkedDirection != Direction.Up)
                FindNeighbours(point, Direction.Down);
            if (checkedDirection != Direction.Right)
                FindNeighbours(point, Direction.Left);
            if (checkedDirection != Direction.Left)
                FindNeighbours(point, Direction.Right);
        }

        private void FindNeighbours(Point point, Direction direction)
        {
            (int x, int y) coords = point.Coordinates;
            coords.x += DirectionLookup[direction].x;
            coords.y += DirectionLookup[direction].y;

            if (coords.x < 0 || coords.y < 0 || coords.y >= points.Count || coords.x >= points[0].Count)
                return;
            
            Point pointBeingChecked = points[coords.y][coords.x];

            if (pointBeingChecked.HasBeenPlaced || pointBeingChecked.Type != Type)
                return;
            
            AddToPlot(pointBeingChecked, direction);
        }

        public long GetPerimeter()
        {
            long lengthPerim = 0;
            foreach (var point in Points)
            {
                lengthPerim += CheckPerimeter(point);
            }
            
            return lengthPerim;
        }

        public int CheckPerimeter(Point point)
        {
            int perim = 0;
            foreach (var (dir, coord) in DirectionLookup)
            {
                (int x, int y) coords = point.Coordinates;
                coords.x += coord.x;
                coords.y += coord.y;
                if (!Coordinates.Contains(coords))
                {
                    perim++;
                    point.Edges.Add(dir);
                }
            }

            return perim;
        }

        public long GetNumberOfSides()
        {
            if (points.Count == 1)
                return 4;

            long sides = 0;

            foreach (var point in Points)
            {
                if (point.Edges.Contains(Direction.Up))
                {
                    if (!Coordinates.Contains((point.Coordinates.x + 1, point.Coordinates.y)))
                        sides++;
                    else if (!points[point.Coordinates.y][point.Coordinates.x + 1].Edges.Contains(Direction.Up))
                        sides++;
                }
                
                if (point.Edges.Contains(Direction.Right))
                {
                    if (!Coordinates.Contains((point.Coordinates.x, point.Coordinates.y - 1)))
                        sides++;
                    else if (!points[point.Coordinates.y - 1][point.Coordinates.x].Edges.Contains(Direction.Right))
                        sides++;
                }
                
                if (point.Edges.Contains(Direction.Down))
                {
                    if (!Coordinates.Contains((point.Coordinates.x - 1, point.Coordinates.y)))
                        sides++;
                    else if (!points[point.Coordinates.y][point.Coordinates.x - 1].Edges.Contains(Direction.Down))
                        sides++;
                }
                
                if (point.Edges.Contains(Direction.Left))
                {
                    if (!Coordinates.Contains((point.Coordinates.x, point.Coordinates.y + 1)))
                        sides++;
                    else if (!points[point.Coordinates.y + 1][point.Coordinates.x].Edges.Contains(Direction.Left))
                        sides++;
                }
            }

            return sides;
        }
    }

    public enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }
}