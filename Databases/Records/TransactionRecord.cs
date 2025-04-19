using Coverd.Common.Enums;
using Databases.Interfaces;

namespace Databases.Records;

/// <summary>
///  Class that represents a row in a transactions table 
/// </summary>
public class TransactionRecord : IRecord
{
    /// <summary>
    /// Unique ID for transaction
    /// </summary>
    public long TransactionId { get; set; }
    
    /// <summary>
    /// Unique ID for user
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Transaction type
    /// </summary>
    public TransactionTypeEnum TransactionTypeEnum { get; set; }
    
    /// <summary>
    /// Date and time transaction occured
    /// </summary>
    public DateTime TransactionDateTimeUtc { get; set; }
    
    /// <summary>
    /// What is the status of the transaction?
    /// </summary>
    public TransactionStatusEnum TransactionProcessed { get; set; }
    
    /// <summary>
    /// How much was the transaction for? (positive numbers only, USD)
    /// </summary>
    public double Amount { get; set; }
    
    /// <summary>
    /// If transaction failed, why?
    /// </summary>
    public TransactionFailureReasonEnum TransactionFailureReason { get; set; }

    /// <summary>
    /// Creates a new TransactionRecord object
    /// </summary>
    /// <param name="transactionId"></param>
    /// <param name="userId"></param>
    /// <param name="transactionTypeEnum"></param>
    /// <param name="amount"></param>
    /// <param name="transactionDateTimeUtc"></param>
    /// <param name="transactionProcessed"></param>
    public TransactionRecord(long transactionId, int userId, TransactionTypeEnum transactionTypeEnum, double amount, 
        DateTime transactionDateTimeUtc, TransactionStatusEnum transactionProcessed)
    {
        this.TransactionId = transactionId;
        this.UserId = userId;
        this.TransactionTypeEnum = transactionTypeEnum;
        this.Amount = amount;
        this.TransactionDateTimeUtc = transactionDateTimeUtc;
        this.TransactionProcessed = transactionProcessed;
    }
}