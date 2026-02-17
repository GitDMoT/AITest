using UnityEngine;

/// <summary>
/// Blade Zombie: Weak humanoid, 200 HP, 1 XP, 1 spawn point.
/// </summary>
public class BladeZombie : EnemyBase
{
    protected override void Start()
    {
        EnemyName = "Blade Zombie";
        MaxHealth = 200; ExperienceValue = 1; SpawnPointCost = 1;
        MoveSpeed = 2.5f; AttackDamage = 20; AttackRange = 1.5f;
        AttackCooldown = 2f; AttackWindup = 0.8f;
        base.Start();
    }
}
