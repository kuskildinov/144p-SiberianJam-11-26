using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGame : MiniGame
{
    [Header("Cards")]
    [SerializeField] private MemoryCardView _cardPrefab;
    [SerializeField] private Transform _cardsContainer;
    [SerializeField] private Sprite[] _images;

    [Header("UI")]
    [SerializeField] private Text _pairsText;
    [SerializeField] private Text _attemptsText;

    [Header("Settings")]
    [SerializeField, Range(2, 8)] private int _pairsCount = 4;
    [SerializeField] private float _hideDelay = 0.8f;

    private readonly List<MemoryCardView> _cards = new();

    private MiniGamesService _gamesService;

    private MemoryCardView _firstCard;
    private MemoryCardView _secondCard;

    private int _matchedPairs;
    private int _attempts;
    private bool _isChecking;
    private bool _isCompleted;

    private Coroutine _checkCoroutine;

    public override void Initialize(MiniGamesService gamesService)
    {
        _gamesService = gamesService;
        _isCompleted = false;
    }

    public override void StartGame()
    {
        if (_checkCoroutine != null)
        {
            StopCoroutine(_checkCoroutine);
            _checkCoroutine = null;
        }

        foreach (var card in _cards)
        {
            if (card != null)
                Destroy(card.gameObject);
        }

        _cards.Clear();

        _firstCard = null;
        _secondCard = null;

        _matchedPairs = 0;
        _attempts = 0;
        _isChecking = false;
        _isCompleted = false;

        int count = Mathf.Min(_pairsCount, _images.Length);

        List<int> deck = new();

        for (int i = 0; i < count; i++)
        {
            deck.Add(i);
            deck.Add(i);
        }

        for (int i = deck.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            (deck[i], deck[randomIndex]) = (deck[randomIndex], deck[i]);
        }

        foreach (int symbolId in deck)
        {
            MemoryCardView card = Instantiate(_cardPrefab,_cardsContainer);

            card.Initialize(this,symbolId,_images[symbolId]);
            _cards.Add(card);
        }

        UpdateUI();
    }

    public override void PauseGame()
    {
        if (_checkCoroutine != null)
        {
            StopCoroutine(_checkCoroutine);
            _checkCoroutine = null;
        }

        _firstCard?.CompleteFlip();
        _secondCard?.CompleteFlip();

        if (_secondCard != null)
        {
            ResolveCards();
        }
    }

    private void ResolveCards()
    {
        if (_firstCard == null || _secondCard == null)
            return;

        if (_firstCard.SymbolId == _secondCard.SymbolId)
        {
            _firstCard.SetMatched();
            _secondCard.SetMatched();

            _matchedPairs++;
        }
        else
        {
            _firstCard.Hide();
            _secondCard.Hide();
        }

        _firstCard = null;
        _secondCard = null;

        _isChecking = false;
        _checkCoroutine = null;

        UpdateUI();

        if (_matchedPairs >= _cards.Count / 2)
            CompleteGame();
    }

    protected override void CompleteGame()
    {        
        base.CompleteGame();
    }

    public void OnCardClicked(MemoryCardView card)
    {
        if (_isChecking || _isCompleted)
            return;

        if (card.IsMatched)
            return;

        if (card == _firstCard)
            return;

        if (card.IsRevealed)
            return;

        card.Reveal();

        if (_firstCard == null)
        {
            _firstCard = card;
            return;
        }

        _secondCard = card;
        _attempts++;
        _isChecking = true;

        UpdateUI();

        _checkCoroutine = StartCoroutine(CheckCards());
    }

    private void UpdateUI()
    {
        _pairsText.text = $"ПАРЫ: {_matchedPairs}/{_cards.Count / 2}";
        _attemptsText.text = $"ПОПЫТКИ: {_attempts}";
    }

    private IEnumerator CheckCards()
    {
        yield return new WaitForSeconds(_hideDelay);

        ResolveCards();
    }

    private void OnDisable()
    {
        PauseGame();
    }
}
