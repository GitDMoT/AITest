using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Cards/Card Data")]
public class CardData : ScriptableObject
{
    public string CardName;
    [TextArea(2, 4)] public string Description;
    public Sprite CardArt;
    public int EnergyCost = 1;
    public CardType Type;
    public float DamageAmount = 100f;
    public float ShieldAmount = 50f;
    public float AttackArc = 180f;
    public float AttackRange = 2f;
    public float DashDistance = 5f;
    public float DamageMultiplier = 1f;

    public enum CardType { Slash, Block, Whirlwind, Dash }
}
