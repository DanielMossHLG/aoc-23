public static class Day4
{
    public static void Solution()
    {
        List<Card> cards = new List<Card>();
        string rawInput = Utils.GetInput("day4Input1.txt");

        string[] lines = rawInput.Split("\r\n");

        foreach (var line in lines)
        {
            Card card = new Card();
            var idNumbersPair = line.Split(':');

            card.ID = int.Parse(idNumbersPair[0].Substring(5));

            var numberStrings = idNumbersPair[1].Split('|');
            
            card.WinningNumbers = numberStrings[0].Trim().Split(' ');
            card.OwnedNumbers = numberStrings[1].Trim().Split(' ');

            cards.Add(card);
        }

        //Part 1
        int points = 0;
        foreach (var card in cards)
        {
            int matches = 0;

            foreach (var number in card.OwnedNumbers)
            {
                if (card.WinningNumbers.Contains(number) && !string.IsNullOrEmpty(number))
                    matches++;
            }

            card.Value = (int)MathF.Pow(2, matches - 1);
            //Console.WriteLine($"Card: {card.ID} | Matches: {matches} | Value: {card.Value}");
            points += card.Value;
        }
        Console.WriteLine($"Total points: {points}");
        
        //Part 2
        int sum = 0;
        for (int i = 0; i < cards.Count; i++)
        {
            int matches = 0;

            foreach (var number in cards[i].OwnedNumbers)
            {
                if (cards[i].WinningNumbers.Contains(number) && !string.IsNullOrEmpty(number))
                    matches++;
            }
            
            //Console.WriteLine($"Card: {cards[i].ID} | Matches: {matches} | Amount: {cards[i].Amount}");

            for (int j = 1; j <= matches; j++)
            {
                var indexNext = cards.FindIndex(x => x.ID == cards[i].ID + j);
                if (indexNext == -1) break;

                cards[indexNext].Amount += cards[i].Amount;
            }

            sum += cards[i].Amount;
        }
        Console.WriteLine($"Total number of cards: {sum}");
    }
}

public class Card
{
    public int ID = -1;
    public string[] WinningNumbers = Array.Empty<string>();
    public string[] OwnedNumbers = Array.Empty<string>();
    public int Value = 0;
    public int Amount = 1;
}