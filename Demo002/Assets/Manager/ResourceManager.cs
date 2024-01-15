using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourceManager : Singleton<ResourceManager>
{
    public List<EnemyDataBase> TrashEnemyDataBaseLibrary { get; private set; }
    public List<EnemyDataBase> TrashBossDataBaseLibrary { get; private set; }
    public List<ElementData> ElementDataLibrary { get; private set; }
    public List<ElementData> CompoundDataLibrary { get; private set; }
    public List<LevelData> LevelDataLibrary { get; private set; }
    public List<PlayerDataBase> PlayerDataLibrary { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        AssembleResource();
    }

    private void AssembleResource()
    {
        TrashEnemyDataBaseLibrary = Resources.LoadAll<EnemyDataBase>("ScriptableObjects/ForAllEnemy/ForEnemy/ForTrashEnemy").ToList();
        TrashBossDataBaseLibrary = Resources.LoadAll<EnemyDataBase>("ScriptableObjects/ForAllEnemy/ForBoss/ForTrashBoss").ToList();
        ElementDataLibrary = Resources.LoadAll<ElementData>("ScriptableObjects/ForAllElement/ForElement").ToList();
        LevelDataLibrary = Resources.LoadAll<LevelData>("ScriptableObjects/ForLevel").ToList();
        CompoundDataLibrary = Resources.LoadAll<ElementData>("ScriptableObjects/ForAllElement/ForCompound").ToList();
        PlayerDataLibrary = Resources.LoadAll<PlayerDataBase>("ScriptableObjects/ForPlayer").ToList();
    }
}
