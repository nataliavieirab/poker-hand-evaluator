namespace PokerHandEvaluator.Domain.Hands;

public sealed class HandEvaluation
{
    public HandClassification Classification { get; }

    public IReadOnlyList<int> TieBreakValues { get; }

    public string Description
    {
        get
        {
            return Classification switch
            {
                HandClassification.HighCard => "Carta Alta",
                HandClassification.OnePair => "Um Par",
                HandClassification.TwoPair => "Dois Pares",
                HandClassification.ThreeOfAKind => "Trinca",
                HandClassification.Straight => "Sequência",
                HandClassification.Flush => "Flush",
                HandClassification.FullHouse => "Full House",
                HandClassification.FourOfAKind => "Quadra",
                HandClassification.StraightFlush => "Straight Flush",
                HandClassification.RoyalFlush => "Royal Flush",
                _ => throw new ArgumentOutOfRangeException(nameof(Classification))
            };
        }
    }

    public HandEvaluation(HandClassification classification, IEnumerable<int> tieBreakValues)
    {
        Classification = classification;
        TieBreakValues = tieBreakValues.ToList().AsReadOnly();
    }
}
