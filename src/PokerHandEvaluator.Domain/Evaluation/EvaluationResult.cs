using PokerHandEvaluator.Domain.Hands;

namespace PokerHandEvaluator.Domain.Evaluation;

public sealed class EvaluationResult
{
    public Winner Winner { get; }

    public HandEvaluation FirstPlayer { get; }

    public HandEvaluation SecondPlayer { get; }

    public string WinnerDescription
    {
        get
        {
            return Winner switch
            {
                Winner.FirstPlayer => "Jogador 1",
                Winner.SecondPlayer => "Jogador 2",
                Winner.Tie => "Empate",
                _ => throw new ArgumentOutOfRangeException(nameof(Winner))
            };
        }
    }

    public EvaluationResult(
        Winner winner,
        HandEvaluation firstPlayer,
        HandEvaluation secondPlayer)
    {
        Winner = winner;
        FirstPlayer = firstPlayer;
        SecondPlayer = secondPlayer;
    }
}
