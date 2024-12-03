public static class Day3_2024
{
	private static int result = 0;
	private static int result2 = 0;
	
	public static void Solution()
	{
		string rawInput = Utils.GetInput("day3_2024.txt").Trim();

		CheckForValidOperation(rawInput);
		CheckForValidOperation(rawInput, true);
	}

	private static void CheckForValidOperation(string input, bool checkDoDont = false)
	{
		int indexOfOperation = input.IndexOf("mul(");
		if (indexOfOperation == -1)
		{
			DisplayResults(checkDoDont);
			return;
		}

		if (checkDoDont)
		{
			int indexOfDont = input.IndexOf("don't()");

			if (indexOfDont > -1 && indexOfDont < indexOfOperation)
			{
				string nextValidString = input.Substring(indexOfDont);
				int nextDoInput = nextValidString.IndexOf("do()");

				if (nextDoInput == -1)
				{
					DisplayResults(checkDoDont);
					return;
				}
				
				CheckForValidOperation(nextValidString.Substring(nextDoInput), checkDoDont);
				return;
			}
		}

		string newInput = input.Substring(indexOfOperation + 4);
		int indexOfEnd = newInput.IndexOf(')');
		if (indexOfEnd == -1)
		{
			CheckForValidOperation(newInput, checkDoDont);
			return;
		}
		string stringToCheck = newInput.Substring(0, indexOfEnd);

		if (stringToCheck.IndexOf(',') == -1)
		{
			CheckForValidOperation(newInput, checkDoDont);
			return;
		}
		
		string[] operations = stringToCheck.Split(',');
		if (operations.Length != 2 || operations[0].Length > 3 || operations[1].Length > 3)
		{
			CheckForValidOperation(newInput, checkDoDont);
			return;
		}

		int value1, value2;
		if (!int.TryParse(operations[0], out value1) || !int.TryParse(operations[1], out value2))
		{
			CheckForValidOperation(newInput, checkDoDont);
			return;
		}
		
		if (checkDoDont)
			result2 += value1 * value2;
		else
			result += value1 * value2;
		CheckForValidOperation(newInput, checkDoDont);
	}

	private static void DisplayResults(bool checkDoDont)
	{
		Console.WriteLine(checkDoDont ? $"Part 2: {result2}" : $"Part 1: {result}");
	}
}