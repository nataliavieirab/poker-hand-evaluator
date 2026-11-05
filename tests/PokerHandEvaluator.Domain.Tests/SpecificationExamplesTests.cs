using PokerHandEvaluator.Domain.Evaluation;
using PokerHandEvaluator.Domain.Exceptions;
using PokerHandEvaluator.Domain.Hands;

namespace PokerHandEvaluator.Domain.Tests;

public sealed class SpecificationExamplesTests
{
    [Theory]
    [InlineData(
        "5C 5P 6E 7P KO",
        "2P 3E 8E 8O DO",
        Winner.SecondPlayer,
        HandClassification.OnePair,
        HandClassification.OnePair)]
    [InlineData(
        "5O 8P 9E JE AP",
        "2P 5P 7O 8E QC",
        Winner.FirstPlayer,
        HandClassification.HighCard,
        HandClassification.HighCard)]
    [InlineData(
        "2O 9P AE AC AP",
        "3O 6O 7O DO QO",
        Winner.SecondPlayer,
        HandClassification.ThreeOfAKind,
        HandClassification.Flush)]
    [InlineData(
        "4O 6E 9C QC QP",
        "3O 6O 7C QO QE",
        Winner.FirstPlayer,
        HandClassification.OnePair,
        HandClassification.OnePair)]
    [InlineData(
        "2C 2O 4P 4O 4E",
        "3P 3O 3E 9E 9O",
        Winner.FirstPlayer,
        HandClassification.FullHouse,
        HandClassification.FullHouse)]
    // Exemplo corrigido: a segunda mão é uma trinca de 7, sem compartilhar cartas.
    [InlineData(
        "3O 4C 5E 6P 7O",
        "2C 3E 7P 7E 7C",
        Winner.FirstPlayer,
        HandClassification.Straight,
        HandClassification.ThreeOfAKind)]
    [InlineData(
        "9O 9C 9E 9P 2O",
        "2C 2E 7P 7O 7C",
        Winner.FirstPlayer,
        HandClassification.FourOfAKind,
        HandClassification.FullHouse)]
    // Exemplo corrigido: straight flush 4-8 não compartilha o 9E com a quadra.
    [InlineData(
        "4E 5E 6E 7E 8E",
        "9O 9C 9E 9P 2O",
        Winner.FirstPlayer,
        HandClassification.StraightFlush,
        HandClassification.FourOfAKind)]
    [InlineData(
        "DC JC QC KC AC",
        "5E 6E 7E 8E 9E",
        Winner.FirstPlayer,
        HandClassification.RoyalFlush,
        HandClassification.StraightFlush)]
    [InlineData(
        "4O 4C 8E 8P KO",
        "4E 4P 8O 8C QO",
        Winner.FirstPlayer,
        HandClassification.TwoPair,
        HandClassification.TwoPair)]
    [InlineData(
        "2O 5C 8E JP AO",
        "2P 5E 8C JO AC",
        Winner.Tie,
        HandClassification.HighCard,
        HandClassification.HighCard)]
    public void Evaluate_SpecificationExample_ReturnsExpectedResult(
        string firstHand,
        string secondHand,
        Winner expectedWinner,
        HandClassification expectedFirstClassification,
        HandClassification expectedSecondClassification)
    {
        var evaluator = new HandEvaluator();

        var result = evaluator.Evaluate(firstHand, secondHand);

        Assert.Equal(expectedWinner, result.Winner);
        Assert.Equal(
            expectedFirstClassification,
            result.FirstPlayer.Classification);
        Assert.Equal(
            expectedSecondClassification,
            result.SecondPlayer.Classification);
    }

    [Theory]
    [InlineData("5C 5P 6E 7P", "2P 3E 8E 8O DO")]
    [InlineData("5C 5P 6E 7S KO", "2P 3E 8E 8O DO")]
    [InlineData("5C 5C 6E 7P KO", "2P 3E 8E 8O DO")]
    public void Evaluate_InvalidSpecificationExample_ThrowsInvalidHandException(
        string firstHand,
        string secondHand)
    {
        var evaluator = new HandEvaluator();

        Assert.Throws<InvalidHandException>(
            () => evaluator.Evaluate(firstHand, secondHand));
    }
}
