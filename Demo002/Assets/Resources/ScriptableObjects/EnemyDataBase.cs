using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "ScriptableObject/Enemy", order = 1)]
public class EnemyDataBase : EnemyData
{
    public GameObject prefab;
    public EnemyType enemyType;
}
[Serializable] public enum EnemyType
{
    normalEnemy = 0,
    miniBoss = 1,
    LevelBoss = 2,
    FinalBoss = 3
};
