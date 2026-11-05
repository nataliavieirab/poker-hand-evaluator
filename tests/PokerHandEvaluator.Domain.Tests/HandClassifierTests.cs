using PokerHandEvaluator.Domain.Hands;

namespace PokerHandEvaluator.Domain.Tests;

public sealed class HandClassifierTests
{
    [Theory]
    [InlineData("2O 4C 6E 8P DO", HandClassification.HighCard)]
    [InlineData("2O 2C 5E 7P KO", HandClassification.OnePair)]
    [InlineData("2O 2C 5E 5P KO", HandClassification.TwoPair)]
    [InlineData("2O 2C 2E 5P KO", HandClassification.ThreeOfAKind)]
    [InlineData("5O 6C 7E 8P 9O", HandClassification.Straight)]
    [InlineData("2O 5O 7O 9O KO", HandClassification.Flush)]
    [InlineData("2O 2C 2E 5P 5O", HandClassification.FullHouse)]
    [InlineData("2O 2C 2E 2P 5O", HandClassification.FourOfAKind)]
    [InlineData("5O 6O 7O 8O 9O", HandClassification.StraightFlush)]
    [InlineData("DO JO QO KO AO", HandClassification.RoyalFlush)]
    public void Classify_Hand_ReturnsExpectedClassification(
        string input,
        HandClassification expected)
    {
        var hand = HandAnalyzer.Parse(input);

        var evaluation = HandClassifier.Classify(hand);

        Assert.Equal(expected, evaluation.Classification);
    }

    [Theory]
    [InlineData("2O 4C 6E 8P DO", "Carta Alta")]
    [InlineData("2O 2C 5E 7P KO", "Um Par")]
    [InlineData("2O 2C 5E 5P KO", "Dois Pares")]
    [InlineData("2O 2C 2E 5P KO", "Trinca")]
    [InlineData("5O 6C 7E 8P 9O", "Sequência")]
    [InlineData("2O 5O 7O 9O KO", "Flush")]
    [InlineData("2O 2C 2E 5P 5O", "Full House")]
    [InlineData("2O 2C 2E 2P 5O", "Quadra")]
    [InlineData("5O 6O 7O 8O 9O", "Straight Flush")]
    [InlineData("DO JO QO KO AO", "Royal Flush")]
    public void Classify_Hand_ReturnsPortugueseDescription(
        string input,
        string expected)
    {
        var hand = HandAnalyzer.Parse(input);

        var evaluation = HandClassifier.Classify(hand);

        Assert.Equal(expected, evaluation.Description);
    }

    [Fact]
    public void Classify_AceLowStraight_ReturnsHighCard()
    {
        var hand = HandAnalyzer.Parse("AO 2C 3E 4P 5O");

        var evaluation = HandClassifier.Classify(hand);

        Assert.Equal(HandClassification.HighCard, evaluation.Classification);
    }

    [Fact]
    public void Classify_AceHighStraight_ReturnsStraight()
    {
        var hand = HandAnalyzer.Parse("DO JC QE KP AO");

        var evaluation = HandClassifier.Classify(hand);

        Assert.Equal(HandClassification.Straight, evaluation.Classification);
    }

    public static IEnumerable<object[]> TieBreakCases()
    {
        yield return new object[] { "2O 4C 6E 8P DO", new[] { 10, 8, 6, 4, 2 } };
        yield return new object[] { "5C 5P 6E 7P KO", new[] { 5, 13, 7, 6 } };
        yield return new object[] { "4O 4C 8E 8P KO", new[] { 8, 4, 13 } };
        yield return new object[] { "2O 2C 2E 5P KO", new[] { 2, 13, 5 } };
        yield return new object[] { "3O 4C 5E 6P 7O", new[] { 7 } };
        yield return new object[] { "2O 5O 7O 9O KO", new[] { 13, 9, 7, 5, 2 } };
        yield return new object[] { "2C 2O 4P 4O 4E", new[] { 4, 2 } };
        yield return new object[] { "9O 9C 9E 9P 2O", new[] { 9, 2 } };
        yield return new object[] { "5E 6E 7E 8E 9E", new[] { 9 } };
        yield return new object[] { "DC JC QC KC AC", Array.Empty<int>() };
    }

    [Theory]
    [MemberData(nameof(TieBreakCases))]
    public void Classify_Hand_ReturnsExpectedTieBreakValues(
        string input,
        int[] expected)
    {
        var hand = HandAnalyzer.Parse(input);

        var evaluation = HandClassifier.Classify(hand);

        Assert.Equal(expected, evaluation.TieBreakValues);
    }
}
