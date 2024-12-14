using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

public static class Day11_2024
{
    public static void Solution()
    {
        var time = DateTime.Now;
        string rawInput = Utils.GetInput("day11_2024.txt").Trim();
        
        Input input = new Input(rawInput);


        input.Blink(75);
        
        Console.WriteLine("Final count: " + input.Nodes.Count + $" times: {DateTime.Now - time}");
    }

    public class Input(string input)
    {
        public List<Node> Nodes = new List<Node>();
        
        public List<long> Numbers = input.Split(' ').Select(long.Parse).ToList();

        public Dictionary<long, (long numberValue, long newNodeValue, int stepsTaken)> Map = new Dictionary<long, (long numberValue, long newNodeValue, int stepsTaken)>();
        
        public void Blink(int steps)
        {
            for (int i = 0; i < Numbers.Count; i++)
            {
                Nodes.Add(new Node(Numbers[i], 0));
            }


            for (int i = 0; i < Nodes.Count; i++)
            {
                List<Node> newNodes = new List<Node>();
                
                while (Nodes[i].CurrentStep < steps)
                {
                    long numberValue, newNodeValue;
                    int stepsTaken;
                    
                    if (Map.ContainsKey(Nodes[i].Value))
                    {
                        (numberValue, newNodeValue, stepsTaken) = Map[Nodes[i].Value];
                    }
                    else
                    {
                        (numberValue, newNodeValue, stepsTaken) = GetOutput(Nodes[i].Value, 0);
                        Map.Add(Nodes[i].Value, (numberValue, newNodeValue, stepsTaken));
                    }
                    
                    Nodes[i].CurrentStep += stepsTaken;

                    if (Nodes[i].CurrentStep <= steps)
                    {
                        Nodes[i].Value = numberValue;
                        Node node = new Node(newNodeValue, Nodes[i].CurrentStep);
                        newNodes.Add(node);
                    }
                }
                
                Nodes.AddRange(newNodes);
            }
        }

        public (long nextValue, long newValue, int steps) GetOutput(long number, int steps)
        {
            if (number == 0)
            {
                return GetOutput(1, steps + 1);
            }

            if (number.ToString().Length % 2 == 0)
            {
                string str = number.ToString();
                long num1 = long.Parse(str.Substring(0, str.Length / 2));
                long num2 = long.Parse(str.Substring(str.Length / 2));
                return (num1, num2, steps + 1);
            }

            return GetOutput(number * 2024, steps + 1);
        }

        public class Node(long value, int steps)
        {
            public long Value = value;
            public int CurrentStep = steps;
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
    
}
