public static class Day5
{
    public static void Solution()
    {
        List<Card> cards = new List<Card>();
        string rawInput = Utils.GetInput("day5Input1.txt");

        string[] lines = rawInput.Split("\r\n");

        List<string> seeds = lines[0].Substring(7).Split(" ").ToList();

        var maps = rawInput.Split("map:")[1].Trim().Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();
        maps.RemoveAt(maps.Count - 1);

        foreach (var thing in maps)
            Console.WriteLine("Map: " + thing);
    }
}