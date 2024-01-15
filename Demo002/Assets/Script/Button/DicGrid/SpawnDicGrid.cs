using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnDicGrid : MonoBehaviour
{
    public Dic DicType;
    [SerializeField] private EnemyDicGrid _enemyDicGrid;
    [SerializeField] private BossDicGrid _bossDicGrid;
    [SerializeField] private ElementDicGrid _elementDicGrid;
    [SerializeField] private CompoundDicGrid _compoundDicGrid;

    private void Start() 
    {
        switch (DicType)
        {
            case Dic.Enemy:
            SpawnEnemyDicGrid();
            break;
            case Dic.Boss:
            SpawnBossDicGrid();
            break;
            case Dic.Element:
            SpawnElementDicGrid();
            break;
            case Dic.Compound:
            SpawnCompoundDicGrid();
            break;
        }
    }

    private void SpawnEnemyDicGrid()
    {
        List<EnemyDataBase> AllEnemyData = ResourceManager.Instance.TrashEnemyDataBaseLibrary;
        for (int i = 0; i < AllEnemyData.Count; i++)
        {
            GameObject newButton = Instantiate(_enemyDicGrid.gameObject, this.transform);
            newButton.GetComponent<EnemyDicGrid>().SetButton(AllEnemyData[i]);
        }
    }
    private void SpawnBossDicGrid()
    {
        List<EnemyDataBase> AllBossData = ResourceManager.Instance.TrashBossDataBaseLibrary;
        for (int i = 0; i < AllBossData.Count; i++)
        {
            GameObject newButton = Instantiate(_bossDicGrid.gameObject, this.transform);
            newButton.GetComponent<BossDicGrid>().SetButton(AllBossData[i]);
        }
    }
    private void SpawnElementDicGrid()
    {
        List<ElementData> AllElementData = ResourceManager.Instance.ElementDataLibrary;
        for (int i = 0; i < AllElementData.Count; i++)
        {
            GameObject newButton = Instantiate(_elementDicGrid.gameObject, this.transform);
            newButton.GetComponent<ElementDicGrid>().SetButton(AllElementData[i]);
        }
    }
    private void SpawnCompoundDicGrid()
    {
        List<ElementData> AllCompoundData = ResourceManager.Instance.CompoundDataLibrary;
        for (int i = 0; i < AllCompoundData.Count; i++)
        {
            GameObject newButton = Instantiate(_compoundDicGrid.gameObject, this.transform);
            newButton.GetComponent<CompoundDicGrid>().SetButton(AllCompoundData[i]);
        }
    }
}
[Serializable] public enum Dic
{
    Enemy = 0,
    Boss = 1,
    Element = 2,
    Compound = 3,
};
