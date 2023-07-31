using System;
using System.Collections.Generic;

namespace ClockPatienceGame
{
    public enum Rank
    {
        A, _2, _3, _4, _5, _6, _7, _8, _9, T, J, Q, K
    }

    public enum Suit
    {
        H, D, C, S
    }

public class Card
{
    public Rank Rank { get; }
    public Suit Suit { get; }

    public Card(Rank rank, Suit suit)
    {
        Rank = rank;
        Suit = suit;
    }

    public override string ToString()
    {
        return $"{Rank}{Suit}";
    }
}
public class ClockPile
{
    private List<Card> cards = new List<Card>();

    public bool IsEmpty => cards.Count == 0;
    public Card? TopCard => IsEmpty ? null : cards[cards.Count - 1];

    public void AddCard(Card card)
    {
        cards.Add(card);
    }

    public void RemoveTopCard()
    {
        if (!IsEmpty)
            cards.RemoveAt(cards.Count - 1);
    }
}
public class ClockPatienceGame
{
    private List<ClockPile> piles = new List<ClockPile>();
    private Card? currentCard;

    public void CreateClockPiles()
    {
        for (int i = 0; i < 13; i++)
        {
            piles.Add(new ClockPile());
        }
    }

public void DealCards(List<Card> deck)
{
    int pileIndex = 0;

    for (int i = 0; i < deck.Count; i++)
    {
        piles[pileIndex].AddCard(deck[deck.Count - 1 - i]);

        pileIndex = (pileIndex + 12) % 13;
    }
}


public (int exposedCount, Card lastExposedCard) PlayGame()
{
    int exposedCount = 1;
    Card? currentCard = piles[12].TopCard;

    while (true)
    {
        exposedCount++;
        if (currentCard == null || piles[(int)currentCard.Rank].IsEmpty)
        {
            break;
        }

        int pileIndex = (int)currentCard.Rank;
        currentCard = piles[pileIndex].TopCard;
        piles[pileIndex].RemoveTopCard();
    }
    exposedCount++;
    return (exposedCount, currentCard);
}

}

    class Program
    {
        static void Main(string[] args)
        {
            List<Card> deck = new List<Card>();

            Console.WriteLine("Enter the deck of cards, 13 cards in each line, and '#' on a new line to finish:");
            string input = "";

            while (true)
            {
                string line = Console.ReadLine();
                if (line == "#")
                    break;

                input += line + " ";
            }

            string[] cards = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (cards.Length != 52)
            {
                Console.WriteLine("Invalid input. Please enter exactly 52 cards in the format 'RankSuit' (e.g., 'KS' for King of Spades).");
                return;
            }

            foreach (string card in cards)
            {
                if (card.Length != 2)
                {
                    Console.WriteLine("Invalid card format. Please use the format 'RankSuit' (e.g., 'KS' for King of Spades).");
                    return;
                }

                try
                {
                    Rank rank = (Rank)Enum.Parse(typeof(Rank), card.Substring(0, 1), ignoreCase: true);
                    Suit suit = (Suit)Enum.Parse(typeof(Suit), card.Substring(1, 1));
                    deck.Add(new Card(rank, suit));
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Invalid card. Please enter a valid card in the format 'RankSuit' (e.g., 'KS' for King of Spades).");
                    return;
                }
            }

            ClockPatienceGame game = new ClockPatienceGame();
            game.CreateClockPiles();
            game.DealCards(deck);
            (int exposedCount, Card lastExposedCard) = game.PlayGame();

            Console.WriteLine(exposedCount.ToString("D2") + ", " + (lastExposedCard != null ? lastExposedCard.ToString() : "No card exposed"));
        }
    }
}

/*
© Copyright 2023
Erik-Sten Parisvä
Made for Exigy
*/
