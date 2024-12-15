public static class Day11_2_2024
{
    public static void Solution()
    {
        var time = DateTime.Now;
        string rawInput = Utils.GetInput("day11_2024.txt").Trim();
        
        Input input = new Input(rawInput);
        input.Result = input.Numbers.Count;

        input.Blink(input.Numbers, 60, 20);
        
        Console.WriteLine("Final count: " + input.Result + $" times: {DateTime.Now - time}");
    }

    public class Input(string input)
    {
        public List<long> Numbers = input.Split(' ').Select(long.Parse).ToList();

        public Dictionary<long, List<long>> Map = new Dictionary<long, List<long>>();

        public long Result;
        
        public void Blink(List<long> input, int steps, int incrementer = 5)
        {
            for (int i = 0; i < steps;)
            {
                int startCount = input.Count;
                for (int j = 0; j < startCount; j++)
                {
                    List<long> output;
                    //Console.WriteLine(j + " " + input.Count);
                    if (Map.TryGetValue(input[j], out var value))
                    {
                        output = value;
                    }
                    else
                    {
                        output = GetOutput(input[j], incrementer);
                        Map.Add(input[j], output);
                    }

                    input[j] = output[0];
                    Result += output.Count - 1; 
                    List<long> o = output.Skip(1).ToList();
                    Blink(o, steps - (i + incrementer), incrementer);
                }
                i += incrementer;
            }
        }

        public List<long> GetOutput(long input, int steps)
        {
            List<long> output = [input];
            for (int i = 0; i < steps; i++)
            {
                int startCount = output.Count;
                for (int j = 0; j < startCount; j++)
                {
                    var o = GetNextStep(output[j]);
                    output[j] = o[0];

                    if (o.Count > 1)
                    {
                        output.Add(o[1]);
                    }
                }
            }

            return output;
        }
        
        public Dictionary <long, List<long>> NextStepMap = new Dictionary<long, List<long>>();

        public List<long> GetNextStep(long number)
        {
            if (number == 0)
            {
                return [1];
            }
            
            if (NextStepMap.TryGetValue(number, out var list))
            {
                return list;
            }

            if (number.ToString().Length % 2 == 0)
            {
                string str = number.ToString();
                long num1 = long.Parse(str.Substring(0, str.Length / 2));
                long num2 = long.Parse(str.Substring(str.Length / 2));
                NextStepMap.Add(number, [num2, num1]);
                return [num1, num2];
            }
            
            // var figs = (MathF.Floor(MathF.Log10(number)) + 1);
            // if (figs % 2 == 0)
            // {
            //     long multi = (long)(Math.Pow(10, figs / 2));
            //     long num1 = number / multi;
            //     long num2 = number - (num1 * multi);
            //     NextStepMap.Add(number, [num2, num1]);
            //     return [num1, num2];
            // }

            NextStepMap.Add(number, [number * 2024]);
            return [number * 2024];
        }

        public class Node(long value, int steps)
        {
            public long Value = value;
            public int CurrentStep = steps;
        }
        
        public void ShowNumbers()
        {
            string output = "";

            foreach (var num in Numbers)
            {
                output += num + " ";
            }
            
            Console.WriteLine(output);
        }
    }
}