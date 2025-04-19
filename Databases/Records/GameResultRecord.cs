using Coverd.Common.Enums;
using Databases.Interfaces;

namespace Databases.Records;

/// <summary>
///  Class that represents a row in a users table 
/// </summary>
public class GameResultRecord : IRecord
{
    /// <summary>
    /// Unique ID for game
    /// </summary>
    public long GameId { get; set; }
    
    /// <summary>
    /// Unique ID for user
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// What game was played
    /// </summary>
    public GameTypeEnum GameType { get; set; }
    
    /// <summary>
    /// What was the result of the game played
    /// </summary>
    public GameResultEnum GameResult { get; set; }
    
    /// <summary>
    /// How much money was wagered
    /// </summary>
    public decimal Wager { get; set; }

    /// <summary>
    /// What time and date was the game was played (UTC)
    /// </summary>
    public DateTime GameDateTimeUtc { get; set; }

    /// <summary>
    /// Id of the transaction player was betting against
    /// </summary>
    public long TransactionId { get; set; }


    /// <summary>
    /// Creates a new UserRecord object
    /// </summary>
    /// <param name="gameId"></param>
    /// <param name="userId"></param>
    /// <param name="gameType"></param>
    /// <param name="gameResult"></param>
    /// <param name="gameDateTimeUtc"></param>
    /// <param name="wager"></param>
    /// <param name="transactionId"></param>
    public GameResultRecord(long gameId, int userId, GameTypeEnum gameType, GameResultEnum gameResult,
        DateTime gameDateTimeUtc, decimal wager, long transactionId)
    {
        this.GameId = gameId;
        this.UserId = userId;
        this.GameType = gameType;
        this.GameResult = gameResult;
        this.GameDateTimeUtc = gameDateTimeUtc;
        this.Wager = wager;
        this.TransactionId = transactionId;
    }
}