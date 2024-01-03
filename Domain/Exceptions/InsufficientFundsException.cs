namespace Domain.Exceptions;

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException() : base("Sender account balance is insufficient.") { }
}
