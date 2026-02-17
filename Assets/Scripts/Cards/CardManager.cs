using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages deck, hand, and card draw. Auto-draws every 5s. P/O/I/U play cards.
/// </summary>
public class CardManager : MonoBehaviour
{
    public List<CardData> StartingDeck = new List<CardData>();
    public float DrawInterval = 5f;
    public int MaxHandSize = 10;
    public EnergySystem EnergySystem;
    public PlayerController Player;
    public CardEffectExecutor EffectExecutor;

    public List<CardData> Hand { get; private set; } = new List<CardData>();
    private List<CardData> drawPile = new List<CardData>();
    private List<CardData> discardPile = new List<CardData>();
    private float drawTimer = 0f;
    private readonly KeyCode[] cardKeys = { KeyCode.P, KeyCode.O, KeyCode.I, KeyCode.U };

    private void Start() { InitializeDeck(); }

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;
        drawTimer += Time.deltaTime;
        if (drawTimer >= DrawInterval) { drawTimer = 0f; DrawCard(); }
        for (int i = 0; i < cardKeys.Length; i++)
            if (Input.GetKeyDown(cardKeys[i])) TryPlayCard(i);
    }

    private void InitializeDeck()
    {
        drawPile.Clear(); discardPile.Clear(); Hand.Clear();
        drawPile.AddRange(StartingDeck);
        ShuffleDeck();
        for (int i = 0; i < 4; i++) DrawCard();
    }

    public void DrawCard()
    {
        if (Hand.Count >= MaxHandSize) return;
        if (drawPile.Count == 0)
        {
            if (discardPile.Count == 0) return;
            drawPile.AddRange(discardPile); discardPile.Clear(); ShuffleDeck();
        }
        Hand.Add(drawPile[0]); drawPile.RemoveAt(0);
    }

    private void TryPlayCard(int indexFromRight)
    {
        int handIndex = Hand.Count - 1 - indexFromRight;
        if (handIndex < 0 || handIndex >= Hand.Count) return;
        CardData card = Hand[handIndex];
        if (!EnergySystem.TrySpendEnergy(card.EnergyCost)) return;
        if (EffectExecutor != null) EffectExecutor.ExecuteCard(card, Player);
        Hand.RemoveAt(handIndex); discardPile.Add(card);
    }

    private void ShuffleDeck()
    {
        for (int i = drawPile.Count - 1; i > 0; i--)
        { int j = Random.Range(0, i + 1); (drawPile[i], drawPile[j]) = (drawPile[j], drawPile[i]); }
    }

    public float GetDrawTimerProgress() { return Mathf.Clamp01(drawTimer / DrawInterval); }
    public void AddCardToDeck(CardData card) { discardPile.Add(card); }
}
