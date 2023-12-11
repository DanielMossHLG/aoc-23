using System.Diagnostics;

public static class Day10
{
    private static string[] _lines = Array.Empty<string>();
    private static Dictionary<(int x, int y), Char> _blockers = new Dictionary<(int x, int y), char>();
    private static List<(int x, int y)> _freeTiles = new List<(int x, int y)>();
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
        (int x, int y) lastCoordinates = currentCoordinates;

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
            _blockers.Add((currentCoordinates.x, currentCoordinates.y), _lines[currentCoordinates.y][currentCoordinates.x]);
            var temp = FindNext(currentCoordinates, lastCoordinates);
            lastCoordinates = currentCoordinates;
            currentCoordinates = temp;
            totalLength++;
            if (_lines[currentCoordinates.y][currentCoordinates.x] == 'S') break;
        }
        _blockers.Add((currentCoordinates.x, currentCoordinates.y), _lines[currentCoordinates.y][currentCoordinates.x]);
        Console.WriteLine($"Part 1: {totalLength / 2}");

        while (true)
        {
            int count = _freeTiles.Count;
            for (int y = 0; y < _lines.Length; y++)
                for (int x = 0; x < _lines[y].Length; x++)
                    CheckFree(x, y);
            
            if (_freeTiles.Count == count) break;
        }

        var encased = (_lines.Length * _lines[0].Length) - _blockers.Count - _freeTiles.Count;

        Console.WriteLine($"Total tiles: {_lines.Length * _lines[0].Length} | Blockers: {_blockers.Count} | Free tiles: {_freeTiles.Count} | Encapsulated: {encased}");
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

    private static void CheckFree(int x, int y)
    {
        if (_blockers.ContainsKey((x, y)) || _freeTiles.Contains((x, y))) return;

        //Check edges
        if (x == 0 || y == 0 || x == _lines[y].Length - 1 || y == _lines.Length - 1)
        {
            _freeTiles.Add((x, y));
            return;
        }

        for (int i = x; i >= 0; i--)
        {
            if (_blockers.ContainsKey((i, y)))
            {
                if (_blockers[(i, y)] == '|') break;
                if (_blockers[(i, y)] == 'J')
                {
                    if (CheckBelowLeft(i, y))
                    {
                        Console.WriteLine("Checked below left");
                        _freeTiles.Add((x, y));
                        return;
                    }
                    break;
                }
                if (_blockers[(i, y)] == '7')
                {
                    if (CheckAboveLeft(i, y))
                    {
                        Console.WriteLine("Checked above left");
                        _freeTiles.Add((x, y));
                        return;
                    }
                    break;
                }
            }
            if (_freeTiles.Contains((i, y))) 
            {
                _freeTiles.Add((x, y));
                return;
            }
        }
        for (int i = x; i < _lines[y].Length; i++)
        {
            if (_blockers.ContainsKey((i, y)))
            {
                if (_blockers[(i, y)] == '|') break;
                if (_blockers[(i, y)] == 'L')
                {
                    if (CheckBelowRight(i, y))
                    {
                        Console.WriteLine("Checked below right");
                        _freeTiles.Add((x, y));
                        return;
                    }
                    break;
                }
                if (_blockers[(i, y)] == 'F')
                {
                    if (CheckAboveRight(i, y))
                    {
                        Console.WriteLine("Checked above right");
                        _freeTiles.Add((x, y));
                        return;
                    }
                    break;
                }
            }
            if (_freeTiles.Contains((i, y))) 
            {
                _freeTiles.Add((x, y));
                return;
            }
        }
        for (int i = y; i >= 0; i--)
        {
            if (_blockers.ContainsKey((x, i)))
            {
                if (_blockers[(x, i)] == '-') break;
                if (_blockers[(x, i)] == 'J')
                {
                    if (CheckRightUp(x, i))
                    {
                        Console.WriteLine("Checked right up");
                        _freeTiles.Add((x, y));
                        return;
                    }
                    break;
                }
                if (_blockers[(x, i)] == 'L')
                {
                    if (CheckLeftUp(x, i))
                    {
                        Console.WriteLine("Checked left up");
                        _freeTiles.Add((x, y));
                        return;
                    }
                    break;
                }
            }
            if (_freeTiles.Contains((x, i))) 
            {
                _freeTiles.Add((x, y));
                return;
            }
        }
        for (int i = y; i <_lines.Length; i++)
        {
            if (_blockers.ContainsKey((x, i)))
            {
                if (_blockers[(x, i)] == '-') break;
                if (_blockers[(x, i)] == '7')
                {
                    if (CheckRightDown(x, i))
                    {
                        Console.WriteLine($"Checked right down");
                        _freeTiles.Add((x, y));
                        return;
                    }
                    break;
                }
                if (_blockers[(x, i)] == 'J')
                {
                    if (CheckLeftDown(x, i))
                    {
                        Console.WriteLine("Checked left down");
                        _freeTiles.Add((x, y));
                        return;
                    }
                    break;
                }
            }
            if (_freeTiles.Contains((x, i))) 
            {
                _freeTiles.Add((x, y));
                return;
            }
        }
    }

    private static bool CheckBelowLeft(int x, int y)
    {
        while (x >= 0)
        {
            Console.WriteLine($"Checking below left: {x},{y} | Currently: {_lines[y][x]}");
            if (_freeTiles.Contains((x, y))) return true;
            if (_blockers.ContainsKey((x, y)) && _blockers[(x, y)] is '|' or '7' or 'F') return false;
            x--;
        }
        return true;
    }
    
    private static bool CheckAboveLeft(int x, int y)
    {
        while (x >= 0)
        {
            if (_freeTiles.Contains((x, y))) return true;
            if (_blockers.ContainsKey((x, y)) && _blockers[(x, y)] is '|' or 'J' or 'L') return false;
            x--;
        }
        return true;
    }
    
    private static bool CheckBelowRight(int x, int y)
    {
        while (x < _lines[y].Length)
        {
            if (_freeTiles.Contains((x, y))) return true;
            if (_blockers.ContainsKey((x, y)) && _blockers[(x, y)] is '|' or '7' or 'F') return false;
            x++;
        }
        return true;
    }
    
    private static bool CheckAboveRight(int x, int y)
    {
        while (x < _lines[y].Length)
        {
            if (_freeTiles.Contains((x, y))) return true;
            if (_blockers.ContainsKey((x, y)) && _blockers[(x, y)] is '|' or 'J' or 'L') return false;
            x++;
        }
        return true;
    }
    
    private static bool CheckRightUp(int x, int y)
    {
        while (y >= 0)
        {
            if (_freeTiles.Contains((x, y))) return true;
            if (_blockers.ContainsKey((x, y)) && _blockers[(x, y)] is '-' or 'L' or 'F') return false;
            y--;
        }
        return true;
    }
    
    private static bool CheckLeftUp(int x, int y)
    {
        while (y >= 0)
        {
            if (_freeTiles.Contains((x, y))) return true;
            if (_blockers.ContainsKey((x, y)) && _blockers[(x, y)] is '-' or 'J' or '7') return false;
            y--;
        }
        return true;
    }
    
    private static bool CheckRightDown(int x, int y)
    {
        while (y < _lines.Length)
        {
            if (_freeTiles.Contains((x, y))) return true;
            if (_blockers.ContainsKey((x, y)) && _blockers[(x, y)] is '-' or 'L' or 'F') return false;
            y++;
        }
        return true;
    }
    
    private static bool CheckLeftDown(int x, int y)
    {
        while (y < _lines.Length)
        {
            if (_freeTiles.Contains((x, y))) return true;
            if (_blockers.ContainsKey((x, y)) && _blockers[(x, y)] is '-' or 'J' or '7') return false;
            y++;
        }
        return true;
    }
}