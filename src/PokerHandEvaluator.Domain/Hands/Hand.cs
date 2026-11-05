using PokerHandEvaluator.Domain.Cards;
using PokerHandEvaluator.Domain.Exceptions;

namespace PokerHandEvaluator.Domain.Hands;

public sealed class Hand
{
    public IReadOnlyList<Card> Cards { get; }

    public Hand(IEnumerable<Card> cards)
    {
        if (cards is null)
        {
            throw new InvalidHandException("A mão não pode ser nula.");
        }

        var cardList = cards.ToList();

        if (cardList.Count != 5)
        {
            throw new InvalidHandException(
                $"A mão deve conter exatamente cinco cartas. Quantidade recebida: {cardList.Count}.");
        }

        var duplicateCard = cardList
            .GroupBy(card => card)
            .FirstOrDefault(group => group.Count() > 1);

        if (duplicateCard is not null)
        {
            throw new InvalidHandException($"Carta repetida na mão: {duplicateCard.Key}.");
        }

        Cards = cardList.AsReadOnly();
    }
}
