using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "ScriptableObject/Level", order = 5)]
public class LevelData : ScriptableObject
{
    [SerializeField] private LevelStats _levelStats;
    public LevelStats BaseStats => _levelStats;
}
[Serializable] public struct LevelStats
{
    public string sceneName;
    public Sprite levelSp;
}
