using System.Diagnostics.CodeAnalysis;
using Coverd.Common.Enums;
using Databases.Interfaces;
using Databases.Records;
using Databases.Tables;
using NLog;

namespace Databases;

/// <summary>
///  Class that represents a database containing multiple tables
/// </summary>
public class Database : IDatabase
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    private readonly Users UsersTable;
    private readonly AccountBalances AccountBalancesTable;
    private readonly Transactions TransactionsTable;
    private readonly GameResults GameResultsTable;
    private readonly CreditCardTransactions CreditCardTransactionsTable;
    
    public Database()
    {
        UsersTable = new Users();
        AccountBalancesTable = new AccountBalances();
        TransactionsTable = new Transactions();
        GameResultsTable = new GameResults();
        CreditCardTransactionsTable = new CreditCardTransactions();
    }

    #region User Management

    /// <summary>
    /// Create user by inserting a row into users table and account balances table
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <param name="passwordSalt"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="address"></param>
    /// <param name="startingBalance"></param>
    public void CreateUser(string firstName, string lastName, string email, string password, string passwordSalt, string phoneNumber,
        string address, decimal startingBalance = 0)
    {
        int userId = UsersTable.GetNextUserId();
        UserRecord userRecord = new UserRecord(userId, firstName, lastName, phoneNumber, email, password, DateTime.UtcNow);
        try
        {
            UsersTable.Insert(userId, userRecord);
            AccountBalancesTable.Insert(userId, new AccountBalanceRecord(userId, startingBalance, 100));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        logger.Info($"User created with userId: {userId} and a default account balance.");
    }
    
    public UserRecord GetOrCreateUser(int userId)
    {
        IRecord? record = UsersTable.Get(userId);
        if (record is null)
        {
            logger.Info($"User {userId} not found, creating a test one.");
            CreateUser("Test", "User", "test@test.com", "123456", "123456", "123456", 
                "123 EZ Street", 100);
            record = UsersTable.Get(userId);
        }

        return (record as UserRecord)!;
    }


    public void CancelUser(int userId)
    {
        if (UsersTable.Delete(userId))
        {
            logger.Info($"User {userId} deleted.");
        }
        else
        {
            logger.Error($"User {userId} not found.");
        }
    }

    #endregion

    #region Transaction Management

    /// <summary>
    /// Deposit USD into an account for user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="transactionId"></param>
    /// <param name="transactionAmount"></param>
    /// <param name="transactionDateTimeUtc"></param>
    /// <param name="merchantName"></param>
    /// <param name="creditCardNumber"></param>
    public bool AddCreditCardTransaction(int userId,
        long transactionId, decimal transactionAmount, DateTime transactionDateTimeUtc,
        string merchantName, string creditCardNumber)
    {
        CreditCardTransactionRecord transactionRecord = new CreditCardTransactionRecord(transactionId, userId,
            transactionDateTimeUtc, transactionAmount, 0, false, merchantName, creditCardNumber);
        try
        {
            bool success = CreditCardTransactionsTable.Insert(userId, transactionRecord) > 0;
            string transactionStatusMessage = success ? "added" : "failed to add"; 
            logger.Info($"User with userId: {userId} {transactionStatusMessage} a new credit card transaction {transactionId} " +
                        $"from {merchantName} with transaction amount: {transactionAmount}.");
            
            return success;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }


    /// <summary>
    /// insert transaction into transactions database table
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="amount"></param>
    /// <param name="transactionId"></param>
    /// <returns></returns>
    private long? InsertTransaction(int userId, double amount)
    {
        TransactionRecord transactionRecord = new TransactionRecord(TransactionsTable.GetNextTransactionId(), userId, 
            TransactionTypeEnum.Deposit, amount, DateTime.UtcNow, TransactionStatusEnum.Pending);
        if (TransactionsTable.Insert(userId, transactionRecord) == 0)
        {
            logger.Error($"Transaction {transactionRecord.TransactionId} failed to deposit.");
            return null;
        }

        return transactionRecord.TransactionId;
    }
    
    /// <inheritdoc/>
    public bool UpdateTransaction(int userId, long transactionId, TransactionFailureReasonEnum transactionFailureReasonEnum)
    {
        IRecord? record = TransactionsTable.Get(userId, transactionId);
        if (record is TransactionRecord transactionRecord)
        {
            transactionRecord.TransactionFailureReason = transactionFailureReasonEnum;
            return TransactionsTable.Update(userId, transactionRecord) > 0;
        }

        return false;
    }

    /// <inheritdoc/>
    public decimal GetAccountBalance(int userId)
    {
        IRecord? record = this.AccountBalancesTable.Get(userId);
        return record is not AccountBalanceRecord balanceRecord ? 0 : balanceRecord.CoverdCashBalance;
    }

    /// <summary>
    /// Update the balance for a user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="amount"></param>
    /// <param name="updatedCashBalance"></param>
    /// <param name="updatedPracticeCoinsBalance"></param>
    /// <param name="transactionStatus"></param>
    /// <returns></returns>
    private bool UpdateAccountBalance (int userId, double amount, decimal updatedCashBalance, double updatedPracticeCoinsBalance, TransactionTypeEnum transactionStatus)
    {
        AccountBalanceRecord updatedRecord = new AccountBalanceRecord(userId, updatedCashBalance, updatedPracticeCoinsBalance);
        if (AccountBalancesTable.Update(userId, updatedRecord) > 0)
        {
            string type = transactionStatus == TransactionTypeEnum.Deposit ? "Deposited" : "Withdrew";
            logger.Info($"{type} ${amount} to the account balance of User: {userId}. " +
                        $"User now has {updatedCashBalance} Covered Cash and {updatedPracticeCoinsBalance} Practice Coins. ");
            return true;
        }
        else
        {
            logger.Error($"Failed to deposit ${amount} to the account balance.");
            return false;
        }
    }

    #endregion

    #region Game Management

    /// <summary>
    /// Deposit USD into an account for user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="transactionId"></param>
    /// <param name="wager"></param>
    /// <param name="gameType"></param>
    /// <param name="gameResult"></param>
    /// <param name="balanceChange"></param>
    public void InsertGameData(int userId, long transactionId, decimal wager, GameTypeEnum gameType, GameResultEnum gameResult, decimal balanceChange)
    {
        long gameId = GameResultsTable.GetNextGameId();
        GameResultRecord gameResultRecord = new GameResultRecord(gameId, userId, gameType, gameResult, DateTime.UtcNow, wager, transactionId); 
        try
        {
            if (GameResultsTable.Insert(userId, gameResultRecord) > 0)
            {
                IRecord? record = AccountBalancesTable.Get(userId);
                if (record is AccountBalanceRecord accountBalanceRecord)
                {
                    accountBalanceRecord.CoverdCashBalance += balanceChange;
                    int updatedBalance = AccountBalancesTable.Update(userId, accountBalanceRecord);
                    if (updatedBalance == 0)
                    {
                        logger.Error($"Failed to update account balance for userId: {userId} when inserting gameId: ${gameId}.");
                    }
                }
            }
            else
            {
                logger.Error($"Failed to log game data with game id: ${gameId}.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        logger.Info($"Game data logged with gameId: {gameId} for userId: {userId}.");
    }
    
    #endregion
    
    
}