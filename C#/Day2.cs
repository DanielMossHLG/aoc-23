// using System;
// using System.Collections;
// using System.Collections.Generic;

var rawInput = Utils.GetInput("Day2Input1.txt");

string[] lines = rawInput.Split('\n');


//Part1
Dictionary<int, Dictionary<string, int>> parsedGames = new Dictionary<int, Dictionary<string, int>>();
Dictionary<string, int> maxAllowed = new Dictionary<string, int>
{
    {"red", 12},
    {"green", 13},
    {"blue", 14}
};

foreach (var line in lines)
{
    var lineSplit = line.Split(':');

    var pulls = lineSplit[1].Split(';');
    if (pulls == null) continue;
    Dictionary<string, int> gameResults = new Dictionary<string, int>();
    foreach(var pull in pulls)
    {
        foreach (var rollValue in pull.Split(", "))
        {
            var trimmedVal = rollValue.Trim();
            var values = trimmedVal.Split(' ');
            int value = int.Parse(values[0]);
            if (!gameResults.ContainsKey(values[1]))
                gameResults.Add(values[1], value);
            else if (gameResults[values[1]] < value)
                gameResults[values[1]] = value;
        }
    }
    parsedGames.Add(int.Parse(lineSplit[0].Substring(5)), gameResults);
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