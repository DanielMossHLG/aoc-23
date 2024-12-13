public static class Day11_2024
{
    public static void Solution()
    {
        var time = DateTime.Now;
        string rawInput = Utils.GetInput("day11_2024.txt").Trim();
        
        Input input = new Input(rawInput);


        input.Blink(1);
        
        Console.WriteLine("Final count: " + input.Nodes.Count + $" times: {DateTime.Now - time}");
    }

    public class Input(string input)
    {
        public List<long> Numbers = input.Split(' ').Select(long.Parse).ToList();
        
        public List<Node> Nodes = new List<Node>();
        
        public Dictionary<long, Map> Maps = new Dictionary<long, Map>();

        public void Blink(int steps)
        {
            for (int i = 0; i < Numbers.Count; i++)
            {
                Node node = new Node(0, Numbers[i]);
                Nodes.Add(node);
            }
            
            for (int i = 0; i < Nodes.Count; i++)
            {
                while (Nodes[i].Steps < steps)
                {
                    if (Maps.TryGetValue(Nodes[i].Value, out Map map))
                    {
                        if (Nodes[i].Steps + map.StepsToSplit <= steps)
                        {
                            Nodes[i].Value = map.SplitsTo[0];
                            Nodes[i].Steps += map.StepsToSplit;
                            Node node = new Node(Nodes[i].Steps, map.SplitsTo[1]);
                            Nodes.Insert(i + 1, node);
                            Nodes[i].Steps += map.StepsToSplit;
                        }
                        Console.WriteLine($"Found a {Nodes[i].Value}. Could not get value because it had {Nodes[i].Steps} steps.");
                        continue;
                    }
                    Nodes[i].Steps += GetStepsToSplit(Nodes[i], Nodes[i].Value, 0, Nodes[i].Value, steps);
                    
                    if (Nodes[i].Value == 2)
                        Console.WriteLine($"Found a 2: Node steps - {Nodes[i].Steps} | {steps}");
                }
            }
            
            ShowNumbers();
            // foreach (var (value, map) in Maps)
            // {
            //     Console.WriteLine($"Map for {value}: {map.SplitsTo[0]} | {map.SplitsTo[1]} - {map.StepsToSplit}");
            // }
        }

        public int GetStepsToSplit(Node node, long startValue, int steps, long value, int totalSteps)
        {
            if (node.Steps + steps > totalSteps)
            {
                return steps;
            }
            
            if (value == 0)
            {
                node.Value = 1;
                return GetStepsToSplit(node, startValue, steps + 1, 1, totalSteps);
            }

            
            string val = value.ToString();
            if (val.Length % 2 == 0)
            {
                Map map = new Map(startValue, steps + 1)
                {
                    SplitsTo = [long.Parse(val.Substring(0, val.Length / 2)), long.Parse(val.Substring(val.Length / 2))]
                };
                Maps.Add(startValue, map);

                node.Value = map.SplitsTo[0];
                Node newNode = new Node(node.Steps + map.StepsToSplit, map.SplitsTo[1]);
                Nodes.Insert(Nodes.IndexOf(node) + 1, newNode);
                
                return map.StepsToSplit;
            }
            
            node.Value = value * 2024;
            return GetStepsToSplit(node, startValue, steps + 1, value * 2024, totalSteps);
        }
        
        
        public void ShowNumbers()
        {
            string output = "";

            foreach (var node in Nodes)
            {
                output += node.Value + " ";
            }
            
            Console.WriteLine(output);
        }
    }

    public class Node(int steps, long value)
    {
        public int Steps = steps;
        public long Value = value;
    }

    public class Map(long start, int steps)
    {
        public long StartNumber = start;

        public int StepsToSplit = steps;

        public long[] SplitsTo = [];
    }

}