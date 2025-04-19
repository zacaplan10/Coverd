using Databases.Interfaces;

namespace Databases.Records;

/// <summary>
///  Class that represents a row in an account balance table 
/// </summary>
public class AccountBalanceRecord : IRecord
{
    /// <summary>
    /// Unique ID for user
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// Current balance for a user
    /// </summary>
    public decimal CoverdCashBalance { get; set; }
    
    /// <summary>
    /// Current balance for a user
    /// </summary>
    public double PracticeCoinsBalance { get; set; }

    /// <summary>
    /// Creates a new AccountBalanceRecord object
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="coverdCashBalance"></param>
    /// <param name="practiceCoinsBalance"></param>
    public AccountBalanceRecord(int userId, decimal coverdCashBalance, double practiceCoinsBalance)
    {
        this.UserId = userId;
        this.CoverdCashBalance = coverdCashBalance;
        this.PracticeCoinsBalance = practiceCoinsBalance;
        
    }
}