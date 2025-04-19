namespace BlackjackGameEngine.GameEngine;

public enum Suit
{
    Hearts, Diamonds, Clubs, Spades
}

public enum Rank
{
    Two = 2, Three, Four, Five, Six, Seven,
    Eight, Nine, Ten, Jack, Queen, King, Ace
}

public class Card
{
    public Suit Suit { get; }
    public Rank Rank { get; }

    public Card(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public int Value => Rank switch
    {
        >= Rank.Two and <= Rank.Ten => (int)Rank,
        Rank.Jack or Rank.Queen or Rank.King => 10,
        Rank.Ace => 11,
        _ => 0
    };

    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
}
