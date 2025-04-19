using Databases.Interfaces;
using Databases.Records;
using NLog;

namespace Databases.Tables;

public class Transactions : ITable
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    
    private readonly Dictionary<int, List<TransactionRecord>>  _transactions;
    private readonly object _transactionsLock = new();
    
    private long NextTransactionId {get; set;}

    public Transactions()
    {
        _transactions = new Dictionary<int, List<TransactionRecord>> ();
        NextTransactionId = 0;
    }

    /// <summary>
    /// Get next available transaction ID and increments the stored value for the next get attempt
    /// </summary>
    /// <returns>next available user ID</returns>
    public long GetNextTransactionId()
    {
        lock (_transactionsLock)
        {
            return NextTransactionId++;
        }
    }
    
    public int Insert(int userId, IRecord record)
    {
        if (record is not TransactionRecord transactionRecord)
        {
            logger.Error($"Record is not a TransactionRecord, instead it is a {record.GetType().Name}");
            return 0;
        }

        lock (_transactionsLock)
        {
            try
            {
                if (_transactions.TryGetValue(userId, out var transaction))
                {
                    transaction.Add(transactionRecord);
                }
                else
                {
                    _transactions.Add(userId, new List<TransactionRecord> { transactionRecord });
                }
                
                return 1;
            }
            catch (ArgumentException e)
            {
                logger.Error($"Transaction entry already exists for User ID: {userId}, see error: " + e.Message);
                return 0;
            }
        }
    }
    
    public int Update(int userId, IRecord record)
    {
        if (record is not TransactionRecord transactionRecord)
        {
            logger.Error($"Record is not a TransactionRecord, instead it is a {record.GetType().Name}");
            return 0;
        }
        
        lock (_transactionsLock)
        {
            if (_transactions[userId].RemoveAll(x => x.TransactionId == transactionRecord.TransactionId) > 0)
            {
                _transactions[userId].Add(transactionRecord);
            }
            else
            {
                logger.Warn($"No record to update, adding a new row with transactionId: {transactionRecord.TransactionId} for user: {userId}");
                _transactions.Add(userId, new List<TransactionRecord> { transactionRecord });
                return 1;
            }
            
            return 1;
        }
    }

    public bool Delete(int userId)
    {
        lock (_transactionsLock)
        {
            if (_transactions.ContainsKey(userId))
            {
                _transactions.Remove(userId);
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc/>
    public IRecord? Get(int userId, long? rowId = null)
    {
        lock (_transactionsLock)
        {
            return _transactions[userId].FirstOrDefault(x => x.TransactionId == rowId);
        }
    }
}