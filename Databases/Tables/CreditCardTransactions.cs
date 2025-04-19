using Databases.Interfaces;
using Databases.Records;
using NLog;

namespace Databases.Tables
{
    public class CreditCardTransactions : ITable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<int, List<CreditCardTransactionRecord>> _creditCardTransactions;
        private readonly object _creditCardTransactionsLock = new();

        private long NextTransactionId { get; set; }

        public CreditCardTransactions()
        {
            _creditCardTransactions = new Dictionary<int, List<CreditCardTransactionRecord>>();
            NextTransactionId = 0;
        }

        public long GetNextTransactionId()
        {
            lock (_creditCardTransactionsLock)
            {
                return NextTransactionId++;
            }
        }

        public int Insert(int userId, IRecord record)
        {
            if (record is not CreditCardTransactionRecord transaction)
            {
                logger.Error($"Record is not a CreditCardTransactionRecord, instead it is a {record.GetType().Name}");
                throw new ArgumentException("Record is not a CreditCardTransactionRecord");
            }

            lock (_creditCardTransactionsLock)
            {
                try
                {
                    if (_creditCardTransactions.TryGetValue(userId, out var transactions))
                    {
                        transactions.Add(transaction);
                    }
                    else
                    {
                        _creditCardTransactions[userId] = new List<CreditCardTransactionRecord> { transaction };
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
            if (record is not CreditCardTransactionRecord transaction)
            {
                logger.Error($"Record is not a CreditCardTransactionRecord, instead it is a {record.GetType().Name}");
                return 0;
            }

            lock (_creditCardTransactionsLock)
            {
                if (_creditCardTransactions.TryGetValue(userId, out var transactions))
                {
                    var removed = transactions.RemoveAll(t => t.TransactionId == transaction.TransactionId);
                    transactions.Add(transaction);

                    if (removed == 0)
                    {
                        logger.Warn($"No record to update, added a new one with transactionId: {transaction.TransactionId} for user: {userId}");
                    }

                    return 1;
                }
                else
                {
                    _creditCardTransactions[userId] = new List<CreditCardTransactionRecord> { transaction };
                    logger.Warn($"No existing list for user {userId}, creating new with transactionId: {transaction.TransactionId}");
                    return 1;
                }
            }
        }

        public bool Delete(int userId)
        {
            lock (_creditCardTransactionsLock)
            {
                return _creditCardTransactions.Remove(userId);
            }
        }

        public IRecord? Get(int userId, long? rowId = null)
        {
            if (rowId == null) return null;

            lock (_creditCardTransactionsLock)
            {
                if (_creditCardTransactions.TryGetValue(userId, out var transactions))
                {
                    return transactions.FirstOrDefault(x => x.TransactionId == rowId.Value);
                }

                return null;
            }
        }
    }
}
