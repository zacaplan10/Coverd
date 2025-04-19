using Coverd.Common.Enums;

namespace CoverdWebApi.Models
{
    public class TransactionModel
    {
        public int UserId { get; set; }
        public double Amount { get; set; }

        public TransactionTypeEnum TransactionType { get; set; }
    }
}
