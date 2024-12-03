public static class Day2_2024
{
	public static void Solution()
	{
		string rawInput = Utils.GetInput("day2_2024.txt").Trim();
		
		string[] lines = rawInput.Split('\n');

		int safeReports = 0;
		int safeDampenedReports = 0;

		for (int i = 0; i < lines.Length; i++)
		{
			string[] stringReport = lines[i].Trim().Split();
			int[] report = Array.ConvertAll(stringReport, int.Parse);

			if (CheckReport(report))
			{
				safeReports++;
				safeDampenedReports++;
			}
			else if (CheckReportRecursive(report))
				safeDampenedReports++;
		}
		
		Console.WriteLine($"Part 1: {safeReports} | Part 2: {safeDampenedReports}");
	}

	public static bool CheckReport(int[] report)
	{
		int currentChecker = report[0];

		if (currentChecker == report[1])
			return false;
		
		bool isAscending = report[1] > report[0];
		
		for (int i = 1; i < report.Length; i++)
		{
			if (isAscending)
			{
				if (report[i] <= currentChecker || report[i] - currentChecker > 3)
					return false;
			}
			else
			{
				if (report[i] >= currentChecker || report[i] - currentChecker < -3)
					return false;
			}
			currentChecker = report[i];
		}

		return true;
	}

	public static bool CheckReportRecursive(int[] report, int indexToIgnore = 0)
	{
		if (indexToIgnore >= report.Length)
			return false;
		
		List<int> reportList = new List<int>(report);
		reportList.RemoveAt(indexToIgnore);
		
		if (!CheckReport(reportList.ToArray()))
			return CheckReportRecursive(report, indexToIgnore+1);

		return true;
	}
}