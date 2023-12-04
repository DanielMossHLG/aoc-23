public static class Day2
{
    public static void Solution()
    {
        string rawInput = Utils.GetInput("Day2Input1.txt");

        string[] lines = rawInput.Split('\n');

        Dictionary<int, Dictionary<string, int>> parsedGames = new Dictionary<int, Dictionary<string, int>>();
        Dictionary<string, int> maxAllowed = new Dictionary<string, int>
        {
            {"red", 12},
            {"green", 13},
            {"blue", 14}
        };

        foreach (var line in lines)
        {
            Dictionary<string, int> gameResults = new Dictionary<string, int>();

            var nameLinePair = line.Split(':');

            foreach(var pull in nameLinePair[1].Split(';'))
            {
                foreach (var rollValue in pull.Split(", "))
                {
                    var trimmedVal = rollValue.Trim();
                    var kvp = trimmedVal.Split(' ');
                    string key = kvp[1];
                    int value = int.Parse(kvp[0]);
                    if (!gameResults.ContainsKey(key))
                        gameResults.Add(key, value);
                    else if (gameResults[key] < value)
                        gameResults[key] = value;
                }
            }
            var gameID = int.Parse(nameLinePair[0].Substring(5));
            parsedGames.Add(gameID, gameResults);
        }

        int sum = 0;
        int sumPowers = 0;

        for (int i = 1; i <= parsedGames.Count; i++)
        {
            bool isValid = true;
            int power = 1;
            foreach ((var colour, var value) in maxAllowed)
            {
                power *= parsedGames[i][colour];
                if (!isValid) continue;
                if (parsedGames[i][colour] > value)
                    isValid = false;
            }
            if (isValid)
                sum += i;

            sumPowers += power;
        }


        Console.WriteLine($"Part 1: {sum} | Part 2: {sumPowers}");
    }
    
}

