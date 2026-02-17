using UnityEngine;

/// <summary>
/// Base enemy class. Moves toward player, performs telegraphed attacks.
/// </summary>
public class EnemyBase : MonoBehaviour
{
    public string EnemyName = "Enemy";
    public int MaxHealth = 200;
    public int CurrentHealth;
    public int ExperienceValue = 1;
    public int SpawnPointCost = 1;
    public float MoveSpeed = 2f;
    public int AttackDamage = 25;
    public float AttackRange = 1.5f;
    public float AttackCooldown = 2f;
    public float AttackWindup = 0.8f;

    public bool IsAlive => CurrentHealth > 0;
    protected PlayerController player;
    protected float attackTimer = 0f;
    protected bool isAttacking = false;
    protected float windupTimer = 0f;

    protected virtual void Start()
    {
        CurrentHealth = MaxHealth;
        player = GameManager.Instance.Player;
    }

    protected virtual void Update()
    {
        if (!IsAlive || player == null || !player.IsAlive) return;
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;
        float dist = Vector3.Distance(transform.position, player.GetPosition());
        if (isAttacking) UpdateWindup();
        else if (dist <= AttackRange) { attackTimer += Time.deltaTime; if (attackTimer >= AttackCooldown) StartAttack(); }
        else MoveToward();
    }

    protected virtual void MoveToward()
    {
        Vector3 dir = (player.GetPosition() - transform.position).normalized;
        transform.position += dir * MoveSpeed * Time.deltaTime;
    }

    protected virtual void StartAttack() { isAttacking = true; windupTimer = 0f; }

    protected virtual void UpdateWindup()
    {
        windupTimer += Time.deltaTime;
        if (windupTimer >= AttackWindup)
        {
            if (Vector3.Distance(transform.position, player.GetPosition()) <= AttackRange * 1.2f)
                player.TakeDamage(AttackDamage);
            isAttacking = false; attackTimer = 0f;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        if (!IsAlive) return;
        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
        if (!IsAlive) Die();
    }

    protected virtual void Die()
    {
        if (GameManager.Instance.ExperienceSystem != null)
            GameManager.Instance.ExperienceSystem.AddExperience(ExperienceValue);
        Destroy(gameObject);
    }
}
