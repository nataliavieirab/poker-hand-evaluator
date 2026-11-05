namespace PokerHandEvaluator.Domain.Exceptions;

public sealed class InvalidHandException : Exception
{
    public InvalidHandException(string message)
        : base(message)
    {
    }
}
