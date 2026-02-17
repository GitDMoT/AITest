using UnityEngine;

/// <summary>
/// Executes card effects: Slash, Block, Whirlwind, Dash.
/// </summary>
public class CardEffectExecutor : MonoBehaviour
{
    public LayerMask EnemyLayer;

    public void ExecuteCard(CardData card, PlayerController player)
    {
        switch (card.Type)
        {
            case CardData.CardType.Slash: ExecuteSlash(card, player); break;
            case CardData.CardType.Block: player.AddShield(card.ShieldAmount); break;
            case CardData.CardType.Whirlwind: ExecuteWhirlwind(card, player); break;
            case CardData.CardType.Dash: ExecuteDash(card, player); break;
        }
    }

    private void ExecuteSlash(CardData card, PlayerController player)
    {
        DamageInArc(player.GetPosition(), player.FacingDirection, card.AttackArc, card.AttackRange, card.DamageAmount * card.DamageMultiplier);
    }

    private void ExecuteWhirlwind(CardData card, PlayerController player)
    {
        DamageInArc(player.GetPosition(), player.FacingDirection, 360f, card.AttackRange, card.DamageAmount * card.DamageMultiplier);
    }

    private void ExecuteDash(CardData card, PlayerController player)
    {
        float dmg = card.DamageAmount * card.DamageMultiplier;
        Vector3 start = player.GetPosition();
        Vector3 end = start + player.FacingDirection * card.DashDistance;
        Collider2D[] hits = Physics2D.OverlapAreaAll(
            new Vector2(Mathf.Min(start.x, end.x) - 1f, Mathf.Min(start.y, end.y) - 1f),
            new Vector2(Mathf.Max(start.x, end.x) + 1f, Mathf.Max(start.y, end.y) + 1f), EnemyLayer);
        foreach (var h in hits) { var e = h.GetComponent<EnemyBase>(); if (e != null) e.TakeDamage(Mathf.RoundToInt(dmg)); }
        player.transform.position = end;
    }

    private void DamageInArc(Vector3 origin, Vector3 dir, float arc, float range, float dmg)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, range, EnemyLayer);
        float halfArc = arc / 2f;
        foreach (var h in hits)
        {
            if (Vector3.Angle(dir, (h.transform.position - origin).normalized) <= halfArc)
            { var e = h.GetComponent<EnemyBase>(); if (e != null) e.TakeDamage(Mathf.RoundToInt(dmg)); }
        }
    }
}
