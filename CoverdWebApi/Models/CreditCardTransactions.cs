using System.Transactions;
using Coverd.Common.Enums;

namespace ThreadPoolWebApi.Models
{
    public class CreditCardTransactions
    {
        public int UserId { get; set; }
        public int CreditCardId { get; set; }
        public List<CreditCardTransactionModel> Transactions { get; set; }
    }
}
