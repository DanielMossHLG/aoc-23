public static class Day13_2024
{
    public static void Solution()
    {
        var time = DateTime.Now;
        string rawInput = Utils.GetInput("day13_2024.txt").Trim();

        var machineBlocks = rawInput.Split("\r\n\r\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        long part1 = 0;
        long part2 = 0;

        int i = 0;

        foreach (var machineBlock in machineBlocks)
        {
            Console.WriteLine($"Block {i} | Time: {DateTime.Now - time}");
            i++;
            var lines = machineBlock.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var machine = new Machine();
            
            List<(int x, int y)> positions = new List<(int x, int y)>();

            foreach (var line in lines)
            {
                int posX = int.Parse(line.Substring(line.IndexOf("X") + 2,line.IndexOf(",") - line.IndexOf("X") - 2));
                int posY = int.Parse(line.Substring(line.IndexOf("Y") + 2));
                
                positions.Add((posX, posY));
            }
            
            machine.ButtonA = positions[0];
            machine.ButtonB = positions[1];
            machine.PrizeCoordinates = positions[2];
            
            machine.GetMinimumClicks(true);
            part1 += machine.MinimumCost;
            
            // machine.GetMinimumClicks(false);
            // part2 += machine.MinimumCost;
        }
        
        Console.WriteLine($"Part 1: {part1} | Part2: {part2} | Time: {DateTime.Now - time}");
    }

    public class Machine
    {
        public (long x, long y) ButtonA;
        public (long x, long y) ButtonB;
        public (long x, long y) PrizeCoordinates;

        public (long A, long B) MinimumClicks;
        public long MinimumCost = 0;
        

        public void GetMinimumClicks(bool part1)
        {
            MinimumCost = 0;

            if (PrizeCoordinates.x % 2 != 0 && (ButtonB.x % 2 == 0 && ButtonA.x % 2 == 0))
                return;
            if (PrizeCoordinates.y % 2 != 0 && (ButtonB.y % 2 == 0 && ButtonA.y % 2 == 0))
                return;

            (long x, long y) button1;
            (long x, long y) button2;

            float buttonARelativeValueX = PrizeCoordinates.x / (float)ButtonA.x / 3f;
            float buttonARelativeValueY = PrizeCoordinates.y / (float)ButtonA.y / 3f;
            
            float buttonBRelativeValueX = PrizeCoordinates.x / (float)ButtonB.x;
            float buttonBRelativeValueY = PrizeCoordinates.y / (float)ButtonB.y;
            
            float buttonAValue = MathF.Min(buttonARelativeValueX, buttonARelativeValueY);
            float buttonBValue = MathF.Max(buttonBRelativeValueX, buttonBRelativeValueY);

            button1 = buttonAValue < buttonBValue ? ButtonA : ButtonB;
            button2 = buttonAValue < buttonBValue ? ButtonB : ButtonA;
            
            var coords = PrizeCoordinates;
            if (!part1)
            {
                coords.x += 10000000000000;
                coords.y += 10000000000000;
            }
            
            long xHCF = GetHCF(ButtonA.x, ButtonB.x);
            long yHCF = GetHCF(ButtonA.y, ButtonB.y);

            if (coords.x % xHCF != 0 || coords.y % yHCF != 0)
                return;
            
            
            long maxA = Math.Min(coords.x + 1 / ButtonB.x, coords.y + 1 / ButtonB.y);
            if (part1)
                maxA = Math.Min(100, maxA);

            long minA = 0;
            
            for (long a = minA; a <= maxA; a++)
            {
                long xRequired = coords.x - a * button1.x;
                long yRequired = coords.y - a * button1.y;

                if (xRequired < 0 || yRequired < 0)
                    return;

                if (xRequired == 0 && yRequired == 0)
                {
                    MinimumCost = a * 3;
                    MinimumClicks = (a, 0);
                    return;
                }

                if (xRequired % button2.x != 0 || yRequired % button2.y != 0)
                    continue;
                    
                long b = xRequired / button2.x;
                if (b != yRequired / button2.y)
                    continue;
                
                long value = a * 3 + b;

                Console.WriteLine($"HCFx: {xHCF}, YCFx: {yHCF}, a: {a} | b: {b} | value: {value}");
                MinimumCost = value;
                MinimumClicks = (a, b);
                return;
            }
        }
    }
    
    private static long GetHCF(long a, long b)
    {
        long biggest = a > b ? a : b;

        for (long hcf = biggest; hcf > 0; hcf--)
        {
            if (a % hcf == 0 && b % hcf == 0)
                return hcf;
        }

        return 1;
    }
}

