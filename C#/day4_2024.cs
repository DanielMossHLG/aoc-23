public static class Day4_2024
{
	private static string k_stringToExpect = "XMAS";
	
	private static List<List<char>> _input = new List<List<char>>();

	private static Dictionary<Direction, (int xMult, int yMult)> DirectionLookup =
		new Dictionary<Direction, (int xMult, int yMult)>()
		{
			{Direction.Up, (0, -1)},
			{Direction.Left, (-1, 0)},
			{Direction.Down, (0, 1)},
			{Direction.Right, (1, 0)},
			
			{Direction.LU, (-1, -1)},
			{Direction.LD, (-1, 1)},
			{Direction.RU, (1, -1)},
			{Direction.RD, (1, 1)},
		};
	
	public static void Solution()
	{
		string rawInput = Utils.GetInput("day4_2024.txt").Trim();

		List<string> lines = rawInput.Split('\n').ToList();
		

		for (int i = 0; i < lines.Count; i++)
		{
			_input.Add(lines[i].ToCharArray().ToList());
		}

		int part1Result = 0;
		
		for (int y = 0; y < _input.Count; y++)
		{
			for (int x = 0; x < _input[y].Count; x++)
			{
				if (_input[y][x] != 'X')
					continue;
				
				if (CheckDirection(y, x, Direction.Up))
					part1Result++;
				if (CheckDirection(y, x, Direction.Down))
					part1Result++;
				if (CheckDirection(y, x, Direction.Left))
					part1Result++;
				if (CheckDirection(y, x, Direction.Right))
					part1Result++;
				if (CheckDirection(y, x, Direction.LU))
					part1Result++;
				if (CheckDirection(y, x, Direction.LD))
					part1Result++;
				if (CheckDirection(y, x, Direction.RU))
					part1Result++;
				if (CheckDirection(y, x, Direction.RD))
					part1Result++;
			}

		}

		Console.WriteLine($"Part 1: {part1Result}");
	}
	

	private static bool CheckDirection(int y, int x, Direction direction)
	{
		(int xMulti, int yMulti) multipliers = DirectionLookup[direction];

		if (multipliers.xMulti < 0 && x < 3) return false;
		if (multipliers.xMulti > 0 && x + 3 > _input[0].Count - 1) return false;

		if (multipliers.yMulti < 0 && y < 3) return false;
		if (multipliers.yMulti > 0 && y + 3 > _input.Count - 1) return false;
		
		for (int i = 1; i < 4; i++)
		{
			if (_input[y + i * multipliers.yMulti][x + i * multipliers.xMulti] != k_stringToExpect[i])
			{
				
				return false;
			}
		}
		Console.WriteLine($"X: {x} Y: {y} | {_input[y][x]}");
		return true;
	}
}

public enum Direction
{
	Up,
	Down,
	Left,
	Right,
	LU,
	LD,
	RU,
	RD
}