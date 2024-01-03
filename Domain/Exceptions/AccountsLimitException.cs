namespace Domain.Exceptions;

public class AccountsLimitException : Exception
{
    public AccountsLimitException() : base("User can have maximum 2 accounts.") { }
}
