using PokerHandEvaluator.Domain.Cards;
using PokerHandEvaluator.Domain.Exceptions;

namespace PokerHandEvaluator.Domain.Hands;

public static class HandAnalyzer
{
    public static Hand Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new InvalidHandException("A mão não pode ser vazia.");
        }

        var tokens = input.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

        if (tokens.Length != 5)
        {
            throw new InvalidHandException(
                $"A mão deve conter exatamente cinco cartas. Quantidade recebida: {tokens.Length}.");
        }

        var cards = tokens.Select(ParseCard).ToList();

        return new Hand(cards);
    }

    private static Card ParseCard(string token)
    {
        if (token.Length != 2)
        {
            throw new InvalidHandException(
                $"Carta inválida: '{token}'. Cada carta deve ter exatamente dois caracteres.");
        }

        var value = ParseValue(token[0]);
        var suit = ParseSuit(token[1]);

        return new Card(value, suit);
    }

    private static CardValue ParseValue(char symbol)
    {
        return symbol switch
        {
            '2' => CardValue.Two,
            '3' => CardValue.Three,
            '4' => CardValue.Four,
            '5' => CardValue.Five,
            '6' => CardValue.Six,
            '7' => CardValue.Seven,
            '8' => CardValue.Eight,
            '9' => CardValue.Nine,
            'D' => CardValue.Ten,
            'J' => CardValue.Jack,
            'Q' => CardValue.Queen,
            'K' => CardValue.King,
            'A' => CardValue.Ace,
            _ => throw new InvalidHandException($"Valor de carta inválido: '{symbol}'.")
        };
    }

    private static Suit ParseSuit(char symbol)
    {
        return symbol switch
        {
            'O' => Suit.Diamonds,
            'C' => Suit.Hearts,
            'E' => Suit.Spades,
            'P' => Suit.Clubs,
            _ => throw new InvalidHandException($"Naipe inválido: '{symbol}'.")
        };
    }
}
