using Coverd.Common.Enums;
using System.Collections.Generic;

namespace BlackjackGameEngine.GameEngine
{
    /// <summary>
    /// Representation of a blackjack game state snapshot.
    /// </summary>
    public class BlackjackState
    {
        /// <summary>
        /// The list of card descriptions currently in the player's hand.
        /// </summary>
        public List<string> PlayerHand { get; set; } = new List<string>();
        
        /// <summary>
        /// The list of card descriptions currently in the dealer's hand.
        /// </summary>
        public List<string> DealerHand { get; set; } = new List<string>();
        
        /// <summary>
        /// The total value of the player's hand.
        /// </summary>
        public int PlayerValue { get; set; }
        
        /// <summary>
        /// The total value of the dealer's hand.
        /// </summary>
        public int DealerValue { get; set; }

        /// <summary>
        /// The player's current account balance after the latest round.
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Indicates whether the game round has ended.
        /// </summary>
        public bool IsGameOver { get; set; }

        /// <summary>
        /// The result of the game round (e.g., Win, Lose, Push).
        /// </summary>
        public string GameResult { get; set; }
    }
}