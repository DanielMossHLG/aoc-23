public static class Day7_2024
{
    public static void Solution()
    {
        int time = DateTime.Now.Millisecond;
        string rawInput = Utils.GetInput("day7_2024.txt").Trim();

        string[] lines = rawInput.Split('\n', StringSplitOptions.TrimEntries);

        long part1 = 0;
        long part2 = 0;

        foreach (var line in lines)
        {
            string[] splitLine = line.Split(':', StringSplitOptions.TrimEntries);
            
            long value = long.Parse(splitLine[0]);
            List<int> components = new List<int>();
            
            string[] splitComponents = splitLine[1].Split(' ', StringSplitOptions.TrimEntries);
            foreach (var component in splitComponents)
                components.Add(int.Parse(component));

            Entry entry = new Entry(value, components);

            (int a, int b) = entry.ValidateOperations();

            if (a > 0)
                part1 += entry.Value;
            if (b > 0)
                part2 += entry.Value;
        }
        
        Console.WriteLine($"Part 1: {part1} | Part 2: {part2}");
    }

    public class Entry
    {
        public long Value;
        public List<int> Components;

        private int NumberOfOperations;

        public Entry(long value, List<int> components)
        {
            Value = value;
            Components = components;

            NumberOfOperations = 0;
            NumberOfOperations = Components.Count - 1;
        }

        public (int a, int b) ValidateOperations()
        {
            int validSolutions = 0;
            int validSolutionsWithConcat = 0;
            
            List<int> operations = new List<int>();

            for (int i = 0; i < NumberOfOperations; i++)
            {
                operations.Add(0);
            }

            for (int i = 0; i <  NumberOfOperations; i++)
            {
                //TODO - make a proper permutable here
                foreach (var perm in perms)
                    if (AnswerValid(perm.ToList()))
                        validSolutions++;
            }

            return (validSolutions, validSolutionsWithConcat);
        }

        private bool AnswerValid(List<int> operations)
        {
            long output = Components[0];

            for (int i = 1; i < Components.Count; i++)
            {
                if (operations[i-1] == 1)
                    output += Components[i];
                else if (operations[i-1] == 2)
                    output *= Components[i];
                else
                    output = long.Parse($"{output}{Components[i]}");
            }

            return output == Value;
        }
    }
}