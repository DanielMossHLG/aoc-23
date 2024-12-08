public static class Day7_2024
{
    public static void Solution()
    {
        int time = DateTime.Now.Millisecond;
        string rawInput = Utils.GetInput("day7_2024.txt").Trim();

        string[] lines = rawInput.Split('\n', StringSplitOptions.TrimEntries);

        long part1 = 0;

        foreach (var line in lines)
        {
            string[] splitLine = line.Split(':', StringSplitOptions.TrimEntries);
            
            long value = long.Parse(splitLine[0]);
            List<int> components = new List<int>();
            
            string[] splitComponents = splitLine[1].Split(' ', StringSplitOptions.TrimEntries);
            foreach (var component in splitComponents)
                components.Add(int.Parse(component));

            Entry entry = new Entry(value, components);

            if (entry.ValidateOperations() > 0)
                part1 += entry.Value;
        }
        
        Console.WriteLine($"Part 1: {part1}");
    }

    public class Entry
    {
        public long Value;
        public List<int> Components;

        private int NumberOfOperations;

        [Flags]
        internal enum Operations
        {
            FirstMulti = 1 << 0,
            SecondMulti = 1 << 1,
            ThirdMulti = 1 << 2,
            FourthMulti = 1 << 3,
            FifthMulti = 1 << 4,
            SixthMulti = 1 << 5,
            SeventhMulti = 1 << 6,
            EighthMulti = 1 << 7,
            NinethMulti = 1 << 8,
            TenthMulti = 1 << 9,
            EleventhMulti = 1 << 10,
            TwelvethMulti = 1 << 11,
            ThirteenthMulti = 1 << 12,
            FourteenthMulti = 1 << 13,
            FifteenthMulti = 1 << 14,
        }

        public Entry(long value, List<int> components)
        {
            Value = value;
            Components = components;

            NumberOfOperations = 0;
            NumberOfOperations = Components.Count - 1;
        }

        public int ValidateOperations()
        {
            int validSolutions = 0;

            for (int i = 0; i < Math.Pow(2, NumberOfOperations); i++)
            {
                Operations operations = (Operations)i;
                if (AnswerValid(operations))
                    validSolutions++;
            }

            return validSolutions;
        }

        private bool AnswerValid(Operations operations)
        {
            int output = Components[0];

            Dictionary<int, bool> multiplierLookup = new Dictionary<int, bool>()
            {
                { 1, operations.HasFlag(Operations.FirstMulti) },
                { 2, operations.HasFlag(Operations.SecondMulti) },
                { 3, operations.HasFlag(Operations.ThirdMulti) },
                { 4, operations.HasFlag(Operations.FourthMulti) },
                { 5, operations.HasFlag(Operations.FifthMulti) },
                { 6, operations.HasFlag(Operations.SixthMulti) },
                { 7, operations.HasFlag(Operations.SeventhMulti) },
                { 8, operations.HasFlag(Operations.EighthMulti) },
                { 9, operations.HasFlag(Operations.NinethMulti) },
                { 10, operations.HasFlag(Operations.TenthMulti) },
                { 11, operations.HasFlag(Operations.EleventhMulti) },
                { 12, operations.HasFlag(Operations.TwelvethMulti) },
                { 13, operations.HasFlag(Operations.ThirteenthMulti) },
                { 14, operations.HasFlag(Operations.FourteenthMulti) },
                { 15, operations.HasFlag(Operations.FifteenthMulti) }
            };

            for (int i = 1; i < Components.Count; i++)
            {
                if (multiplierLookup[i])
                    output *= Components[i];
                else
                    output += Components[i];
            }
            
            return output == Value;
        }
    }
}