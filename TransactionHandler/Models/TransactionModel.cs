using Coverd.Common.Enums;

namespace TransactionHandler.Models;

    public class TransactionModel
    {
        public int UserId { get; set; }
        public double Amount { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
    }
