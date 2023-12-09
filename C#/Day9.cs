public static class Day9
{
    public static void Solution()
    {
        string rawInput = Utils.GetInput("day9input1.txt");
        string[] lines = rawInput.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        int futureSum = 0;
        int pastSum = 0;
        foreach (var line in lines)
        {
            List<int> original = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            List<List<int>> extrapolations = FindExtrapolations(original);
            
            int sum = 0;
            int retroSum = 0;
            for (int i = extrapolations.Count - 1; i >= 0; i--)
            {
                sum += extrapolations[i][extrapolations[i].Count - 1];

                retroSum = extrapolations[i][0] - retroSum;
            }
            futureSum += sum;
            pastSum += retroSum;
        }
        Console.WriteLine($"Part 1: {futureSum} | Part 2: {pastSum}");
    }

    public static List<List<int>> FindExtrapolations(List<int> list)
    {
        List<List<int>> extrap = new List<List<int>>(){ list };
        int index = 0;

        while (true)
        {
            List<int> newExtry = new List<int>();
            for (int i = 1; i < list.Count - index; i++)
            newExtry.Add(extrap[index][i] - extrap[index][i-1]);

            extrap.Add(newExtry);

            if (newExtry.Where(x => x != 0).ToList().Count == 0)
                break;

            index++;
        }
        return extrap;
    }
}