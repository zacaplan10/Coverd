using Coverd.Common.Enums;
using Databases.Records;

namespace Databases;

public interface IDatabase
{
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
    void CreateUser(string firstName, string lastName, string email, string password, string passwordSalt, string phoneNumber, 
        string address, decimal startingBalance = 0);

    /// <summary>
    /// Get user from database or create dummy row if doesn't exist
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    UserRecord GetOrCreateUser(int userId);

    /// <summary>
    /// Deposit USD into an account for user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="transactionId"></param>
    /// <param name="transactionAmount"></param>
    /// <param name="transactionDateTimeUtc"></param>
    /// <param name="merchantName"></param>
    /// <param name="creditCardNumber"></param>
    bool AddCreditCardTransaction(int userId, long transactionId, decimal transactionAmount, DateTime transactionDateTimeUtc,
        string merchantName, string creditCardNumber);

    /// <summary>
    /// Adjust existing transaction
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="transactionId"></param>
    /// <param name="transactionFailureReasonEnum"></param>
    bool UpdateTransaction(int userId, long transactionId, TransactionFailureReasonEnum? transactionFailureReasonEnum);

    /// <summary>
    /// Record a game played for a user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="transactionId"></param>
    /// <param name="wager"></param>
    /// <param name="gameType"></param>
    /// <param name="gameResult"></param>
    /// <param name="balanceChange"></param>
    void InsertGameData(int userId, long transactionId, decimal wager, GameTypeEnum gameType, GameResultEnum gameResult, decimal balanceChange);

    /// <summary>
    /// Gets the account balance for a user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    decimal GetAccountBalance(int userId);
}