public static class Day5_2024
{
	//private static Dictionary<string, List<string>> precedingPageMapper = new Dictionary<string, List<string>>();
	private static int part1Result = 0;
	private static int part2Result = 0;
	
	

	private static Dictionary<string, PageNode> pageNodes = new Dictionary<string, PageNode>();
	
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

			if (!pageNodes.ContainsKey(splitRules[0]))
			{
				pageNodes.Add(splitRules[0], new PageNode(splitRules[0]));
			}
			if (!pageNodes.ContainsKey(splitRules[1]))
			{
				pageNodes.Add(splitRules[1], new PageNode(splitRules[1]));
			}

			pageNodes[splitRules[0]].AddLater(pageNodes[splitRules[1]]);
			

			// if (!precedingPageMapper.ContainsKey(splitRules[1]))
			// 	precedingPageMapper.Add(splitRules[1], new List<string>(){splitRules[0]});
			// else
			// 	precedingPageMapper[splitRules[1]].Add(splitRules[0]);
		}

		foreach (var page in pages)
		{
			string[] splitPages = page.Trim().Split(',');
			CheckValidPage(splitPages.ToList());
		}
		
		Console.WriteLine($"Part 1: {part1Result} | Part 2: {part2Result}");
	}

	private static void CheckValidPage(List<string> pages)
	{
		if (ValidatePageOrder(pages))
			part1Result += int.Parse(pages[pages.Count / 2]);
		else
			ReorderLoop(pages);
	}

	private static void ReorderLoop(List<string> pages)
	{
		while (!ValidatePageOrder(pages))
			ReorderPages(ref pages);

		string pageTxt = "";

		foreach (string page in pages)
			pageTxt += page + ",";
		
		Console.WriteLine(pageTxt);

		part2Result += int.Parse(pages[pages.Count / 2]);
	}

	private static void ReorderPages(ref List<string> pagesOriginal)
	{
		int wrongIndex = -1;
		
		for (int i = 0; i < pagesOriginal.Count; i++)
		{
			PageNode currentNode = pageNodes[pagesOriginal[i]];
			
			if (currentNode.EarlierPages.Count == 0)
				continue;
			
			foreach (var prevPage in currentNode.EarlierPages)
			{
				int prevIndex = pagesOriginal.IndexOf(prevPage.ID);
				if (prevIndex > i)
				{
					wrongIndex = i;
				}
			}

			if (wrongIndex != -1)
				break;
		}

		var pages = pagesOriginal;
		var reordering = pages[wrongIndex];
		pages.RemoveAt(wrongIndex);

		for (int x = 0; x < pages.Count; x++)
		{
			if (pageNodes[reordering].ValidatePlacement(pages, x, pageNodes))
			{
				pages.Insert(x, reordering);
				return;
			}
		}
		pages.Add(reordering);
	}

	private static bool ValidatePageOrder(List<string> pages)
	{
		for (int i = 0; i < pages.Count; i++)
		{
			PageNode currentNode = pageNodes[pages[i]];
			
			if (currentNode.EarlierPages.Count == 0)
				continue;
			

			foreach (var prevPage in currentNode.EarlierPages)
			{
				int prevIndex = pages.IndexOf(prevPage.ID);
				if (prevIndex > i)
					return false;
			}
		}

		return true;
	}
}

public class PageNode
{
	public PageNode(string id)
	{
		ID = id;
	}
	
	public string ID;

	public List<PageNode> EarlierPages = new List<PageNode>();
	public List<PageNode> LaterPages = new List<PageNode>();


	public void AddLater(PageNode laterPage)
	{
		LaterPages.Add(laterPage);
		laterPage.EarlierPages.Add(this);
	}

	public bool ValidatePlacement(List<string> check, int index, Dictionary<string, PageNode> pageNodes)
	{
		for (int lower = 0; lower < index; lower++)
		{
			if (LaterPages.Contains(pageNodes[check[lower]]))
				return false;
		}
		
		for (int higher = check.Count - 1; higher >= index; higher--)
		{
			if (EarlierPages.Contains(pageNodes[check[higher]]))
				return false;
		}

		return true;
	}
}