namespace BlackjackGameEngine.GameEngine;

public class Player
{
    public readonly Hand Hand = new();
    public decimal Balance { get; set; }

    public Player(decimal startingBalance)
    {
        this.Balance = startingBalance;
    }

    public int HandValue => Hand.Value;

    public void DrawCard(Card card) => Hand.Add(card);

    public void ClearHand() => Hand.Clear();

    public void AdjustBalance(int amount) => Balance += amount;

    public override string ToString() => Hand.ToString();
}
