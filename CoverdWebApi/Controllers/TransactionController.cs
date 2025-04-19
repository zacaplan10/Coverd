using Microsoft.AspNetCore.Mvc;
using NLog;
using ThreadPoolWebApi.Models;
using TransactionHandler.Interfaces;

namespace CoverdWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        public readonly ITransactionHandler TransactionHandler;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        public TransactionController(ITransactionHandler transactionHandler)
        {
            this.TransactionHandler = transactionHandler;
        }
        
        [HttpPost("addTransaction")]
        public IActionResult PostCreditCardTransaction([FromBody] CreditCardTransactionModel? transaction)
        {
            if (transaction == null)
            {
                return BadRequest("Invalid request body");
            }
            
            bool success = false;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                success = DoCreditCardTransactionWork(transaction);
            });

            return success ? Ok("Transaction is being processed.") : Ok("Transaction was not processed.");
        }

        private bool DoCreditCardTransactionWork(CreditCardTransactionModel model)
        {
            
            bool success = TransactionHandler.DoTransaction(model.UserId, model.TransactionAmount, 
                model.TransactionDateTimeUtc, model.TransactionId, model.MerchantName, model.CreditCardNumber);
            string action = success ? "Processed" : "Failed";
            logger.Info($"{action} transaction for {model.UserId}, amount: {model.TransactionAmount}");
            return success;
        }
    }
}
