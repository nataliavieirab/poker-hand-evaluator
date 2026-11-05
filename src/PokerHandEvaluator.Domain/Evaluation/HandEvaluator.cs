using PokerHandEvaluator.Domain.Exceptions;
using PokerHandEvaluator.Domain.Hands;

namespace PokerHandEvaluator.Domain.Evaluation;

public sealed class HandEvaluator
{
    public EvaluationResult Evaluate(string? firstHand, string? secondHand)
    {
        var first = HandAnalyzer.Parse(firstHand);
        var second = HandAnalyzer.Parse(secondHand);

        var sharedCard = first.Cards.Intersect(second.Cards).FirstOrDefault();

        if (sharedCard is not null)
        {
            throw new InvalidHandException(
                $"A carta '{sharedCard}' está presente nas duas mãos.");
        }

        var firstEvaluation = HandClassifier.Classify(first);
        var secondEvaluation = HandClassifier.Classify(second);

        var winner = Compare(firstEvaluation, secondEvaluation);

        return new EvaluationResult(winner, firstEvaluation, secondEvaluation);
    }

    private static Winner Compare(HandEvaluation first, HandEvaluation second)
    {
        if (first.Classification != second.Classification)
        {
            return first.Classification > second.Classification
                ? Winner.FirstPlayer
                : Winner.SecondPlayer;
        }

        var valueCount = Math.Min(
            first.TieBreakValues.Count,
            second.TieBreakValues.Count);

        for (var index = 0; index < valueCount; index++)
        {
            if (first.TieBreakValues[index] != second.TieBreakValues[index])
            {
                return first.TieBreakValues[index] > second.TieBreakValues[index]
                    ? Winner.FirstPlayer
                    : Winner.SecondPlayer;
            }
        }

        return Winner.Tie;
    }
}
