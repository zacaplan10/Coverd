namespace BlackjackGameEngine.GameEngine;

using System.Collections.Generic;
using System.Linq;

public class Hand
{
    private readonly List<Card> _cards = new();

    public void Add(Card card) => _cards.Add(card);

    public void Clear() => _cards.Clear();

    public IEnumerable<Card> Cards => _cards;

    public int Value
    {
        get
        {
            int sum = _cards.Sum(c => c.Value);
            int aceCount = _cards.Count(c => c.Rank == Rank.Ace);

            while (sum > 21 && aceCount > 0)
            {
                sum -= 10;
                aceCount--;
            }

            return sum;
        }
    }

    public override string ToString()
    {
        return string.Join(", ", _cards) + $" (Total: {Value})";
    }
}
