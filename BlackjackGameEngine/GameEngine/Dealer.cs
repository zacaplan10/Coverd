namespace BlackjackGameEngine.GameEngine;

public class Dealer : Player
{
    public Dealer() : base(Decimal.MaxValue) { }

    public string ShowFirstCard()
    {
        var first = this.Hand.Cards.FirstOrDefault();
        return first != null ? first.ToString() : "(no cards)";
    }
}