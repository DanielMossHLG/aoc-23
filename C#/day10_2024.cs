public static class Day10_2024
{
    public static void Solution()
    {
        var time = DateTime.Now;
        string rawInput = Utils.GetInput("day10_2024.txt").Trim();
        
        List<Node> startingNodes = new List<Node>();
        
        var lines = rawInput.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        for (int y = 0; y < lines.Length; y++)
        {
            var list = new List<Node>();
            TopoMap.Add(list);
            for (int x = 0; x < lines[y].Length; x++)
            {
                TopoMap[y].Add(new Node(x, y, int.Parse(lines[y][x].ToString())));
                if (TopoMap[y][x].Value == 0)
                    startingNodes.Add(TopoMap[y][x]);
            }
        }

        int result = 0;
        int result2 = 0;

        foreach (var node in startingNodes)
        {
            result += node.GetPeaksThatCanBeReached().Count;
            result2 += node.PathsToPeaks.Count;
        }
        
        Console.WriteLine($"Part 1: {result} | Part 2: {result2} | Time taken: {DateTime.Now - time}");
    }

    public static List<List<Node>> TopoMap = new List<List<Node>>();

    public class Node(int x, int y, int value)
    {
        public (int x, int y) Coordinates = (x,y);
        public int Value = value;
        
        public List<string> PathsToPeaks = new List<string>();

        public bool TryDescend(int originalX, int originalY, string pathToPeaks)
        {
            bool canLeft = CanDescend(-1, 0);
            bool canRight = CanDescend(1, 0);
            bool canDown = CanDescend(0, -1);
            bool canUp = CanDescend(0, 1);
            
            if (canLeft)
                if (TopoMap[y][x - 1].Value == 0 && y == originalY && x - 1 == originalX && !TopoMap[originalY][originalX].PathsToPeaks.Contains(pathToPeaks + Coordinates))
                {
                    pathToPeaks += Coordinates;
                    TopoMap[originalY][originalX].PathsToPeaks.Add(pathToPeaks);
                    return true;
                }
            if (canRight)
                if (TopoMap[y][x + 1].Value == 0 && y == originalY && x + 1 == originalX && !TopoMap[originalY][originalX].PathsToPeaks.Contains(pathToPeaks + Coordinates))
                {
                    pathToPeaks += Coordinates;
                    TopoMap[originalY][originalX].PathsToPeaks.Add(pathToPeaks);
                    return true;
                }
            if (canDown)
                if (TopoMap[y - 1][x].Value == 0 && y - 1 == originalY && x == originalX && !TopoMap[originalY][originalX].PathsToPeaks.Contains(pathToPeaks + Coordinates))
                {
                    pathToPeaks += Coordinates;
                    TopoMap[originalY][originalX].PathsToPeaks.Add(pathToPeaks);
                    return true;
                }
            
            if (canUp)
                if (TopoMap[y + 1][x].Value == 0 && y + 1 == originalY && x == originalX && !TopoMap[originalY][originalX].PathsToPeaks.Contains(pathToPeaks + Coordinates))
                {
                    pathToPeaks += Coordinates;
                    TopoMap[originalY][originalX].PathsToPeaks.Add(pathToPeaks);
                    return true;
                }


            return (canLeft && TopoMap[y][x - 1].TryDescend(originalX, originalY, pathToPeaks + Coordinates))
                   || (canRight && TopoMap[y][x + 1].TryDescend(originalX, originalY, pathToPeaks + Coordinates))
                   || (canDown && TopoMap[y - 1][x].TryDescend(originalX, originalY, pathToPeaks + Coordinates))
                   || (canUp && TopoMap[y + 1][x].TryDescend(originalX, originalY, pathToPeaks + Coordinates));
        }

        public bool CanDescend(int xDir, int yDir)
        {
            return GetNeighbour(xDir, yDir) == Value - 1;
        }

        public int GetNeighbour(int x, int y)
        {
            int newY = Coordinates.y + y;
            int newX = Coordinates.x + x;
            if (newY < 0 || newY >= TopoMap.Count || newX < 0 || newX >= TopoMap[0].Count)
                return -10;
            return TopoMap[newY][newX].Value;
        }

        public List<Node> GetPeaksThatCanBeReached()
        {
            List<Node> peaks = new List<Node>();
            
            int bottomX = (int)MathF.Max(Coordinates.x - 10, 0);
            int topX = (int)MathF.Min(Coordinates.x + 10, TopoMap[0].Count);
            
            int bottomY = (int)MathF.Max(Coordinates.y - 10, 0);
            int topY = (int)MathF.Min(Coordinates.y + 10, TopoMap.Count);

            for (int y = bottomY; y < topY; y++)
            {
                for (int x = bottomX; x < topX; x++)
                {
                    if (TopoMap[y][x].Value == 9)
                    {
                        if (TopoMap[y][x].TryDescend(Coordinates.x, Coordinates.y, ""))
                        {
                            peaks.Add(TopoMap[y][x]);
                        }
                        while (TopoMap[y][x].TryDescend(Coordinates.x, Coordinates.y, ""))
                        { }
                    }
                }
            }

            return peaks;
        }
    }
}