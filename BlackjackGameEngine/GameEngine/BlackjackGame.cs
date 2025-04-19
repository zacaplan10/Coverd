using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Coverd.Common.Enums;
using Databases;

namespace BlackjackGameEngine.GameEngine;

public class BlackjackGame
{
    private readonly Deck Deck;
    private readonly Player Player;
    private readonly Dealer Dealer;
    private const GameTypeEnum GameType = GameTypeEnum.Blackjack; 

    private decimal CurrentBet;
    private int UserId;
    private bool RoundOver;
    private GameResultEnum LastGameResult;
    
    private IDatabase Database {get; set;}

    public BlackjackGame(IDatabase database, int numberOfDecks, int userId)
    {
        this.Database = database;
        // Get balance from database if needed
        this.Database.GetOrCreateUser(userId);
        decimal balance = this.Database.GetAccountBalance(userId);
        
        Deck = new Deck(numberOfDecks);
        Player = new Player(balance);
        Dealer = new Dealer();
        UserId = userId;
    }

    /// <summary>
    /// Start new round of blackjack by checking balance is sufficient, shuffling and dealing cards 
    /// </summary>
    public bool StartNewRound(decimal bet)
    {
        if (Player.Balance < bet)
            return false;

        RoundOver = false;
        LastGameResult = GameResultEnum.InProgress;
        CurrentBet = bet;
        Player.Hand.Clear();
        Dealer.Hand.Clear();
        Deck.Shuffle();

        Player.Hand.Add(Deck.Draw());
        Dealer.Hand.Add(Deck.Draw());
        Player.Hand.Add(Deck.Draw());
        Dealer.Hand.Add(Deck.Draw());

        if (Player.HandValue == 21)
        {
            LastGameResult = GameResultEnum.BlackjackForUser;
            Player.Balance += CurrentBet * 2.5M;
            EndRound(LastGameResult);
        }

        return true;
    }

    /// <summary>
    /// Player hit actions
    /// </summary>
    public void PlayerHit()
    {
        if (RoundOver) return;

        Player.Hand.Add(Deck.Draw());
        
        switch (Player.HandValue)
        {
            case 21:
                LastGameResult = GameResultEnum.BlackjackForUser;
                Player.Balance += CurrentBet * 2.5M;
                EndRound(LastGameResult);
                break;
            case > 21:
                LastGameResult = GameResultEnum.WinForHouse;
                Player.Balance -= CurrentBet;
                EndRound(LastGameResult);
                break;
        }
    }

    /// <summary>
    /// Player stand actions
    /// </summary>
    public void PlayerStand()
    {
        if (RoundOver)
        {
            return;
        }

        while (Dealer.HandValue < 17)
        {
            Dealer.Hand.Add(Deck.Draw());
        }

        GameResultEnum result = ResolveGame();
        EndRound(result);
    }

    /// <summary>
    /// Handle any outstanding actions when player stands
    /// </summary>
    /// <returns></returns>
    private GameResultEnum ResolveGame()
    {
        int playerTotal = Player.HandValue;
        int dealerTotal = Dealer.HandValue;

        if (dealerTotal > 21 || playerTotal > dealerTotal)
        {
            LastGameResult = GameResultEnum.WinForUser;
            Player.Balance += CurrentBet;
        }
        else if (playerTotal < dealerTotal)
        {
            LastGameResult = GameResultEnum.WinForHouse;
            Player.Balance -= CurrentBet;
        }
        else
        {
            LastGameResult = GameResultEnum.Tie;
        }

        return LastGameResult;
    }

    /// <summary>
    /// Determine game results and adjust balances
    /// </summary>
    /// <param name="result"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private void EndRound(GameResultEnum result)
    {
        RoundOver = true;
        decimal balanceChange = 0;
        switch (result)
        {
            case GameResultEnum.BlackjackForUser:
                balanceChange += CurrentBet * 2.5M;
                break;
            case GameResultEnum.WinForUser:
                balanceChange += CurrentBet * 2;
                break;
            case GameResultEnum.WinForHouse:
                balanceChange = 0;
                break;
            case GameResultEnum.Tie:
                balanceChange += CurrentBet;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(result), result, null);
        }
        
        Database.InsertGameData(UserId, -1, CurrentBet, GameType, result, balanceChange);
    }

    /// <summary>
    /// Getter for current game state
    /// </summary>
    /// <returns>game state snapshot</returns>
    public BlackjackState GetState()
    {
        return new BlackjackState
        {
            PlayerHand = Player.Hand.Cards.Select(c => c.ToString()).ToList(),
            DealerHand = RoundOver
                ? Dealer.Hand.Cards.Select(c => c.ToString()).ToList()
                : new List<string> { Dealer.Hand.Cards.First().ToString(), "[Hidden]" },
            PlayerValue = Player.HandValue,
            DealerValue = RoundOver ? Dealer.HandValue : 0,
            Balance = Player.Balance,
            IsGameOver = RoundOver,
            GameResult = GetGameResultDescription(LastGameResult)
        };
    }

    /// <summary>
    /// Convert GameResultEnum to description to pass to front end
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static string GetGameResultDescription(GameResultEnum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString()) ?? throw new InvalidOperationException();
        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

        return attributes.Length > 0 ? attributes[0].Description : value.ToString();
    }
}
