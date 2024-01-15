using System;
using UnityEngine;

public abstract class EnemyData : ScriptableObject
{   public Faction faction;
    [SerializeField] private EnemyStats _enemyStats;
    public EnemyStats BaseStats => _enemyStats;
}

[Serializable] public struct EnemyStats
{
    public Sprite enemySp;
    public AudioClip hitSound;
    public string enemyName;
    public float enemyHealth;
    public float enemyDamage;
    public int rewardExperience;
    [Range(0,10)] public float enemySpeed;
    [TextArea(1,5)]public string enemyDescription;
}
[Serializable] public enum Faction
{
    None = 0,
    MagicItem = 1,
    Werewolf = 2
}