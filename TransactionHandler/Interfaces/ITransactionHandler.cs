using Coverd.Common.Enums;

namespace TransactionHandler.Interfaces;

public interface ITransactionHandler
{
    bool DoTransaction(int userId, decimal transactionAmount, 
        DateTime transactionDateTimeUtc, long transactionId, string merchantName, string creditCardNumber);
    
    bool UpdateTransaction(int userId, long transactionId, TransactionFailureReasonEnum? failureReason = null);

}