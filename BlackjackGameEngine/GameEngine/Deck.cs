using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackGameEngine.GameEngine;

public class Deck
{
    private readonly Stack<Card> _cards = new();
    private readonly int _numDecks;
    private readonly Random _rand = new();

    public Deck(int numDecks = 1)
    {
        _numDecks = numDecks;
        Initialize();
    }

    public void Initialize()
    {
        _cards.Clear();
        List<Suit> suits = Enum.GetValues(typeof(Suit)).Cast<Suit>().ToList();
        List<Rank>  ranks = Enum.GetValues(typeof(Rank)).Cast<Rank>().ToList();

        for (int d = 0; d < _numDecks; d++)
        {
            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    _cards.Push(new Card(suit, rank));
                }
            }
        }

        Shuffle();
    }

    public void Shuffle()
    {
        var shuffled = _cards.OrderBy(_ => _rand.Next()).ToList();
        _cards.Clear();
        foreach (var card in shuffled)
        {
            _cards.Push(card);
        }
    }

    public void ShuffleIfNeeded()
    {
        if (_cards.Count < 15) Initialize();
    }
    
    public Card Draw()
    {
        if (_cards.Count == 0)
            throw new InvalidOperationException("The deck is empty.");

        return _cards.Pop();
    }

    public int CardsRemaining => _cards.Count;
}
