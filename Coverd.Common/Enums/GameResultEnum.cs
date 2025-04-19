using System.ComponentModel;

namespace Coverd.Common.Enums;

public enum GameResultEnum
{
    [Description("Win for user!")]
    WinForUser = 1,
    [Description("Blackjack for user!")]
    BlackjackForUser = 2,
    [Description("User loses")]
    WinForHouse = 3,
    [Description("Game is a push")]
    Tie = 4, 
    [Description("Game in progress")]
    InProgress = 5,
    [Description("Insufficient funds, please add money to your account")]
    InsufficientFunds = 5,
}

