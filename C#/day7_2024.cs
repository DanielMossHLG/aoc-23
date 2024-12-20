public static class Day7_2024
{
    public static void Solution()
    {
        var time = DateTime.Now;
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
        
        Console.WriteLine($"Part 1: {part1} | Part 2: {part2} | Time: {DateTime.Now - time}");
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
            
            List<OperationSlot> operations = new List<OperationSlot>();
            
            for (int i = 0; i < NumberOfOperations; i++)
            {
                operations.Add(new OperationSlot());
            }
            
            for (int i = 0; i < NumberOfOperations - 1; i++)
            {
                operations[i].Next = operations[i + 1];
            }
            

            for (int i = 0; i < MathF.Pow(3, NumberOfOperations); i++)
            {
                int check = AnswerValid(operations);
                if (check == 1)
                    validSolutions++;
                if (check > 0)
                    validSolutionsWithConcat++;
                
                operations[0].IncreaseValue();
            }

            return (validSolutions, validSolutionsWithConcat);
        }

        private int AnswerValid(List<OperationSlot> operations)
        {
            long output = Components[0];
            
            bool containsConcat = false;

            for (int i = 1; i < Components.Count; i++)
            {
                if (operations[i-1].Value == 0)
                    output += Components[i];
                else if (operations[i-1].Value == 1)
                    output *= Components[i];
                else
                {
                    output = long.Parse($"{output}{Components[i]}");
                    containsConcat = true;
                }
            }

            if (output != Value)
                return 0;
            if (containsConcat)
                return 2;

            return 1;
        }
    }


    public class OperationSlot
    {
        public int Value;

        public OperationSlot? Next;

        public void IncreaseValue()
        {
            if (Value < 2)
                Value++;
            else
            {
                Value = 0;
                if (Next != null)
                    Next.IncreaseValue();
            }
        }
    }
}