using Databases.Interfaces;
using Databases.Records;
using NLog;

namespace Databases.Tables;

public class Users : ITable
{
    
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    
    private readonly Dictionary<int, UserRecord> _users;
    private readonly object _usersLock = new();
    private int NextUserId {get; set;}

    public Users()
    {
        _users = new Dictionary<int, UserRecord>();
        NextUserId = 0;
    }
    
    /// <summary>
    /// Get next available user ID and increments the stored value for the next get attempt
    /// </summary>
    /// <returns>next available user ID</returns>
    public int GetNextUserId() => NextUserId++;
    
    public int Insert(int userId, IRecord record)
    {
        if (record is not UserRecord userRecord)
        {
            logger.Error($"Record is not a UserRecord, instead it is a {record.GetType().Name}");
            return 0;
        }

        lock (_usersLock)
        {
            try
            {
                _users.Add(userId, userRecord);
                return 1;
            }
            catch (ArgumentException e)
            {
                logger.Error("User ID already exists, see error: " + e.Message);
                return 0;
            }
        }
    }
    
    public int Update(int userId, IRecord record)
    {
        if (record is not UserRecord userRecord)
        {
            logger.Error($"Record is not a UserRecord, instead it is a {record.GetType().Name}");
            return 0;
        }
        
        lock (_usersLock)
        {
            _users[userId] = userRecord;
            return 1;
        }
    }

    public bool Delete(int userId)
    {
        lock (_usersLock)
        {
            if (_users.ContainsKey(userId))
            {
                _users.Remove(userId);
                return true;
            }
        }

        return false;
    }

    public IRecord? Get(int userId, long? rowId = null)
    {
        lock (_usersLock)
        {
            return _users.TryGetValue(userId, out var record) ? record : null;
        }
    }
}