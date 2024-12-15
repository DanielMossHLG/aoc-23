public static class Day11_3_2024
{
    public static void Solution()
    {
        var time = DateTime.Now;
        string rawInput = Utils.GetInput("day11_2024.txt").Trim();
        
        Input input = new Input(rawInput);

        input.Initialise();

        input.Blink(75, 25);
        
        Console.WriteLine("Final count: " + input.GetResult() + $" times: {DateTime.Now - time}");
    }

    public class Input(string input)
    {
        public List<long> Numbers = input.Split(' ').Select(long.Parse).ToList();

        public Dictionary<long, List<long>> Map = new Dictionary<long, List<long>>();
        
        public Dictionary<long, long> NumberMap = new Dictionary<long, long>();

        public void Initialise()
        {
            for (int i = 0; i < Numbers.Count; i++)
            {
                if (NumberMap.ContainsKey(Numbers[i]))
                    NumberMap[i]++;
                else
                    NumberMap.Add(Numbers[i], 1);
            }
        }

        public long GetResult()
        {
            long result = 0;
            string nums = "";
            foreach (var (key, value) in NumberMap)
            {
                nums += $"key: {key}, value: {value}, ";
                result += value;
            }
            //Console.WriteLine(nums);
            return result;
        }
        
        public void Blink(int steps, int incrementer = 5)
        {
            for (int i = 0; i < steps;)
            {
                Dictionary<long, long> newMap = new Dictionary<long, long>();
                List<long> keys = NumberMap.Keys.ToList();
                
                foreach (var key in keys)
                {
                    List<long> output;
                    if (Map.TryGetValue(key, out var value))
                    {
                        output = value;
                    }
                    else
                    {
                        output = GetOutput(key, incrementer);
                        Map.Add(key, output);
                    }

                    // if (newMap.ContainsKey(output[0]))
                    //     newMap[output[0]] += NumberMap[key];
                    // else
                    //     newMap.Add(output[0], += NumberMap[key]);
                    // if (i == 5)
                    //     Console.WriteLine($"Output 0: {output[0]} | amount: {newMap[output[0]]}");

                    for (int j = 0; j < output.Count; j++)
                    {
                        if (newMap.ContainsKey(output[j]))
                            newMap[output[j]] += NumberMap[key];
                        else
                            newMap.Add(output[j], NumberMap[key]);
                        
                        if (i == 5)
                            Console.WriteLine($"Output {j}: {output[j]} | amount: {newMap[output[j]]}");
                    }
                }
                
                i += incrementer;
                NumberMap = newMap;
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