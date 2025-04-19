namespace ThreadPoolWebApi.Models
{
    public class CreditCardTransactionModel
    {
        /// <summary>
        /// Unique ID for user
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// Unique ID for transaction
        /// </summary>
        public long TransactionId { get; set; }

        /// <summary>
        /// Date and time the transaction occurred (UTC)
        /// </summary>
        public DateTime TransactionDateTimeUtc { get; set; }

        /// <summary>
        /// Total transaction amount (USD)
        /// </summary>
        public decimal TransactionAmount { get; set; }

        /// <summary>
        /// Name of the merchant
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// Credit card number transaction was processed on
        /// </summary>
        public string CreditCardNumber { get; set; }
    }
}
