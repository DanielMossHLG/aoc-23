public static class Day9_2024
{
    public static void Solution()
    {
        var time = DateTime.Now;
        string rawInput = Utils.GetInput("day9_2024.txt").Trim();

        List<string> input = new List<string>();

        for (int i = 0; i < rawInput.Length; i++)
        {
            int number = int.Parse(rawInput[i].ToString());
            
            for (int n = 0; n < number; n++)
                input.Add(i % 2 == 0 ? (i / 2).ToString() : ".");
        }

        int emptyIndex = input.IndexOf(".");

        for (int i = input.Count - 1; i >= 0; i--)
        {
            if (input[i] != ".")
            {
                input[emptyIndex] = input[i];
                input[i] = ".";
                emptyIndex = input.IndexOf(".");

                if (emptyIndex > i - 1)
                    break;
            }
        }

        long result = 0;

        for (int i = 0; i < input.Count; i++)
        {
            if (input[i] == ".")
                break;
            result += int.Parse(input[i]) * i;
        }
        
        Console.WriteLine($"result: {result}");
        
        
        
        
        // ------------------------------------------
        
        List<MemoryBlock> memoryBlocks = new List<MemoryBlock>();
        
        
        for (int i = 0; i < rawInput.Length; i++)
        {
            int number = int.Parse(rawInput[i].ToString());

            MemoryBlock memoryBlock = new MemoryBlock();
            memoryBlock.ID = i % 2 == 0 ? i / 2 : -1;
            memoryBlock.Length = number;
            
            memoryBlocks.Add(memoryBlock);
        }


        for (int i = memoryBlocks.Count - 1; i >= 0; i--)
        {
            if (memoryBlocks[i].ID == -1 || memoryBlocks[i].Seen)
                continue;
            
            int emptyBlockIndex = FindEmptyBlockIndex(memoryBlocks);

            while (emptyBlockIndex != -1 && emptyBlockIndex < i)
            {
                if (memoryBlocks[emptyBlockIndex].Length >= memoryBlocks[i].Length)
                {
                    var block = memoryBlocks[i];
                    memoryBlocks[emptyBlockIndex].Length -= block.Length;
                    memoryBlocks.Insert(emptyBlockIndex, block.Clone());
                    block.ID = -1;
                    emptyBlockIndex = -1;
                }
                else emptyBlockIndex = FindEmptyBlockIndex(memoryBlocks, emptyBlockIndex + 1);
            }
        }


        long result2 = 0;
        int memoryIndex = 0;

        for (int i = 0; i < memoryBlocks.Count; i++)
        {
            for (int j = 0; j < memoryBlocks[i].Length; j++)
            {
                if (memoryBlocks[i].ID != -1)
                    result2 += memoryBlocks[i].ID * memoryIndex;
                memoryIndex++;
            }
        }
        
        Console.WriteLine($"result2: {result2}");
        Console.WriteLine($"Time taken: {DateTime.Now - time}");
    }

    public static int FindEmptyBlockIndex(List<MemoryBlock> memoryBlocks, int startingSearchIndex = 0)
    {
        for (int i = startingSearchIndex; i < memoryBlocks.Count; i++)
        {
            if (memoryBlocks[i].ID == -1 && memoryBlocks[i].Length > 0)
                return i;
        }

        return -1;
    }


    public class MemoryBlock
    {
        public int ID;
        public int Length;
        public bool Seen;

        public MemoryBlock Clone()
        {
            return new MemoryBlock() { ID = ID, Length = Length, Seen = true };
        }
    }
}