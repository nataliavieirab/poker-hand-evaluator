using PokerHandEvaluator.Domain.Cards;

namespace PokerHandEvaluator.Domain.Hands;

public static class HandClassifier
{
    public static HandEvaluation Classify(Hand hand)
    {
        if (hand is null)
        {
            throw new ArgumentNullException(nameof(hand));
        }

        var sortedValues = hand.Cards
            .Select(card => (int)card.Value)
            .OrderByDescending(value => value)
            .ToArray();

        var groups = hand.Cards
            .GroupBy(card => card.Value)
            .OrderByDescending(group => group.Count())
            .ThenByDescending(group => (int)group.Key)
            .ToList();

        var isFlush = hand.Cards.Select(card => card.Suit).Distinct().Count() == 1;
        var isStraight = sortedValues.Distinct().Count() == 5
            && sortedValues[0] - sortedValues[4] == 4;
        var isRoyalFlush = isFlush
            && isStraight
            && sortedValues[0] == (int)CardValue.Ace;

        if (isRoyalFlush)
        {
            return new HandEvaluation(HandClassification.RoyalFlush, Array.Empty<int>());
        }

        if (isFlush && isStraight)
        {
            return new HandEvaluation(HandClassification.StraightFlush, new[] { sortedValues[0] });
        }

        if (groups[0].Count() == 4)
        {
            var kicker = groups.Single(group => group.Count() == 1);
            return new HandEvaluation(
                HandClassification.FourOfAKind,
                new[] { (int)groups[0].Key, (int)kicker.Key });
        }

        if (groups[0].Count() == 3 && groups.Count == 2 && groups[1].Count() == 2)
        {
            return new HandEvaluation(
                HandClassification.FullHouse,
                new[] { (int)groups[0].Key, (int)groups[1].Key });
        }

        if (isFlush)
        {
            return new HandEvaluation(HandClassification.Flush, sortedValues);
        }

        if (isStraight)
        {
            return new HandEvaluation(HandClassification.Straight, new[] { sortedValues[0] });
        }

        if (groups[0].Count() == 3)
        {
            var kickers = groups
                .Where(group => group.Count() == 1)
                .Select(group => (int)group.Key)
                .OrderByDescending(value => value);

            return new HandEvaluation(
                HandClassification.ThreeOfAKind,
                new[] { (int)groups[0].Key }.Concat(kickers));
        }

        if (groups.Count(group => group.Count() == 2) == 2)
        {
            var pairs = groups
                .Where(group => group.Count() == 2)
                .Select(group => (int)group.Key)
                .OrderByDescending(value => value)
                .ToArray();

            var kicker = groups.Single(group => group.Count() == 1);

            return new HandEvaluation(
                HandClassification.TwoPair,
                new[] { pairs[0], pairs[1], (int)kicker.Key });
        }

        if (groups[0].Count() == 2)
        {
            var kickers = groups
                .Where(group => group.Count() == 1)
                .Select(group => (int)group.Key)
                .OrderByDescending(value => value);

            return new HandEvaluation(
                HandClassification.OnePair,
                new[] { (int)groups[0].Key }.Concat(kickers));
        }

        return new HandEvaluation(HandClassification.HighCard, sortedValues);
    }
}
