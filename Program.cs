using System;
using System.Collections.Generic;

namespace ClockPatienceGame
{
    // Specifies shortcuts for Ranks and Suits
    public enum Rank
    {
        A, _2, _3, _4, _5, _6, _7, _8, _9, T, J, Q, K
    }

    public enum Suit
    {
        H, D, C, S
    }
// This class represenets a playing card with a rank and suit
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
// Thsi class represents a pile of cards in the game
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
// This class represents the whole clock patience game
public class ClockPatienceGame
{
    private List<ClockPile> piles = new List<ClockPile>();
    private Card? currentCard;
    
    // creates 13 clock piles
    public void CreateClockPiles()
    {
        for (int i = 0; i < 13; i++)
        {
            piles.Add(new ClockPile());
        }
    }

// Deals the cards to piles in a clockwise direction
public void DealCards(List<Card> deck)
{
    int pileIndex = 0; // starts dealing from 1 o'clock position

    for (int i = 0; i < deck.Count; i++)
    {
        piles[pileIndex].AddCard(deck[deck.Count - 1 - i]);

        pileIndex = (pileIndex + 12) % 13;
    }
}

// Plays the clock patience game and returns the number of exposed cards and the last exposed card
public (int exposedCount, Card lastExposedCard) PlayGame()
{
    int exposedCount = 1; // starts with 1 because the center card is already exposed
    Card? currentCard = piles[12].TopCard;

    while (true)
    {
        exposedCount++;
        if (currentCard == null || piles[(int)currentCard.Rank].IsEmpty)
        {
            // No more face down cards in the current pile, exits the loop
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
// This class handles the users input, errors and exceptions
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

            // Plays the game for the given deck
            ClockPatienceGame game = new ClockPatienceGame();
            game.CreateClockPiles();
            game.DealCards(deck);
            (int exposedCount, Card lastExposedCard) = game.PlayGame();
            // Outputs the last played card and the amount of exposed cards
            Console.WriteLine(exposedCount.ToString("D2") + ", " + (lastExposedCard != null ? lastExposedCard.ToString() : "No card exposed"));
        }
    }
}

/*
Sample Input:
TS QC 8S 8D QH 2D 3H KH 9H 2H TH KS KC
9D JH 7H JD 2S QS TD 2C 4H 5H AD 4D 5D
6D 4S 9S 5S 7S JS 8H 3D 8C 3S 4C 6S 9C
AS 7C AH 6H KD JC 7D AC 5C TC QD 6C 3C
#
Sample Output:
44, KD
 */

/*
© Copyright 2023
Erik-Sten Parisvä
Made for Exigy
*/


