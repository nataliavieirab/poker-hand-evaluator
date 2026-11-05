using PokerHandEvaluator.Domain.Evaluation;
using PokerHandEvaluator.Domain.Exceptions;

namespace PokerHandEvaluator.Domain.Tests;

public sealed class HandEvaluatorTests
{
    [Theory]
    [InlineData("2O 9P AE AC AP", "3O 6O 7O DO QO", Winner.SecondPlayer)]
    [InlineData("9O 9C 9E 9P 2O", "2C 2E 7P 7O 7C", Winner.FirstPlayer)]
    [InlineData("4E 5E 6E 7E 8E", "9O 9C 9E 9P 2O", Winner.FirstPlayer)]
    [InlineData("DC JC QC KC AC", "5E 6E 7E 8E 9E", Winner.FirstPlayer)]
    [InlineData("3O 4C 5E 6P 7O", "2C 3E 7P 7E 7C", Winner.FirstPlayer)]
    public void Evaluate_DifferentClassifications_ReturnsHighestClassificationWinner(
        string firstHand,
        string secondHand,
        Winner expected)
    {
        var evaluator = new HandEvaluator();

        var result = evaluator.Evaluate(firstHand, secondHand);

        Assert.Equal(expected, result.Winner);
    }

    [Theory]
    [InlineData("5C 5P 6E 7P KO", "2P 3E 8E 8O DO", Winner.SecondPlayer)]
    [InlineData("5O 8P 9E JE AP", "2P 5P 7O 8E QC", Winner.FirstPlayer)]
    [InlineData("4O 6E 9C QC QP", "3O 6O 7C QO QE", Winner.FirstPlayer)]
    [InlineData("2C 2O 4P 4O 4E", "3P 3O 3E 9E 9O", Winner.FirstPlayer)]
    [InlineData("4O 4C 8E 8P KO", "4E 4P 8O 8C QO", Winner.FirstPlayer)]
    [InlineData("8O 8C 4E 4P KO", "7O 7C 5E 5P AO", Winner.FirstPlayer)]
    [InlineData("8O 8C 4E 4P KO", "8E 8P 3O 3C AO", Winner.FirstPlayer)]
    [InlineData("9O 8C 7E 6P 5O", "8P 7C 6E 5P 4O", Winner.FirstPlayer)]
    [InlineData("AC KC QC JC 9C", "AO KO QO JO 8O", Winner.FirstPlayer)]
    [InlineData("9C 8C 7C 6C 5C", "8O 7O 6O 5O 4O", Winner.FirstPlayer)]
    [InlineData("9O 9C 9E 5P KO", "8P 8E 8C 5O QO", Winner.FirstPlayer)]
    [InlineData("9O 9C 9E 9P 2O", "8O 8C 8E 8P AO", Winner.FirstPlayer)]
    public void Evaluate_SameClassification_AppliesTieBreakers(
        string firstHand,
        string secondHand,
        Winner expected)
    {
        var evaluator = new HandEvaluator();

        var result = evaluator.Evaluate(firstHand, secondHand);

        Assert.Equal(expected, result.Winner);
    }

    [Theory]
    [InlineData("2O 5C 8E JP AO", "2P 5E 8C JO AC")]
    [InlineData("5O 6C 7E 8P 9O", "5P 6E 7C 8O 9C")]
    [InlineData("AC KC QC JC 9C", "AO KO QO JO 9O")]
    [InlineData("5O 6O 7O 8O 9O", "5C 6C 7C 8C 9C")]
    [InlineData("DC JC QC KC AC", "DO JO QO KO AO")]
    [InlineData("4O 4C 8E 8P KO", "4E 4P 8O 8C KC")]
    [InlineData("5C 5P 6E 7P KO", "5O 5E 6C 7C KC")]
    public void Evaluate_TiedHands_ReturnsTie(string firstHand, string secondHand)
    {
        var evaluator = new HandEvaluator();

        var result = evaluator.Evaluate(firstHand, secondHand);

        Assert.Equal(Winner.Tie, result.Winner);
    }

    [Fact]
    public void Evaluate_ReturnsWinnerAndHandDescriptionsInPortuguese()
    {
        var evaluator = new HandEvaluator();

        var result = evaluator.Evaluate("2O 9P AE AC AP", "3O 6O 7O DO QO");

        Assert.Equal(Winner.SecondPlayer, result.Winner);
        Assert.Equal("Jogador 2", result.WinnerDescription);
        Assert.Equal("Trinca", result.FirstPlayer.Description);
        Assert.Equal("Flush", result.SecondPlayer.Description);
    }

    [Fact]
    public void Evaluate_Tie_ReturnsTieDescription()
    {
        var evaluator = new HandEvaluator();

        var result = evaluator.Evaluate("2O 5C 8E JP AO", "2P 5E 8C JO AC");

        Assert.Equal("Empate", result.WinnerDescription);
    }

    [Fact]
    public void Evaluate_SharedCard_ThrowsInvalidHandException()
    {
        var evaluator = new HandEvaluator();

        Assert.Throws<InvalidHandException>(
            () => evaluator.Evaluate("5C 5P 6E 7P KO", "5C 2P 3E 8E DO"));
    }

    [Fact]
    public void Evaluate_SameValueInDifferentSuits_IsAllowed()
    {
        var evaluator = new HandEvaluator();

        var result = evaluator.Evaluate("5C 5P 6E 7P KO", "5O 2P 3E 8E DO");

        Assert.Equal(Winner.FirstPlayer, result.Winner);
    }

    [Theory]
    [InlineData("5C 5P 6E 7P", "2P 3E 8E 8O DO")]
    [InlineData("5C 5C 6E 7P KO", "2P 3E 8E 8O DO")]
    [InlineData("5C 5P 6E 7S KO", "2P 3E 8E 8O DO")]
    public void Evaluate_InvalidHand_ThrowsInvalidHandException(
        string firstHand,
        string secondHand)
    {
        var evaluator = new HandEvaluator();

        Assert.Throws<InvalidHandException>(
            () => evaluator.Evaluate(firstHand, secondHand));
    }
}
