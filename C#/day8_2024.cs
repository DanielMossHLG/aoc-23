public static class Day8_2024
{
    public static void Solution()
    {
        string rawInput = Utils.GetInput("day8_2024.txt").Trim();

        string[] lines = rawInput.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        Node[][] nodes = new Node[lines.Length][];

        //string output = "";
        
        Dictionary< (int x, int y), Node> nodesDict = new Dictionary<(int x, int y), Node>();
        Dictionary<char, List<Node>> nodesGroupDict = new Dictionary<char, List<Node>>();

        for (int y = 0; y < lines.Length; y++)
        {
            nodes[y] = new Node[lines[y].Length];

            for (int x = 0; x < lines[y].Length; x++)
            {
                nodes[y][x] = new Node(lines[y][x], x, y);
                
                nodesDict.Add((x,y), nodes[y][x]);

                if (nodesGroupDict.ContainsKey(lines[y][x]))
                    nodesGroupDict[lines[y][x]].Add(nodes[y][x]);
                else
                    nodesGroupDict.Add(lines[y][x], [ nodes[y][x] ]);
            
                //output += nodes[y][x].Antenna;
            }
        }

        //Console.WriteLine(output);

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '.')
                    continue;
                
                Node currentNode = nodes[y][x];
                List<Node> commonNodes = nodesGroupDict[lines[y][x]];
                int currentNodeIndex = commonNodes.IndexOf(currentNode);
                
                //Console.WriteLine($"Node: {currentNode.Coordinates} | Current Node Index: {currentNodeIndex}");

                for (int i = currentNodeIndex + 1; i < commonNodes.Count; i++)
                {
                    Node targetNode = commonNodes[i];
                    currentNode.AddAntinode(lines[y][x]);
                    targetNode.AddAntinode(lines[y][x]);
                    //Console.WriteLine($"Target Node: {targetNode.Coordinates} i: {i}");
                    (int x, int y) direction = AddCoords(currentNode.Coordinates, targetNode.Coordinates, true);
                    
                    //Console.WriteLine(direction);
                    
                    (int x, int y) antinodeUpCoords = AddCoords(targetNode.Coordinates, direction, true);
                    (int x, int y) antinodeDownCoords = AddCoords(currentNode.Coordinates, direction, false);

                    while (CheckCoordsValid(antinodeUpCoords, lines[y].Length, lines.Length))
                    {
                        Node antinode = nodes[antinodeUpCoords.y][antinodeUpCoords.x];
                        antinode.AddAntinode(lines[y][x]);
                        
                        antinodeUpCoords = AddCoords(antinodeUpCoords, direction, true);
                    }

                    while (CheckCoordsValid(antinodeDownCoords, lines[y].Length, lines.Length))
                    {
                        //Console.WriteLine($"Coords valid: {antinode2Coords}");
                        Node antinode2 = nodes[antinodeDownCoords.y][antinodeDownCoords.x];
                        antinode2.AddAntinode(lines[y][x]);
                        
                        antinodeDownCoords = AddCoords(antinodeDownCoords, direction, false);
                    }
                }

            }
        }
        
        Console.WriteLine(Results.Part1);
    }

    public static class Results
    {
        public static int Part1 = 0;
    }

    public static (int x, int y) AddCoords((int x, int y) coords1, (int x, int y) coords2, bool subtract)
    {
        return (coords1.x + coords2.x * (subtract ? -1 : 1), coords1.y + coords2.y * (subtract ? -1 : 1));
    }

    public static bool CheckCoordsValid((int x, int y) coords, int xMax, int yMax)
    {
        return !(coords.x < 0 || coords.y < 0 || coords.x >= xMax || coords.y >= yMax);
    }


    public class Node
    {
        public char Antenna;
        public (int x, int y) Coordinates;
        public List<char> Antinodes = new List<char>();

        public Node(char antenna, int x, int y)
        {
            Antenna = antenna;
            Coordinates = (x,y);
        }

        public void AddAntinode(char antinode)
        {
            if (Antinodes.Count == 0)
                Results.Part1++;
            
            Antinodes.Add(antinode);
        }
    }
}