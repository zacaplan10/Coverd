using System;
using Databases.Interfaces;

namespace Databases.Records
{
    /// <summary>
    /// Class that represents a credit card transaction
    /// </summary>
    public class CreditCardTransactionRecord : IRecord
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
        /// Date and time the transaction occurred (UTC)
        /// </summary>
        public DateTime TransactionDateTimeUtc { get; set; }

        /// <summary>
        /// Total transaction amount (USD)
        /// </summary>
        public decimal TransactionAmount { get; set; }

        /// <summary>
        /// Amount covered by rewards or program (USD)
        /// </summary>
        public decimal AmountCovered { get; set; }

        /// <summary>
        /// Whether the transaction is fully covered
        /// </summary>
        public bool IsCovered { get; set; }

        /// <summary>
        /// Name of the merchant
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// Last 4 digits or identifier of the credit card
        /// </summary>
        public string CreditCardNumber { get; set; }

        /// <summary>
        /// Creates a new CreditCardTransactionRecord object
        /// </summary>
        public CreditCardTransactionRecord(
            long transactionId,
            int userId,
            DateTime transactionDateTimeUtc,
            decimal transactionAmount,
            decimal amountCovered,
            bool isCovered,
            string merchantName,
            string creditCardNumber)
        {
            this.TransactionId = transactionId;
            this.UserId = userId;
            this.TransactionDateTimeUtc = transactionDateTimeUtc;
            this.TransactionAmount = transactionAmount;
            this.AmountCovered = amountCovered;
            this.IsCovered = isCovered;
            this.MerchantName = merchantName;
            this.CreditCardNumber = creditCardNumber;
        }
    }
}