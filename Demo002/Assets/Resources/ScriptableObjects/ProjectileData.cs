using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New H-p", menuName = "ScriptableObject/Projectile", order = 3)]
public class ProjectileData : ScriptableObject
{
    [SerializeField] private ProjectileStats _projectileStats;
    public ProjectileStats BaseStats => _projectileStats;
    [Space(20)]
    public Projectile Prefab;
    public float spawnDelayTime;
}
[Serializable] public struct ProjectileStats
{
    public float speed;
    public float damage;
    public float knockBackForce;
    public float stayTime;
    public bool penetrate;
}
