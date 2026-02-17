using UnityEngine;

/// <summary>
/// Ogre: Large rare brute, 500 HP, 5 XP, 5 spawn points.
/// </summary>
public class Ogre : EnemyBase
{
    protected override void Start()
    {
        EnemyName = "Ogre";
        MaxHealth = 500; ExperienceValue = 5; SpawnPointCost = 5;
        MoveSpeed = 1.5f; AttackDamage = 50; AttackRange = 2f;
        AttackCooldown = 3f; AttackWindup = 1.2f;
        base.Start();
    }
}
