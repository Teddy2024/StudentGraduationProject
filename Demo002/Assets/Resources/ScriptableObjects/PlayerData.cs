using System;
using UnityEngine;

public abstract class PlayerData : ScriptableObject
{
    [SerializeField] private PlayerStats _playerStats;
    public PlayerStats BaseStats => _playerStats;
}
[Serializable] public struct PlayerStats
{
    public Sprite playerSp;
    public string playerName;
    public float maxHealth;
    [Range(0,50)] public float speed;
    [TextArea(1,3)] public string playerDescription;
}
