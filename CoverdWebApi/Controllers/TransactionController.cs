using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using Coverd.Common.Enums;
using Databases;
using NLog;
using ThreadPoolWebApi.Models;
using TransactionHandler.Interfaces;

namespace ThreadPoolWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly IDatabase Database;
        public readonly ITransactionHandler TransactionHandler;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        public TransactionController(IDatabase database, ITransactionHandler transactionHandler)
        {
            this.Database = database;
            this.TransactionHandler = transactionHandler;
        }
        
        [HttpPost]
        public IActionResult PostCreditCardTransaction([FromBody] CreditCardTransactionModel? transaction)
        {
            if (transaction == null)
            {
                return BadRequest("Invalid request body");
            }
            
            ThreadPool.QueueUserWorkItem(_ =>
            {
                DoCreditCardTransactionWork(transaction);
            });

            return Accepted("Transaction is being processed.");
        }

        private void DoCreditCardTransactionWork(CreditCardTransactionModel model)
        {
            
            bool success = TransactionHandler.DoTransaction(model.UserId, model.TransactionAmount, 
                model.TransactionDateTimeUtc, model.TransactionId, model.MerchantName, model.CreditCardNumber);
            string action = success ? "Processed" : "Failed";
            logger.Info($"{action} transaction for {model.UserId}, amount: {model.TransactionAmount}");
        }
    }
}
