namespace Domain.Exceptions;

public class NegativeAmountException : Exception
{
    public NegativeAmountException() : base("Sum must be positive number.") { }
}
