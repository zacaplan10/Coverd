using Databases.Interfaces;

namespace Databases.Records;

/// <summary>
///  Class that represents a row in a users table 
/// </summary>
public class UserRecord : IRecord
{
    /// <summary>
    /// Unique ID for user
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// First name of user
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Last name of user
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Phone number of user
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// User email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// User password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// date and time a user registered for Coverd
    /// </summary>
    public DateTime RegistrationDateTimeUtc { get; set; }
    
    /// <summary>
    /// last time and date a user logged into Coverd
    /// </summary>
    public DateTime? LastLoginUtc { get; set; }

    /// <summary>
    /// Creates a new UserRecord object
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <param name="lastLoginUtc"></param>
    /// <param name="registrationDateTimeUtc"></param>
    public UserRecord(int userId, string firstName, string lastName, string phoneNumber, string email, string password, 
        DateTime registrationDateTimeUtc, DateTime? lastLoginUtc = null)
    {
        this.UserId = userId;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.PhoneNumber = phoneNumber;
        this.Email = email;
        this.Password = password;
        this.RegistrationDateTimeUtc = registrationDateTimeUtc;
        this.LastLoginUtc = lastLoginUtc;
    }
}