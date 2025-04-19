using Databases.Interfaces;
using Databases.Records;
using NLog;

namespace Databases.Tables;

public class GameResults : ITable
{
    private readonly Dictionary<int, List<GameResultRecord>> _gameResults;
    private readonly object _gameResultsLock = new();
    
    private long NextGameId {get; set;}
    
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public GameResults()
    {
        _gameResults = new Dictionary<int, List<GameResultRecord>> ();
        NextGameId = 0;
    }
    
    /// <summary>
    /// Get next available game ID and increments the stored value for the next get attempt
    /// </summary>
    /// <returns>next available user ID</returns>
    public long GetNextGameId() => NextGameId++;
    
    public int Insert(int userId, IRecord record)
    {
        if (record is not GameResultRecord gameResultRecord)
        {
            logger.Error($"Record is not a GameResultRecord, instead it is a {record.GetType().Name}");
            return 0;
        }

        lock (_gameResultsLock)
        {
            try
            {
                _gameResults.Add(userId, new List<GameResultRecord> { gameResultRecord });
                return 1;
            }
            catch (ArgumentException e)
            {
                logger.Error($"Game results entry already exists for User ID: {userId}, see error: " + e.Message);
                return 0;
            }
        }
    }
    
    public int Update(int userId, IRecord record)
    {
        lock (_gameResultsLock)
        {
            if (record is GameResultRecord gameResultRecord)
            {
                if (_gameResults[userId].Any())
                {
                    _gameResults[userId].Add(gameResultRecord);
                }
                else
                {
                    _gameResults.Add(userId, new List<GameResultRecord> { gameResultRecord });
                }
                return 1;
            }
        }

        return 0;
    }

    public bool Delete(int userId)
    {
        lock (_gameResultsLock)
        {
            if (_gameResults.ContainsKey(userId))
            {
                _gameResults.Remove(userId);
                return true;
            }
        }

        return false;
    }

    public IRecord? Get(int userId, long? rowId = null)
    {
        lock (_gameResultsLock)
        {
            return _gameResults[userId].FirstOrDefault(x => x.GameId == rowId);
        }
    }
}