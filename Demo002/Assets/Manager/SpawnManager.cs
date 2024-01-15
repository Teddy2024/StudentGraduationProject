using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnManager : Singleton<SpawnManager>
{
    public SpawnData ToEditSpawnData{ private get ; set ; }
    List<SpawnEnemy> CurrentSpawnEnemyList = new List<SpawnEnemy>();
    List<Coroutine> _activeEffectCoroutines = new List<Coroutine>();
    
    [Header("生成區域")]
    [SerializeField] private Vector2 sampleArea;
    [SerializeField] private ProgressBar _progressBar;

    Transform _player; 

    void Start()
    {
        _player = GameManager.Instance.GetPlayer().transform;
        if(ToEditSpawnData != null)EditAllSpawnData();
    } 

    public void AddSpawnData(SpawnData TimeToAddSpawnData)
    {
        ToEditSpawnData = TimeToAddSpawnData;
        EditAllSpawnData();
    }

    #region 修改生成資料
    void EditAllSpawnData()
    {
        if(!ToEditSpawnData.EditSpawnEnemyEffects.Any())return;
        StopActiveEffectCoroutines();
        foreach (var spawnEffect in ToEditSpawnData.EditSpawnEnemyEffects)
        {
            if (spawnEffect is SpawnEnemy spawnEnemyEffect)
            {
                CurrentSpawnEnemyList.RemoveAll(item => item.enemyDataBase == spawnEnemyEffect.enemyDataBase);
                CurrentSpawnEnemyList.Add(spawnEnemyEffect);
            }
            else if(spawnEffect is SpawnBoss spawnBossEffect)
            {
                SpawnEnemy(spawnBossEffect.enemyDataBase, spawnBossEffect.spawnArea);
                _progressBar?.AwakePoint(ToEditSpawnData.ApplyTime);
            }
            else if(spawnEffect is DeleteEnemy deleteEnemyEffect)
            {
                CurrentSpawnEnemyList.RemoveAll(item => item.enemyDataBase == deleteEnemyEffect.enemyDataBase);
            }
            else if(spawnEffect is DeleteAllEnemy deleteAllEnemyEffect)
            {
                CurrentSpawnEnemyList.Clear();
            }
        }
        ToEditSpawnData = null;
        ApplyAllSpawnData();
    }
    #endregion
    #region 應用生成資料
    void ApplyAllSpawnData()
    {
        foreach (var spawnEffect in CurrentSpawnEnemyList)
        {
            Coroutine enemyCoroutine = StartCoroutine(SpawnEnemyWithDelay(spawnEffect.enemyDataBase, 
            spawnEffect.spawnArea, spawnEffect.spawnTime));
            _activeEffectCoroutines.Add(enemyCoroutine);
        }
    }
    IEnumerator SpawnEnemyWithDelay(EnemyDataBase enemyDataBase, Vector2 spawnArea, float spawnTime)
    {
        while (_player != null)
        {
            if (GameManager.Instance.GameIsPaused)// 如果游戏暂停，则等待直到游戏恢复
            {
                yield return new WaitUntil(() => !GameManager.Instance.GameIsPaused);
                continue;
            }
            SpawnEnemy(enemyDataBase, spawnArea);
            yield return new WaitForSeconds(spawnTime);
        }
    }
    #endregion
    #region 敵人生成邏輯
    void SpawnEnemy(EnemyDataBase enemyDataBase, Vector2 spawnArea)
    {
        Vector3 position = GeneratePosition() + _player.transform.position;
        bool isNormalEnemy = (enemyDataBase.enemyType == EnemyType.normalEnemy);
        int radCount =  isNormalEnemy ? Random.Range(0,3) : 1;

        for (int i = 0; i < radCount; i++)
        {
            GameObject newEnemy = Instantiate(enemyDataBase.prefab, position, Quaternion.identity);
            EnemyDefault enemyClass = newEnemy.GetComponent<EnemyDefault>();
            enemyClass.EnemyDataSet(enemyDataBase);

            if(isNormalEnemy)
            {
                EnemyAIManager.Instance.OnEnemiesAdd(enemyClass);
            }
            else
            {
                EnemyAIManager.Instance.OnBossesAdd(enemyClass);
            }

            newEnemy.transform.parent = transform;
        }                                                     

        Vector3 GeneratePosition()
        {
            Vector3 position = new Vector3();
            float f = UnityEngine.Random.value > 0.5f ? -1f : 1f;
            if(UnityEngine.Random.value > 0.5f)
            {
                position.x = UnityEngine.Random.Range(-spawnArea.x, spawnArea.x);
                position.y = spawnArea.y * f;
            }
            else
            {
                position.y = UnityEngine.Random.Range(-spawnArea.y, spawnArea.y);
                position.x = spawnArea.x * f;
            }
            position.z = 0;

            return position;
        }
    }
    #endregion
    #region 生成資料清除
    private void StopActiveEffectCoroutines()
    {
        foreach(var coroutine in _activeEffectCoroutines)
        {
            StopCoroutine(coroutine);
        }
        _activeEffectCoroutines.Clear();
    }
    #endregion
    #region 繪製生成區域
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(sampleArea.x * 2, sampleArea.y * 2, 0));
    }
    #endregion
}
