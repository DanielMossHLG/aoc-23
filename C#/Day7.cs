public static class Day7
{
    public static void Solution()
    {
        string cardStrength = "AKQJT98765432";
        string rawInput = Utils.GetInput("day7input1.txt");
        
        string[] lines = rawInput.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        List<Game> games = new List<Game>();
        
        foreach (var line in lines)
        {
            var game = new Game();
            var handBet = line.Split(' ');

            game.Hand = handBet[0];
            game.Bet = int.Parse(handBet[1]);
            game.BaseType = GetHandType(game.Hand);
            game.JokerType = GetHandTypeJoker(game.Hand);
            
            games.Add(game);
        }

        games.Sort((a, b) =>
        {
            if (a.BaseType != b.BaseType)
            {
                return b.BaseType - a.BaseType;
            }

            for (int i = 0; i < a.Hand.Length; i++)
            {
                if (a.Hand[i] == b.Hand[i]) continue;

                return cardStrength.IndexOf(b.Hand[i]) - cardStrength.IndexOf(a.Hand[i]);
            }
            return 0;
        });

        int totalWinningsPart1 = 0;
        for (int i = 0; i < games.Count; i++)
        {
            totalWinningsPart1 += games[i].Bet * (i + 1);
        }
        
        cardStrength = "AKQT98765432J";
        games.Sort((a, b) =>
        {
            if (a.JokerType != b.JokerType)
            {
                return b.JokerType - a.JokerType;
            }

            for (int i = 0; i < a.Hand.Length; i++)
            {
                if (a.Hand[i] == b.Hand[i]) continue;

                return cardStrength.IndexOf(b.Hand[i]) - cardStrength.IndexOf(a.Hand[i]);
            }
            return 0;
        });

        int totalWinningsPart2 = 0;
        for (int i = 0; i < games.Count; i++)
        {
            totalWinningsPart2 += games[i].Bet * (i + 1);
        }
        Console.WriteLine($"Part1: {totalWinningsPart1} | Part2: {totalWinningsPart2}");
    }
    
    private static HandType GetHandTypeJoker(string hand)
    {
        int highest = 0;
        int secondHighest = 0;
        string checkedLetters = "";
        int numOfJokers = 0;
        foreach (var card in hand)
        {
            if (card == 'J')
            {
                numOfJokers++;
                continue;
            }
            if (checkedLetters.Contains(card)) continue;
            checkedLetters += card;
            var amountOf = new string(hand.Where(x=> x == card).ToArray());
            if (amountOf.Length < secondHighest) continue;

            if (amountOf.Length > highest)
            {
                secondHighest = highest;
                highest = amountOf.Length;
            }
            else secondHighest = amountOf.Length;
        }

        highest += numOfJokers;
        if (highest == 5)
            return HandType.FiveOAK;
        if (highest == 4)
            return HandType.FourOAK;
        if (highest == 3 && secondHighest == 2)
            return HandType.FullHouse;
        if (highest == 3)
            return HandType.ThreeOAK;
        if (highest == 2 && secondHighest == 2)
            return HandType.TwoPair;
        if (highest == 2)
            return HandType.OnePair;

        return HandType.HighCard;
    }

    private static HandType GetHandType(string hand)
    {
        int highest = 0;
        int secondHighest = 0;
        string checkedLetters = "";
        foreach (var card in hand)
        {
            if (checkedLetters.Contains(card)) continue;
            checkedLetters += card;
            var amountOf = new string(hand.Where(x=> x == card).ToArray());
            if (amountOf.Length < secondHighest) continue;

            if (amountOf.Length > highest)
            {
                secondHighest = highest;
                highest = amountOf.Length;
            }
            else secondHighest = amountOf.Length;
        }
        if (highest == 5)
            return HandType.FiveOAK;
        if (highest == 4)
            return HandType.FourOAK;
        if (highest == 3 && secondHighest == 2)
            return HandType.FullHouse;
        if (highest == 3)
            return HandType.ThreeOAK;
        if (highest == 2 && secondHighest == 2)
            return HandType.TwoPair;
        if (highest == 2)
            return HandType.OnePair;

        return HandType.HighCard;
    }
}

public class Game
{
    public string Hand = "";
    public int Bet;
    public HandType BaseType;
    public HandType JokerType;
}

public enum HandType
{
    FiveOAK,
    FourOAK,
    FullHouse,
    ThreeOAK,
    TwoPair,
    OnePair,
    HighCard
}