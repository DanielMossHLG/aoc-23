public static class Day5
{
    public static void Solution()
    {
        int numOfMaps = 0;
        long lowestLocationPartOne = long.MaxValue;
        long lowestLocationPartTwo = long.MaxValue;
        List<Map> maps = new List<Map>();
        string rawInput = Utils.GetInput("day5Input1.txt");
        
    
        string[] lines = rawInput.Split("\n");
    
        List<long> seeds = lines[0].Substring(7).Split(" ").Select(x => long.Parse(x)).ToList();
    
        var mapSplit = rawInput.Split("map:");
    
        for (int i = 1; i < mapSplit.Length; i++)
        {
            var mapStrings = mapSplit[i].Trim().Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList();
            if (i != mapSplit.Length - 1 ) mapStrings.RemoveAt(mapStrings.Count - 1);
    
            numOfMaps++;
    
            for (int k = 0; k < mapStrings.Count; k++)
            {
                var rawData = mapStrings[k];
                var split = rawData.Split(' ');
                Map map;
                if (k == 0)
                {
                    map = new Map();
                    map.Id = i;
                    maps.Add(map);
                }
                else map = maps[i - 1];
    
                SourceToDestination std = new SourceToDestination();
                std.DestinationStart = long.Parse(split[0]);
                std.SourceStart = long.Parse(split[1]);
                std.Range = long.Parse(split[2]);
                map.Lookup.Add(std);
            }
            
        }
    
        foreach (long seed in seeds)
        {
            long location = seed;
            for (int i = 0; i < numOfMaps; i++)
            {
                var validLookup = maps[i].Lookup.Where(x => x.SourceStart <= location && x.SourceEnd >= location).ToList();
                if (validLookup.Count > 0) 
                    location += validLookup[0].Conversion;
            }
        
            if (location < lowestLocationPartOne) lowestLocationPartOne = location;
        }
    
        for (int i = 0; i < seeds.Count - 1; i+= 2)
        {
            for (long j = 0; j <= seeds[i + 1]; j++)
            {
                long location = seeds[i] + j;
                long shortestBoundary = long.MaxValue;
                for (int k = 0; k < numOfMaps; k++)
                {
                    var validLookup = maps[k].Lookup.Where(x => x.SourceStart <= location && x.SourceEnd >= location).ToList();
                    if (validLookup.Count > 0)
                    {
                        if (shortestBoundary != 0 && validLookup[0].SourceEnd - location < shortestBoundary)
                            shortestBoundary = validLookup[0].SourceEnd - location;
    
                        location += validLookup[0].Conversion;
                    }
                    else
                    {
                        var boundsLookup = maps[k].Lookup.Where(x => x.SourceStart <= location && x.SourceEnd >= location).ToList();
                        if (boundsLookup.Count > 0)
                        {
                            boundsLookup.Sort((x, y) => x.SourceStart > y.SourceStart ? 1 : -1);
                            shortestBoundary = boundsLookup[0].SourceEnd - location;
                        }
                    }
                }
                if (location < lowestLocationPartTwo) lowestLocationPartTwo = location;
                j += shortestBoundary;
            }
        }
        
        Console.WriteLine($"Part 1: {lowestLocationPartOne} | Part 2: {lowestLocationPartTwo}");
    }
}

    public class Map
{
    public int Id = 0;
    public List<SourceToDestination> Lookup = new List<SourceToDestination>();
    public Dictionary<long, long> GetIndex = new Dictionary<long, long>();
}

public class SourceToDestination
{
    public long DestinationStart = 0;
    public long SourceStart = 0;
    public long Range = 0;
    public long SourceEnd => SourceStart + Range - 1;
    public long Conversion => DestinationStart - SourceStart;
}