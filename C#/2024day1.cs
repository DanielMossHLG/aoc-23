public static class Day1_2024
{
	public static void Solution()
	{
		string rawInput = Utils.GetInput("2024day1_1.txt");
		
		string[] lines = rawInput.Split('\n');
		

		List<int> list1 = new List<int>();
		List<int> list2 = new List<int>();
		
		List<int> unsortedList1 = new List<int>();
		
		string firstLine = lines[0].Trim();
		var firstParsed = firstLine.Split();
			
		if (string.IsNullOrEmpty(firstLine) || firstLine == " ")
			return;
			
		unsortedList1.Add(int.Parse(firstParsed[0]));
		list1.Add(int.Parse(firstParsed[0]));
		list2.Add(int.Parse(firstParsed[^1]));
		
		
		for (int i = 1; i < lines.Length; i++)
		{
			string line = lines[i].Trim();
			var parsed = line.Split();
			
			if (string.IsNullOrEmpty(line) || line == " ")
				return;

			var new1 = int.Parse(parsed[0]);
			var new2 = int.Parse(parsed[^1]);

			unsortedList1.Add(new1);
			AddToList(ref list1, new1);
			AddToList(ref list2, new2);
		}
		
		long result1 = 0;
		long result2 = 0;
		Console.WriteLine($"Part 1 {list1.Count} | unsorted: {unsortedList1.Count}");

		for (int i = 0; i < list1.Count; i++)
		{
			result1 += (int)MathF.Abs(list1[i] - list2[i]);
			
			int currentI = unsortedList1[i];
			if (list2.LastIndexOf(currentI) >= 0)
				result2 += currentI * (list2.LastIndexOf(currentI) - list2.IndexOf(currentI) + 1);
		}
		Console.WriteLine($"Part 1: {result1} | Part 2: {result2}");
		
		
	}

	private static void AddToList(ref List<int> list, int value)
	{
		if (value <= list[0])
		{
			list.Insert(0, value);
			return;
		}
		if (value >= list[^1])
		{
			list.Add(value);
			return;
		}
		
		for (int i = 0; i < list.Count; i++)
		{
			if (value <= list[i])
			{
				list.Insert(i, value);
				return;
			}
		}
	}
}