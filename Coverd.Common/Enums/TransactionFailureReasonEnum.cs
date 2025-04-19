namespace Coverd.Common.Enums;

public enum TransactionFailureReasonEnum
{
    Timeout = 1,
    CardDeclined = 2,
    AuthorizationFailure = 3,
    IncorrectCardInformation = 4
}