using PokerHandEvaluator.Domain.Cards;
using PokerHandEvaluator.Domain.Exceptions;
using PokerHandEvaluator.Domain.Hands;

namespace PokerHandEvaluator.Domain.Tests;

public sealed class HandAnalyzerTests
{
    [Fact]
    public void Parse_ValidHand_ReturnsFiveCards()
    {
        var hand = HandAnalyzer.Parse("5C 5P 6E 7P KO");

        Assert.Equal(5, hand.Cards.Count);
        Assert.Equal(CardValue.Five, hand.Cards[0].Value);
        Assert.Equal(Suit.Hearts, hand.Cards[0].Suit);
        Assert.Equal(CardValue.King, hand.Cards[4].Value);
        Assert.Equal(Suit.Diamonds, hand.Cards[4].Suit);
    }

    [Fact]
    public void Parse_HandWithExtraWhitespace_ReturnsHand()
    {
        var hand = HandAnalyzer.Parse("  5C  5P\t6E\n7P KO  ");

        Assert.Equal(5, hand.Cards.Count);
    }

    [Theory]
    [InlineData("5C 5P 6E 7P")]
    [InlineData("5C 5P 6E 7P KO AO")]
    public void Parse_HandWithWrongCardCount_ThrowsInvalidHandException(string input)
    {
        Assert.Throws<InvalidHandException>(() => HandAnalyzer.Parse(input));
    }

    [Fact]
    public void Parse_HandWithDuplicateCard_ThrowsInvalidHandException()
    {
        Assert.Throws<InvalidHandException>(() => HandAnalyzer.Parse("5C 5C 6E 7P KO"));
    }

    [Theory]
    [InlineData("1C 5P 6E 7P KO")]
    [InlineData("0C 5P 6E 7P KO")]
    [InlineData("TC 5P 6E 7P KO")]
    public void Parse_HandWithInvalidValue_ThrowsInvalidHandException(string input)
    {
        Assert.Throws<InvalidHandException>(() => HandAnalyzer.Parse(input));
    }

    [Theory]
    [InlineData("5C 5P 6E 7S KO")]
    [InlineData("5C 5P 6E 7X KO")]
    public void Parse_HandWithInvalidSuit_ThrowsInvalidHandException(string input)
    {
        Assert.Throws<InvalidHandException>(() => HandAnalyzer.Parse(input));
    }

    [Theory]
    [InlineData("10O 5P 6E 7P KO")]
    [InlineData("5 5P 6E 7P KO")]
    public void Parse_HandWithInvalidCardFormat_ThrowsInvalidHandException(string input)
    {
        Assert.Throws<InvalidHandException>(() => HandAnalyzer.Parse(input));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Parse_NullOrEmptyHand_ThrowsInvalidHandException(string? input)
    {
        Assert.Throws<InvalidHandException>(() => HandAnalyzer.Parse(input));
    }

    [Fact]
    public void Parse_LowercaseCard_ThrowsInvalidHandException()
    {
        Assert.Throws<InvalidHandException>(() => HandAnalyzer.Parse("5c 5P 6E 7P KO"));
    }
}
