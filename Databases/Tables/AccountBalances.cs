using Databases.Interfaces;
using Databases.Records;
using NLog;

namespace Databases.Tables;

public class AccountBalances : ITable
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    
    private readonly Dictionary<int, AccountBalanceRecord> _accountBalances;
    private readonly object _accountBalancesLock = new();

    public AccountBalances()
    {
        _accountBalances = new Dictionary<int, AccountBalanceRecord>();
    }
    
    public int Insert(int userId, IRecord record)
    {
        if (record is not AccountBalanceRecord accountBalanceRecord)
        {
            logger.Error($"Record is not a AccountBalanceRecord, instead it is a {record.GetType().Name}");
            return 0;
        }

        lock (_accountBalancesLock)
        {
            try
            {
                _accountBalances.Add(userId, accountBalanceRecord);
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
        if (record is not AccountBalanceRecord accountBalanceRecord)
        {
            logger.Error($"Record is not a AccountBalanceRecord, instead it is a {record.GetType().Name}");
            return 0;
        }
        
        lock (_accountBalancesLock)
        {
            _accountBalances[userId] = accountBalanceRecord;
            return 1;
        }
    }

    public bool Delete(int userId)
    {
        lock (_accountBalancesLock)
        {
            if (_accountBalances.ContainsKey(userId))
            {
                _accountBalances.Remove(userId);
                return true;
            }
        }

        return false;
    }

    public IRecord? Get(int userId, long? rowId = null)
    {
        lock (_accountBalancesLock)
        {
            return _accountBalances[userId];
        }
    }
}