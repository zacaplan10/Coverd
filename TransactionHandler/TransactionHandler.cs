using System.Transactions;
using Coverd.Common.Enums;
using Databases;
using TransactionHandler.Interfaces;
using TransactionHandler.Models;

namespace TransactionHandler;

public class TransactionHandler : ITransactionHandler
{
    public IDatabase Database { get; }
    
    
    public TransactionHandler(IDatabase database)
    {
        this.Database = database;
    }

    public bool DoTransaction(int userId, decimal transactionAmount, DateTime transactionDateTimeUtc,
        long transactionId, string merchantName, string creditCardNumber)
    {
        return Database.AddCreditCardTransaction(userId, transactionId, transactionAmount, transactionDateTimeUtc, merchantName, creditCardNumber);
    }

    public bool UpdateTransaction(int userId, long transactionId, TransactionFailureReasonEnum? failureReason = null)
    {
        return Database.UpdateTransaction(userId, transactionId, TransactionFailureReasonEnum.IncorrectCardInformation);
    }
    
    

    
}