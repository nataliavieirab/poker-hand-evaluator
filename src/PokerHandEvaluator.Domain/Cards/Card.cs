namespace PokerHandEvaluator.Domain.Cards;

public sealed class Card : IEquatable<Card>
{
    public CardValue Value { get; }

    public Suit Suit { get; }

    public Card(CardValue value, Suit suit)
    {
        Value = value;
        Suit = suit;
    }

    public bool Equals(Card? other)
    {
        return other is not null && Value == other.Value && Suit == other.Suit;
    }

    public override bool Equals(object? obj)
    {
        return obj is Card other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Suit);
    }

    public override string ToString()
    {
        return $"{GetValueSymbol(Value)}{GetSuitSymbol(Suit)}";
    }

    private static char GetValueSymbol(CardValue value)
    {
        return value switch
        {
            CardValue.Two => '2',
            CardValue.Three => '3',
            CardValue.Four => '4',
            CardValue.Five => '5',
            CardValue.Six => '6',
            CardValue.Seven => '7',
            CardValue.Eight => '8',
            CardValue.Nine => '9',
            CardValue.Ten => 'D',
            CardValue.Jack => 'J',
            CardValue.Queen => 'Q',
            CardValue.King => 'K',
            CardValue.Ace => 'A',
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }

    private static char GetSuitSymbol(Suit suit)
    {
        return suit switch
        {
            Suit.Diamonds => 'O',
            Suit.Hearts => 'C',
            Suit.Spades => 'E',
            Suit.Clubs => 'P',
            _ => throw new ArgumentOutOfRangeException(nameof(suit))
        };
    }
}
