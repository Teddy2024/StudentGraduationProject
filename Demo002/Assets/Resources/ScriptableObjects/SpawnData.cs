using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewSpawnData", menuName = "ScriptableObject/Spawn", order = 4)]
public class SpawnData : ScriptableObject
{

    [Header("調用時間點(分鐘)"), Tooltip("在經過幾分鐘後，調用生成修改。")]public int ApplyTime;
    [SerializeReference] public List<EditSpawnEnemyEffect> EditSpawnEnemyEffects;
    [ContextMenu(nameof(AddSpawnEnemy))] void AddSpawnEnemy(){ EditSpawnEnemyEffects.Add(new SpawnEnemy());}
    [ContextMenu(nameof(AddSpawnBoss))] void AddSpawnBoss(){ EditSpawnEnemyEffects.Add(new SpawnBoss());}
    [ContextMenu(nameof(DeleteEnemy))] void DeleteSpawnEnemy(){ EditSpawnEnemyEffects.Add(new DeleteEnemy());}
    [ContextMenu(nameof(DeleteAllEnemy))] void DeleteAllEnemy(){ EditSpawnEnemyEffects.Add(new DeleteAllEnemy());}
}

[Serializable] public class EditSpawnEnemyEffect{}

[Serializable] public class SpawnEnemy : EditSpawnEnemyEffect
{
    public EnemyDataBase enemyDataBase;
    public Vector2 spawnArea;//(15, 7)
    [Range(0,20)] public float spawnTime = 0.5f;
}
[Serializable] public class SpawnBoss : EditSpawnEnemyEffect
{
    public EnemyDataBase enemyDataBase;
    public Vector2 spawnArea;//(15, 7)
}
[Serializable] public class DeleteEnemy : EditSpawnEnemyEffect
{
    public EnemyDataBase enemyDataBase;
}
[Serializable] public class DeleteAllEnemy : EditSpawnEnemyEffect{}
