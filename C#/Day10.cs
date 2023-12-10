public static class Day10
{
    private static string[] _lines;
    private static Dictionary<char, List<(int, int)>> _lookup = new Dictionary<char, List<(int, int)>>
    {
        {'|', new List<(int, int)>{(0, 1), (0, -1)}},
        {'-', new List<(int, int)>{(1, 0), (-1, 0)}},
        {'L', new List<(int, int)>{(0, -1), (1, 0)}},
        {'J', new List<(int, int)>{(-1, 0), (0, -1)}},
        {'7', new List<(int, int)>{(-1, 0), (0, 1)}},
        {'F', new List<(int, int)>{(1, 0), (0, 1)}},
    };
    public static void Solution()
    {
        string rawInput = Utils.GetInput("day10input1.txt");
        _lines = rawInput.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        (int x, int y) currentCoordinates = (-1, -1);
        (int x, int y) lastCoordinates = (-1, -1);

        for (int i = 0; i < _lines.Length; i++)
        {
            var index = _lines[i].IndexOf("S");
            if (index != -1)
            {
                currentCoordinates.y = i;
                currentCoordinates.x = index;
                break;
            }
        }
        int totalLength = 1;
        lastCoordinates = currentCoordinates;

        Action findFirstPipe = delegate
        {
            for (int y = -1; y <= 1; y++)
                for (int x = -1; x <= 1; x++)
                {
                    if (x == y) continue;

                    var check = CheckIndex(currentCoordinates.x + x, currentCoordinates.y + y);

                    if (check != '.' && _lookup[check].Contains((x, y)))
                    {
                        currentCoordinates.x += x;
                        currentCoordinates.y += y;
                        return;
                    }
                }
        };
        findFirstPipe();
        while (true)
        {
            var temp = FindNext(currentCoordinates, lastCoordinates);
            lastCoordinates = currentCoordinates;
            currentCoordinates = temp;
            totalLength++;
            if (_lines[currentCoordinates.y][currentCoordinates.x] == 'S') break;
        }
        Console.WriteLine($"Part 1: {totalLength / 2}");
    }

    private static (int x, int y) FindNext((int x, int y) currentCoordinates, (int x, int y) lastCoordinates)
    {
        var options = _lookup[_lines[currentCoordinates.y][currentCoordinates.x]];

        foreach ((int x, int y) option in options)
        {
            if (currentCoordinates.x + option.x != lastCoordinates.x || currentCoordinates.y + option.y != lastCoordinates.y)
            {
                currentCoordinates.x += option.x;
                currentCoordinates.y += option.y;
                return currentCoordinates;
            }
        }
        return currentCoordinates;
    }

    private static Char CheckIndex(int x, int y)
    {
        if (y < 0 || y >= _lines.Length || x < 0 || x >= _lines[y].Length) return '.';
        return _lines[y][x];
    }
}