public static class Day5_2024
{
	private static Dictionary<string, List<string>> precedingPageMapper = new Dictionary<string, List<string>>();
	private static int part1Result = 0;
	
	public static void Solution()
	{
		string rawInput = Utils.GetInput("day5_2024.txt").Trim();

		string[] lines = rawInput.Split("\n\r");

		string rawRules = lines[0].Trim();
		string rawPages = lines[1].Trim();

		string[] rules = rawRules.Split('\n');
		string[] pages = rawPages.Split('\n');

		foreach (var rule in rules)
		{
			string[] splitRules = rule.Trim().Split('|');

			if (!precedingPageMapper.ContainsKey(splitRules[1]))
				precedingPageMapper.Add(splitRules[1], new List<string>(){splitRules[0]});
			else
				precedingPageMapper[splitRules[1]].Add(splitRules[0]);
		}

		foreach (var page in pages)
		{
			string[] splitPages = page.Trim().Split(',');
			CheckValidPage(splitPages.ToList());
		}
		
		Console.WriteLine(part1Result);
	}

	private static void CheckValidPage(List<string> pages)
	{
		if (ValidatePageOrder(pages))
			part1Result += int.Parse(pages[pages.Count / 2]);
		else
			ReorderPages(pages);
	}

	private static void ReorderPages(List<string> pagesOriginal)
	{
		List<string> pages = new List<string>();
		
		for (int i = 0; i < pages.Count; i++)
		{
			foreach (string checker in precedingPageMapper[pagesOriginal[0]])
			{
				//if (pages.Contains())
			}
		}
	}

	private static bool ValidatePageOrder(List<string> pages)
	{
		for (int i = 0; i < pages.Count; i++)
		{
			if (!precedingPageMapper.ContainsKey(pages[i]))
				continue;
			

			foreach (var prevPage in precedingPageMapper[pages[i]])
			{
				int prevIndex = pages.IndexOf(prevPage);
				if (prevIndex > i)
					return false;
			}
		}

		return true;
	}
}